using DiscordBot.Quoting;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Serialization
{
    public class QuoteSettings : IBotSetting
    {
        public List<Quote> quotes { get; set; } = new List<Quote>();

        public string GetSaveName() => "quoteSettings.json";

        public void RequiresSaving() => savingRequired = true;
        public bool GetRequiresSaving() => savingRequired;
        public bool savingRequired;
    }
}
