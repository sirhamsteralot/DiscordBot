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
        public QuoteSettings quoteSettings;
        public RemindMeSettings remindMeSettings;

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

                string quoteSettingsPath = completeSettingsPath + '/' + "quoteSettings.json";

                if (File.Exists(quoteSettingsPath))
                {
                    try
                    {
                        quoteSettings = JsonSerializer.Deserialize<QuoteSettings>(File.ReadAllText(quoteSettingsPath));
                    }
                    catch (JsonException)
                    {
                        quoteSettings = new QuoteSettings();
                    }
                }
                else
                {
                    quoteSettings = new QuoteSettings();
                }

                string remindMeSettingsPath = completeSettingsPath + '/' + "remindMeSettings.json";

                if (File.Exists(remindMeSettingsPath))
                {
                    try
                    {
                        remindMeSettings = JsonSerializer.Deserialize<RemindMeSettings>(File.ReadAllText(remindMeSettingsPath));
                    }
                    catch (JsonException)
                    {
                        remindMeSettings = new RemindMeSettings();
                    }
                }
                else
                {
                    remindMeSettings = new RemindMeSettings();
                }
            } else
            {
                remindMeSettings = new RemindMeSettings();
                quoteSettings = new QuoteSettings();
                twitchSettings = new TwitchNotifySettings();
                customResponses = new CustomResponseSettings();
                systemSettings = new SystemSettings();
                rssFeeds = new RSSFeeds();
            }
        }

        public async void SerializeAsync(bool saveAll = true)
        {
            string completeSettingsPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + '/' + settingsPath;

            if (!Directory.Exists(completeSettingsPath))
                Directory.CreateDirectory(completeSettingsPath);

            if (saveAll || systemSettings.GetRequiresSaving())
            {
                string fileName = completeSettingsPath + '/' + systemSettings.GetSaveName();
                using FileStream createStream = File.Create(fileName);
                await JsonSerializer.SerializeAsync(createStream, systemSettings);
                await createStream.DisposeAsync();
            }

            if (saveAll || rssFeeds.GetRequiresSaving())
            {
                string fileName = completeSettingsPath + '/' + rssFeeds.GetSaveName();
                using FileStream createStreamRss = File.Create(fileName);
                await JsonSerializer.SerializeAsync(createStreamRss, rssFeeds);
                await createStreamRss.DisposeAsync();
            }

            if (saveAll || customResponses.GetRequiresSaving())
            {
                string fileName = completeSettingsPath + '/' + customResponses.GetSaveName();
                using FileStream createCRstream = File.Create(fileName);
                await JsonSerializer.SerializeAsync(createCRstream, customResponses);
                await createCRstream.DisposeAsync();
            }

            if (saveAll || twitchSettings.GetRequiresSaving())
            {
                string fileName = completeSettingsPath + '/' + twitchSettings.GetSaveName();
                using FileStream createTNstream = File.Create(fileName);
                await JsonSerializer.SerializeAsync(createTNstream, twitchSettings);
                await createTNstream.DisposeAsync();
            }

            if (saveAll || quoteSettings.GetRequiresSaving())
            {
                string fileName = completeSettingsPath + '/' + quoteSettings.GetSaveName();
                using FileStream createQSstream = File.Create(fileName);
                await JsonSerializer.SerializeAsync(createQSstream, quoteSettings);
                await createQSstream.DisposeAsync();
            }

            if (saveAll || remindMeSettings.GetRequiresSaving())
            {
                string fileName = completeSettingsPath + '/' + remindMeSettings.GetSaveName();
                using FileStream createRSstream = File.Create(fileName);
                await JsonSerializer.SerializeAsync(createRSstream, remindMeSettings);
                await createRSstream.DisposeAsync();
            }
        }
    }
}
