using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Serialization
{
    public class SystemSettings : IBotSetting
    {
        public string commandCode { get; set; } = "H@";

        public string twitchAuthorizationCode { get; set; } = "";

        public HashSet<ulong> trustedUsers { get; set; } = new HashSet<ulong>();
        public HashSet<ulong> bannedUsers { get; set; } = new HashSet<ulong>();

        public string GetSaveName() => "systemsettings.json";

        public void RequiresSaving() => savingRequired = true;
        public bool GetRequiresSaving() => savingRequired;
        public bool savingRequired;
    }
}
