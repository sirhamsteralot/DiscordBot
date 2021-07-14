using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.RSSFeedLink
{
    public class RSSFeed
    {
        public string rssURL { get; set; }
        public string lastGUID { get; set; }
        public ulong channelId { get; set; }
    }
}
