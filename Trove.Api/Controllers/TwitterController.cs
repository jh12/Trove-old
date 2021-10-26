using System;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Trove.Shared.Models;
using Trove.Twitter;

namespace Trove.Api.Controllers
{
    [ApiController]
    [Route("rss/twitter")]
    public class TwitterController : ControllerBase
    {
        private readonly TwitterService _twitterService;

        public TwitterController(TwitterService twitterService)
        {
            _twitterService = twitterService;
        }

        [HttpGet("{username}/tweets")]
        public async Task<IActionResult> GetTweets(string username, CancellationToken cancellationToken)
        {
            return await CreateFeed(username, item => item.Content, cancellationToken);
        }

        [HttpGet("{username}/tweets/markdown")]
        public async Task<IActionResult> GetTweetsMarkdown(string username, CancellationToken cancellationToken)
        {
            return await CreateFeed(username, FormatAsMarkdown, cancellationToken);
        }

        private async Task<FileContentResult> CreateFeed(string username, Func<NewsItem, string> contentFunc, CancellationToken cancellationToken)
        {
            NewsItem[] newsItems = await _twitterService.GetUserTweetsAsync(username, cancellationToken);

            NewsCreator? creator = newsItems.FirstOrDefault()?.Creator;

            SyndicationFeed feed = new SyndicationFeed($"Tweets by {creator.Name}", creator.Description, creator.Url);

            SyndicationItem[] items = newsItems
                .Select(i => new SyndicationItem(i.Title, contentFunc(i), i.Source, i.Id, i.CreatedAt))
                .ToArray();

            feed.Items = items;

            return await CreateFile(feed);
        }

        private string FormatAsMarkdown(NewsItem newsItem)
        {
            StringBuilder builder = new StringBuilder(newsItem.Content);

            foreach (NewsMedia media in newsItem.Media)
            {
                string linkStr = $"![]({media.Url})";

                if (builder.Capacity < media.UrlStart)
                    builder.Append(linkStr);
                else
                    builder.Insert(media.UrlStart, linkStr);
            }

            return builder.ToString();
        }

        private async Task<FileContentResult> CreateFile(SyndicationFeed feed)
        {
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Encoding = Encoding.UTF8,
                NewLineHandling = NewLineHandling.Entitize,
                Indent = true,
                Async = true
            };

            using (MemoryStream stream = new MemoryStream())
            {
                await using (XmlWriter xmlWriter = XmlWriter.Create(stream, settings))
                {
                    feed.SaveAsRss20(xmlWriter);
                    await xmlWriter.FlushAsync();
                }

                return File(stream.ToArray(), "application/rss+xml; charset=utf-8");
            }
        }
    }
}