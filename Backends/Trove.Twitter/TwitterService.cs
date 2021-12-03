using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Trove.Shared.Models;
using Trove.Shared.Services;
using Trove.Twitter.Mappers;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Trove.Twitter
{
    public class TwitterService : ITwitterService
    {
        private readonly ITwitterClient _client;
        private readonly IMemoryCache _memoryCache;
        
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        public TwitterService (ITwitterClient client, IMemoryCache memoryCache)
        {
            _client = client;
            _memoryCache = memoryCache;
        }

        public async Task<NewsItem[]> GetUserTweetsAsync(string username, CancellationToken cancellationToken)
        {
            string cacheKey = $"twitter_{username.ToLower()}";

            try
            {
                await _semaphore.WaitAsync(cancellationToken);

                return await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15);
                    return await GetNewsItems(username, cancellationToken);
                });
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task UpdateTweetsCacheAsync(string username, CancellationToken cancellationToken)
        {
            string cacheKey = $"twitter_{username.ToLower()}";

            try
            {
                await _semaphore.WaitAsync(cancellationToken);

                NewsItem[] newsItems = await GetNewsItems(username, cancellationToken);
                _memoryCache.Set(cacheKey, newsItems, TimeSpan.FromMinutes(15));
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task<NewsItem[]> GetNewsItems(string username, CancellationToken cancellationToken)
        {
            ITweet[] tweets = await _client.Timelines.GetUserTimelineAsync(new GetUserTimelineParameters(username) { ExcludeReplies = true, IncludeRetweets = true, PageSize = 50 });

            NewsItem[] newsItems = tweets.Select(TweetMapper.Map).ToArray();
            return newsItems;
        }
    }
}