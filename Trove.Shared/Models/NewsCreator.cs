using System;

namespace Trove.Shared.Models
{
    public class NewsCreator
    {
        public string Name { get; }
        public string Description { get; }
        public Uri Url { get; }
        public Uri ProfileImage { get; }

        public NewsCreator(string name, string description, Uri url, Uri profileImage)
        {
            Name = name;
            Description = description;
            Url = url;
            ProfileImage = profileImage;
        }
    }
}