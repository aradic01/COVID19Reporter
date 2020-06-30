using System;
using System.Net;
using System.Threading.Tasks;
using covid19DataRetrieveAndSend.Common.RestSharp;
using covid19DataRetrieveAndSend.Services;
using covid19DataRetrieveAndSend.Services.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RestSharp;

namespace CovidData.UnitTests
{
    [TestFixture]
    public class CovidDataProviderTests
    {
        private Mock<IConfiguration> _configuration;
        private Mock<ILogger<CovidDataProvider>> _logger;
        private Mock<IRestClientFactory> _restClientFactory;
        private Mock<IRestClient> _restClient;

        private ICovidDataProvider _service;

        [SetUp]
        public void Setup()
        {
            _configuration = new Mock<IConfiguration>(MockBehavior.Strict);
            _logger = new Mock<ILogger<CovidDataProvider>>();
            _restClientFactory = new Mock<IRestClientFactory>(MockBehavior.Strict);
            _restClient = new Mock<IRestClient>(MockBehavior.Strict);

            _configuration.Setup(x => x["covidStatisticsUrl"]).Returns("http://nekiurl.api.com");
            _configuration.Setup(x => x["RapidApiKey"]).Returns("12808941kakafkjaf781249814bfga");

            _restClientFactory.Setup(x => x.Create(It.IsAny<string>())).Returns(_restClient.Object);

            _restClient.Setup(x => x.ExecuteGetAsync(It.IsAny<RestRequest>(), default)).ReturnsAsync(SuccessfulRestResponse());

            _service = new CovidDataProvider(_restClientFactory.Object, _configuration.Object, _logger.Object);
        }

        [Test]
        //GivenWhenThen
        public void GetData_WithUnauthorizedResponse_ShouldThrowException()
        {
            //arrange
            _restClient.Setup(x => x.ExecuteGetAsync(It.IsAny<RestRequest>(), default)).ReturnsAsync(UnauthorizedRestResponse());

            //act
            //Action a =()=> _service.GetData();
            //assert

            //Assert.AreEqual();
            Assert.ThrowsAsync<ApplicationException>(() => _service.GetData());
        }

        private IRestResponse UnauthorizedRestResponse()
        {
            return new RestResponse
            {
                StatusCode = HttpStatusCode.Unauthorized,
                ResponseStatus = ResponseStatus.Completed
            };
        }

        [Test]
        public async Task Test2()
        {

        }

        private static IRestResponse SuccessfulRestResponse()
        {
            return new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                ResponseStatus = ResponseStatus.Completed
            };
        }
    }
}