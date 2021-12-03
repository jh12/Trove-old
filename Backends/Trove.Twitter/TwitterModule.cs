using Autofac;
using Microsoft.Extensions.Hosting;
using Serilog;
using Trove.Shared.Services;
using Trove.Twitter.Services;
using Tweetinvi;
using Tweetinvi.Models;

namespace Trove.Twitter
{
    public class TwitterModule : Module
    {
        private readonly TwitterOptions _options;
        private readonly ILogger _logger;

        public TwitterModule(TwitterOptions options, ILogger logger)
        {
            _options = options;
            _logger = logger.ForContext<TwitterModule>();
        }

        protected override void Load(ContainerBuilder builder)
        {
            if (_options == null)
            {
                _logger.Error("No options defined for section \"Twitter\"");

                return;
            }

            var credentials = new ReadOnlyConsumerCredentials(_options.Key, _options.Secret, _options.BearerToken);

            TwitterClient twitterClient = new TwitterClient(credentials);
            var readOnlyTwitterCredentials = twitterClient.Credentials;

            builder.RegisterInstance(twitterClient).As<ITwitterClient>().SingleInstance();

            builder.RegisterType<TwitterService>().As<ITwitterService>().SingleInstance();
            builder.RegisterType<TwitterCacheRefresherService>().As<IHostedService>().SingleInstance(); 
        }
    }
}