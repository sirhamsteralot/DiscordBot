using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            string completeSettingsPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + '/' + settingsPath;

            if (Directory.Exists(completeSettingsPath))
            {
                string systemSettingsPath = completeSettingsPath + '/' + "systemsettings.json";

                if (File.Exists(systemSettingsPath))
                {
                    systemSettings = JsonSerializer.Deserialize<SystemSettings>(File.ReadAllText(systemSettingsPath));
                } else
                {
                    systemSettings = new SystemSettings();
                }
            } else
            {
                systemSettings = new SystemSettings();
            }
        }

        public async void SerializeAsync()
        {
            string completeSettingsPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + '/' + settingsPath;

            if (!Directory.Exists(completeSettingsPath))
                Directory.CreateDirectory(completeSettingsPath);

            string fileName = completeSettingsPath + '/' + "systemsettings.json";
            using FileStream createStream = File.Create(fileName);
            await JsonSerializer.SerializeAsync(createStream, systemSettings);
            await createStream.DisposeAsync();
        }
    }
}
