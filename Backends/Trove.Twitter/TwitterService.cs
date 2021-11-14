using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Trove.Shared.Models;
using Trove.Twitter.Mappers;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Trove.Twitter
{
    public class TwitterService
    {
        private readonly ITwitterClient _client;
        private readonly IMemoryCache _memoryCache;

        public TwitterService(ITwitterClient client, IMemoryCache memoryCache)
        {
            _client = client;
            _memoryCache = memoryCache;
        }

        public async Task<NewsItem[]> GetUserTweetsAsync(string username, CancellationToken cancellationToken)
        {
            string cacheKey = $"twitter_{username.ToLower()}";

            async Task<NewsItem[]> GetNewsItems(ICacheEntry entry)
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15);

                ITweet[] tweets = await _client.Timelines.GetUserTimelineAsync(new GetUserTimelineParameters(username) { ExcludeReplies = true, IncludeRetweets = true, PageSize = 50 });

                NewsItem[] newsItems = tweets.Select(TweetMapper.Map).ToArray();
                return newsItems;
            }

            return await _memoryCache.GetOrCreateAsync(cacheKey, GetNewsItems);
        }
    }
}