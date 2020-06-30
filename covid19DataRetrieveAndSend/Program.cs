using System;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Topshelf;

namespace covid19DataRetrieveAndSend
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
               .WriteTo.File("covid19StatsNotifier.log")
               .CreateLogger();

            var services = DependencyConfiguration.Configure();

            var exitCode = HostFactory.Run(x =>
            {
                x.Service<RetrieveAndSendService>(s =>
                {
                    s.ConstructUsing(retrieveAndSendService => services.GetService<RetrieveAndSendService>());
                    s.WhenStarted(retrieveAndSendService => retrieveAndSendService.Start());
                    s.WhenStopped(retrieveAndSendService => retrieveAndSendService.Stop());
                });

                x.RunAsLocalSystem();
                x.SetServiceName("COVID19StatsService");
                x.SetDisplayName("COVID19 stats service");
                x.SetDescription("A service that automatically gets and sends COVID19 latest data every 24 hours");
            });

            var exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;
        }
    }
}