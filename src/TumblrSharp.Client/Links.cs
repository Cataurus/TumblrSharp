using Newtonsoft.Json;

namespace DontPanic.TumblrSharp
{
    /// <summary>
    /// Links
    /// </summary>
    public class Links
    {
        /// <summary>
        /// previous link
        /// </summary>
        [JsonProperty(PropertyName = "previous")]
        public Link Previous { get; set; }

        /// <summary>
        /// next link
        /// </summary>
        [JsonProperty(PropertyName = "next")]
        public Link Next { get; set; }

        /// <summary>
        /// next link
        /// </summary>
        [JsonProperty(PropertyName = "terms_of_service")]
        public Link TermsOfService { get; set; }
    }
}