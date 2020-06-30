using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using covid19DataRetrieveAndSend.Common.Extensions;
using covid19DataRetrieveAndSend.Common.RestSharp;
using covid19DataRetrieveAndSend.Models;
using covid19DataRetrieveAndSend.Services.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace covid19DataRetrieveAndSend.Services
{
    public class CovidDataProvider : ICovidDataProvider
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IRestClient _restClient;

        public CovidDataProvider(IRestClientFactory restClientFactory, IConfiguration configuration, ILogger<CovidDataProvider> logger)
        {
            _configuration = configuration;
            _logger = logger;

            var dataSourceUrl = configuration["covidStatisticsUrl"];
            _restClient = restClientFactory.Create(dataSourceUrl);
        }

        public async Task<IEnumerable<Covid19Stats>> GetData()
        {
            _logger.LogInformation("Getting COVID19 data.");
            var request = BuildRequest();

            var response = await _restClient.ExecuteGetAsync(request);
            if (!response.IsSuccessful)
            {
                _logger.LogError($"Couldn't get COVID19 statistics because {response.ErrorMessage}.");
                throw new ApplicationException("Couldn't get Covid statistics.");
            }

            var responseContent = response.Content.DeserializeJson<RapidApiResponseModel<Covid19Stats>>();

            return responseContent.Envelope.Data;
        }

        private RestRequest BuildRequest()
        {
            var request = new RestRequest(Method.GET) { RequestFormat = DataFormat.Json };

            request.AddHeader("x-rapidapi-key", _configuration["RapidApiKey"]);

            return request;
        }
    }
}
