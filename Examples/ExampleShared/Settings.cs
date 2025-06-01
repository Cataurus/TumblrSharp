using System;

namespace ExampleShared
{
    public static class Settings
    {
        // your consumer- and accessToken 

        public static readonly string CONSUMER_KEY = Environment.GetEnvironmentVariable("TUMBLR_CONSUMER_KEY") ?? "***";
        public static readonly string CONSUMER_SECRET = Environment.GetEnvironmentVariable("TUMBLR_CONSUMER_SECRET") ?? "***";

        public static readonly string OAUTH_TOKEN_KEY = Environment.GetEnvironmentVariable("TUMBLR_ACCESS_KEY") ?? "***";
        public static readonly string OAUTH_TOKEN_SECRET = Environment.GetEnvironmentVariable("TUMBLR_ACCESS_SECRET") ?? "***";
    }
}
