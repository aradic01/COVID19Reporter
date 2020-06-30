using RestSharp;

namespace covid19DataRetrieveAndSend.Common.RestSharp
{
    public class RestClientFactory : IRestClientFactory
    {
        public IRestClient Create(string baseUrl)
        {
            return new RestClient(baseUrl);
        }
    }
}
