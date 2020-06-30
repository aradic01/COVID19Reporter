using covid19DataRetrieveAndSend.Common.Helpers;
using Newtonsoft.Json;

namespace covid19DataRetrieveAndSend.Common.Extensions
{
    public static class StringExtensions
    {
        public static T DeserializeJson<T>(this string value)
        {
            return JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings
            {
                ContractResolver = new GenericTypeNameContractResolver(),
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Include
            });
        }
    }
}
