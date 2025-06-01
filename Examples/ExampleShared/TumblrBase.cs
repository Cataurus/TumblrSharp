using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using DontPanic.TumblrSharp.OAuth;
using System;

namespace ExampleShared
{
    public class TumblrBase
    {
        protected TumblrClient client;

        public TumblrBase()
        {
            if (Settings.CONSUMER_KEY == "***")
            {
                Console.WriteLine("Change in sourcecode the consumerKey or better setting as environment variable, etc...!");
                Console.WriteLine();

                throw new Exception();
            }

            this.client = new TumblrClientFactory().Create<TumblrClient>(Settings.CONSUMER_KEY, Settings.CONSUMER_SECRET, new Token(Settings.OAUTH_TOKEN_KEY, Settings.OAUTH_TOKEN_SECRET));
        }
    }
}
