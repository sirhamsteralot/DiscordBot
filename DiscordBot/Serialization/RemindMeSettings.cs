using DiscordBot.RemindMe;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Serialization
{
    public class RemindMeSettings : IBotSetting
    {
        public List<RemindMeItem> remindMeItems { get; set; } = new List<RemindMeItem>();

        public string GetSaveName() => "remindMeSettings.json";

        public void RequiresSaving() => savingRequired = true;
        public bool GetRequiresSaving() => savingRequired;
        public bool savingRequired;
    }
}
