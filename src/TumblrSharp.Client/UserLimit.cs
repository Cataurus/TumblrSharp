using Newtonsoft.Json;
using System;

namespace DontPanic.TumblrSharp.Client
{
    /// <summary>
    /// userlimit
    /// </summary>
    public class UserLimit
    {
        /// <summary>
        /// description
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// limit
        /// </summary>
        [JsonProperty("limit")]
        public int Limit { get; set; }

        /// <summary>
        /// remaining
        /// </summary>
        [JsonProperty("remaining")]
        public int Remaining { get; set; }

        /// <summary>
        /// reset at
        /// </summary>
        [JsonProperty("reset_at")]
        [JsonConverter(typeof(TimestampConverter))]
        public DateTime ResetAt { get; set; }
    }
}