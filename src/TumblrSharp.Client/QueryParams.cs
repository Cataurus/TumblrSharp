using Newtonsoft.Json;
using System;

namespace DontPanic.TumblrSharp.Client
{
    /// <summary>
    /// Params of query
    /// </summary>
    public class QueryParams
    {
        /// <summary>
        /// offset
        /// </summary>
        [JsonProperty(PropertyName = "offset")]
        public int Offset { get; set; }

        /// <summary>
        /// limit
        /// </summary>
        [JsonProperty(PropertyName = "limit")]
        public int Limit { get; set; }

        /// <summary>
        /// before a timestamp
        /// </summary>
        [JsonProperty(PropertyName = "before")]
        [JsonConverter(typeof(TimestampConverter))]
        public DateTime Before { get; set; }
    }
}