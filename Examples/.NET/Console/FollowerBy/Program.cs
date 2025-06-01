using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using DontPanic.TumblrSharp.OAuth;
using ExampleShared;
using System;

namespace FollowerBy
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            Console.WriteLine("FollowerBy Sample");
            Console.WriteLine("-----------------");

            // create TumblrClient
            using TumblrClient tumblrClient = new TumblrClientFactory().Create<TumblrClient>(Settings.CONSUMER_KEY, Settings.CONSUMER_SECRET, new Token(Settings.OAUTH_TOKEN_KEY, Settings.OAUTH_TOKEN_SECRET));

            // get your blogs
            var userInfo = await tumblrClient.GetUserInfoAsync();

            string blogName;

            if (userInfo.Blogs.Length > 1)
            {
                // display blogs
                Console.WriteLine();
                Console.WriteLine("Blogs:");

                int idx = 0;
                foreach (var blog in userInfo.Blogs)
                {
                    idx++;
                    Console.WriteLine("\t" + idx.ToString() + ". " + blog.Name);
                }

                //select a blog
                Console.WriteLine();
                Console.Write("select a blog (1-" + idx.ToString() + "): ");

                blogName = userInfo.Blogs[Convert.ToUInt32(Console.ReadLine()) - 1].Name;
            }
            else
            {
                blogName = userInfo.Blogs[0].Name;

                Console.WriteLine();
                Console.WriteLine($"blog {blogName} used");
            }
            
            Console.WriteLine();
            Console.Write("Please enter a blog name that you would like to test if it follows you: ");

            string followerBlogName = Console.ReadLine();

            var isFollower = await tumblrClient.GetFollowedByAsync(blogName, followerBlogName);

            Console.WriteLine();

            if (isFollower)
            {
                Console.Write("Is a follower of your blog");
            }
            else
            {
                Console.Write("Is not a follower");
            }

            Console.WriteLine();
        }
    }
}
