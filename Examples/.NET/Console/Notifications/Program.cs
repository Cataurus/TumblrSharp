using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using DontPanic.TumblrSharp.OAuth;
using ExampleShared;
using System;
using System.Threading.Tasks;

namespace Notifications
{
    class Program
    {
        static async Task Main()
        {
            Console.WriteLine("Notification Sample");
            Console.WriteLine("-------------------");
            Console.WriteLine("");

            // create TumblrClient
            TumblrClient tumblrClient;

            try
            {
                tumblrClient = new TumblrClientFactory().Create<TumblrClient>(Settings.CONSUMER_KEY, Settings.CONSUMER_SECRET, new Token(Settings.OAUTH_TOKEN_KEY, Settings.OAUTH_TOKEN_SECRET));
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Has the static fields for the consumer token been set in the source code of this example?");
                Console.WriteLine("");
                Console.ForegroundColor = ConsoleColor.White;

                return;
            }

            // identify the name of a blog
            string blogName;

            var userInfo = await tumblrClient.GetUserInfoAsync();

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

            // get notifications

            Console.WriteLine();
            Console.WriteLine("for all types");
            Console.WriteLine("-------------");
            Console.WriteLine();

            var notifications = await tumblrClient.GetNotifications(blogName);

            while (notifications.Length > 0)
            {
                DateTime before = DateTime.Now;

                foreach (var item in notifications)
                {
                    before = item.TimeStamp;

                    Console.WriteLine($"{item.TimeStamp} {item.Type} TargetPostID: {item.TargetPostId} PostId: {item.PostId}");
                }

                notifications = await tumblrClient.GetNotifications(blogName, before);
            }

            Console.WriteLine();
            Console.WriteLine("not more found");

            Console.WriteLine();
            Console.WriteLine("for like");
            Console.WriteLine("-------------");
            Console.WriteLine();

            notifications = await tumblrClient.GetNotifications(blogName, NotificationsTypes.Like);

            while (notifications.Length > 0)
            {
                DateTime before = DateTime.Now;

                foreach (var item in notifications)
                {
                    before = item.TimeStamp;

                    Console.WriteLine($"{item.TimeStamp} {item.PostType} {item.FromTumblrLogName} {item.MediaUrl}");
                }

                notifications = await tumblrClient.GetNotifications(blogName, before, NotificationsTypes.Like);
            }

            Console.WriteLine();
            Console.WriteLine("not more found");

            Console.WriteLine();
            Console.WriteLine("for new follower");
            Console.WriteLine("-------------");
            Console.WriteLine();

            notifications = await tumblrClient.GetNotifications(blogName, NotificationsTypes.Follower);

            while (notifications.Length > 0)
            {
                DateTime before = DateTime.Now;

                foreach (var item in notifications)
                {
                    before = item.TimeStamp;

                    Console.WriteLine($"{item.TimeStamp} {item.Type} PostId: {item.PostId}");
                }

                notifications = await tumblrClient.GetNotifications(blogName, before, NotificationsTypes.Follower);
            }

            Console.WriteLine();
            Console.WriteLine("not more found");

            tumblrClient.Dispose();
        }
    }
}
