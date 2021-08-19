using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Serialization
{
    public interface IBotSetting
    {
        public string GetSaveName();
        public void RequiresSaving();
        public bool GetRequiresSaving();
    }
}
