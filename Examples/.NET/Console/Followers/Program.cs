using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using DontPanic.TumblrSharp.OAuth;
using ExampleShared;
using System;

namespace Followers
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Follower sample!");

            // create TumblrClient
            TumblrClient tumblrClient;

            try
            {
                tumblrClient = new TumblrClientFactory().Create<TumblrClient>(Settings.CONSUMER_KEY, Settings.CONSUMER_SECRET, new Token(Settings.OAUTH_TOKEN_KEY, Settings.OAUTH_TOKEN_SECRET));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating TumblrClient: " + ex.Message);
                
                return;
            }
            
            // get your blogs
            var userInfo = tumblrClient.GetUserInfoAsync().GetAwaiter().GetResult();

            // display blogs
            Console.WriteLine();
            Console.WriteLine("Blogs:");

            int idx = -1;
            foreach (var blog in userInfo.Blogs)
            {
                idx++;
                Console.WriteLine(idx.ToString() + ". " + blog.Name);
            }

            //select a blog
            Console.WriteLine();
            Console.Write("select a blog (0-" + idx.ToString() + "): ");
            string blogName = userInfo.Blogs[Convert.ToUInt32(Console.ReadLine())].Name;

            // get first 20 follower
            BlogBase[] blogs = tumblrClient.GetFollowersAsync(blogName).GetAwaiter().GetResult().Result;

            // display follower
            Console.WriteLine();
            Console.WriteLine("first 20th follower:");
            foreach (var blog in blogs)
            {
                Console.WriteLine(blog.Name);
            }

            Console.ReadLine();
        }
    }
}
