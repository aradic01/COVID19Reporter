using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using covid19DataRetrieveAndSend.Common.Email.Contracts;
using covid19DataRetrieveAndSend.Common.Excel;
using covid19DataRetrieveAndSend.Models;
using covid19DataRetrieveAndSend.Services.Contracts;
using Microsoft.Extensions.Logging;

namespace covid19DataRetrieveAndSend.Services
{
    public class CovidStatisticsEmailNotifier : ICovidStatisticsNotifier
    {
        private readonly ILogger _logger;
        private readonly IEmailSender _emailSender;
        private readonly IExcelHelper _excelHelper;

        public CovidStatisticsEmailNotifier(ILogger<CovidStatisticsEmailNotifier> logger, IEmailSender emailSender, IExcelHelper excelHelper)
        {
            _logger = logger;
            _emailSender = emailSender;
            _excelHelper = excelHelper;
        }

        public async Task NotifySubscribers(IEnumerable<Covid19Stats> data)
        {
            var subscribers = GetSubscribers();

            _logger.LogInformation($"Total {subscribers.Count()} subscribers to notify.");

            var xlsData = _excelHelper.GenerateXlsFile(data);

            var attachments = new Dictionary<string, byte[]> { { $"CoronaStatistics_{DateTime.UtcNow}", xlsData } };

            await _emailSender.SendEmails(subscribers, "Daily Corona update", "Find your report attached below.", attachments);

            _logger.LogDebug("Successfully notified subscribers.");
        }

        private IEnumerable<string> GetSubscribers()
        {
            //Think about how you might get subscribers.
            //It might be in configuration, might be on a different service
            //or in the database
            //Good option would be to hide implementation details behind Repository interface
            //That way notifier will not change in case subscriber details change
            //Therefore we've implemented SRP!
            return new[] { "kristicevic.antonio@gmail.com" };
        }
    }
}
