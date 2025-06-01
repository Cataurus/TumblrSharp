using System.Threading.Tasks;

namespace AzureFunction
{
    public interface IMyTumblrService
    {
        Task<string> GetUser();

        Task<string> GetBlog(string blogName);
    }
}