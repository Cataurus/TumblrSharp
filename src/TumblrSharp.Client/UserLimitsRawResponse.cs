using Newtonsoft.Json;

namespace DontPanic.TumblrSharp.Client
{
    internal class UserLimitsRawResponse
    {
        [JsonProperty("user")]
        public UserLimits User { get; set; } 
    }
}
