using DontPanic.TumblrSharp.Client;
using Newtonsoft.Json;

namespace DontPanic.TumblrSharp
{
    /// <summary>
    /// Link
    /// </summary>
    public class Link
    {
        /// <summary>
        /// type
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        /// <summary>
        /// HttpMethod
        /// </summary>
        [JsonProperty(PropertyName = "method")]
        public string Method { get; set; }

        /// <summary>
        /// HRef
        /// </summary>
        [JsonProperty(PropertyName = "href")]
        public string HRef { get; set; }

        /// <summary>
        /// queryparameter
        /// </summary>
        [JsonProperty(PropertyName = "query_params")]
        public QueryParams QueryParams { get; set; }
    }
}