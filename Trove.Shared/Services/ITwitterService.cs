using System.Threading;
using System.Threading.Tasks;
using Trove.Shared.Models;

namespace Trove.Shared.Services
{
    public interface ITwitterService
    {
        Task<NewsItem[]> GetUserTweetsAsync(string username, CancellationToken cancellationToken);
        Task UpdateTweetsCacheAsync(string username, CancellationToken cancellationToken);
    }
}