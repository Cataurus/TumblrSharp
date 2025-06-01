using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using DontPanic.TumblrSharp.OAuth;
using ExampleShared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilteredContent
{
    class Program
    {
        static async Task Main()
        {
            Console.WriteLine("Filtered Content Sample");
            Console.WriteLine("-----------------------");
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

            int selectedMenu = -1;

            while (selectedMenu == -1)
            {
                selectedMenu = DisplayMenuAndSelect();

                switch (selectedMenu)
                {
                    case 0:
                        break;
                    case 1:
                        await DisplayFilteredContents(tumblrClient).ConfigureAwait(true);
                        selectedMenu = -1;
                        break;
                    case 2:
                        await SetAFilter(tumblrClient).ConfigureAwait(true);
                        selectedMenu = -1;
                        break;
                    case 3:
                        await SetAFilterList(tumblrClient).ConfigureAwait(true);
                        selectedMenu = -1;
                        break;
                    case 4:
                        await DeleteFilter(tumblrClient).ConfigureAwait(true);
                        selectedMenu = -1;
                        break;
                    case 5:
                        await DeleteAllFilter(tumblrClient);
                        selectedMenu = -1;
                        break;
                }
            }
        }

        private static async Task SetAFilter(TumblrClient tumblrClient)
        {
            Console.WriteLine("");
            Console.WriteLine("add as string:");
            Console.WriteLine("**************");
            Console.WriteLine("");

            string newFilteredContent = InputAFilter();

            try
            {
                await tumblrClient.SetFilteredContentAsync(newFilteredContent).ConfigureAwait(true);
                DisplayOperationSuccess();
            }
            catch (TumblrException exp)
            {
                DisplayNoSuccess(exp);
            }
        }

        private static async Task SetAFilterList(TumblrClient tumblrClient)
        {
            Console.WriteLine("add a List of filter:");
            Console.WriteLine("*********************");

            List<string> filteredContentsToSet = [];

            while (true)
            {
                string newFilteredContent = InputAFilter();
                if (string.IsNullOrEmpty(newFilteredContent))
                {
                    break;
                }
                filteredContentsToSet.Add(newFilteredContent);
            }

            try
            {
                await tumblrClient.SetFilteredContentAsync(filteredContentsToSet).ConfigureAwait(true);

                DisplayOperationSuccess();
            }
            catch (TumblrException exp)
            {
                DisplayNoSuccess(exp);
            }
        }

        private static async Task DeleteFilter(TumblrClient tumblrClient)
        {
            Console.WriteLine();
            Console.WriteLine($"Remove a spezific filter");
            Console.WriteLine("*************************");

            try
            {
                var item  = InputAFilter();

                await tumblrClient.DeleteFilteredContent(item).ConfigureAwait(true);

                DisplayOperationSuccess();
            }
            catch (TumblrException exp)
            {
                DisplayNoSuccess(exp);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
        }

        private static async Task DeleteAllFilter(TumblrClient tumblrClient)
        {
            Console.WriteLine();
            Console.WriteLine($"Delete filtered content: All");
            Console.WriteLine("****************************");

            try
            {
                var filteredContents = await tumblrClient.GetFilteredContent().ConfigureAwait(true);

                foreach (var item in filteredContents)
                {
                    await tumblrClient.DeleteFilteredContent(item).ConfigureAwait(true);
                }

                DisplayOperationSuccess();
            }
            catch (TumblrException exp)
            {
                DisplayNoSuccess(exp);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
        }

        private static int DisplayMenuAndSelect()
        {
            int result = -1;

            Console.WriteLine();
            Console.WriteLine("filteredContent - menu");
            Console.WriteLine("----------------------");
            Console.WriteLine();

            Console.WriteLine("1. View filtered content");
            Console.WriteLine("2. Add a filter");
            Console.WriteLine("3. Add a List to filtered content");
            Console.WriteLine("4. Delete a filter");
            Console.WriteLine("5. Delete all Filter");
            Console.WriteLine();

            Console.WriteLine("0. Exit");
            Console.WriteLine();

            Console.Write("Input Your select: ");

            var input = Console.ReadLine();

            switch (input)
            {
                case "1" :
                    {
                        result = 1;
                        break;
                    }
                case "2":
                    {
                        result = 2;
                        break;
                    }
                case "3":
                    {
                        result = 3;
                        break;
                    }
                case "4":
                    {
                        result = 4;
                        break;
                    }
                case "5":
                    {
                        result = 5;
                        break;
                    }
                case "0":
                    {
                        result = 0;
                        break;
                    }
                default:
                    break;
            }

            return result;
        }

        public static string InputAFilter()
        {
            Console.WriteLine();
            Console.Write("Input a filter (empty => break): ");

            string newFilteredContent = Console.ReadLine();

            return newFilteredContent;
        }

        private static async Task DisplayFilteredContents(TumblrClient tumblrClient)
        {   
            var filteredContents = await tumblrClient.GetFilteredContent().ConfigureAwait(true);

            Console.WriteLine();
            Console.WriteLine("Current filtered Content:");
            Console.WriteLine("*************************");
            Console.WriteLine();

            if (filteredContents.Length == 0)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("empty");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                foreach (var filteredContent in filteredContents)
                {
                    Console.WriteLine(filteredContent);
                }
            }

            Console.WriteLine();
        }

        private static void DisplayNoSuccess(TumblrException exp)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Operation not success - Error: {exp.Message}");
            foreach (var item in exp.Errors)
            {
                Console.WriteLine($"Details - {item.Detail}");
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void DisplayOperationSuccess()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Operation success");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
