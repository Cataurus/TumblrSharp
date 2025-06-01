using System;
using System.Net.Http;
using DontPanic.TumblrSharp.OAuth;

namespace DontPanic.TumblrSharp
{
	/// <summary>
	/// Represents a blog API method.
	/// </summary>
	public class BlogMethod : ApiMethod
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BlogMethod"/> class.
		/// </summary>
		/// <param name="blogName">
		/// The name of the blog target of the method call. Can be passed with or without the trailing ".tumblr.com".
		/// </param>
		/// <param name="methodName">
		/// The name of the method to call. The method url will be automatically built using
		/// this information together with the <paramref name="blogName"/>.
		/// </param>
		/// <param name="oAuthToken">
		/// The OAuth <see cref="Token"/> to use for the call. Can be <b>null</b> if the 
		/// method does not require OAuth.
		/// </param>
		/// <param name="httpMethod">
		/// The required <see cref="HttpMethod"/> for the Tumblr API call. Only <see cref="HttpMethod.Get"/> and
		/// <see cref="HttpMethod.Post"/> are supported.
		/// </param>
		/// <param name="parameters">
		/// The parameters for the Tumblr API call. Can be <b>null</b> if the method does not require parameters.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <list type="bullet">
		/// <item>
		///		<description>
		///			<paramref name="blogName"/> is <b>null</b>.
		///		</description>
		///	</item>
		///	<item>
		///		<description>
		///			<paramref name="methodName"/> is <b>null</b>.
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
		///			<paramref name="methodName"/> is empty.
		///		</description>
		///	</item>
		/// </list>
		/// </exception>
		public BlogMethod(
			string blogName, 
			string methodName, 
			Token oAuthToken,
			HttpMethod httpMethod,
			MethodParameterSet parameters = null)
			: base(String.Format("https://api.tumblr.com/v2/blog/{0}/{1}", Validate(blogName), methodName), oAuthToken, httpMethod, parameters)
		{
			if (blogName == null)
				throw new ArgumentNullException(nameof(blogName));

			if (blogName.Length == 0)
				throw new ArgumentException("Blog name cannot be empty.", nameof(blogName));

			if (methodName == null)
				throw new ArgumentNullException("methodName");

			if (methodName.Length == 0)
				throw new ArgumentException("Method name cannot be empty.", nameof(methodName));
		}

		/// <summary>
		/// Validates the specified blog name and formats it as a Tumblr URL if necessary.
		/// </summary>
		/// <param name="blogName">The name of the blog to validate. Cannot be <see langword="null"/> or empty.</param>
		/// <returns>A string representing the blog name formatted as a Tumblr URL if it does not already contain a period.</returns>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="blogName"/> is <see langword="null"/>.</exception>
		/// <exception cref="ArgumentException">Thrown if <paramref name="blogName"/> is an empty string.</exception>
		public static string Validate(string blogName)
		{
            if (blogName == null)
            {
                throw new ArgumentNullException(nameof(blogName));
            }

            if (blogName.Length == 0)
                throw new ArgumentException("Blog name cannot be empty.", nameof(blogName));

			return (blogName.Contains(".")) ? blogName : String.Format("{0}.tumblr.com", blogName);
		}
	}
}
