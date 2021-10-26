using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Trove.Shared.Models;
using Trove.Twitter.Mappers;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Models.V2;
using Tweetinvi.Parameters;
using Tweetinvi.Parameters.V2;

namespace Trove.Twitter
{
    public class TwitterService
    {
        private readonly ITwitterClient _client;

        public TwitterService(ITwitterClient client)
        {
            _client = client;
        }

        public async Task<NewsItem[]> GetUserTweetsAsync(string username, CancellationToken cancellationToken)
        {            
            ITweet[] tweets = await _client.Timelines.GetUserTimelineAsync(new GetUserTimelineParameters(username)
            {
                ExcludeReplies = true,
                PageSize = 50
            });

            return tweets.Select(TweetMapper.Map).ToArray();
        }
    }
}