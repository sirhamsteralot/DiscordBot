using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.TwitchNotify
{
    public class TwitchChannel
    {
        public string ChannelName { get; set; }
        public bool LastStatus { get; set; }
        public ulong channelId { get; set; }
    }
}
