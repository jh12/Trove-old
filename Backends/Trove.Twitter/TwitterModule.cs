using Autofac;
using Microsoft.Extensions.Configuration;
using Tweetinvi;
using Tweetinvi.Logic;
using Tweetinvi.Models;

namespace Trove.Twitter
{
    public class TwitterModule : Module
    {
        private readonly IConfiguration _configuration;

        public TwitterModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            IConfigurationSection twitterSection = _configuration.GetSection("Twitter");

            if (!twitterSection.Exists())
            {
                // TODO: Log warning
                return;
            }

            var credentials = new ReadOnlyConsumerCredentials(twitterSection["Key"], twitterSection["Secret"], "AAAAAAAAAAAAAAAAAAAAALh2VAEAAAAAIIUA4KHoz4cKHrGWrhH9j%2BY3X6s%3DEIpzZe0pLJhfZ2nIKFYhwxk051Qi0WsVCuSsnmT0agU57ZePvL");

            TwitterClient twitterClient = new TwitterClient(credentials);
            var readOnlyTwitterCredentials = twitterClient.Credentials;

            builder.RegisterInstance(twitterClient).As<ITwitterClient>().SingleInstance();

            // TODO: Make per request
            builder.RegisterType<TwitterService>().SingleInstance();
        }
    }
}