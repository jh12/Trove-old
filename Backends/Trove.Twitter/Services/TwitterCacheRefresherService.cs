using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using Trove.Shared.Services;

namespace Trove.Twitter.Services
{
    public class TwitterCacheRefresherService : BackgroundService
    {
        private readonly IOptionsMonitor<TwitterOptions> _optionsDelegate;
        private readonly ITwitterService _twitterService;
        private readonly ILogger _logger;

        public TwitterCacheRefresherService(IOptionsMonitor<TwitterOptions> optionsDelegate, ITwitterService twitterService, ILogger logger)
        {
            _optionsDelegate = optionsDelegate;
            _twitterService = twitterService;
            _logger = logger.ForContext<TwitterCacheRefresherService>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    string[] usersToAutoRefresh = _optionsDelegate.CurrentValue.AutoRefresh;
                    TimeSpan refreshInterval = _optionsDelegate.CurrentValue.AutoRefreshInterval;

                    foreach (string user in usersToAutoRefresh)
                    {
                        _logger.Information("Refreshing tweets for {TwitterUser}", user);
                        await _twitterService.UpdateTweetsCacheAsync(user, stoppingToken);
                    }

                    await Task.Delay(refreshInterval, stoppingToken);
                }
            }
            catch (TaskCanceledException)
            {
            }
        }
    }
}