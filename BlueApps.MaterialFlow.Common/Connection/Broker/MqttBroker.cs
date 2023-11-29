using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Server;

namespace BlueApps.MaterialFlow.Common.Connection.Broker
{
    public class MqttBroker
    {
        private readonly MqttServer _broker;
        private readonly MqttFactory _mqttFactory;
        private readonly ILogger<MqttBroker> _logger;
        private readonly IConfiguration _configuration;

        public MqttBroker(ILogger<MqttBroker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _mqttFactory = new MqttFactory();

            var options = GetOptions();

            _broker = _mqttFactory.CreateMqttServer(options);

            _broker.ClientConnectedAsync += ClientConnectedAsync;
            _broker.ClientDisconnectedAsync += ClientDisconnectedAsync;
        }

        private Task ClientDisconnectedAsync(ClientDisconnectedEventArgs e)
        {
            _logger.LogInformation($"The client: {e.ClientId} has been disconnected.");
            return Task.CompletedTask;
        }

        private Task ClientConnectedAsync(ClientConnectedEventArgs e)
        {
            _logger.LogInformation($"The client: {e.ClientId} has been connected.");
            return Task.CompletedTask;
        }

        private MqttServerOptions GetOptions()
        {
            var options = _mqttFactory.CreateServerOptionsBuilder()
                .WithDefaultEndpointPort(int.Parse(_configuration["BrokerPort"]))
                .WithDefaultEndpoint()
                .Build();

            return options;
        }

        public async Task RunBrokerAsync() //TODO: Create topics
        {
            await _broker.StartAsync();
            _logger.LogInformation("The broker has been startet successfully");
        }
    }
}
