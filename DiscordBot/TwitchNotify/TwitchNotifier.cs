﻿using Discord.WebSocket;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using TwitchLib.Client;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;

namespace DiscordBot.TwitchNotify
{
    public class TwitchNotifier
    {
        Timer timer;

        public TwitchNotifier()
        {
            timer = new Timer(CheckForLive, this, 60000, Program.settings.twitchSettings.twitchPollingDelay);
        }
        

        public async void CheckForLive(object state)
        {
            try
            {
                if (Program.settings.systemSettings.twitchAuthorizationCode == "")
                    return;

                foreach (var channel in Program.settings.twitchSettings.twitchChannels)
                {
                    string Url = "https://api.twitch.tv/helix/streams?user_login=" + channel.ChannelName;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

                    request.Method = "Get";
                    request.Timeout = 5000;
                    request.Headers.Add("Client-ID", "u363xalk2xef1g4miyom7wc7zq3fzn");
                    request.Headers.Add("Authorization", "Bearer " + Program.settings.systemSettings.twitchAuthorizationCode);

                    using (var s = request.GetResponse().GetResponseStream())
                    {
                        using (var sr = new System.IO.StreamReader(s))
                        {
                            bool newStatus = false;

                            var jsonObject = JObject.Parse(sr.ReadToEnd());

                            var theValue = jsonObject.SelectToken("data[0].type");
                            if (theValue == null)
                                newStatus = false;
                            else
                                newStatus = theValue.ToString() == "live";

                            if (newStatus && channel.LastStatus != newStatus)
                            {
                                var discordChannel = Program._client.GetChannel(channel.channelId) as ISocketMessageChannel;

                                if (string.IsNullOrEmpty(channel.MentionId))
                                    await discordChannel.SendMessageAsync($"{channel.ChannelName} went Live!\nhttps://twitch.tv/{channel.ChannelName}");
                                else
                                    await discordChannel.SendMessageAsync($"<@&{channel.MentionId}> {channel.ChannelName} went Live!\nhttps://twitch.tv/{channel.ChannelName}");
                            }

                            channel.LastStatus = newStatus;

                            Program.settings.twitchSettings.RequiresSaving();
                            Program.settings.SerializeAsync(false);
                        }
                    }
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

    }
}
