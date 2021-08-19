using System;
using System.Collections.Generic;
using System.Text;
using DiscordBot.CustomCommands;

namespace DiscordBot.Serialization
{
    public class CustomResponseSettings : IBotSetting
    {
        public HashSet<CustomResponse> responses { get; set; } = new HashSet<CustomResponse>();

        public string GetSaveName() => "customresponses.json";

        public void RequiresSaving() => savingRequired = true;
        public bool GetRequiresSaving() => savingRequired;
        public bool savingRequired;
    }
}
