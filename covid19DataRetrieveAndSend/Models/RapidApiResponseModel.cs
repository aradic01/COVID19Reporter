using System;
using System.Collections.Generic;
using covid19DataRetrieveAndSend.Common.Attributes;
using Newtonsoft.Json;

namespace covid19DataRetrieveAndSend.Models
{
    public class RapidApiResponseModel<T> where T : new()
    {
        public bool Error { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        [JsonProperty("Data")]
        public DataEnvelope<T> Envelope { get; set; }

#pragma warning disable 693
        public class DataEnvelope<T> where T : new()
#pragma warning restore 693
        {
            public DateTime LastChecked { get; set; }
            [JsonPropertyGenericTypeName(0)]
            public ICollection<T> Data { get; set; }
        }
    }
}
