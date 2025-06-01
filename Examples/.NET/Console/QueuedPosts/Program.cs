using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using DontPanic.TumblrSharp.OAuth;
using ExampleShared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QueuedPosts
{
    public class Tumblr
    {
        private TumblrClient client = null;

        public Tumblr()
        {
            if (Settings.CONSUMER_KEY == "***")
            {
                Console.WriteLine("Change in sourcecode the consumerKey or better setting as environment variable, etc...!");
                Console.WriteLine();

                throw new Exception();
            }

            this.client = new TumblrClientFactory().Create<TumblrClient>(Settings.CONSUMER_KEY, Settings.CONSUMER_SECRET, new Token(Settings.OAUTH_TOKEN_KEY, Settings.OAUTH_TOKEN_SECRET));
        }

        public async Task<List<string>> GetBlog()
        {
            List<string> result = new List<string>();

            UserInfo userInfo = await client.GetUserInfoAsync();

            foreach (var blog in userInfo.Blogs)
            {
                result.Add(blog.Name);
            }

            return result;
        }

        public async Task<Int32> GetCountOfQueued(string blog)
        {
            Int32 result = 0;

            BasePost[] test;

            test = await client.GetQueuedPostsAsync(blog);

            while (test.Length == 20)
            {
                result += 20;

                test = await client.GetQueuedPostsAsync(blog);
            }

            result += test.Length;

            return result;
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            Tumblr tumblr = new Tumblr();

            var blogs = tumblr.GetBlog().GetAwaiter().GetResult();

            Console.WriteLine("Your blogs:");
            Console.WriteLine("");

            for (int i = 0; i < blogs.Count; i++)
            {
                Console.WriteLine($"   {i}. {blogs[i]} ");
            }

            Console.WriteLine("");

            Console.Write("Please select a blog: ");
            var blogIdx = Convert.ToInt32(Console.ReadLine());

            var count = tumblr.GetCountOfQueued(blogs[blogIdx]).GetAwaiter().GetResult();

            Console.WriteLine("");
            Console.WriteLine($"You have {count} posts in Queued");

            Console.ReadLine();

        }
    }
}
