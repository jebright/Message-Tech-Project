using DailyWatchlistEmails.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace DailyWatchlistEmails
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            using var host = Bootstrap();

            Log.Information("************************ Execution beginning ************************");
            bool result = await DoWork(host.Services);
            if (result)
            {
                Log.Information("************************ Execution completed successfully ************************");
            }
            else
            {
                Log.Information("************************ Execution failed ************************");
            }
        }

        private static async Task<bool> DoWork(IServiceProvider services)
        {
            using var serviceScope = services.CreateScope();
            var provider = serviceScope.ServiceProvider;

            var watchlistEmailService = provider.GetRequiredService<IWatchlistEmailService>();
            return await watchlistEmailService.Execute();
        }

        private static IHost Bootstrap()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            //Setup Serilog
            Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(builder.Build())
                    .Enrich.FromLogContext()
                    .CreateLogger();

            return CreateHostBuilder().Build();
        }

        private static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                {
                    services.AddHttpClient();
                    services.AddTransient<IWatchlistEmailService, WatchlistEmailService>()
                    .AddTransient<IFoolContext, FoolContext>()
                    .AddTransient<IFoolRepository, FoolRepository>();
                })
                .UseSerilog();

        }

    }
}
