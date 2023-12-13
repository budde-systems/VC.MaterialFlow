using BlueApps.MaterialFlow.Common.Connection.Packets;
using BlueApps.MaterialFlow.Common.Connection.Packets.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Packets;
using System.Text;

namespace BlueApps.MaterialFlow.Common.Connection.Client;

public class MqttClient : IClient //abstract?
{
    public event EventHandler<MessagePacketEventArgs>? OnReceivingMessage;
    public event EventHandler? ClientConnected;
    public event EventHandler? ClientDisconnected;

    public string BrokerIPAddress { get; private set; } = null!;

    public bool IsConnected { get; private set; }

    private readonly string _clientId;
    private readonly string _topicPlc;
    private readonly string _topicWebService;
    private readonly List<string> _topics = new();

    private readonly IConfiguration _configuration;
    private readonly ILogger<MqttClient> _logger;

    private readonly IMqttClient _client;
    private readonly MqttFactory _mqttFactory;

    public MqttClient(ILogger<MqttClient> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;

        _clientId = $"{configuration["MyClientId"] ?? "Client"}-{Guid.NewGuid()}";
        _topicPlc = configuration["PLC_To_Workerservice"] ?? "";
        _topicWebService = configuration["Webservice_To_Workerservice"] ?? "";

        _mqttFactory = new MqttFactory();
        _client = _mqttFactory.CreateMqttClient();
        _client.ConnectedAsync += Connected;
        _client.DisconnectedAsync += Disconnected;
        _client.ApplicationMessageReceivedAsync += MessageReceived;
    }

    public void AddTopics(params string[]? topics)
    {
        if (topics != null)
            _topics.AddRange(topics);
    }

    private Task MessageReceived(MqttApplicationMessageReceivedEventArgs? msg) //TODO: Bestätigung ergänzen? => msg.AcknowledgeAsync()
    {
        var buffer = msg?.ApplicationMessage?.PayloadSegment.Array;

        if (buffer != null)
        {
            var messageEvent = new MessagePacketEventArgs
            {
                Message = DeserializeData(buffer),
                ClientId = msg!.ClientId ?? ""
            };

            messageEvent.Message.Topic = msg.ApplicationMessage.Topic;

            //_logger.LogInformation($"Message received, FROM: {messageEvent.Message.Topic}/ DATA: {messageEvent.Message.Data}"); //remove it later

            OnReceivingMessage?.Invoke(this, messageEvent);
        }

        return Task.CompletedTask;
    }

    private Task Disconnected(MqttClientDisconnectedEventArgs arg)
    {
        if (arg.ClientWasConnected)
        {
            IsConnected = _client.IsConnected;
            ClientDisconnected?.Invoke(this, arg);
        }

        return Task.CompletedTask;
    }

    private Task Connected(MqttClientConnectedEventArgs arg)
    {
        _logger.LogInformation($"The client {_clientId} has been connected to broker: {BrokerIPAddress}:{_configuration["BrokerPort"]}");
        ClientConnected?.Invoke(this, EventArgs.Empty);

        IsConnected = _client.IsConnected;

        return Task.CompletedTask;
    }

    
    public async Task Connect(CancellationToken cancellationToken)
    {
        BrokerIPAddress = _configuration["BrokerIPAddress"] ?? "127.0.0.1";
        var options = GetOptions();
        await _client.ConnectAsync(options, cancellationToken);
        await SubscribeToTopics();
    }

    private MqttClientOptions GetOptions() => _mqttFactory.CreateClientOptionsBuilder()
        .WithClientId(_clientId)
        .WithTcpServer(BrokerIPAddress, int.Parse(_configuration["BrokerPort"] ?? "1883"))
        .Build();

    private async Task SubscribeToTopics() //TODO: to all topics from commondata / configuration
    {
        if (_topics.Count > 0)
        {
        }


        MqttClientSubscribeOptions subscribeOptions;

        if (_topics.Count > 0)
        {
            subscribeOptions = _mqttFactory.CreateSubscribeOptionsBuilder().Build();
            subscribeOptions.TopicFilters.AddRange(_topics.Select(topic => new MqttTopicFilter { Topic = topic }));
           
        }
        else
        {
            subscribeOptions = _mqttFactory.CreateSubscribeOptionsBuilder()
                .WithTopicFilter(_topicPlc)
                .WithTopicFilter(_topicWebService)
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
        var data = Encoding.ASCII.GetString(buffer);

        var msgPacket = new MessagePacket
        {
            Data = data
        };

        return msgPacket;
    }
}