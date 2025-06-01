using DontPanic.TumblrSharp.OAuth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DontPanic.TumblrSharp.Client
{
    /// <summary>
    /// Encapsulates the Tumblr API.
    /// </summary>
    public class TumblrClient : TumblrClientBase
    {
        private bool disposed;
        private readonly string apiKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="TumblrClient"/> class.
        /// </summary>
        /// <param name="hashProvider">
        /// A <see cref="IHmacSha1HashProvider"/> implementation used to generate a
        /// HMAC-SHA1 hash for OAuth purposes.
        /// </param>
        /// <param name="consumerKey">
        /// The consumer key.
        /// </param>
        /// <param name="consumerSecret">
        /// The consumer secret.
        /// </param>
        /// <param name="oAuthToken">
        /// An optional access token for the API. If no access token is provided, only the methods
        /// that do not require OAuth can be invoked successfully.
        /// </param>
        /// <remarks>
        ///  You can get a consumer key and a consumer secret by registering an application with Tumblr:<br/>
        /// <br/>
        /// http://www.tumblr.com/oauth/apps
        /// <br/><br/>platform: .Net Standard 2.0, .Net Core 2.2+
        /// </remarks>
        public TumblrClient(IHmacSha1HashProvider hashProvider, string consumerKey, string consumerSecret, Token oAuthToken = null) : base(hashProvider, consumerKey, consumerSecret, oAuthToken)
        {
            this.apiKey = consumerKey;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TumblrClient"/> class.
        /// </summary>
        /// <param name="httpClientFactory">
        /// <see cref="IHttpClientFactory">HttpClientFactory</see> to create internal <see cref="HttpClient">HttpClient</see>
        /// </param>
        /// <param name="hashProvider">
        /// A <see cref="IHmacSha1HashProvider"/> implementation used to generate a
        /// HMAC-SHA1 hash for OAuth purposes.
        /// </param>
        /// <param name="consumerKey">
        /// The consumer key.
        /// </param>
        /// <param name="consumerSecret">
        /// The consumer secret.
        /// </param>
        /// <param name="oAuthToken">
        /// An optional access token for the API. If no access token is provided, only the methods
        /// that do not require OAuth can be invoked successfully.
        /// </param>
        /// <remarks>
        /// You can get a consumer key and a consumer secret by registering an application with Tumblr App-Registration:<br/> http://www.tumblr.com/oauth/apps 
        /// <br/><br/>platform: .Net Standard 2.0+, .Net Core 2.2+
        /// </remarks>
        public TumblrClient(IHttpClientFactory httpClientFactory, IHmacSha1HashProvider hashProvider, string consumerKey, string consumerSecret, Token oAuthToken = null)
            : base(httpClientFactory, hashProvider, consumerKey, consumerSecret, oAuthToken)
        {
            this.apiKey = consumerKey;
        }

        #region Public Methods

        #region Blog Methods

        #region Block

        /// <summary>
        /// Get the blogs that the requested blog is currently blocking
        /// </summary>
        /// <param name="blogName">name of the blog, you must be an admin </param>
        /// <param name="startIndex">Block number to start at</param>
        /// <param name="count">The number of blocks to retrieve, 1-20, inclusive</param>
        /// <returns>a task of enumerable list of <see cref="BlogBase"/></returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="blogName"/> is <b>null</b>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="blogName"/> is empty.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <list type="bullet">
        /// <item>
        ///		<description>
        ///			<paramref name="startIndex"/> is less than 0.
        ///		</description>
        ///	</item>
        ///	<item>
        ///		<description>
        ///			<paramref name="count"/> is less than 1 or greater than 20.
        ///		</description>
        ///	</item>
        /// </list>
        /// </exception>
        /// <example>
        /// This example lists all the names of the blogs that have been blocked.
        /// <code>
        ///     BlogBase[] blogs;
        ///     
        ///     do
        ///     {
        ///         blogs = await tumblrClient.GetBlog("NameOfYourBlog");
        ///    
        ///         foreach (var blog in blogs)
        ///         {
        ///             Console.WriteLine(blog.Name);
        ///         }
        ///     }
        ///     while ( blogs.Length == 20 )
        /// </code>
        /// </example>
        public Task<BlogBase[]> GetBlock(string blogName, int startIndex = 0, int count = 20)
        {
            if (blogName == null)
                throw new ArgumentNullException(nameof(blogName));

            if (blogName.Length == 0)
                throw new ArgumentException("Blog name cannot be empty.", nameof(blogName));

            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex), "startIndex must be greater or equal to zero.");

            if (count < 1 || count > 20)
                throw new ArgumentOutOfRangeException(nameof(count), "count must be between 1 and 20.");

            MethodParameterSet parameters = new MethodParameterSet
            {
                { "offset", startIndex, 0 },
                { "limit", count, 20 }
            };

            return CallApiMethodAsync<BlocksResponse, BlogBase[]>(
              new BlogMethod(blogName, "blocks", OAuthToken, HttpMethod.Get, parameters),
              r => r.Blogs,
              CancellationToken.None);
        }

        /// <summary>
        /// Block a Blog.
        /// Note that this endpoint is rate limited to 60 requests per minute.
        /// </summary>
        /// 
        /// <param name="blogName">name of the blog, you must be an admin</param>
        /// <param name="toBlockedBlogName">The tumblelog to block, specified by any blog identifier</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <list type="bullet">
        /// <item>
        ///		<description>
        ///			<paramref name="blogName"/> is <b>null</b>.
        ///		</description>
        ///	</item>
        ///	<item>
        ///		<description>
        ///			<paramref name="toBlockedBlogName"/> may be null
        ///		</description>
        ///	</item>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <list type="bullet">
        ///	<item>
        ///		<description>
        ///         <paramref name="blogName"/> is empty.
        ///     </description>
        ///	</item>
        ///	<item>
        ///		<description>
        ///			<paramref name="toBlockedBlogName"/> is empty
        ///		</description>
        ///	</item>
        /// </list>
        /// </exception>
        /// 
        /// <example>
        ///     <code>
        ///         string nameOfTheBlogToBolocked = "UnsolicitedBlog";
        ///         
        ///         try
        ///         {
        ///             await tumblrClient.SetBlock("NameOfYourBlog", nameOfTheBlogToBolocked);
        ///         }
        ///         except (TumblrException exp)
        ///         {
        ///             Console.WriteLine($"The operation could not be completed: {exp.msg}");
        ///         }
        ///         
        ///         Console.WriteLine("Success");
        ///     </code>
        /// </example>
        public Task SetBlock(string blogName, string toBlockedBlogName)
        {
            if (blogName == null)
                throw new ArgumentNullException(nameof(blogName));

            if (blogName.Length == 0)
                throw new ArgumentException("Blog name cannot be empty", nameof(blogName));

            if (toBlockedBlogName == null)
            {
                throw new ArgumentNullException(nameof(toBlockedBlogName), $"{nameof(toBlockedBlogName)} is null");
            }

            if (toBlockedBlogName.Length == 0)
            {
                throw new ArgumentException($"when {nameof(toBlockedBlogName)} is empty", nameof(toBlockedBlogName));
            }

            toBlockedBlogName = BlogMethod.Validate(toBlockedBlogName);

            MethodParameterSet parameters = new MethodParameterSet
            {
                { "blocked_tumblelog", toBlockedBlogName, "" }
            };

            return CallApiMethodNoResultAsync(
                    new BlogMethod(blogName, "blocks", OAuthToken, HttpMethod.Post, parameters),
                    CancellationToken.None);
        }

        /// <summary>
        /// Block a Blog.
        /// Note that this endpoint is rate limited to 60 requests per minute.
        /// </summary>
        /// 
        /// <param name="blogName">name of the blog, you must be an admin</param>
        /// <param name="postID">The anonymous post ID (asks, submissions) to block</param>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <list type="bullet">
        /// <item>
        ///		<description>
        ///			<paramref name="blogName"/> is <b>null</b>.
        ///		</description>
        ///	</item>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <list type="bullet">
        ///	<item>
        ///		<description>
        ///         <paramref name="blogName"/> is empty.
        ///     </description>
        ///	</item>
        ///	<item>
        ///		<description>
        ///			<paramref name="postID"/> must be greater as 0
        ///		</description>
        ///	</item>
        /// </list>
        /// </exception>
        /// 
        /// <example>
        ///     <code>         
        ///         try
        ///         {
        ///             await tumblrClient.SetBlock("NameOfYourBlog", 122563);
        ///         }
        ///         except (TumblrException exp)
        ///         {
        ///             Console.WriteLine($"The operation could not be completed: {exp.msg}");
        ///         }
        ///         
        ///         Console.WriteLine("Success");
        ///     </code>
        /// </example>
        public Task SetBlock(string blogName, long postID)
        {
            if (blogName == null)
                throw new ArgumentNullException(nameof(blogName));

            if (postID <= 0)
            {
                throw new ArgumentException($"{nameof(postID)} must be greater as 0", nameof(postID));
            }

            MethodParameterSet parameters = new MethodParameterSet
            {
                { "post_id", postID, 0 }
            };

            return CallApiMethodNoResultAsync(
                    new BlogMethod(blogName, "blocks", OAuthToken, HttpMethod.Post, parameters),
                    CancellationToken.None);
        }

        /// <summary>
        /// Remove blocks.
        /// Note that this endpoint is rate limited to 60 requests per minute.
        /// </summary>
        /// <param name="blogName">name of the blog, you must be an admin</param>
        /// <param name="blockedBlogName">The tumblelog whose block to remove, specified by any blog identifier. Is parameter null or empty, this will clear all anonymous IP blocks.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="blogName"/> is <b>null</b>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="blogName"/> is empty.
        /// </exception>
        /// 
        /// <example>
        /// 
        /// removes the block for the blog "UnsolicitedBlog"
        /// 
        ///     <code>
        ///         string nameOfTheBlogToBolocked = "UnsolicitedBlog";
        ///         
        ///         try
        ///         {
        ///             await tumblrClient.RemoveBlock("YourBlog", nameOfTheBlogToBolocked);
        ///         }
        ///         except (Exception exp)
        ///         {
        ///             Console.WriteLine($"The operation could not be completed: {exp.msg}");
        ///         }
        ///         
        ///         Console.WriteLine("Success");
        ///     </code>
        ///     
        /// clear all anonymous IP blocks
        /// 
        ///     <code>
        ///         try
        ///         {
        ///             await tumblrClient.RemoveBlock("NameOfYourBlog");
        ///         }
        ///         except (TumblrException exp)
        ///         {
        ///             Console.WriteLine($"The operation could not be completed: {exp.msg}");
        ///         }
        ///         
        ///         Console.WriteLine("Success");
        ///     </code>
        /// </example>
        public Task RemoveBlock(string blogName, string blockedBlogName = null)
        {
            if (blogName == null)
                throw new ArgumentNullException(nameof(blogName));

            if (blogName.Length == 0)
                throw new ArgumentException("Blog name cannot be empty.", nameof(blogName));

            blockedBlogName = BlogMethod.Validate(blockedBlogName);

            MethodParameterSet parameters = new MethodParameterSet
            {
                { "blocked_tumblelog", blockedBlogName, "" }
            };

            if (string.IsNullOrEmpty(blockedBlogName))
            {
                parameters.Add("anonymous_only", true);
            }

            return CallApiMethodNoResultAsync(
                    new BlogMethod(blogName, "blocks", OAuthToken, HttpMethod.Post, parameters),
                    CancellationToken.None);
        }

        #endregion

        #region GetBlogInfoAsync

        /// <summary>
        /// Asynchronously retrieves general information about the blog, such as the title, number of posts, and other high-level data.
        /// </summary>
        /// <remarks>
        /// See: http://www.tumblr.com/docs/en/api/v2#blog-info.
        /// </remarks>
        /// <param name="blogName">
        /// The name of the blog.
        /// </param>
        /// <returns>
        /// A Task&lt;<see cref="BlogInfo"/>&gt; that can be used to track the operation. If the task succeeds, the <see cref="Task{BlogInfo}.Result"/> will
        /// carry a <see cref="BlogInfo"/> instance. Otherwise <see cref="Task.Exception"/> will carry a <see cref="TumblrException"/>
        /// representing the error occurred during the call.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object has been disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="blogName"/> is <b>null</b>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="blogName"/> is empty.
        /// </exception>
        public Task<BlogInfo> GetBlogInfoAsync(string blogName)
        {
            if (disposed)
                throw new ObjectDisposedException("TumblrClient");

            if (blogName == null)
                throw new ArgumentNullException("blogName");

            if (blogName.Length == 0)
                throw new ArgumentException("Blog name cannot be empty.", "blogName");

            MethodParameterSet parameters = new MethodParameterSet();
            parameters.Add("api_key", apiKey);

            return CallApiMethodAsync<BlogInfoResponse, BlogInfo>(
              new BlogMethod(blogName, "info", OAuthToken, HttpMethod.Get, parameters),
              r => r.Blog,
              CancellationToken.None);
        }

        #endregion

        #region GetBlogLikesAsync

        /// <summary>
        /// Asynchronously retrieves the publicly exposed likes from a blog.
        /// </summary>
        /// <remarks>
        /// See: http://www.tumblr.com/docs/en/api/v2#blog-likes
        /// </remarks>
        /// <param name="blogName">
        /// The name of the blog.
        /// </param>
        /// <param name="startIndex">
        /// The offset at which to start retrieving the likes. Use 0 to start retrieving from the latest like.
        /// </param>
        /// <param name="count">
        /// The number of likes to retrieve. Must be between 1 and 20.
        /// </param>
        /// <param name="before">
        /// The timestamp before when to retrieve likes. 
        /// </param>
        /// <param name="after">
        /// The timestamp after when to retrieve likes. 
        /// </param>
        /// <returns>
        /// A <see cref="Task{Likes}"/> that can be used to track the operation. If the task succeeds, the <see cref="Task{Likes}.Result"/> will
        /// carry a <see cref="Likes"/> instance. Otherwise <see cref="Task.Exception"/> will carry a <see cref="TumblrException"/>
        /// representing the error occurred during the call.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object has been disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="blogName"/> is <b>null</b>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="blogName"/> is empty.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <list type="bullet">
        /// <item>
        ///		<description>
        ///			<paramref name="startIndex"/> is less than 0.
        ///		</description>
        ///	</item>
        ///	<item>
        ///		<description>
        ///			<paramref name="count"/> is less than 1 or greater than 20.
        ///		</description>
        ///	</item>
        /// </list>
        /// </exception>
        public Task<Likes> GetBlogLikesAsync(string blogName, int startIndex = 0, int count = 20, DateTime? before = null, DateTime? after = null)
        {
            if (disposed)
                throw new ObjectDisposedException("TumblrClient");

            if (blogName == null)
                throw new ArgumentNullException("blogName");

            if (blogName.Length == 0)
                throw new ArgumentException("Blog name cannot be empty.", "blogName");

            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "startIndex must be greater or equal to zero.");

            if (count < 1 || count > 20)
                throw new ArgumentOutOfRangeException("count", "count must be between 1 and 20.");

            MethodParameterSet parameters = new MethodParameterSet
            {
                { "api_key", apiKey },
                { "offset", startIndex, 0 },
                { "limit", count, 0 },
                { "before", before.HasValue ? DateTimeHelper.ToTimestamp(before.Value).ToString() : null, null },
                { "after", after.HasValue ? DateTimeHelper.ToTimestamp(after.Value).ToString() : null, null }
            };

            return CallApiMethodAsync<Likes>(
              new BlogMethod(blogName, "likes", null, HttpMethod.Get, parameters),
              CancellationToken.None);
        }

        #endregion

        #region Posts

        #region GetPostsAsync

        /// <summary>
        /// Asynchronously retrieves published posts from a blog.
        /// </summary>
        /// <remarks>
        /// See: http://www.tumblr.com/docs/en/api/v2#posts
        /// </remarks>
        /// <param name="blogName">
        /// The name of the blog.
        /// </param>
        /// <param name="startIndex">
        /// The offset at which to start retrieving the posts. Use 0 to start retrieving from the latest post.
        /// </param>
        /// <param name="count">
        /// The number of posts to retrieve. Must be between 1 and 20.
        /// </param>
        /// <param name="type">
        /// The <see cref="PostType"/> to retrieve.
        /// </param>
        /// <param name="includeReblogInfo">
        /// Whether or not to include reblog info with the posts.
        /// </param>
        /// <param name="includeNotesInfo">
        /// Whether or not to include notes info with the posts.
        /// </param>
        /// <param name="filter">
        /// A <see cref="PostFilter"/> to apply.
        /// </param>
        /// <param name="tag">
        /// A tag to filter by.
        /// </param>
        /// <returns>
        /// A <see cref="Task{Posts}"/> that can be used to track the operation. If the task succeeds, the <see cref="Task{Posts}.Result"/> will
        /// carry a <see cref="Posts"/> instance. Otherwise <see cref="Task.Exception"/> will carry a <see cref="TumblrException"/>
        /// representing the error occurred during the call.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object has been disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="blogName"/> is <b>null</b>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="blogName"/> is empty.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <list type="bullet">
        /// <item>
        ///		<description>
        ///			<paramref name="startIndex"/> is less than 0.
        ///		</description>
        ///	</item>
        ///	<item>
        ///		<description>
        ///			<paramref name="count"/> is less than 1 or greater than 20.
        ///		</description>
        ///	</item>
        /// </list>
        /// </exception>
        public Task<Posts> GetPostsAsync(string blogName, long startIndex = 0, int count = 20, PostType type = PostType.All, bool includeReblogInfo = false, bool includeNotesInfo = false, PostFilter filter = PostFilter.Html, string tag = null)
        {
            if (disposed)
                throw new ObjectDisposedException("TumblrClient");

            if (blogName == null)
                throw new ArgumentNullException("blogName");

            if (blogName.Length == 0)
                throw new ArgumentException("Blog name cannot be empty.", "blogName");

            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "startIndex must be greater or equal to zero.");

            if (count < 1 || count > 20)
                throw new ArgumentOutOfRangeException("count", "count must be between 1 and 20.");

            string methodName;
            switch (type)
            {
                case PostType.Text: methodName = "posts/text"; break;
                case PostType.Quote: methodName = "posts/quote"; break;
                case PostType.Link: methodName = "posts/link"; break;
                case PostType.Answer: methodName = "posts/answer"; break;
                case PostType.Video: methodName = "posts/video"; break;
                case PostType.Audio: methodName = "posts/audio"; break;
                case PostType.Photo: methodName = "posts/photo"; break;
                case PostType.Chat: methodName = "posts/chat"; break;
                case PostType.All:
                default: methodName = "posts"; break;
            }

            MethodParameterSet parameters = new MethodParameterSet();
            parameters.Add("api_key", apiKey);
            parameters.Add("offset", startIndex, 0);
            parameters.Add("limit", count, 0);
            parameters.Add("reblog_info", includeReblogInfo, false);
            parameters.Add("notes_info", includeNotesInfo, false);
            parameters.Add("filter", filter.ToString().ToLowerInvariant(), "html");
            parameters.Add("tag", tag);

            return CallApiMethodAsync<Posts>(
              new BlogMethod(blogName, methodName, null, HttpMethod.Get, parameters),
              CancellationToken.None);
        }

        #endregion

        #region GetPostAsync

        /// <summary>
        /// Asynchronously retrieves a specific post by id.
        /// </summary>
        /// <param name="blogName">
        /// Blog name to reference
        /// </param>
        /// <param name="id">
        /// The id of the post to retrieve.
        /// </param>
        /// <param name="includeReblogInfo">
        /// Whether or not to include reblog info with the posts.
        /// </param>
        /// <param name="includeNotesInfo">
        /// Whether or not to include notes info with the posts.
        /// </param>
        /// <returns>
        /// A <see cref="Task{BasePost}"/> that can be used to track the operation. If the task succeeds, the <see cref="Task{BasePost}.Result"/> will
        /// carry a <see cref="BasePost"/> instance representing the desired post. Otherwise <see cref="Task.Exception"/> will carry a 
        /// <see cref="TumblrException"/> if the post with the specified id cannot be found.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object has been disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="blogName"/> is <b>null</b>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="blogName"/> is empty.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        ///	<paramref name="id"/> is less than 0.
        /// </exception>
        public Task<BasePost> GetPostAsync(string blogName, long id, bool includeReblogInfo = false, bool includeNotesInfo = false)
        {
            if (disposed)
                throw new ObjectDisposedException("TumblrClient");

            if (blogName == null)
                throw new ArgumentNullException("blogName");

            if (blogName.Length == 0)
                throw new ArgumentException("Blog name cannot be empty.", "blogName");

            if (id < 0)
                throw new ArgumentOutOfRangeException("id", "id must be greater or equal to zero.");

            MethodParameterSet parameters = new MethodParameterSet();
            parameters.Add("api_key", apiKey);
            parameters.Add("id", id, 0);
            parameters.Add("reblog_info", includeReblogInfo, false);
            parameters.Add("notes_info", includeNotesInfo, false);

            return CallApiMethodAsync<Posts, BasePost>(
              new BlogMethod(blogName, "posts", null, HttpMethod.Get, parameters),
              p => p.Result.FirstOrDefault(),
              CancellationToken.None);
        }

        #endregion

        #region DeletePostAsync

        /// <summary>
        /// Asynchronously deletes a post.
        /// </summary>
        /// <remarks>
        /// See: http://www.tumblr.com/docs/en/api/v2#deleting-posts
        /// </remarks>
        /// <param name="blogName">
        /// The name of the blog to which the post to delete belongs.
        /// </param>
        /// <param name="postId">
        /// The identifier of the post to delete.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to track the operation. If the task fails, <see cref="Exception"/> 
        /// will carry a <see cref="TumblrException"/> representing the error occurred during the call.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object has been disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="blogName"/> is <b>null</b>.
        ///	</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="blogName"/> is empty.
        ///	</exception>
        ///	<exception cref="System.ArgumentOutOfRangeException">
        ///	<paramref name="postId"/> is less than 0.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// This <see cref="TumblrClient"/> instance does not have an OAuth token specified.
        /// </exception>
        public Task DeletePostAsync(string blogName, long postId)
        {
            if (disposed)
                throw new ObjectDisposedException("TumblrClient");

            if (blogName == null)
                throw new ArgumentNullException("blogName");

            if (blogName.Length == 0)
                throw new ArgumentException("Blog name cannot be empty.", "blogName");

            if (postId < 0)
                throw new ArgumentOutOfRangeException("postId", "Post ID must be greater or equal to zero.");

            if (OAuthToken == null)
                throw new InvalidOperationException("DeletePostAsync method requires an OAuth token to be specified.");

            MethodParameterSet parameters = new MethodParameterSet();
            parameters.Add("id", postId);

            return CallApiMethodNoResultAsync(
              new BlogMethod(blogName, "post/delete", OAuthToken, HttpMethod.Post, parameters),
              CancellationToken.None);
        }

        #endregion

        #region CreatePostAsync

        /// <summary>
        /// Asynchronously creates a new post.
        /// </summary>
        /// <remarks>
        /// See: http://www.tumblr.com/docs/en/api/v2#posting
        /// </remarks>
        /// <param name="blogName">
        /// The name of the blog where to post to (must be one of the current user's blogs).
        /// </param>
        /// <param name="postData">
        /// The data that represents the type of post to create. See <see cref="PostData"/> for how
        /// to create various post types.
        /// </param>
        /// <returns>
        /// A <see cref="Task{PostCreationInfo}"/> that can be used to track the operation. If the task succeeds, the <see cref="Task{PostCreationInfo}.Result"/> will
        /// carry a <see cref="PostCreationInfo"/> instance. Otherwise <see cref="Task.Exception"/> will carry a <see cref="TumblrException"/>
        /// representing the error occurred during the call.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object has been disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <list type="bullet">
        /// <item>
        ///		<description>
        ///			<paramref name="blogName"/> is <b>null</b>.
        ///		</description>
        ///	</item>
        ///	<item>
        ///		<description>
        ///			<paramref name="postData"/> is <b>null</b>.
        ///		</description>
        ///	</item>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="blogName"/> is empty.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// This <see cref="TumblrClient"/> instance does not have an OAuth token specified.
        /// </exception>
        public Task<PostCreationInfo> CreatePostAsync(string blogName, PostData postData)
        {
            return CreatePostAsync(blogName, postData, CancellationToken.None);
        }

        /// <summary>
        /// Asynchronously creates a new post.
        /// </summary>
        /// <remarks>
        /// See: http://www.tumblr.com/docs/en/api/v2#posting
        /// </remarks>
        /// <param name="blogName">
        /// The name of the blog where to post to (must be one of the current user's blogs).
        /// </param>
        /// <param name="postData">
        /// The data that represents the type of post to create. See <see cref="PostData"/> for how
        /// to create various post types.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> that can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A <see cref="Task{T}"/> that can be used to track the operation. If the task succeeds, the <see cref="Task{T}.Result"/> will
        /// carry a <see cref="PostCreationInfo"/> instance. Otherwise <see cref="Task.Exception"/> will carry a <see cref="TumblrException"/>
        /// representing the error occurred during the call.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object has been disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <list type="bullet">
        /// <item>
        ///		<description>
        ///			<paramref name="blogName"/> is <b>null</b>.
        ///		</description>
        ///	</item>
        ///	<item>
        ///		<description>
        ///			<paramref name="postData"/> is <b>null</b>.
        ///		</description>
        ///	</item>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="blogName"/> is empty.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// This <see cref="TumblrClient"/> instance does not have an OAuth token specified.
        /// </exception>
        public Task<PostCreationInfo> CreatePostAsync(string blogName, PostData postData, CancellationToken cancellationToken)
        {
            if (disposed)
                throw new ObjectDisposedException("TumblrClient");

            if (blogName == null)
                throw new ArgumentNullException("blogName");

            if (blogName.Length == 0)
                throw new ArgumentException("Blog name cannot be empty.", "blogName");

            if (postData == null)
                throw new ArgumentNullException("postData");

            if (OAuthToken == null)
                throw new InvalidOperationException("CreatePostAsync method requires an OAuth token to be specified.");

            return CallApiMethodAsync<PostCreationInfo>(
              new BlogMethod(blogName, "post", OAuthToken, HttpMethod.Post, postData.ToMethodParameterSet()),
              cancellationToken);
        }

        #endregion

        #region EditPostAsync

        /// <summary>
        /// Asynchronously edits an existing post.
        /// </summary>
        /// <remarks>
        /// See: http://www.tumblr.com/docs/en/api/v2#editing
        /// </remarks>
        /// <param name="blogName">
        /// The name of the blog where the post to edit is (must be one of the current user's blogs).
        /// </param>
        /// <param name="postId">
        /// The identifier of the post to edit.
        /// </param>
        /// <param name="postData">
        /// The data that represents the updated information for the post. See <see cref="PostData"/> for how
        /// to create various post types.
        /// </param>
        /// <returns>
        /// A <see cref="Task{PostCreationInfo}"/> that can be used to track the operation. If the task succeeds, the <see cref="Task{PostCreationInfo}.Result"/> will
        /// carry a <see cref="PostCreationInfo"/> instance. Otherwise <see cref="Task.Exception"/> will carry a <see cref="TumblrException"/>
        /// representing the error occurred during the call.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <list type="bullet">
        /// <item>
        ///		<description>
        ///			<paramref name="blogName"/> is <b>null</b>.
        ///		</description>
        ///	</item>
        ///	<item>
        ///		<description>
        ///			<paramref name="postData"/> is <b>null</b>.
        ///		</description>
        ///	</item>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <list type="bullet">
        /// <item>
        ///		<description>
        ///			<paramref name="blogName"/> is empty.
        ///		</description>
        ///	</item>
        ///	<item>
        ///		<description>
        ///			<paramref name="postId"/> is less than 0.
        ///		</description>
        ///	</item>
        /// </list>
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// This <see cref="TumblrClient"/> instance does not have an OAuth token specified.
        /// </exception>
        public Task<PostCreationInfo> EditPostAsync(string blogName, long postId, PostData postData)
        {
            return EditPostAsync(blogName, postId, postData, CancellationToken.None);
        }

        /// <summary>
        /// Asynchronously edits an existing post.
        /// </summary>
        /// <remarks>
        /// See: http://www.tumblr.com/docs/en/api/v2#editing
        /// </remarks>
        /// <param name="blogName">
        /// The name of the blog where the post to edit is (must be one of the current user's blogs).
        /// </param>
        /// <param name="postId">
        /// The identifier of the post to edit.
        /// </param>
        /// <param name="postData">
        /// The data that represents the updated information for the post. See <see cref="PostData"/> for how
        /// to create various post types.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> that can be used to cancel the operation.
        /// </param>
        /// <returns>
        /// A <see cref="Task{T}"/> that can be used to track the operation. If the task succeeds, the <see cref="Task{T}.Result"/> will
        /// carry a <see cref="PostCreationInfo"/> instance. Otherwise <see cref="Task.Exception"/> will carry a <see cref="TumblrException"/>
        /// representing the error occurred during the call.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object has been disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <list type="bullet">
        /// <item>
        ///		<description>
        ///			<paramref name="blogName"/> is <b>null</b>.
        ///		</description>
        ///	</item>
        ///	<item>
        ///		<description>
        ///			<paramref name="postData"/> is <b>null</b>.
        ///		</description>
        ///	</item>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <list type="bullet">
        /// <item>
        ///		<description>
        ///			<paramref name="blogName"/> is empty.
        ///		</description>
        ///	</item>
        ///	<item>
        ///		<description>
        ///			<paramref name="postId"/> is less than 0.
        ///		</description>
        ///	</item>
        /// </list>
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// This <see cref="TumblrClient"/> instance does not have an OAuth token specified.
        /// </exception>
        public Task<PostCreationInfo> EditPostAsync(string blogName, long postId, PostData postData, CancellationToken cancellationToken)
        {
            if (disposed)
                throw new ObjectDisposedException("TumblrClient");

            if (blogName == null)
                throw new ArgumentNullException("blogName");

            if (blogName.Length == 0)
                throw new ArgumentException("Blog name cannot be empty.", "blogName");

            if (postId < 0)
                throw new ArgumentOutOfRangeException("postId", "Post ID must be greater or equal to zero.");

            if (postData == null)
                throw new ArgumentNullException("postData");

            if (OAuthToken == null)
                throw new InvalidOperationException("EditPostAsync method requires an OAuth token to be specified.");

            var parameters = postData.ToMethodParameterSet();
            parameters.Add("id", postId);

            return CallApiMethodAsync<PostCreationInfo>(
              new BlogMethod(blogName, "post/edit", OAuthToken, HttpMethod.Post, parameters),
              cancellationToken);
        }

        #endregion

        #endregion

        #region GetFollowersAsync

        /// <summary>
        /// Asynchronously retrieves a blog's followers.
        /// </summary>
        /// <remarks>
        /// See: http://www.tumblr.com/docs/en/api/v2#blog-followers
        /// </remarks>
        /// <param name="blogName">
        /// The name of the blog.
        /// </param>
        /// <param name="startIndex">
        /// The offset at which to start retrieving the followers. Use 0 to start retrieving from the latest follower.
        /// </param>
        /// <param name="count">
        /// The number of followers to retrieve. Must be between 1 and 20.
        /// </param>
        /// <returns>
        ///  A <see cref="Task{Followers}"/> that can be used to track the operation. If the task succeeds, the <see cref="Task{Followers}.Result"/> will
        /// carry a <see cref="Followers"/> instance. Otherwise <see cref="Task.Exception"/> will carry a <see cref="TumblrException"/>
        /// A <see cref="Followers"/> instance.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object has been disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="blogName"/> is <b>null</b>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="blogName"/> is empty.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <list type="bullet">
        /// <item>
        ///		<description>
        ///			<paramref name="startIndex"/> is less than 0.
        ///		</description>
        ///	</item>
        ///	<item>
        ///		<description>
        ///			<paramref name="count"/> is less than 1 or greater than 20.
        ///		</description>
        ///	</item>
        /// </list>
        /// </exception>
        public Task<Followers> GetFollowersAsync(string blogName, int startIndex = 0, int count = 20)
        {
            if (disposed)
                throw new ObjectDisposedException("TumblrClient");

            if (blogName == null)
                throw new ArgumentNullException("blogName");

            if (blogName.Length == 0)
                throw new ArgumentException("Blog name cannot be empty.", "blogName");

            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "startIndex must be greater or equal to zero.");

            if (count < 1 || count > 20)
                throw new ArgumentOutOfRangeException("count", "count must be between 1 and 20.");

            if (OAuthToken == null)
                throw new InvalidOperationException("GetFollowersAsync method requires an OAuth token to be specified.");

            MethodParameterSet parameters = new MethodParameterSet();
            parameters.Add("offset", startIndex, 0);
            parameters.Add("limit", count, 0);

            return CallApiMethodAsync<Followers>(
              new BlogMethod(blogName, "followers", OAuthToken, HttpMethod.Get, parameters),
              CancellationToken.None);
        }

        #endregion

        #region GetFollowedByAsync

        /// <summary>
        /// This method can be used to check if one of your blogs is followed by another blog.
        /// </summary>
        /// <param name="blogName">name of your blog</param>
        /// <param name="query">The name of the blog that may be following your blog</param>
        /// <returns>True when the queried blog follows your blog, false otherwise.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <list type="bullet">
        /// <item>
        ///		<description>
        ///			<paramref name="blogName"/> cannot be null.
        ///		</description>
        ///	</item>
        ///	<item>
        ///		<description>
        ///			<paramref name="query"/> cannot be null.
        ///		</description>
        ///	</item>
        /// </list>
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// <list type="bullet">
        /// <item>
        ///		<description>
        ///			<paramref name="blogName"/> cannot be empty.
        ///		</description>
        ///	</item>
        ///	<item>
        ///		<description>
        ///			<paramref name="query"/> cannot be empty.
        ///		</description>
        ///	</item>
        /// </list>
        /// </exception>
        public async Task<bool> GetFollowedByAsync(string blogName, string query)
        {
            if (blogName == null)
                throw new ArgumentNullException(nameof(blogName));

            if (blogName.Length == 0)
                throw new ArgumentException("Blog name cannot be empty.", nameof(blogName));

            if (query == null)
                throw new ArgumentNullException(nameof(query));

            if (query.Length == 0)
                throw new ArgumentException("Query cannot be empty.", nameof(query));

            MethodParameterSet parameters = new MethodParameterSet
            {
                {"query", query}
            };

            var result = await CallApiMethodAsync<FollowerBy>(
              new BlogMethod(blogName, "followed_by", OAuthToken, HttpMethod.Get, parameters),
              CancellationToken.None, null);

            return result.IsFollower;
        }

        #endregion

        #region ReblogAsync

        /// <summary>
        /// Asynchronously reblogs a post.
        /// </summary>
        /// <remarks>
        /// See: http://www.tumblr.com/docs/en/api/v2#reblogging
        /// </remarks>
        /// <param name="blogName">
        /// The name of the blog where to reblog the psot (must be one of the current user's blogs).
        /// </param>
        /// <param name="postId">
        /// The identifier of the post to reblog.
        /// </param>
        /// <param name="reblogKey">
        /// The post reblog key.
        /// </param>
        /// <param name="comment">
        /// An optional comment to add to the reblog.
        /// </param>
        /// <returns>
        /// A <see cref="Task{PostCreationInfo}"/> that can be used to track the operation. If the task succeeds, the <see cref="Task{PostCreationInfo}.Result"/> will
        /// carry a <see cref="PostCreationInfo"/> instance. Otherwise <see cref="Task.Exception"/> will carry a <see cref="TumblrException"/>
        /// representing the error occurred during the call.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object has been disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <list type="bullet">
        /// <item>
        ///		<description>
        ///			<paramref name="blogName"/> is <b>null</b>.
        ///		</description>
        ///	</item>
        ///	<item>
        ///		<description>
        ///			<paramref name="reblogKey"/> is <b>null</b>.
        ///		</description>
        ///	</item>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// /// <list type="bullet">
        /// <item>
        ///		<description>
        ///			<paramref name="blogName"/> is empty.
        ///		</description>
        ///	</item>
        ///	<item>
        ///		<description>
        ///			<paramref name="reblogKey"/> is empty.
        ///		</description>
        ///	</item>
        /// </list>
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// This <see cref="TumblrClient"/> instance does not have an OAuth token specified.
        /// </exception>
        public Task<PostCreationInfo> ReblogAsync(string blogName, long postId, string reblogKey, string comment = null)
        {
            if (disposed)
                throw new ObjectDisposedException("TumblrClient");

            if (blogName == null)
                throw new ArgumentNullException("blogName");

            if (blogName.Length == 0)
                throw new ArgumentException("Blog name cannot be empty.", "blogName");

            if (postId <= 0)
                throw new ArgumentException("Post ID must be greater than 0.", "postId");

            if (reblogKey == null)
                throw new ArgumentNullException("reblogKey");

            if (reblogKey.Length == 0)
                throw new ArgumentException("reblogKey cannot be empty.", "reblogKey");

            if (OAuthToken == null)
                throw new InvalidOperationException("ReblogAsync method requires an OAuth token to be specified.");

            MethodParameterSet parameters = new MethodParameterSet();
            parameters.Add("id", postId);
            parameters.Add("reblog_key", reblogKey);
            parameters.Add("comment", comment, null);

            return CallApiMethodAsync<PostCreationInfo>(
              new BlogMethod(blogName, "post/reblog", OAuthToken, HttpMethod.Post, parameters),
              CancellationToken.None);
        }

        /// <summary>
        /// Asynchronously reblogs a post.
        /// </summary>
        /// <remarks>
        /// See: http://www.tumblr.com/docs/en/api/v2#reblogging
        /// </remarks>
        /// <param name="blogName">
        /// The name of the blog where to reblog the psot (must be one of the current user's blogs).
        /// </param>
        /// <param name="postId">
        /// The identifier of the post to reblog.
        /// </param>
        /// <param name="reblogKey">
        /// The post reblog key.
        /// </param>
        /// <param name="comment">
        /// An optional comment to add to the reblog.
        /// </param>
        /// <param name="state">
        /// Post creation state
        /// </param>
        /// <param name="publish_On">
        /// if state is <see cref="PostCreationState.Queue"/> is this the publishingtime 
        /// </param>
        /// <returns>
        /// A <see cref="Task{T}"/> that can be used to track the operation. If the task succeeds, the <see cref="Task{T}.Result"/> will
        /// carry a <see cref="PostCreationInfo"/> instance. Otherwise <see cref="Task.Exception"/> will carry a <see cref="TumblrException"/>
        /// representing the error occurred during the call.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object has been disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <list type="bullet">
        /// <item>
        ///		<description>
        ///			<paramref name="blogName"/> is <b>null</b>.
        ///		</description>
        ///	</item>
        ///	<item>
        ///		<description>
        ///			<paramref name="reblogKey"/> is <b>null</b>.
        ///		</description>
        ///	</item>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// /// <list type="bullet">
        /// <item>
        ///		<description>
        ///			<paramref name="blogName"/> is empty.
        ///		</description>
        ///	</item>
        ///	<item>
        ///		<description>
        ///			<paramref name="reblogKey"/> is empty.
        ///		</description>
        ///	</item>
        ///	<item>
        ///	    <description>
        ///	        <paramref name="publish_On"/> is in the past.
        ///	    </description>
        /// </item>
        /// </list>
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// This <see cref="TumblrClient"/> instance does not have an OAuth token specified.
        /// </exception>
        public Task<PostCreationInfo> ReblogAsync(string blogName, long postId, string reblogKey, PostCreationState state, DateTime? publish_On = null, string comment = null)
        {
            if (state == PostCreationState.Published)
            {
                return ReblogAsync(blogName, postId, reblogKey, comment);
            }
            else
            {

                if (disposed)
                    throw new ObjectDisposedException("TumblrClient");

                if (blogName == null)
                    throw new ArgumentNullException("blogName");

                if (blogName.Length == 0)
                    throw new ArgumentException("Blog name cannot be empty.", "blogName");

                if (postId <= 0)
                    throw new ArgumentException("Post ID must be greater than 0.", "postId");

                if (reblogKey == null)
                    throw new ArgumentNullException("reblogKey");

                if (reblogKey.Length == 0)
                    throw new ArgumentException("ReblogKey cannot be empty.", "reblogKey");

                if (OAuthToken == null)
                    throw new InvalidOperationException("ReblogAsync method requires an OAuth token to be specified.");

                if (publish_On != null)
                {
                    if (DateTime.Now >= publish_On)
                        throw new ArgumentException("Published_On must be in the future");
                }

                MethodParameterSet parameters = new MethodParameterSet
                {
                    { "id", postId },
                    { "reblog_key", reblogKey },
                    { "comment", comment, null },
                    { "state", state.ToString().ToLowerInvariant() }
                };

                if (state == PostCreationState.Queue && publish_On != null)
                {
                    parameters.Add("publish_on", publish_On.Value.ToUniversalTime().ToString("R"));
                }

                return CallApiMethodAsync<PostCreationInfo>(
                    new BlogMethod(blogName, "post/reblog", OAuthToken, HttpMethod.Post, parameters),
                    CancellationToken.None);
            }
        }

        #endregion
                
        #region GetDraftPostsAsync

        /// <summary>
        /// Asynchronously returns draft posts.
        /// </summary>
        /// <remarks>
        /// See: http://www.tumblr.com/docs/en/api/v2#blog-drafts
        /// </remarks>
        /// <param name="blogName">
        /// The name of the blog for which to retrieve drafted posts. 
        /// </param>
        /// <param name="sinceId">
        /// Return posts that have appeared after the specified ID. Use this parameter to page through 
        /// the results: first get a set of posts, and then get posts since the last ID of the previous set. 
        /// </param>
        /// <param name="filter">
        /// A <see cref="PostFilter"/> to apply.
        /// </param>
        /// <returns>
        /// A Task&lt;<see cref="BasePost"/>[]&gt; that can be used to track the operation. If the task succeeds, the Task&lt;<see cref="BasePost"/>[]&gt;.Result will
        /// carry an array of posts. Otherwise <see cref="Task.Exception"/> will carry a <see cref="TumblrException"/>
        /// representing the error occurred during the call.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object has been disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="blogName"/> is <b>null</b>.
        ///	</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="blogName"/> is empty.
        ///	</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="sinceId"/> is less than 0.
        ///	</exception>
        /// <exception cref="InvalidOperationException">
        /// This <see cref="TumblrClient"/> instance does not have an OAuth token specified.
        /// </exception>
        public Task<BasePost[]> GetDraftPostsAsync(string blogName, long sinceId = 0, PostFilter filter = PostFilter.Html)
        {
            if (disposed)
                throw new ObjectDisposedException("TumblrClient");

            if (blogName == null)
                throw new ArgumentNullException("blogName");

            if (blogName.Length == 0)
                throw new ArgumentException("Blog name cannot be empty.", "blogName");

            if (sinceId < 0)
                throw new ArgumentOutOfRangeException("sinceId", "sinceId must be greater or equal to zero.");

            if (OAuthToken == null)
                throw new InvalidOperationException("GetDraftPostsAsync method requires an OAuth token to be specified.");

            MethodParameterSet parameters = new MethodParameterSet();
            parameters.Add("since_id", sinceId, 0);
            parameters.Add("filter", filter.ToString().ToLowerInvariant(), "html");

            return CallApiMethodAsync<PostCollection, BasePost[]>(
              new BlogMethod(blogName, "posts/draft", OAuthToken, HttpMethod.Get, parameters),
              r => r.Posts,
              CancellationToken.None);
        }

        #endregion

        #region GetSubmissionPostsAsync

        /// <summary>
        /// Asynchronously retrieves submission posts.
        /// </summary>
        /// <remarks>
        /// See: http://www.tumblr.com/docs/en/api/v2#blog-submissions
        /// </remarks>
        /// <param name="blogName">
        /// The name of the blog for which to retrieve submission posts. 
        /// </param>
        /// <param name="startIndex">
        /// The post number to start at. Pass 0 to start from the first post.
        /// </param>
        /// <param name="filter">
        /// A <see cref="PostFilter"/> to apply.
        /// </param>
        /// <returns>
        /// A Task&lt;<see cref="BasePost"/>[]&gt; that can be used to track the operation. If the task succeeds, the Task&lt;<see cref="BasePost"/>[]&gt;.Result; will
        /// carry an array of posts. Otherwise <see cref="Task.Exception"/> will carry a <see cref="TumblrException"/>
        /// representing the error occurred during the call.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object has been disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="blogName"/> is <b>null</b>.
        ///	</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="blogName"/> is empty.
        ///	</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="startIndex"/> is less than 0.
        ///	</exception>
        /// <exception cref="InvalidOperationException">
        /// This <see cref="TumblrClient"/> instance does not have an OAuth token specified.
        /// </exception>
        public Task<BasePost[]> GetSubmissionPostsAsync(string blogName, long startIndex = 0, PostFilter filter = PostFilter.Html)
        {
            if (disposed)
                throw new ObjectDisposedException("TumblrClient");

            if (blogName == null)
                throw new ArgumentNullException("blogName");

            if (blogName.Length == 0)
                throw new ArgumentException("Blog name cannot be empty.", "blogName");

            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "startIndex must be greater or equal to zero.");

            if (OAuthToken == null)
                throw new InvalidOperationException("GetSubmissionPostsAsync method requires an OAuth token to be specified.");

            MethodParameterSet parameters = new MethodParameterSet();
            parameters.Add("offset", startIndex);
            parameters.Add("filter", filter.ToString().ToLowerInvariant(), "html");

            return CallApiMethodAsync<PostCollection, BasePost[]>(
              new BlogMethod(blogName, "posts/submission", OAuthToken, HttpMethod.Get, parameters),
              r => r.Posts,
              CancellationToken.None);
        }

        #endregion

        #region GetNotifications


        /// <summary>
        /// Retrieve the activity items for a specific blog, in reverse chronological order.
        /// </summary>
        /// 
        /// <param name="blogName">The name of the blog.</param>
        /// <param name="before">Unix epoch timestamp that begins the page, defaults to request time.</param>
        /// <param name="notificationsTypes">one or more types to filter by</param>
        /// 
        /// <returns> A task that provides an enumeration of <see cref="Notification"/></returns>
        /// 
        /// <exception cref="ArgumentNullException" >
        /// <list type="bullet">
        /// <item>
        ///     <description>
        ///         <paramref name="blogName">name of the blog is null</paramref>
        ///     </description>
        /// </item>
        /// <item>
        ///     <description>
        ///         <paramref name="before">the date is null</paramref>
        ///     </description>
        /// </item>
        /// </list>
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="blogName">the name of blog is empty</paramref>
        /// </exception>
        public Task<Notification[]> GetNotifications(string blogName, DateTime before, NotificationsTypes notificationsTypes = NotificationsTypes.All)
        {
            if (blogName == null)
                throw new ArgumentNullException(nameof(blogName));

            if (blogName.Length == 0)
                throw new ArgumentException(nameof(blogName));

            MethodParameterSet parameters = new MethodParameterSet
            {
                { "before", new DateTimeOffset(before).ToUnixTimeSeconds(), 0 }
            };

            if (notificationsTypes != NotificationsTypes.All)
            {
                string[] arrayOfNotificationsTypes = notificationsTypes.ToTumblrStringArray();

                for (int i = 0; i < arrayOfNotificationsTypes.Length; i++)
                {
                    parameters.Add($"types[{i}]", arrayOfNotificationsTypes[i], "");
                }
            }

            return CallApiMethodAsync<NotificationsResponse, Notification[]>(
              new BlogMethod(blogName, "notifications", OAuthToken, HttpMethod.Get, parameters),
              r => r.Notifications,
              CancellationToken.None);
        }

        /// <summary>
        /// Retrieve the activity items for a specific blog, in reverse chronological order.
        /// </summary>
        /// <param name="blogName">The name of the blog.</param>
        /// <param name="notificationsTypes">one or more types to filter by</param>
        /// <returns>A task that provides an enumeration of <see cref="Notification"/></returns>
        /// <exception cref="ArgumentNullException" >
        ///     <paramref name="blogName">name of the blog is null</paramref>
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="blogName">the name of blog is empty</paramref>
        /// </exception>
        public Task<Notification[]> GetNotifications(string blogName, NotificationsTypes notificationsTypes = NotificationsTypes.All)
        {
            if (blogName == null)
                throw new ArgumentNullException(nameof(blogName));

            if (blogName.Length == 0)
                throw new ArgumentException(nameof(blogName));

            MethodParameterSet parameters = new MethodParameterSet();

            if (notificationsTypes != NotificationsTypes.All)
            {
                string[] arrayOfNotificationsTypes = notificationsTypes.ToTumblrStringArray();

                for (int i = 0; i < arrayOfNotificationsTypes.Length; i++)
                {
                    parameters.Add($"types[{i}]", arrayOfNotificationsTypes[i], "");
                }
            }

            return CallApiMethodAsync<NotificationsResponse, Notification[]>(
              new BlogMethod(blogName, "notifications", OAuthToken, HttpMethod.Get, parameters),
              r => r.Notifications,
              CancellationToken.None);
        }

        #endregion

        #region Queue

        #region GetQueuedPostsAsync

        /// <summary>
        /// Asynchronously returns posts in the current user's queue.
        /// </summary>
        /// <remarks>
        /// See: http://www.tumblr.com/docs/en/api/v2#blog-queue
        /// </remarks>
        /// <param name="blogName">
        /// The name of the blog for which to retrieve queued posts.
        /// </param>
        /// <param name="startIndex">
        /// The offset at which to start retrieving the posts. Use 0 to start retrieving from the latest post.
        /// </param>
        /// <param name="count">
        /// The number of posts to retrieve. Must be between 1 and 20.
        /// </param>
        /// <param name="filter">
        /// A <see cref="PostFilter"/> to apply.
        /// </param>
        /// <returns>
        /// A Task&lt;<see cref="BasePost"/>[]&gt; that can be used to track the operation. If the task succeeds, the Task&lt;<see cref="BasePost"/>[]&gt;.Result will
        /// carry an array of posts. Otherwise <see cref="Task.Exception"/> will carry a <see cref="TumblrException"/>
        /// representing the error occurred during the call.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object has been disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="blogName"/> is <b>null</b>.
        ///	</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="blogName"/> is empty.
        ///	</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <list type="bullet">
        /// <item>
        ///		<description>
        ///			<paramref name="startIndex"/> is less than 0.
        ///		</description>
        ///	</item>
        ///	<item>
        ///		<description>
        ///			<paramref name="count"/> is less than 1 or greater than 20.
        ///		</description>
        ///	</item>
        /// </list>
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// This <see cref="TumblrClient"/> instance does not have an OAuth token specified.
        /// </exception>
        /// 
        /// <example>
        /// Returns the first five posts that are still on queue.
        /// <code>
        ///     var queueList = await tumblrClient.GetQueuedPostsAsync("NameOfYourBlog", 0, 5);
        ///     
        ///     foreach (var item in queueList)
        ///     {
        ///         Console.WriteLine($"PostID {item.Id}");
        ///     }
        /// </code>
        /// </example>
        public Task<BasePost[]> GetQueuedPostsAsync(string blogName, long startIndex = 0, int count = 20, PostFilter filter = PostFilter.Html)
        {
            if (disposed)
                throw new ObjectDisposedException("TumblrClient");

            if (blogName == null)
                throw new ArgumentNullException("blogName");

            if (blogName.Length == 0)
                throw new ArgumentException("Blog name cannot be empty.", "blogName");

            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "startIndex must be greater or equal to zero.");

            if (count < 1 || count > 20)
                throw new ArgumentOutOfRangeException("count", "count must be between 1 and 20.");

            if (OAuthToken == null)
                throw new InvalidOperationException("GetQueuedPostsAsync method requires an OAuth token to be specified.");

            MethodParameterSet parameters = new MethodParameterSet
            {
                { "offset", startIndex, 0 },
                { "limit", count, 0 },
                { "filter", filter.ToString().ToLowerInvariant(), "html" }
            };

            return CallApiMethodAsync<PostCollection, BasePost[]>(
              new BlogMethod(blogName, "posts/queue", OAuthToken, HttpMethod.Get, parameters),
              r => r.Posts,
              CancellationToken.None);
        }

        #endregion

        #region QueueRecorder

        /// <summary>
        /// This allows you to reorder a post within the queue, moving it after an existing queued post, or to the top.
        /// </summary>
        /// <param name="blogName">blog identifier for queue</param>
        /// <param name="postId">Post ID to move</param>
        /// <param name="insertAfter">Which post ID to move it after, or 0 to make it the first post</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to track the operation. If the task fails, <see cref="Exception"/> 
        /// will carry a <see cref="TumblrException"/> representing the error occurred during the call.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object has been disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="blogName"/> is <b>null</b>.
        ///	</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="blogName"/> is empty.
        ///	</exception>
        ///	<exception cref="ArgumentOutOfRangeException">
        /// <list type="bullet">
        /// <item>
        ///		<description>
        ///			<paramref name="postId"/> is less than 0 or smaller
        ///		</description>
        ///	</item>
        ///	<item>
        ///		<description>
        ///			<paramref name="insertAfter"/> is less than smaller 0
        ///		</description>
        ///	</item>
        /// </list>
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// This <see cref="TumblrClient"/> instance does not have an OAuth token specified
        /// </exception>
        /// Move the post in queue with Id 123456 to 3rd position
        /// <example>
        ///     <code>
        ///         tumblrClient.QueueRecorder("NameOfYourBlog", 123456, 2);
        ///     </code>
        /// </example>
        public Task QueueRecorder(string blogName, long postId, long insertAfter = 0)
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(TumblrClient));

            if (blogName == null)
                throw new ArgumentNullException(nameof(blogName));

            if (blogName.Length == 0)
                throw new ArgumentException("Blog name cannot be empty.", nameof(blogName));

            if (postId <= 0)
                throw new ArgumentOutOfRangeException(nameof(postId), "Post ID must be greater to zero.");

            if (insertAfter < 0)
                throw new ArgumentOutOfRangeException(nameof(insertAfter), "InsertAfter must be greater or equal to zero.");

            MethodParameterSet parameters = new MethodParameterSet
            {
                { "post_id", postId.ToString() },
                { "insert_after", insertAfter.ToString() }
            };

            return CallApiMethodNoResultAsync(
              new BlogMethod(blogName, "posts/queue/reorder", OAuthToken, HttpMethod.Post, parameters),
              CancellationToken.None);
        }

        #endregion

        #region QueueShuffle

        /// <summary>
        /// This randomly shuffles the queue for the specified blog
        /// </summary>
        /// <param name="blogName">blog identifier for queue</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to track the operation. If the task fails, <see cref="Exception"/> 
        /// will carry a <see cref="TumblrException"/> representing the error occurred during the call.
        /// </returns>
        /// 
        /// <example>
        ///     <code>
        ///         try
        ///         {
        ///             tumblrClient.QueueShuffle("NameOfYourBlog");
        ///             
        ///             Console.WriteLine("Operation not successful");
        ///         }
        ///         except (TumblrException exp)
        ///         {
        ///             Console.WriteLine($"Operation not successful - {exp.msg}");
        ///         }
        ///     </code>
        /// </example>
        public Task QueueShuffle(string blogName)
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(TumblrClient));

            if (blogName == null)
                throw new ArgumentNullException(nameof(blogName));

            if (blogName.Length == 0)
                throw new ArgumentException("Blog name cannot be empty.", nameof(blogName));

            return CallApiMethodNoResultAsync(
              new BlogMethod(blogName, "posts/queue/shuffle", OAuthToken, HttpMethod.Post),
              CancellationToken.None);
        }

        #endregion

        #endregion

        #endregion

        #region User Methods

        #region GetUserInfoAsync

        /// <summary>
        /// Asynchronously retrieves the user's account information that matches the OAuth credentials submitted with the request.
        /// </summary>
        /// <remarks>
        /// See: http://www.tumblr.com/docs/en/api/v2#user-methods
        /// </remarks>
        /// <returns>
        /// A <see cref="Task"/> that can be used to track the operation. If the task succeeds, the <see cref="Task{UserInfo}.Result"/> will
        /// carry a <see cref="UserInfo"/> instance. Otherwise <see cref="Task.Exception"/> will carry the <see cref="TumblrException"/>
        /// generated during the call.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object has been disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// This <see cref="TumblrClient"/> instance does not have an OAuth token specified.
        /// </exception>
        public Task<UserInfo> GetUserInfoAsync()
        {
            if (disposed)
                throw new ObjectDisposedException("TumblrClient");

            if (OAuthToken == null)
                throw new InvalidOperationException("GetUserInfoAsync method requires an OAuth token to be specified.");

            return CallApiMethodAsync<UserInfoResponse, UserInfo>(
              new UserMethod("info", OAuthToken, HttpMethod.Get),
              r => r.User,
              CancellationToken.None);
        }

        #endregion

        #region GetFollowingAsync

        /// <summary>
        /// Asynchronously retrieves the blog that the current user is following.
        /// </summary>
        /// <remarks>
        /// See: http://www.tumblr.com/docs/en/api/v2#m-ug-following
        /// </remarks>
        /// <param name="startIndex">
        /// The offset at which to start retrieving the followed blogs. Use 0 to start retrieving from the latest followed blog.
        /// </param>
        /// <param name="count">
        /// The number of following blogs to retrieve. Must be between 1 and 20.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to track the operation. If the task succeeds, the <see cref="Task{Following}.Result"/> will
        /// carry a <see cref="Following"/> instance. Otherwise <see cref="Task.Exception"/> will carry the <see cref="TumblrException"/>
        /// generated during the call.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object has been disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// This <see cref="TumblrClient"/> instance does not have an OAuth token specified.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <list type="bullet">
        /// <item>
        ///		<description>
        ///			<paramref name="startIndex"/> is less than 0.
        ///		</description>
        ///	</item>
        ///	<item>
        ///		<description>
        ///			<paramref name="count"/> is less than 1 or greater than 20.
        ///		</description>
        ///	</item>
        /// </list>
        /// </exception>
        public Task<Following> GetFollowingAsync(long startIndex = 0, int count = 20)
        {
            if (disposed)
                throw new ObjectDisposedException("TumblrClient");

            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "startIndex must be greater or equal to zero.");

            if (count <= 0 || count > 20)
                throw new ArgumentOutOfRangeException("count", "count must be between 1 and 20.");

            if (OAuthToken == null)
                throw new InvalidOperationException("GetFollowingAsync method requires an OAuth token to be specified.");

            MethodParameterSet parameters = new MethodParameterSet();
            parameters.Add("offset", startIndex, 0);
            parameters.Add("limit", count, 0);

            return CallApiMethodAsync<Following>(
              new UserMethod("following", OAuthToken, HttpMethod.Get, parameters),
              CancellationToken.None);
        }

        #endregion

        #region GetLikesAsync

        /// <summary>
        /// Asynchronously retrieves the current user's likes.
        /// </summary>
        /// <remarks>
        /// See: http://www.tumblr.com/docs/en/api/v2#m-ug-likes
        /// </remarks>
        /// <param name="startIndex">
        /// The offset at which to start retrieving the likes. Use 0 to start retrieving from the latest like.
        /// </param>
        /// <param name="count">
        /// The number of likes to retrieve. Must be between 1 and 20.
        /// </param>
        /// <param name="before">
        /// The timestamp before when to retrieve likes. 
        /// </param>
        /// <param name="after">
        /// The timestamp after when to retrieve likes. 
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to track the operation. If the task succeeds, the <see cref="Task{Likes}.Result"/> will
        /// carry a <see cref="Likes"/> instance. Otherwise <see cref="Task.Exception"/> will carry the <see cref="TumblrException"/>
        /// generated during the call.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object has been disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// This <see cref="TumblrClient"/> instance does not have an OAuth token specified.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <list type="bullet">
        /// <item>
        ///		<description>
        ///			<paramref name="startIndex"/> is less than 0.
        ///		</description>
        ///	</item>
        ///	<item>
        ///		<description>
        ///			<paramref name="count"/> is less than 1 or greater than 20.
        ///		</description>
        ///	</item>
        /// </list>
        /// </exception>
        public Task<Likes> GetLikesAsync(long startIndex = 0, int count = 20, DateTime? before = null, DateTime? after = null)
        {
            if (disposed)
                throw new ObjectDisposedException("TumblrClient");

            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "startIndex must be greater or equal to zero.");

            if (count <= 0 || count > 20)
                throw new ArgumentOutOfRangeException("count", "count must be between 1 and 20.");

            if (OAuthToken == null)
                throw new InvalidOperationException("GetLikesAsync method requires an OAuth token to be specified.");

            MethodParameterSet parameters = new MethodParameterSet
            {
                { "offset", startIndex, 0 },
                { "limit", count, 0 },
                { "before", before.HasValue ? DateTimeHelper.ToTimestamp(before.Value).ToString() : null, null },
                { "after", after.HasValue ? DateTimeHelper.ToTimestamp(after.Value).ToString() : null, null }
            };

            return CallApiMethodAsync<Likes>(
              new UserMethod("likes", OAuthToken, HttpMethod.Get, parameters),
              CancellationToken.None);
        }

        #endregion

        #region LikeAsync

        /// <summary>
        /// Asynchronously likes a post.
        /// </summary>
        /// <remarks>
        /// See: http://www.tumblr.com/docs/en/api/v2#m-up-like
        /// </remarks>
        /// <param name="postId">
        /// The identifier of the post to like.
        /// </param>
        /// <param name="reblogKey">
        /// The reblog key for the post.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to track the operation. If the task fails, <see cref="Task.Exception"/> 
        /// will carry a <see cref="TumblrException"/>
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object has been disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reblogKey"/> is <b>null</b>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="reblogKey"/> is empty.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="postId"/> is less than 0.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// This <see cref="TumblrClient"/> instance does not have an OAuth token specified.
        /// </exception>
        public Task LikeAsync(long postId, string reblogKey)
        {
            if (disposed)
                throw new ObjectDisposedException("TumblrClient");

            if (postId <= 0)
                throw new ArgumentOutOfRangeException("Post ID must be greater than 0.", "postId");

            if (reblogKey == null)
                throw new ArgumentNullException("reblogKey");

            if (reblogKey.Length == 0)
                throw new ArgumentException("reblogKey cannot be empty.", "reblogKey");

            if (OAuthToken == null)
                throw new InvalidOperationException("LikeAsync method requires an OAuth token to be specified.");

            MethodParameterSet parameters = new MethodParameterSet();
            parameters.Add("id", postId);
            parameters.Add("reblog_key", reblogKey);

            return CallApiMethodNoResultAsync(
              new UserMethod("like", OAuthToken, HttpMethod.Post, parameters),
              CancellationToken.None);
        }

        #endregion

        #region UnlikeAsync

        /// <summary>
        /// Asynchronously unlikes a post.
        /// </summary>
        /// <remarks>
        /// See: http://www.tumblr.com/docs/en/api/v2#m-up-unlike
        /// </remarks>
        /// <param name="postId">
        /// The identifier of the post to like.
        /// </param>
        /// <param name="reblogKey">
        /// The reblog key for the post.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to track the operation. If the task fails, <see cref="Task.Exception"/> 
        /// will carry a <see cref="TumblrException"/>
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object has been disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reblogKey"/> is <b>null</b>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="reblogKey"/> is empty.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="postId"/> is less than 0.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// This <see cref="TumblrClient"/> instance does not have an OAuth token specified.
        /// </exception>
        public Task UnlikeAsync(long postId, string reblogKey)
        {
            if (disposed)
                throw new ObjectDisposedException("TumblrClient");

            if (postId <= 0)
                throw new ArgumentException("Post ID must be greater than 0.", "postId");

            if (reblogKey == null)
                throw new ArgumentNullException("reblogKey");

            if (reblogKey.Length == 0)
                throw new ArgumentException("reblogKey cannot be empty.", "reblogKey");

            if (OAuthToken == null)
                throw new InvalidOperationException("UnlikeAsync method requires an OAuth token to be specified.");

            MethodParameterSet parameters = new MethodParameterSet();
            parameters.Add("id", postId);
            parameters.Add("reblog_key", reblogKey);

            return CallApiMethodNoResultAsync(
              new UserMethod("unlike", OAuthToken, HttpMethod.Post, parameters),
              CancellationToken.None);
        }

        #endregion

        #region FollowAsync

        /// <summary>
        /// Asynchronously follows a blog.
        /// </summary>
        /// <remarks>
        /// See: http://www.tumblr.com/docs/en/api/v2#m-up-follow
        /// </remarks>
        /// <param name="blogUrl">
        /// The url of the blog to follow.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to track the operation. If the task fails, <see cref="Exception"/> 
        /// will carry a <see cref="TumblrException"/>
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object has been disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="blogUrl"/> is <b>null</b>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="blogUrl"/> is empty.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// This <see cref="TumblrClient"/> instance does not have an OAuth token specified.
        /// </exception>
        public Task FollowAsync(string blogUrl)
        {
            if (disposed)
                throw new ObjectDisposedException("TumblrClient");

            if (blogUrl == null)
                throw new ArgumentNullException("blogUrl");

            if (blogUrl.Length == 0)
                throw new ArgumentException("Blog url cannot be empty.", "blogUrl");

            if (OAuthToken == null)
                throw new InvalidOperationException("FollowAsync method requires an OAuth token to be specified.");

            MethodParameterSet parameters = new MethodParameterSet();
            parameters.Add("url", blogUrl);

            return CallApiMethodNoResultAsync(
              new UserMethod("follow", OAuthToken, HttpMethod.Post, parameters),
              CancellationToken.None);
        }

        #endregion

        #region UnfollowAsync

        /// <summary>
        /// Asynchronously unfollows a blog.
        /// </summary>
        /// <remarks>
        /// See: http://www.tumblr.com/docs/en/api/v2#m-up-unfollow
        /// </remarks>
        /// <param name="blogUrl">
        /// The url of the blog to unfollow.
        /// </param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to track the operation. If the task fails, <see cref="Exception"/> 
        /// will carry a <see cref="TumblrException"/>
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object has been disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="blogUrl"/> is <b>null</b>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="blogUrl"/> is empty.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// This <see cref="TumblrClient"/> instance does not have an OAuth token specified.
        /// </exception>
        public Task UnfollowAsync(string blogUrl)
        {
            if (disposed)
                throw new ObjectDisposedException("TumblrClient");

            if (blogUrl == null)
                throw new ArgumentNullException("blogUrl");

            if (blogUrl.Length == 0)
                throw new ArgumentException("Blog url cannot be empty.", "blogUrl");

            if (OAuthToken == null)
                throw new InvalidOperationException("UnfollowAsync method requires an OAuth token to be specified.");

            MethodParameterSet parameters = new MethodParameterSet();
            parameters.Add("url", blogUrl);

            return CallApiMethodNoResultAsync(
              new UserMethod("unfollow", OAuthToken, HttpMethod.Post, parameters),
              CancellationToken.None);
        }

        #endregion

        #region GetTaggedPostsAsync

        /// <summary>
        /// Asynchronously retrieves posts that have been tagged with a specific <paramref name="tag"/>.
        /// </summary>
        /// <remarks>
        /// See: http://www.tumblr.com/docs/en/api/v2#m-up-tagged
        /// </remarks>
        /// <param name="tag">
        /// The tag on the posts to retrieve.
        /// </param>
        /// <param name="before">
        /// The timestamp of when to retrieve posts before. 
        /// </param>
        /// <param name="count">
        /// The number of posts to retrieve.
        /// </param>
        /// <param name="filter">
        /// A <see cref="PostFilter"/>.
        /// </param>
        /// <returns>
        /// A Task&lt;<see cref="BasePost"/>[]&gt; that can be used to track the operation. If the task succeeds, the Task&lt;<see cref="BasePost"/>[]&gt;.Result will
        /// carry an array of <see cref="BasePost">posts</see>. Otherwise <see cref="Exception"/> will carry a <see cref="TumblrException"/>
        /// representing the error occurred during the call.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object has been disposed.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="tag"/> is <b>null</b>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="tag"/> is empty.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// This <see cref="TumblrClient"/> instance does not have an OAuth token specified.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <paramref name="count"/> is less than 1 or greater than 20.
        /// </exception>
        public Task<BasePost[]> GetTaggedPostsAsync(string tag, DateTime? before = null, int count = 20, PostFilter filter = PostFilter.Html)
        {
            if (disposed)
                throw new ObjectDisposedException("TumblrClient");

            if (tag == null)
                throw new ArgumentNullException("tag");

            if (tag.Length == 0)
                throw new ArgumentException("Tag cannot be empty.", "tag");

            if (count < 1 || count > 20)
                throw new ArgumentOutOfRangeException("count", "count must be between 1 and 20.");

            MethodParameterSet parameters = new MethodParameterSet();
            parameters.Add("api_key", apiKey);
            parameters.Add("tag", tag);
            parameters.Add("before", before.HasValue ? DateTimeHelper.ToTimestamp(before.Value).ToString() : null, null);
            parameters.Add("limit", count, 0);
            parameters.Add("filter", filter.ToString().ToLowerInvariant(), "html");

            return CallApiMethodAsync<BasePost[]>(
              new ApiMethod("https://api.tumblr.com/v2/tagged", OAuthToken, HttpMethod.Get, parameters),
              CancellationToken.None,
              new JsonConverter[] { new PostArrayConverter() });
        }

        #endregion

        #region GetDashboardPostsAsync

        /// <summary>
        /// Asynchronously retrieves posts from the current user's dashboard.
        /// </summary>
        /// See:  http://www.tumblr.com/docs/en/api/v2#m-ug-dashboard
        /// <param name="sinceId">
        ///  Return posts that have appeared after the specified ID. Use this parameter to page through the results: first get a set 
        ///  of posts, and then get posts since the last ID of the previous set.  
        /// </param>
        /// <param name="startIndex">
        /// The post number to start at.
        /// </param>
        /// <param name="count">
        /// The number of posts to return.
        /// </param>
        /// <param name="type">
        /// The <see cref="PostType"/> to return.
        /// </param>
        /// <param name="includeReblogInfo">
        /// Whether or not the response should include reblog info.
        /// </param>
        /// <param name="includeNotesInfo">
        /// Whether or not the response should include notes info.
        /// </param>
        /// <returns>
        /// A Task&lt;<see cref="BasePost"/>[]&gt; that can be used to track the operation. If the task succeeds, the Task&lt;<see cref="BasePost"/>[]&gt;.Result"/> will
        /// carry an array of <see cref="BasePost">posts</see>. Otherwise <see cref="Exception"/> will carry a <see cref="TumblrException"/>
        /// representing the error occurred during the call.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object has been disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// This <see cref="TumblrClient"/> instance does not have an OAuth token specified.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <list type="bullet">
        /// <item>
        ///		<description>
        ///			<paramref name="sinceId"/> is less than 0.
        ///		</description>
        ///	</item>
        /// <item>
        ///		<description>
        ///			<paramref name="startIndex"/> is less than 0.
        ///		</description>
        ///	</item>
        ///	<item>
        ///		<description>
        ///			<paramref name="count"/> is less than 1 or greater than 20.
        ///		</description>
        ///	</item>
        /// </list>
        /// </exception>
        public Task<BasePost[]> GetDashboardPostsAsync(long sinceId = 0, long startIndex = 0, int count = 20, PostType type = PostType.All, bool includeReblogInfo = false, bool includeNotesInfo = false)
        {
            if (disposed)
                throw new ObjectDisposedException("TumblrClient");

            if (sinceId < 0)
                throw new ArgumentOutOfRangeException("sinceId", "sinceId must be greater or equal to zero.");

            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "startIndex must be greater or equal to zero.");

            if (count < 1 || count > 20)
                throw new ArgumentOutOfRangeException("count", "count must be between 1 and 20.");

            if (OAuthToken == null)
                throw new InvalidOperationException("GetDashboardPostsAsync method requires an OAuth token to be specified.");

            MethodParameterSet parameters = new MethodParameterSet();
            parameters.Add("type", type.ToString().ToLowerInvariant(), "all");
            parameters.Add("since_id", sinceId, 0);
            parameters.Add("offset", startIndex, 0);
            parameters.Add("limit", count, 0);
            parameters.Add("reblog_info", includeReblogInfo, false);
            parameters.Add("notes_info", includeNotesInfo, false);

            return CallApiMethodAsync<PostCollection, BasePost[]>(
              new UserMethod("dashboard", OAuthToken, HttpMethod.Get, parameters),
              r => r.Posts,
              CancellationToken.None);
        }

        /// <summary>
        /// Asynchronously retrieves posts from the current user's dashboard.
        /// </summary>
        /// See:  http://www.tumblr.com/docs/en/api/v2#m-ug-dashboard
        /// <param name="Id">
        ///  Returns posts that appeared either after or before the given Id. after or before The <paramref name="drashboardType"/> parameter takes a number.
        /// </param>
        /// <param name="drashboardType">
        /// <see cref="DashboardOption.After"/> returns newer posts,
        /// <see cref="DashboardOption.Before"/> returns older posts
        /// </param>
        /// <param name="startIndex">
        /// The post number to start at.
        /// </param>
        /// <param name="count">
        /// The number of posts to return.
        /// </param>
        /// <param name="type">
        /// The <see cref="PostType"/> to return.
        /// </param>
        /// <param name="includeReblogInfo">
        /// Whether or not the response should include reblog info.
        /// </param>
        /// <param name="includeNotesInfo">
        /// Whether or not the response should include notes info.
        /// </param>
        /// <returns>
        /// A Task&lt;<see cref="BasePost"/>[]&gt; that can be used to track the operation. If the task succeeds, the Task&lt;<see cref="BasePost"/>[]&gt;.Result"/> will
        /// carry an array of <see cref="BasePost">posts</see>. Otherwise <see cref="Exception"/> will carry a <see cref="TumblrException"/>
        /// representing the error occurred during the call.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object has been disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// This <see cref="TumblrClient"/> instance does not have an OAuth token specified.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <list type="bullet">
        /// <item>
        ///		<description>
        ///			<paramref name="Id"/> is less than 0.
        ///		</description>
        ///	</item>
        /// <item>
        ///		<description>
        ///			<paramref name="startIndex"/> is less than 0.
        ///		</description>
        ///	</item>
        ///	<item>
        ///		<description>
        ///			<paramref name="count"/> is less than 1 or greater than 20.
        ///		</description>
        ///	</item>
        /// </list>
        /// </exception>
        public Task<BasePost[]> GetDashboardPostsAsync(long Id, DashboardOption drashboardType, long startIndex = 0, int count = 20, PostType type = PostType.All, bool includeReblogInfo = false, bool includeNotesInfo = false)
        {
            if (disposed)
                throw new ObjectDisposedException("TumblrClient");

            if (Id < 0)
                throw new ArgumentOutOfRangeException("ID", "Id must be greater or equal to zero.");

            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "startIndex must be greater or equal to zero.");

            if (count < 1 || count > 20)
                throw new ArgumentOutOfRangeException("count", "count must be between 1 and 20.");

            if (OAuthToken == null)
                throw new InvalidOperationException("GetDashboardPostsAsync method requires an OAuth token to be specified.");

            MethodParameterSet parameters = new MethodParameterSet();

            parameters.Add("type", type.ToString().ToLowerInvariant(), "all");

            switch (drashboardType)
            {
                case DashboardOption.Before:
                    parameters.Add("before_id", Id, 0);
                    break;
                case DashboardOption.After:
                    parameters.Add("after_id", Id, 0);
                    break;
            }

            parameters.Add("offset", startIndex, 0);
            parameters.Add("limit", count, 0);
            parameters.Add("reblog_info", includeReblogInfo, false);
            parameters.Add("notes_info", includeNotesInfo, false);

            return CallApiMethodAsync<PostCollection, BasePost[]>(
                new UserMethod("dashboard", OAuthToken, HttpMethod.Get, parameters),
                r => r.Posts,
                CancellationToken.None);
        }

        #endregion

        #region GetUserLikesAsync

        /// <summary>
        /// Asynchronously retrieves the current user's likes.
        /// </summary>
        /// <remarks>
        /// See: http://www.tumblr.com/docs/en/api/v2#m-ug-likes
        /// </remarks>
        /// <param name="startIndex">
        /// The offset at which to start retrieving the likes. Use 0 to start retrieving from the latest like.
        /// </param>
        /// <param name="count">
        /// The number of likes to retrieve. Must be between 1 and 20.
        /// </param>
        /// <returns>
        /// A <see cref="Task{Likes}"/> that can be used to track the operation. If the task succeeds, the <see cref="Task{Likes}.Result"/> will
        /// carry a <see cref="Likes"/> instance. Otherwise <see cref="Task.Exception"/> will carry a <see cref="TumblrException"/>
        /// representing the error occurred during the call.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// The object has been disposed.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// This <see cref="TumblrClient"/> instance does not have an OAuth token specified.
        /// </exception>
        /// <exception cref="System.ArgumentOutOfRangeException">
        /// <list type="bullet">
        /// <item>
        ///		<description>
        ///			<paramref name="startIndex"/> is less than 0.
        ///		</description>
        ///	</item>
        ///	<item>
        ///		<description>
        ///			<paramref name="count"/> is less than 1 or greater than 20.
        ///		</description>
        ///	</item>
        /// </list>
        /// </exception>
        public Task<Likes> GetUserLikesAsync(int startIndex = 0, int count = 20)
        {
            if (disposed)
                throw new ObjectDisposedException("TumblrClient");

            if (startIndex < 0)
                throw new ArgumentOutOfRangeException("startIndex", "startIndex must be greater or equal to zero.");

            if (count < 1 || count > 20)
                throw new ArgumentOutOfRangeException("count", "count must be between 1 and 20.");

            if (OAuthToken == null)
                throw new InvalidOperationException("GetBlogLikesAsync method requires an OAuth token to be specified.");

            MethodParameterSet parameters = new MethodParameterSet();
            parameters.Add("offset", startIndex, 0);
            parameters.Add("limit", count, 0);

            return CallApiMethodAsync<Likes>(
              new UserMethod("likes", OAuthToken, HttpMethod.Get, parameters),
              CancellationToken.None);
        }

        #endregion

        #region GetUserLimitsAsync

        /// <summary>
        /// Use this method to retrieve information about the various limits for the current user.
        /// </summary>
        /// <returns>A <see cref="Task"/> retrieve a <see cref="UserLimits"/> object</returns>
        /// <exception cref="ObjectDisposedException"></exception>
        public Task<UserLimits> GetUserLimitsAsync()
        {
            if (disposed)
                throw new ObjectDisposedException("TumblrClient");

            MethodParameterSet parameters = new MethodParameterSet();

            return CallApiMethodAsync<UserLimitsRawResponse, UserLimits>(
                new UserMethod("limits", OAuthToken, HttpMethod.Get, parameters),
                x => x.User,
                CancellationToken.None
                );
        }

        #endregion

        #region FilteredContent

        #region const

        private const int maxCountOfFilteredContents = 200;
        private const int maxLengthOfFilteredContent = 250;

        #endregion

        /// <summary>
        /// Get a enumaration of strings as filter
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> of a enumaration of filtered strings</returns>
        public Task<string[]> GetFilteredContent()
        {
            return CallApiMethodAsync<FilteredContentResponse, string[]>(
              new UserMethod("filtered_content", OAuthToken, HttpMethod.Get, null),
              t => t.FilteredContent,
              CancellationToken.None);
        }

        /// <summary>
        /// adding new content filters
        /// </summary>
        /// <param name="filteredContents">a list filter to be set</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to track the operation. If the task succeeds, the new filter was set.
        /// Otherwise <see cref="Task.Exception"/> will carry a <see cref="TumblrException"/>
        /// representing the error occurred during the call.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="filteredContents" />must be none null
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <list type="bullet">
        ///         <item>
        ///             <description>
        ///                 <paramref name="filteredContents" />items in the list of filters must not be empty
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <description>
        ///                 <paramref name="filteredContents" />items in the list of filters must not be whitespaces
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <description>
        ///                 <paramref name="filteredContents" />each filtered string cannot be more than 250 characters in length
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <description>
        ///                 <paramref name="filteredContents" />the maximum of filtered strings is 200
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <description>
        ///                 <paramref name="filteredContents" />already exist
        ///             </description>
        ///         </item>
        ///     </list>
        /// </exception>
        public async Task SetFilteredContentAsync(IEnumerable<string> filteredContents)
        {
            if (filteredContents == null)
                throw new ArgumentNullException(nameof(filteredContents));

            var currentFilteredContents = await GetFilteredContent();

            if (currentFilteredContents.Count() + filteredContents.Count() > maxCountOfFilteredContents)
            {
                string expMessage = string.Format("Each user can have a maximum of {0} filtered strings. You have only {1} free.",
                    maxCountOfFilteredContents,
                    maxCountOfFilteredContents - currentFilteredContents.Count());

                throw new ArgumentException(expMessage, nameof(filteredContents));
            }

            MethodParameterSet parameters = new MethodParameterSet();

            int i = 0;

            foreach (var filteredContent in filteredContents)
            {
                if (filteredContent.Length == 0)
                {
                    throw new ArgumentException(nameof(filteredContents));
                }

                if (filteredContent.Trim().Length == 0)
                {
                    throw new ArgumentException(nameof(filteredContents));
                }

                if (filteredContent.Length > maxLengthOfFilteredContent)
                {
                    string expMessage = string.Format("The filterstring at position {0} of the List {1} is longer than the allowed length of {2} characters.",
                        i,
                        nameof(filteredContents),
                        maxLengthOfFilteredContent);

                    throw new ArgumentException(expMessage, nameof(filteredContents));
                }

                // !!! Tumblr has problems with whitespaces at the beginning of the filters
                parameters.Add($"filtered_content[{i}]", filteredContent.Trim());

                i++;
            }

            await CallApiMethodNoResultAsync(
              new UserMethod("filtered_content", OAuthToken, HttpMethod.Post, parameters),
              CancellationToken.None);
        }

        /// <summary>
        /// adding new content filters
        /// </summary>
        /// <param name="filteredContent">filter to be set</param>
        /// <returns>
        /// A <see cref="Task"/> that can be used to track the operation. If the task succeeds, the new filter was set.
        /// Otherwise <see cref="Task.Exception"/> will carry a <see cref="TumblrException"/>
        /// representing the error occurred during the call.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="filteredContent" />must be none null
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     <list type="bullet">
        ///         <item>
        ///             <description>
        ///                 <paramref name="filteredContent" />must not be empty
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <description>
        ///                 <paramref name="filteredContent" />must not be whitespaces
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <description>
        ///                 <paramref name="filteredContent" />each filtered string cannot be more than 250 characters in length
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <description>
        ///                 <paramref name="filteredContent" />the maximum of filtered strings is 200
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <description>
        ///                 <paramref name="filteredContent" />already exist
        ///             </description>
        ///         </item>
        ///     </list>
        /// </exception>
        public async Task SetFilteredContentAsync(string filteredContent)
        {
            if (filteredContent == null)
                throw new ArgumentNullException(nameof(filteredContent));

            if (filteredContent.Length == 0)
                throw new ArgumentException("must not be empty", nameof(filteredContent));

            if (filteredContent.Trim().Length == 0)
                throw new ArgumentException("must not be whitespaces", nameof(filteredContent));

            if (filteredContent.Length > maxLengthOfFilteredContent)
            {
                string expMessage = string.Format("Each filtered string cannot be more than {0} characters in length.", maxLengthOfFilteredContent);
                throw new ArgumentException(expMessage, nameof(filteredContent));
            }

            var currentFilteredContents = await GetFilteredContent();

            if (currentFilteredContents.Count() + 1 > maxCountOfFilteredContents)
            {
                string expMessage = string.Format("The maximum of {0} filtered strings is reached.", maxCountOfFilteredContents);

                throw new ArgumentException(expMessage, nameof(filteredContent));
            }

            // !!! Tumblr has problems with whitespaces at the beginning of the filters
            filteredContent = filteredContent.Trim();

            // Test is necessary because Tumblr, unlike a list, only throws an exception "BAD REQUEST" when the filter is present.
            foreach (var item in currentFilteredContents)
            {
                if (item == filteredContent)
                {
                    throw new ArgumentException($"{filteredContent} already exist", nameof(filteredContent));
                }
            }


            MethodParameterSet parameters = new MethodParameterSet
            {
                { "filtered_content", filteredContent }
            };

            await CallApiMethodNoResultAsync(
              new UserMethod("filtered_content", OAuthToken, HttpMethod.Post, parameters),
              CancellationToken.None);
        }

        /// <summary>
        /// delete a filter
        /// </summary>
        /// <param name="filteredContent"></param>
        /// <returns>
        ///  A <see cref="Task"/> that can be used to track the operation. If the task succeeds, the filter was delete.
        /// Otherwise <see cref="Task.Exception"/> will carry a <see cref="TumblrException"/>
        /// representing the error occurred during the call.
        /// </returns>
        public async Task DeleteFilteredContent(string filteredContent)
        {
            if (filteredContent == null)
                throw new ArgumentNullException(nameof(filteredContent));

            if (filteredContent.Length == 0)
                throw new ArgumentException(nameof(filteredContent));

            if (filteredContent.Trim().Length == 0)
                throw new ArgumentException(nameof(filteredContent));

            if (filteredContent.Length > maxLengthOfFilteredContent)
            {
                string expMessage = string.Format("Each filtered string cannot be more than {0} characters in length.", maxLengthOfFilteredContent);
                throw new ArgumentException(expMessage, nameof(filteredContent));
            }

            MethodParameterSet parameters = new MethodParameterSet
            {
                { "filtered_content", filteredContent}
            };

            await CallApiMethodNoResultAsync(
              new UserMethod("filtered_content", OAuthToken, HttpMethod.Delete, parameters),
              CancellationToken.None);
        }

        #endregion



        #endregion

        #endregion

        #region IDisposable Implementation

        /// <summary>
        /// Disposes of the object.
        /// </summary>
        /// <param name="disposing">
        /// <b>true</b> if managed resources have to be disposed; otherwise <b>false</b>.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            disposed = true;
            base.Dispose();
        }

        #endregion
    }
}
