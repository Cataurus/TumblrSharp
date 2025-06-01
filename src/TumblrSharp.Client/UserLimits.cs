using Newtonsoft.Json;

namespace DontPanic.TumblrSharp.Client
{
    /// <summary>
    /// limits for user
    /// </summary>
    public class UserLimits
    {
        /// <summary>
        /// limits of secondary blogs you can create per day
        /// </summary>
        [JsonProperty("blogs")]
        public UserLimit Blogs { get; set; }

        /// <summary>
        /// limits of blogs you can follow per day
        /// </summary>
        [JsonProperty("follows")]
        public UserLimit Follows { get; set; }

        /// <summary>
        /// limits of posts you can like per day
        /// </summary>
        [JsonProperty("likes")]
        public UserLimit Likes { get; set; }

        /// <summary>
        /// limits of photos you can upload per day
        /// </summary>
        [JsonProperty("photos")]
        public UserLimit Photos { get; set; }

        /// <summary>
        /// limits of posts you can create per day
        /// </summary>
        [JsonProperty("posts")]
        public UserLimit Posts { get; set; }

        /// <summary>
        /// limits of seconds of video content you can upload per day
        /// </summary>
        [JsonProperty("video_Seconds")]
        public UserLimit VideoSeconds { get; set; }

        /// <summary>
        /// limits the number of video files you can upload per day
        /// </summary>
        [JsonProperty("videos")]
        public UserLimit Videos { get; set; }
    }
}