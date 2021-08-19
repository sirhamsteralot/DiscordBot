using DiscordBot.TwitchNotify;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Serialization
{
    public class TwitchNotifySettings : IBotSetting
    {
        public int twitchPollingDelay { get; set; } = 60000;

        public HashSet<TwitchChannel> twitchChannels { get; set; } = new HashSet<TwitchChannel>();

        public string GetSaveName() => "twitchSettings.json";

        public void RequiresSaving() => savingRequired = true;
        public bool GetRequiresSaving() => savingRequired;
        public bool savingRequired;
    }
}
