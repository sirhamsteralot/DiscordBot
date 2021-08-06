using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Serialization
{
    public class SystemSettings
    {
        public string commandCode { get; set; } = "H@";

        public string twitchAuthorizationCode { get; set; } = "";

        public HashSet<ulong> trustedUsers { get; set; } = new HashSet<ulong>();
        public HashSet<ulong> bannedUsers { get; set; } = new HashSet<ulong>();
    }
}
