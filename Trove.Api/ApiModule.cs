using Autofac;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;

namespace Trove.Api
{
    public class ApiModule : Module
    {
        private readonly IConfiguration _configuration;

        public ApiModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            RegisterLogger(builder);
        }

        private void RegisterLogger(ContainerBuilder containerBuilder)
        {
            LoggerConfiguration builder = new LoggerConfiguration();

            builder
                .ReadFrom
                .Configuration(_configuration);

            builder
                .Enrich.WithUserName()
                .Enrich.WithMachineName()
                .Enrich.WithAssemblyName()
                .Enrich.WithAssemblyVersion()
                .Enrich.FromLogContext();

            Logger logger = builder.CreateLogger();
            Log.Logger = logger;

            containerBuilder.RegisterInstance(logger).As<ILogger>().SingleInstance();
        }
    }
}