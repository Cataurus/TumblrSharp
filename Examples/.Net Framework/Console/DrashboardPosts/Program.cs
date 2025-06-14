﻿using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using ExampleShared;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DashboardPosts
{

    public class Tumblr : TumblrBase
    {
        private long current = 0;

        public async Task<BasePost[]> GetDrashBoardPostAsync()
        {
            BasePost[] result;

            result = await client.GetDashboardPostsAsync(current, 0, 20, PostType.All, false, true);

            if (result.Count() > 0)
            current = result[result.Count() - 1].Id;

            return result;
        }

    }

    class Program
    {
        static void Main()
        {
            Tumblr tumblr = new Tumblr();

            bool cancel = false;

            int k = 1;

            while (!cancel)
            {
                BasePost[] basePosts = tumblr.GetDrashBoardPostAsync().GetAwaiter().GetResult();                

                foreach (var basePost in basePosts)
                {
                    Console.WriteLine($"{k}. Post from {basePost.BlogName} with ID {basePost.Id}");

                    if (basePost.Trails.Count() > 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"--- Post has {basePost.Trails.Count()} trail ---");
                        Console.WriteLine();
                    }

                    int i = 1;

                    foreach (var trail in basePost.Trails)
                    {
                        Console.WriteLine($"   {i}. trail from {trail.Blog.Name}");
                        Console.WriteLine($"   {trail.Content}");
                        Console.WriteLine();

                        i++;
                    }

                    if (basePost.Notes?.Count() > 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"--- Post has {basePost.Notes.Count()} Notes ---");
                        Console.WriteLine();

                        foreach (var note in basePost.Notes)
                        {
                            Console.WriteLine($"   {i}. note from {note.BlogName}");
                            Console.WriteLine();

                            i++;
                        }
                    }

                    Console.WriteLine();

                    k++;
                }

                Console.Write("next posts? (*/n)");

                var key = Console.ReadKey();

                Console.WriteLine();

                if (key.KeyChar == 'n')
                {
                    cancel = true;
                }
            }
        }
    }
}
