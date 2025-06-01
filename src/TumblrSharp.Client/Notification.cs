using Newtonsoft.Json;
using System;

namespace DontPanic.TumblrSharp.Client
{
    /// <summary>
    /// a activity item object
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// The type of activity item, from the list above
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public NotificationsTypes Type { get; set; }

        /// <summary>
        /// A timestamp of when the event happened.
        /// </summary>
        [JsonProperty(PropertyName = "timestamp")]
        [JsonConverter(typeof(TimestampConverter))]
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Time assignment for the parameter before in <seealso cref="TumblrClient.GetNotifications(string, DateTime, NotificationsTypes)">GetNotifications</seealso>
        /// </summary>
        [JsonProperty(PropertyName = "before")]
        [JsonConverter(typeof(TimestampConverter))]
        public DateTime Before { get; set; }

        /// <summary>
        /// If the activity has to do with one of your blog's posts, this will be its I
        /// </summary>
        [JsonProperty(PropertyName = "target_post_id")]
        [JsonConverter(typeof(LongConverter))]
        public long TargetPostId { get; set; }

        /// <summary>
        /// Summary of the post to which the activity relates
        /// </summary>
        [JsonProperty(PropertyName = "target_post_summary")]
        public string TargetPostSummary { get; set; }

        /// <summary>
        /// tumblrlog name of the post to which the activity relates
        /// </summary>
        [JsonProperty(PropertyName = "target_tumblelog_name")]
        public string TargetTumblelogName { get; set; }

        /// <summary>
        /// tumblrlog UUID of the post to which the activity relates
        /// </summary>
        [JsonProperty(PropertyName = "target_tumblelog_uuid")]
        public string TargetTumblelogUuid { get; set; }

        /// <summary>
        /// If the activity is coming from another blog, like a Like or Reblog, this will be its name
        /// </summary>
        [JsonProperty(PropertyName = "from_tumblelog_name")]
        public string FromTumblrLogName { get; set; }

        /// <summary>
        /// If the activity is coming from another blog, like a Like or Reblog, this will be its uuid
        /// </summary>
        [JsonProperty(PropertyName = "from_tumblelog_uuid")]
        public string FromTumblelogUuid { get; set; }

        /// <summary>
        /// If the activity is coming from another blog, like a Like or Reblog, this will be its a adult blog
        /// </summary>
        [JsonProperty(PropertyName = "from_tumblelog_is_adult")]
        [JsonConverter(typeof(BoolConverter))]
        public bool FromTumblelogIsAdult { get; set; }

        /// <summary>
        /// followed
        /// </summary>
        [JsonProperty(PropertyName = "followed")]
        [JsonConverter(typeof(BoolConverter))]
        public bool Followed { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty(PropertyName = "target_root_post_id", NullValueHandling = NullValueHandling.Include)]
        public long? TargetRootPostId { get; set; }

        /// <summary>
        /// private channel
        /// </summary>
        [JsonProperty(PropertyName = "private_channel")]
        [JsonConverter(typeof(BoolConverter))]
        public bool PrivateChannel { get; set; }

        /// <summary>
        /// private channel
        /// </summary>
        [JsonProperty(PropertyName = "target_post_type")]
        [JsonConverter(typeof(EnumStringConverter))]
        public PostType TargetPostType { get; set; }

        /// <summary>
        /// private channel
        /// </summary>
        [JsonProperty(PropertyName = "post_type")]
        [JsonConverter(typeof(EnumStringConverter))]
        public PostType PostType { get; set; }

        /// <summary>
        /// media url
        /// </summary>
        [JsonProperty(PropertyName = "media_url")]
        public string MediaUrl { get; set; }

        /// <summary>
        /// large medial url
        /// </summary>
        [JsonProperty(PropertyName = "media_url_large")]
        public string MediaUrlLarge { get; set; }

        /// <summary>
        /// For reblogs with comment, this will be a summary of the added content
        /// </summary>
        [JsonProperty(PropertyName = "reblog_key")]
        public string ReblogKey { get; set; }

        /// <summary>
        /// For activity like Reblogs and Replies, this will be the relevant post's ID
        /// </summary>
        [JsonProperty(PropertyName = "post_id")]
        public long PostId { get; set; }

        /// <summary>
        /// An array of used in the reblog, if any
        /// </summary>
        [JsonProperty(PropertyName = "post_tags")]
        public string[] PostTags { get; set; }

        /// <summary>
        /// For reblogs with comment, this will be a summary of the added content
        /// </summary>
        [JsonProperty(PropertyName = "added_text")]
        public string AddedText { get; set; }

        /// <summary>
        /// For replies, this will be the text of the reply
        /// </summary>
        [JsonProperty(PropertyName = "reply_text")]
        public string ReplyText { get; set; }
    }
}