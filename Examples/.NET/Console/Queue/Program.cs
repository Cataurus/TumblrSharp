using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using DontPanic.TumblrSharp.OAuth;
using ExampleShared;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Queue
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Queue Sample");
            Console.WriteLine("------------");
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
                Console.WriteLine("Error: You have in the sourcecode for this example to set the static fields for tokens and the name of your blog?");
                Console.WriteLine("");
                Console.ForegroundColor = ConsoleColor.White;

                return;
            }

            var userInfo = await tumblrClient.GetUserInfoAsync();
            var BLOGNAME = userInfo.Blogs[0].Name;

            Console.WriteLine("Create 10 Textpost in queue");
            Console.WriteLine("");

            for (int i = 1; i < 11; i++)
            {
                PostData textPost = PostData.CreateText($"this is a test message with number {i}", "testmessage", null, PostCreationState.Queue);
                var post = await tumblrClient.CreatePostAsync(BLOGNAME, textPost);

                Console.WriteLine($"post create with postid {post.PostId}");
            }

            Console.WriteLine("\nWaiting for 5 seconds...");
            Thread.Sleep(5000);

            await DisplayQueue(tumblrClient, BLOGNAME);

            Console.WriteLine("");
            Console.WriteLine("move item in queue");
            Console.WriteLine("--------------");
            Console.WriteLine("");

            try
            {
                Console.WriteLine("move 2. item to top");

                BasePost[] queueList = await tumblrClient.GetQueuedPostsAsync(BLOGNAME);

                await tumblrClient.QueueRecorder(BLOGNAME, queueList[1].Id);                                
            }
            catch (TumblrException exp)
            {
                Console.WriteLine($"Error: {exp.Message}");
            }

            try
            {
                Console.WriteLine("move 6. item to position 3");

                BasePost[] queueList = await tumblrClient.GetQueuedPostsAsync(BLOGNAME);

                await tumblrClient.QueueRecorder(BLOGNAME, queueList[5].Id, queueList[1].Id);
            }
            catch (Exception exp)
            {
                Console.WriteLine($"Error: {exp.Message}");
            }


            await DisplayQueue(tumblrClient, BLOGNAME);

            Console.WriteLine("");
            Console.WriteLine("shuffle queue");
            Console.WriteLine("--------------");
            Console.WriteLine("");

            try
            {
                await tumblrClient.QueueShuffle(BLOGNAME);
                Console.WriteLine("success");
            }
            catch (TumblrException exp)
            {
                Console.WriteLine($"Error: {exp.Message}");
            }

            await DisplayQueue(tumblrClient, BLOGNAME);

            Console.WriteLine("");
            Console.WriteLine("clear queue");
            Console.WriteLine("--------------");
            Console.WriteLine("");

            await ClearQueue(tumblrClient, BLOGNAME);

            tumblrClient.Dispose();
        }

        private static async Task ClearQueue(TumblrClient tumblrClient, string blogName)
        {
            long k = 0;

            BasePost[] queueList = await tumblrClient.GetQueuedPostsAsync(blogName, k);

            while (queueList.Length > 0)
            {
                foreach (var item in queueList)
                {
                    await tumblrClient.DeletePostAsync(blogName, item.Id);

                    Console.WriteLine($"post {item.Id} delete");
                }

                k += 20;

                queueList = await tumblrClient.GetQueuedPostsAsync(blogName, k);
            }

            
        }

        private static async Task DisplayQueue(TumblrClient tumblrClient, string blogName)
        {
            Console.WriteLine("");
            Console.WriteLine("Overview queue");
            Console.WriteLine("--------------");
            Console.WriteLine("");

            var queueList = await tumblrClient.GetQueuedPostsAsync(blogName);

            long k = 1;

            foreach (var item in queueList)
            {
                Console.WriteLine($"{k}. post with PostId: {item.Id} Body: {(item as TextPost).Body}");

                k++;
            }
        }
    }
}
