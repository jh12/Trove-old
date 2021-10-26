using System;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Trove.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            IConfiguration config = CreateConfig();

            IHostBuilder hostBuilder = Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureAppConfiguration(builder =>
                {
                    string? configurationPath = Environment.GetEnvironmentVariable("configurationpath");

                    if (!string.IsNullOrEmpty(configurationPath))
                        builder.SetBasePath(configurationPath);

                    builder.AddConfiguration(config);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

            return hostBuilder;
        }

        private static IConfiguration CreateConfig()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();

            builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }
    }
}
