using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Ingaia.Challenge.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((webBuilder, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false);
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                var port = Environment.GetEnvironmentVariable("PORT");

                webBuilder.UseStartup<Startup>()
                    .UseUrls($"http://*:{port}");
            });
    }
}
