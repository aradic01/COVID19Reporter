using System;
using System.Threading.Tasks;
using System.Timers;
using covid19DataRetrieveAndSend.Common.Extensions;
using covid19DataRetrieveAndSend.Services.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace covid19DataRetrieveAndSend
{
    public class RetrieveAndSendService
    {
        private readonly ICovidDataProvider _dataProvider;
        private readonly ICovidStatisticsNotifier _notifier;
        private readonly ILogger<RetrieveAndSendService> _logger;
        private readonly Timer _timer;

        public RetrieveAndSendService(IConfiguration configuration, ICovidDataProvider dataProvider, ICovidStatisticsNotifier notifier, ILogger<RetrieveAndSendService> logger)
        {
            _dataProvider = dataProvider;
            _notifier = notifier;
            _logger = logger;
            _timer = new Timer(configuration.GetAs<double>("statisticsSendIntervalInMinutes") * 60 * 1000) { AutoReset = true };
            _timer.Elapsed += timer_Elapsed;
        }

        private async void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            await RetrieveAndSend();
        }

        public void Start()
        {
            _logger.LogDebug("Starting service...");
            RetrieveAndSend().GetAwaiter().GetResult();
            _timer.Start();
        }

        public void Stop()
        {
            _logger.LogDebug("Stopping service...");
            _timer.Stop();
        }

        private async Task RetrieveAndSend()
        {
            try
            {
                var data = await _dataProvider.GetData();

                await _notifier.NotifySubscribers(data);
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong: \"{e.Message}\"");
            }
        }
    }
}
