using Newtonsoft.Json;

namespace DontPanic.TumblrSharp.Client
{
    /// <summary>
    /// Response object for notifications
    /// </summary>
    public class NotificationsResponse
    {
        /// <summary>
        /// An array of activity item objects, see below
        /// </summary>
        [JsonProperty(PropertyName = "notifications")]
        public Notification[] Notifications { get; set; }

        /// <summary>
        /// A pagination links object with a next key to use, to load more activity items
        /// </summary>
        [JsonProperty(PropertyName = "_links")]
        public Links Links { get; set; }
    }
}
