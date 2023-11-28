using BlueApps.MaterialFlow.Common.Connection.Packets;
using BlueApps.MaterialFlow.Common.Connection.Packets.Events;
using BlueApps.MaterialFlow.Common.Models.EventArgs;
using BlueApps.MaterialFlow.Common.Values.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Exceptions;
using MQTTnet.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BlueApps.MaterialFlow.Common.Connection.Client
{
    public class MqttClient : IClient //abstract?
    {
        public event EventHandler<MessagePacketEventArgs> OnReceivingMessage;
        public event EventHandler ClientConnected;
        public event EventHandler ClientDisconnected;

        public string BrokerIPAddress { get; private set; }
        public bool IsConnected { get; private set; } 

        private readonly string _clientId;
        private readonly string _topicPlc;
        private readonly string _topicWebservcice;
        private readonly List<string> _topics = new();

        private readonly IConfiguration _configuration;
        private readonly ILogger<MqttClient> _logger;

        private readonly IMqttClient _client;
        private readonly MqttFactory _mqttFactory;

        public MqttClient(ILogger<MqttClient> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

            _clientId = $"{(configuration["MyClientId"] ?? "Client")}-{Guid.NewGuid()}";
            _topicPlc = configuration["PLC_To_Workerservice"] ?? "";
            _topicWebservcice = configuration["Webservice_To_Workerservice"] ?? "";

            _mqttFactory = new MqttFactory();
            _client = _mqttFactory.CreateMqttClient();
            _client.ConnectedAsync += Connected;
            _client.DisconnectedAsync += Disconnected;
            _client.ApplicationMessageReceivedAsync += MessageReceived;
        }

        public void AddTopics(params string[] topics)
        {
            if (topics != null)
                _topics.AddRange(topics);
        }

        private Task MessageReceived(MqttApplicationMessageReceivedEventArgs msg) //TODO: Bestätigung ergänzen? => msg.AcknowledgeAsync()
        {
            if (msg is null || msg.ApplicationMessage is null)
            {
                return Task.CompletedTask;
            }
            else
            {
                var buffer = msg.ApplicationMessage.PayloadSegment.Array;

                if (buffer != null)
                {
                    MessagePacketEventArgs messageEvent = new MessagePacketEventArgs()
                    {
                        Message = DeserializeData(buffer),
                        ClientId = msg.ClientId ?? ""
                    };
                    
                    messageEvent.Message.Topic = msg.ApplicationMessage.Topic;

                    //_logger.LogInformation($"Message received, FROM: {messageEvent.Message.Topic}/ DATA: {messageEvent.Message.Data}"); //remove it later

                    OnReceivingMessage?.Invoke(this, messageEvent); 
                }

                return Task.CompletedTask;
            }
        }

        private async Task Disconnected(MqttClientDisconnectedEventArgs arg)
        {
            if (arg.ClientWasConnected)
            {
                IsConnected = _client.IsConnected;

                ClientDisconnected?.Invoke(this, arg);
            }
        }

        private Task Connected(MqttClientConnectedEventArgs arg)
        {
            _logger.LogInformation($"The client {_clientId} has been connected to broker: {BrokerIPAddress}:{_configuration["BrokerPort"]}");
            ClientConnected?.Invoke( this, EventArgs.Empty );

            IsConnected = _client.IsConnected;
            
            return Task.CompletedTask;
        }

        public async Task Connect(CancellationToken cancellationToken)
        {
            try
            {
                var ips = GetInterNetworkIPAddresses().Select(x => x.MapToIPv4().ToString()).ToArray();
                string[] ipAddresses = new string[0];

                Array.Resize(ref ipAddresses, ips.Length + 1);
                Array.Copy(ips, 0, ipAddresses, 0, ips.Length);
                ipAddresses[ipAddresses.Length - 1] = _configuration["BrokerIPAddress"];

                for (int i = 0; i < ipAddresses.Length; i++)
                {
                    _logger.LogInformation($"Connection establishment to IP Address {ipAddresses[i]}:{_configuration["BrokerPort"]} is started...");

                    try
                    {
                        var options = GetOptions(ipAddresses[i]);
                        BrokerIPAddress = ipAddresses[i];

                        await _client.ConnectAsync(options, cancellationToken);
                        await SubscribeToTopics();

                        break;
                    }
                    catch (Exception exception)
                    {
                        _logger.LogWarning(exception.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.ToString());
            }
        }

        private MqttClientOptions GetOptions(string ipAddress)
        {
            var options = _mqttFactory.CreateClientOptionsBuilder()
                .WithClientId(_clientId)
                .WithTcpServer(ipAddress, int.Parse(_configuration["BrokerPort"]))
                .Build();

            return options;
        }

        private IEnumerable<IPAddress>? GetInterNetworkIPAddresses()
        {
            var hostname = Dns.GetHostName();
            var addresses = Dns.GetHostEntry(hostname).AddressList
                .Where(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);

            return addresses;
        }

        private async Task SubscribeToTopics() //TODO: to all topics from commondata / configuration
        {
            MqttClientSubscribeOptions? subscribeOptions;

            if (_topics.Count > 0)
            {
                subscribeOptions = _mqttFactory.CreateSubscribeOptionsBuilder()
                .Build();

                foreach (var topic in _topics)
                {
                    var filter = new MqttTopicFilter();
                    filter.Topic = topic;
                    subscribeOptions.TopicFilters.Add(filter);
                } 
            }
            else
            {
                subscribeOptions = _mqttFactory.CreateSubscribeOptionsBuilder()
                .WithTopicFilter(_topicPlc)
                .WithTopicFilter(_topicWebservcice)
                .Build();
            }

            await _client.SubscribeAsync(subscribeOptions);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messagePacket"></param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        public async void SendData(MessagePacket messagePacket)
        {
            ValidateMessagePacket(messagePacket);

            try
            {
                if (_client.IsConnected)
                {
                    var applicationMessage = new MqttApplicationMessageBuilder()
                            .WithTopic(messagePacket.Topic)
                            .WithPayload(SerializeData(messagePacket))
                            .Build();

                    await _client.PublishAsync(applicationMessage);

                    //_logger.LogInformation($"Send message, TO: {messagePacket.Topic} /DATA: {messagePacket.Data}"); //remove it later
                }
                else
                {
                    _logger.LogWarning("Could not send data. The client is not connected");
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.ToString());
            }
        }

        private void ValidateMessagePacket(MessagePacket messagePacket)
        {
            if (messagePacket is null)
                throw new ArgumentNullException(nameof(messagePacket));

            if (string.IsNullOrWhiteSpace(messagePacket.Topic))
                throw new InvalidOperationException("The topic cannot be empty");

            if (string.IsNullOrWhiteSpace(messagePacket.Data))
                throw new InvalidOperationException("The data cannot be empty");
        }

        private byte[] SerializeData(MessagePacket dataPacket)
        {
            var data = Encoding.ASCII.GetBytes(dataPacket.Data);

            return data;
        }

        private MessagePacket DeserializeData(byte[] buffer)
        {
            string data = Encoding.ASCII.GetString(buffer);

            var msgPacket = new MessagePacket()
            {
                Data = data
            };

            return msgPacket;
        }
    }
}
