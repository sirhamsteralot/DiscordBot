using Discord.WebSocket;
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

                        var jsonObject = JObject.Parse(sr.ReadToEnd());
                        

                        // twitch channel is online if stream is not null.
                        bool newStatus = jsonObject?.GetValue("type")?.ToString() ?? "" == "live";

                        if (newStatus && channel.LastStatus != newStatus)
                        {
                            var discordChannel = Program._client.GetChannel(channel.channelId) as ISocketMessageChannel;
                            await discordChannel.SendMessageAsync($"{channel.ChannelName} went Live!/ntwitch.tv/{channel.ChannelName}");
                        }

                        channel.LastStatus = newStatus;
                        Program.settings.SerializeAsync();
                    }
                }
            }
        }
    }
}
