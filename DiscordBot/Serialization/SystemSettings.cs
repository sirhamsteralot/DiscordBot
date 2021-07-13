using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Serialization
{
    public class SystemSettings
    {
        public string commandCode = "H@";

        public HashSet<ulong> trustedUsers = new HashSet<ulong>();
    }
}
