// See https://aka.ms/new-console-template for more information

using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using DontPanic.TumblrSharp.OAuth;
using ExampleShared;

Console.WriteLine("Example for GetUserLimits");
Console.WriteLine("*************************");
Console.WriteLine("");

if (Settings.CONSUMER_KEY == "xxx")
{
    Console.WriteLine("set Consumer- and AccessToken");
    return;
}

using var tumblrClient = new TumblrClientFactory().Create<TumblrClient>(Settings.CONSUMER_KEY, Settings.CONSUMER_SECRET, new Token(Settings.OAUTH_TOKEN_KEY, Settings.OAUTH_TOKEN_SECRET));

var result = await tumblrClient.GetUserLimitsAsync().ConfigureAwait(true);

Console.WriteLine($"Blogs, you can create per day: {result.Blogs.Limit}");