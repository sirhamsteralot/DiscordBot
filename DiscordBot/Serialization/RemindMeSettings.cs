using DiscordBot.RemindMe;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Serialization
{
    public class RemindMeSettings
    {
        public List<RemindMeItem> remindMeItems { get; set; } = new List<RemindMeItem>();
    }
}
