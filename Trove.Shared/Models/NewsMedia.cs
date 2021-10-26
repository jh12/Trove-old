using System;

namespace Trove.Shared.Models
{
    public class NewsMedia
    {
        public MediaType Type { get; }
        public int UrlStart { get; }
        public int UrlEnd { get; }
        public Uri Url { get; }

        public NewsMedia(MediaType type, int urlStart, int urlEnd, Uri url)
        {
            Type = type;
            UrlStart = urlStart;
            UrlEnd = urlEnd;
            Url = url;
        }

        public enum MediaType
        {
            Image,
            Animation,
            Video
        }
    }
}