using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using ExampleShared;
using System;
using System.Threading.Tasks;

namespace CreateTextPost
{
    public class Tumblr : TumblrBase
    {
        public async Task<string> FirstBlog()
        {
            var user = await client.GetUserInfoAsync();
            if (user.Blogs.Length == 0)
            {
                throw new Exception("No blogs found for this user.");
            }
            return user.Blogs[0].Name;
        }

        public async Task<PostCreationInfo> Post(string text)
        {
            //replace blogName with your blogname
            string blogName = await FirstBlog();

            var test = await client.CreatePostAsync(blogName, PostData.CreateText(text));

            return test;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Tumblr tb = new Tumblr();

            Console.WriteLine("Text posted:");
            Console.WriteLine();

            var text = Console.ReadLine();

            var test = tb.Post(text).GetAwaiter().GetResult();

            Console.WriteLine("Post ID: " + test.PostId.ToString());

            Console.ReadKey();
        }
    }
}
