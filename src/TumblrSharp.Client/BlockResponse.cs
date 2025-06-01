using Newtonsoft.Json;

namespace DontPanic.TumblrSharp.Client
{
    /// <summary>
    /// Blocksresponse
    /// </summary>
    public class BlocksResponse
    {
        /// <summary>
        /// array of BlogBase
        /// </summary>
        [JsonProperty(PropertyName = "blocked_tumblelogs")]
        public BlogBase[] Blogs { get; set; }

        /// <summary>
        /// a pagination links object
        /// </summary>
        [JsonProperty(PropertyName = "_links")]
        public Links Links { get; set; }
    }
}
