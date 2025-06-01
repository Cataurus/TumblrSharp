using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using DontPanic.TumblrSharp.Client;
using Microsoft.Extensions.DependencyInjection;
using AzureFunction;

[assembly: FunctionsStartup(typeof(StartUp))]

namespace AzureFunction
{
    public class StartUp : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.UseTumblrClient();

            builder.Services.AddScoped<IMyTumblrService, MyTumblrService>();
        }
    }
}
