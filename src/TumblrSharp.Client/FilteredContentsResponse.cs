using Newtonsoft.Json;

namespace DontPanic.TumblrSharp.Client
{
    internal class FilteredContentResponse
    {
        [JsonProperty(PropertyName = "filtered_content")]
        public string[] FilteredContent { get; set; }
    }
}
