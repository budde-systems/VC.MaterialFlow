using MF152004.Workers.Logic;
using MF152004.Workers;
using BlueApps.MaterialFlow.Common.Connection.Broker;

namespace BlueApps.MaterialFlow.WorkerService
{
    public class MaterialFlowWorker : BackgroundService
    {
        private readonly ILogger<MaterialFlowWorker> _logger;
        private readonly MaterialFlowMng _materialFlowManager;
        private readonly IServiceProvider _services;

        public MaterialFlowWorker(ILogger<MaterialFlowWorker> logger, MaterialFlowMng materialFlowManager, IServiceProvider services)
        {
            _logger = logger;
            _services = services;
            _materialFlowManager = materialFlowManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var srvs = _services.CreateScope();
            var broker = srvs.ServiceProvider.GetService<MqttBroker>();

            if (broker != null)
                await broker.RunBrokerAsync();
            else
                Environment.Exit(0); //TODO: logging etc.

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}