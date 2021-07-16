using System;
using System.Collections.Generic;
using System.Text;
using DiscordBot.CustomCommands;

namespace DiscordBot.Serialization
{
    public class CustomResponseSettings
    {
        public HashSet<CustomResponse> responses { get; set; } = new HashSet<CustomResponse>();
    }
}
