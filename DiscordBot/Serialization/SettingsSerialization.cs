using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DiscordBot.Serialization
{
    public class SettingsSerialization
    {
        public SystemSettings systemSettings;

        readonly string settingsPath = "Settings";

        public SettingsSerialization()
        {

        }

        public SettingsSerialization(string settingsPath)
        {
            this.settingsPath = settingsPath;
        }

        public void Deserialize()
        {
            if (Directory.Exists(settingsPath))
            {
                string systemSettingsPath = settingsPath + '/' + "systemsettings.json";
                if (File.Exists(systemSettingsPath))
                {
                    systemSettings = JsonSerializer.Deserialize<SystemSettings>(File.ReadAllText(systemSettingsPath));
                } else
                {
                    systemSettings = new SystemSettings();
                }
            }
        }

        public async void SerializeAsync()
        {
            if (!Directory.Exists(settingsPath))
                Directory.CreateDirectory(settingsPath);

            string fileName = settingsPath + '/' + "systemsettings.json";
            using FileStream createStream = File.Create(fileName);
            await JsonSerializer.SerializeAsync(createStream, systemSettings);
            await createStream.DisposeAsync();
        }
    }
}
