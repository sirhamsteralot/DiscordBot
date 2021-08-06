using DiscordBot.CustomCommands;
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
        public RSSFeeds rssFeeds;
        public CustomResponseSettings customResponses;
        public TwitchNotifySettings twitchSettings;

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
                    try
                    {
                        systemSettings = JsonSerializer.Deserialize<SystemSettings>(File.ReadAllText(systemSettingsPath));
                    }
                    catch (JsonException)
                    {
                        systemSettings = new SystemSettings();
                    }

                } else
                {
                    systemSettings = new SystemSettings();
                }

                string rssfeedsPath = completeSettingsPath + '/' + "rssfeeds.json";

                if (File.Exists(rssfeedsPath))
                {
                    try
                    {
                        rssFeeds = JsonSerializer.Deserialize<RSSFeeds>(File.ReadAllText(rssfeedsPath));
                    }
                    catch (JsonException) 
                        {
                        rssFeeds = new RSSFeeds();
                    }
                }
                else
                {
                    rssFeeds = new RSSFeeds();
                }

                string customResponsesPath = completeSettingsPath + '/' + "customresponses.json";

                if (File.Exists(customResponsesPath))
                {
                    try
                    {
                        customResponses = JsonSerializer.Deserialize<CustomResponseSettings>(File.ReadAllText(customResponsesPath));
                    }
                    catch (JsonException)
                    {
                        customResponses = new CustomResponseSettings();
                    }
                }
                else
                {
                    customResponses = new CustomResponseSettings();
                }

                string twitchSettingsPath = completeSettingsPath + '/' + "twitchSettings.json";

                if (File.Exists(twitchSettingsPath))
                {
                    try
                    {
                        twitchSettings = JsonSerializer.Deserialize<TwitchNotifySettings>(File.ReadAllText(twitchSettingsPath));
                    }
                    catch (JsonException)
                    {
                        twitchSettings = new TwitchNotifySettings();
                    }
                }
                else
                {
                    twitchSettings = new TwitchNotifySettings();
                }
            } else
            {
                twitchSettings = new TwitchNotifySettings();
                customResponses = new CustomResponseSettings();
                systemSettings = new SystemSettings();
                rssFeeds = new RSSFeeds();
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

            fileName = completeSettingsPath + '/' + "rssfeeds.json";
            using FileStream createStreamRss = File.Create(fileName);
            await JsonSerializer.SerializeAsync(createStreamRss, rssFeeds);
            await createStreamRss.DisposeAsync();

            fileName = completeSettingsPath + '/' + "customresponses.json";
            using FileStream createCRstream = File.Create(fileName);
            await JsonSerializer.SerializeAsync(createCRstream, customResponses);
            await createCRstream.DisposeAsync();

            fileName = completeSettingsPath + '/' + "twitchSettings.json";
            using FileStream createTNstream = File.Create(fileName);
            await JsonSerializer.SerializeAsync(createTNstream, twitchSettings);
            await createCRstream.DisposeAsync();
        }
    }
}
