using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Trove.Api.Controllers
{
    [ApiController]
    [Route("rss/twitter")]
    public class TwitterController : ControllerBase
    {
        [HttpGet("{username}/tweets")]
        public async Task<IActionResult> GetTweets(string username, CancellationToken cancellationToken)
        {
            return Ok(new
            {
                Username = username
            });
        }
    }
}