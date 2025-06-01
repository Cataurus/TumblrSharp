using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using DontPanic.TumblrSharp.OAuth;
using ExampleShared;
using System;
using System.Threading.Tasks;

namespace Block
{
    class Program
    {
        static async Task Main()
        {
            Console.WriteLine("Example - Block method");
            Console.WriteLine("----------------------");

            // create TumblrClient
            TumblrClient tumblrClient = new TumblrClientFactory().Create<TumblrClient>(Settings.CONSUMER_KEY, Settings.CONSUMER_SECRET, new Token(Settings.OAUTH_TOKEN_KEY, Settings.OAUTH_TOKEN_SECRET));

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
                    Console.WriteLine(idx.ToString() + ". " + blog.Name);
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

            bool repeat = true;

            while (repeat)
            {
                int auswahl = DisplayMenü();

                switch (auswahl)
                {
                    case 1:
                        await DisplayBlockedBlogsAsync(tumblrClient, blogName);
                        break;

                    case 2:
                        await BlockABlockAsync(tumblrClient, blogName);
                        break;

                    case 3:
                        await RemoveBlockAsync(tumblrClient, blogName);
                        break;

                    default:
                        repeat = false;
                        break;
                }
            }            
        }

        private static async Task RemoveBlockAsync(TumblrClient tumblrClient, string blogName)
        {
            // block a Blogs
            Console.WriteLine("");
            Console.WriteLine("block a blog:");
            Console.WriteLine("---------------------");
            Console.WriteLine("");

            Console.Write("Input name of blocked blog: ");

            var blockedBlog = Console.ReadLine();

            Console.WriteLine("");

            try
            {
                await tumblrClient.RemoveBlock(blogName, blockedBlog);

                Console.WriteLine("success");
            }
            catch (TumblrException exp)
            {
                Console.WriteLine($"Error: {exp.Message}");
            }
        }

        private static async Task BlockABlockAsync(TumblrClient tumblrClient, string blogName)
        {
            // block a Blogs
            Console.WriteLine("");
            Console.WriteLine("block a blog:");
            Console.WriteLine("---------------------");
            Console.WriteLine("");

            Console.Write("Input a blogname: ");

            string blogNameToBlock = Console.ReadLine();

            Console.WriteLine("");

            try
            {
                await tumblrClient.SetBlock(blogName, blogNameToBlock);

                Console.WriteLine("success");
            }
            catch (TumblrException exp)
            {
                Console.WriteLine($"Error: {exp.Message}");
            }
            
        }

        private static async Task DisplayBlockedBlogsAsync(TumblrClient tumblrClient, string blogName)
        {
            Console.WriteLine("");
            Console.WriteLine("Display blocked blog:");
            Console.WriteLine("---------------------");
            Console.WriteLine("");

            var blockBlogs = await tumblrClient.GetBlock(blogName).ConfigureAwait(true);

            if (blockBlogs.Length == 0)
            {
                Console.WriteLine("no blocked blog");
            }
            else
            {
                int idx = 1;

                foreach (var blockBlog in blockBlogs)
                {
                    Console.WriteLine($"{idx}. {blockBlog.Name}");

                    idx++;
                }
            }
        }

        private static int DisplayMenü()
        {
            Console.WriteLine("");
            Console.WriteLine("blockmenue");
            Console.WriteLine("----------");
            
            Console.WriteLine("");

            Console.WriteLine("1. Display blocked blogs");
            Console.WriteLine("2. block a blog");
            Console.WriteLine("3. Move a blog from blocked list");
            Console.WriteLine("");
            Console.WriteLine("0 or other. exit");
            
            Console.WriteLine("");
            Console.Write("Input: ");

            int result;

            try
            {
                result = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception)
            {
                result = 0;
            }

            if (result > 3)
            {
                result = 0;
            }

            return result;
        }
    }
}
