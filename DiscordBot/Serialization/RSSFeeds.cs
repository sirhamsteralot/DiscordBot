using System;
using System.Collections.Generic;
using System.Text;
using DiscordBot.RSSFeedLink;

namespace DiscordBot.Serialization
{
    public class RSSFeeds
    {
        public List<RSSFeed> trackedFeeds { get; set; } = new List<RSSFeed>();
        public int CheckMillis { get; set; } = 60000;

    }
}
