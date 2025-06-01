using Newtonsoft.Json;
using System;

namespace DontPanic.TumblrSharp.Client
{
    /// <summary>
    /// Converter for long
    /// </summary>
    /// <remarks>
    /// Tumblr has the peculiarity of delivering an empty string with whole numbers.
    /// </remarks>
    public class LongConverter : JsonConverter
    {

        /// <inheritdoc/>
        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(long))
            {
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            long result = 0;

            string longString = reader.Value.ToString();

            if ( string.IsNullOrEmpty(longString) == false)
            {       
                result = Convert.ToInt64(longString);
            }

            return result;
        }

        /// <inheritdoc/>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString().ToLower());
        }
    }
}