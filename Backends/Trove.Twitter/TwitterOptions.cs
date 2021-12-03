using System;

namespace Trove.Twitter
{
    public class TwitterOptions
    {
        public const string Twitter = "Twitter";

        public string Key { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public string BearerToken { get; set; } = string.Empty;
        public string[] AutoRefresh { get; set; } = Array.Empty<string>();
        public TimeSpan AutoRefreshInterval { get; set; } = TimeSpan.FromMinutes(15);
    }
}