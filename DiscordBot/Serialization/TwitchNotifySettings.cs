using DiscordBot.TwitchNotify;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Serialization
{
    public class TwitchNotifySettings
    {
        public int twitchPollingDelay { get; set; } = 60000;

        public HashSet<TwitchChannel> twitchChannels { get; set; } = new HashSet<TwitchChannel>();
    }
}
