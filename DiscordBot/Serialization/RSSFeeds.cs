using System;
using System.Collections.Generic;
using System.Text;
using DiscordBot.RSSFeedLink;

namespace DiscordBot.Serialization
{
    public class RSSFeeds : IBotSetting
    {
        public List<RSSFeed> trackedFeeds { get; set; } = new List<RSSFeed>();
        public int CheckMillis { get; set; } = 60000;

        public string GetSaveName() => "rssfeeds.json";

        public void RequiresSaving() => savingRequired = true;
        public bool GetRequiresSaving() => savingRequired;
        public bool savingRequired;
    }
}
