using BlueApps.MaterialFlow.Common.Connection.Broker;
using BlueApps.MaterialFlow.Common.Connection.Client;
using MF152004.Workers;
using MF152004.Workers.Data;
using MF152004.Workers.Logic;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;

namespace BlueApps.MaterialFlow.WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                IHost host = Host.CreateDefaultBuilder(args)
                    .ConfigureServices(services =>
                    {
                        services.AddWindowsService(options => options.ServiceName = "BlueApps_MaterialFlow");

                        services.AddScoped<MqttBroker>();
                        services.AddSingleton<ContextService>();
                        services.AddSingleton<MqttClient>();
                        services.AddSingleton<MaterialFlowMng>();
                        services.AddHostedService<MaterialFlowWorker>();

                    })
                    .Build();

                host.Run();
            }
        }
    }
}