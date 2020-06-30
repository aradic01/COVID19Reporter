using RestSharp;

namespace covid19DataRetrieveAndSend.Common.RestSharp
{
    public interface IRestClientFactory
    {
        IRestClient Create(string baseUrl);
    }
}
