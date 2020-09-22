using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.AzureAppServices;
using Microsoft.IdentityModel.Logging;

namespace BrewViewServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IdentityModelEventSource.ShowPII = true;
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureLogging(builder =>
                {
                    builder.AddAzureWebAppDiagnostics();
                    builder.AddApplicationInsights();
                })
                .ConfigureServices(collection =>
                {
                    collection.Configure<AzureFileLoggerOptions>(options =>
                        {
                            options.FileSizeLimit = 5 * 1024;
                            options.RetainedFileCountLimit = 5;
                        })
                        .Configure<AzureBlobLoggerOptions>(options => options.BlobName = "log.txt");
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseStartup<Startup>();
                });
        }
    }
}