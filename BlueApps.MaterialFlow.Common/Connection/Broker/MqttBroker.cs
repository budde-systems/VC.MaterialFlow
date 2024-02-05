using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Server;

namespace BlueApps.MaterialFlow.Common.Connection.Broker;

public class MqttBroker
{
    private readonly MqttServer _broker;
    private readonly ILogger<MqttBroker> _logger;
    private readonly IConfiguration _configuration;
    
    public int Port => int.Parse(_configuration["BrokerPort"] ?? "1883");

    public MqttBroker(ILogger<MqttBroker> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        
        var mqttFactory = new MqttFactory();

        var options = mqttFactory.CreateServerOptionsBuilder()
            .WithDefaultEndpointPort(Port)
            .WithDefaultEndpoint()
            .Build();

        _broker = mqttFactory.CreateMqttServer(options);

        _broker.ClientConnectedAsync += ClientConnectedAsync;
        _broker.ClientDisconnectedAsync += ClientDisconnectedAsync;
    }

    private Task ClientDisconnectedAsync(ClientDisconnectedEventArgs e)
    {
        _logger.LogInformation("MQTT client: {0} has been disconnected", e.ClientId);
        return Task.CompletedTask;
    }

    private Task ClientConnectedAsync(ClientConnectedEventArgs e)
    {
        _logger.LogInformation("MQTT client: {0} has been connected", e.ClientId);
        return Task.CompletedTask;
    }

    public async Task RunBrokerAsync()
    {
        await _broker.StartAsync();
        _logger.LogInformation("MQTT broker has been started successfully at port {0}", Port);
    }
}