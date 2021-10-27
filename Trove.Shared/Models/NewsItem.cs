using System;

namespace Trove.Shared.Models
{
    public class NewsItem
    {
        public string Id { get; }
        public Uri Source { get; }

        public NewsCreator Creator { get; }
        public string Title { get; }
        public string Content { get; }

        public DateTimeOffset CreatedAt { get; }

        public NewsMedia[] Media { get; }

        public NewsItem(string id, Uri source, NewsCreator creator, string title, string content, DateTimeOffset createdAt, NewsMedia[] media)
        {
            Id = id;
            Source = source;
            Creator = creator;
            Title = title;
            Content = content;
            CreatedAt = createdAt;
            Media = media;
        }
    }
}