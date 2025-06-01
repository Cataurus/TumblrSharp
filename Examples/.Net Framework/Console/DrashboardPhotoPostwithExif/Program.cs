using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using ExampleShared;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DashboardPhotoPostwithExif
{
    class Program
    {
        public class Tumblr : TumblrBase
        {
            private long current = 0;
            
            public async Task<PhotoPost> GetDrashBoardPostAsync()
            {
                PhotoPost result = null;

                bool PhotoPostwithExifGefunden = false;

                while (!PhotoPostwithExifGefunden)
                {
                    BasePost[] BasePosts = await client.GetDashboardPostsAsync(current, 0, 20, PostType.Photo);

                    foreach (var basePost in BasePosts)
                    {
                        if (basePost.Type == PostType.Photo)
                        {
                            PhotoPost photoPost = basePost as PhotoPost;

                            Console.Clear();
                            Console.WriteLine($"Search {photoPost.Id}");

                            if (photoPost.Photo.Exif != null)
                            {
                                PhotoPostwithExifGefunden = true;
                                result = photoPost;
                                break;
                            }
                        }
                    }

                    if (BasePosts.Count() > 0)
                        current = BasePosts[BasePosts.Count() - 1].Id;

                }

                return result;
            }
        }

        static void Main()
        {
            Tumblr tumblr = new Tumblr();

            PhotoPost photoPost = tumblr.GetDrashBoardPostAsync().GetAwaiter().GetResult();

            Console.WriteLine(photoPost.Photo.Caption);
            Console.WriteLine(photoPost.Photo.OriginalSize.ImageUrl);
            
            Console.WriteLine("Camera: " + photoPost.Photo.Exif.Camera);
            Console.WriteLine("Exposure: " + photoPost.Photo.Exif.Exposure);
            Console.WriteLine("Aperture: " + photoPost.Photo.Exif.Aperture);
            Console.WriteLine("Iso: " + photoPost.Photo.Exif.ISO);
            Console.WriteLine("FocalLength: " + photoPost.Photo.Exif.FocalLength);

            Console.ReadKey();
        }
    }
}
