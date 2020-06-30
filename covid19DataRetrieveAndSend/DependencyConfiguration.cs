using System;
using System.Net;
using System.Net.Mail;
using covid19DataRetrieveAndSend.Common.Email;
using covid19DataRetrieveAndSend.Common.Email.Contracts;
using covid19DataRetrieveAndSend.Common.Excel;
using covid19DataRetrieveAndSend.Common.Extensions;
using covid19DataRetrieveAndSend.Common.RestSharp;
using covid19DataRetrieveAndSend.Services;
using covid19DataRetrieveAndSend.Services.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using SmtpClient = covid19DataRetrieveAndSend.Common.Email.SmtpClient;

namespace covid19DataRetrieveAndSend
{
    internal class DependencyConfiguration
    {
        public static IServiceProvider Configure()
        {
            var services = new ServiceCollection();

            RegisterLogger(services);
            RegisterConfigurations(services);

            services.AddScoped<RetrieveAndSendService>();
            services.AddScoped<IEmailSender, EMailSender>();
            services.AddScoped<ISmtpClient>(provider =>
            {
                var credentialsConfig = provider.GetService<IConfiguration>();
                var emailConfig = provider.GetService<IConfiguration>().GetSection("emailConfig");
                var usernameKey = emailConfig.GetSection("credentials")["usernameEnvKey"];
                var passwordKey = emailConfig.GetSection("credentials")["passwordEnvKey"];

                var clientConfig = new SmtpClientConfiguration
                {
                    DeliveryMethod = emailConfig["deliveryMethod"].ToEnumOrDefault<SmtpDeliveryMethod>(),
                    EnableSsl = emailConfig.GetAs<bool>("enableSsl"),
                    Host = emailConfig["host"],
                    Port = emailConfig.GetAs<int>("port"),
                    NetworkCredential = new NetworkCredential(credentialsConfig[usernameKey], credentialsConfig[passwordKey])
                };

                return new SmtpClient(clientConfig);
            });
            services.AddSingleton<IRestClientFactory, RestClientFactory>();
            services.AddTransient<ICovidDataProvider, CovidDataProvider>();
            services.AddTransient<ICovidStatisticsNotifier, CovidStatisticsEmailNotifier>();
            services.AddTransient<IExcelHelper, ExcelHelper>();

            //Add other dependencies    

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }

        private static void RegisterLogger(ServiceCollection services)
        {
            services.AddLogging(logging =>
            {
                logging.AddConsole();
                logging.AddSerilog();
            })
                .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information)
                .AddTransient<CovidDataProvider>()
                .AddTransient<CovidStatisticsEmailNotifier>()
                .AddTransient<RetrieveAndSendService>();
        }

        private static void RegisterConfigurations(IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
               .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables(prefix: "Covid19.")
               .Build();

            services.AddSingleton<IConfiguration>(configuration);
        }
    }
}
