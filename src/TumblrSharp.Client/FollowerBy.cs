using Newtonsoft.Json;

namespace DontPanic.TumblrSharp.Client
{
    /// <summary>
    /// Represents the answer for followed_by
    /// </summary>
    internal class FollowerBy
    {
        /// <summary>
        /// blog followed
        /// </summary>
        [JsonConverter(typeof(BoolConverter))]
        [JsonProperty(PropertyName = "followed_by")]
        public bool IsFollower { get; set; }
    }
}
