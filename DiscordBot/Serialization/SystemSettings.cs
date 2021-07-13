using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Serialization
{
    public class SystemSettings
    {
        public string commandCode = "H@";

        public List<ulong> trustedUsers = new List<ulong>();
    }
}
