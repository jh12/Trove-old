using System;
using System.Collections.Concurrent;
using System.Linq;
using Trove.Shared.Models;
using Tweetinvi.Models;
using Tweetinvi.Models.Entities;

namespace Trove.Twitter.Mappers
{
    internal static class TweetMapper
    {
        private static readonly ConcurrentDictionary<long, NewsCreator> _userIdToCreator = new ConcurrentDictionary<long, NewsCreator>();

        internal static NewsItem Map(ITweet tweet)
        {
            return new NewsItem(
                tweet.IdStr,
                new Uri(tweet.Url),
                Map(tweet.CreatedBy),
                tweet.Text, // TODO: Trim
                tweet.Text,
                tweet.CreatedAt.UtcDateTime,
                tweet.Media.Select(Map).ToArray());
        }

        private static NewsCreator Map(IUser user)
        {
            if (_userIdToCreator.TryGetValue(user.Id, out NewsCreator creator))
                return creator;

            return new NewsCreator(user.Name, user.Description, new Uri(user.Url), new Uri(user.ProfileImageUrlFullSize));
        }

        private static NewsMedia Map(IMediaEntity media)
        {
            NewsMedia.MediaType mediaType = media.MediaType switch
            {
                "photo" => NewsMedia.MediaType.Image,
                "animated_gif" => NewsMedia.MediaType.Animation,
                "video" => NewsMedia.MediaType.Video,
                _ => throw new ArgumentOutOfRangeException(media.MediaType)
            };

            return new NewsMedia(mediaType, media.Indices[0], media.Indices[1], new Uri(media.MediaURLHttps));
        }
    }
}