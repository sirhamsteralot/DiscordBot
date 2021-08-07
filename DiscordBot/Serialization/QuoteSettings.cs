using DiscordBot.Quoting;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Serialization
{
    public class QuoteSettings
    {
        public List<Quote> quotes { get; set; } = new List<Quote>();
    }
}
