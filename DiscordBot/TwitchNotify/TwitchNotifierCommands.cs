using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.TwitchNotify
{
    public class TwitchNotifierCommands : ICommandGroup
    {
        public void RegisterCommands(CommandManager manager)
        {
            manager.AddCommand("settwitchpollingdelay", SetTwitchPollingDelay, "sets the polling delay for twitch. usage: settwitchpollingdelay {delay(ms)}");
            manager.AddCommand("addtwitchchannel", AddChannel, "adds a twitch channel to give live notifications on. usage: addtwitchchannel {channelname} {discord channel}");
            manager.AddCommand("removetwitchchannel", RemoveChannel, "removes a twitch channel to check. usage: removeChannel {channelname}");
            manager.AddCommand("livecheck", LiveCheck, "triggers a live check");
            manager.AddCommand("setauthcode", SetAuthorizationCode, "sets the twitch 0auth code to make requests to the twitch api, usage: setauthcode {code}");
        }

        private async Task SetAuthorizationCode(SocketMessage message)
        {
            if (!PermissionsChecker.IsMessageFromTrustedUser(message))
                return;

            string[] split = message.Content.Split(' ');

            if (split.Length > 1)
            {
                Program.settings.systemSettings.twitchAuthorizationCode = split[1];
                Program.settings.systemSettings.RequiresSaving();
                Program.settings.SerializeAsync(false);
                await message.Channel.SendMessageAsync($"set 0auth code!");
                return;
            }

            await message.Channel.SendMessageAsync("missing arguments");
        }

        private async Task RemoveChannel(SocketMessage message)
        {
            if (!PermissionsChecker.IsMessageFromTrustedUser(message))
                return;

            string[] split = message.Content.Split(' ');

            if (split.Length > 1)
            {
                int removed = Program.settings.twitchSettings.twitchChannels.RemoveWhere(x => x.ChannelName == split[1]);
                Program.settings.twitchSettings.RequiresSaving();
                Program.settings.SerializeAsync(false);
                await message.Channel.SendMessageAsync($"removed {removed} channels to follow!");
                return;
            }

            await message.Channel.SendMessageAsync("missing arguments");
        }

        public async Task LiveCheck(SocketMessage message)
        {
            if (!PermissionsChecker.IsMessageFromTrustedUser(message))
                return;

            await message.Channel.SendMessageAsync($"Checking {Program.settings.twitchSettings.twitchChannels.Count} channels!");

            Program.twitchNotifier.CheckForLive(this);
        }

        public async Task SetTwitchPollingDelay(SocketMessage message)
        {
            if (!PermissionsChecker.IsMessageFromTrustedUser(message))
                return;


            string[] split = message.Content.Split(' ');

            if (split.Length > 1)
            {
                
                int value;
                if (int.TryParse(split[1], out value))
                {
                    Program.settings.twitchSettings.twitchPollingDelay = value;
                    Program.settings.twitchSettings.RequiresSaving();
                    Program.settings.SerializeAsync(false);

                    await message.Channel.SendMessageAsync("changed value and saved!");
                }


                await message.Channel.SendMessageAsync("Could not parse the delay value!");
                return;
            }

            await message.Channel.SendMessageAsync("Arguments missing!");
        }

        public async Task AddChannel(SocketMessage message)
        {
            if (!PermissionsChecker.IsMessageFromTrustedUser(message))
                return;

            string[] split = message.Content.Split(' ');

            if (split.Length > 2)
            {
                var channel = new TwitchChannel
                {
                    ChannelName = split[1],
                    LastStatus = false
                };

                Program.settings.twitchSettings.twitchChannels.Add(channel);

                var guildChannel = message.Channel as SocketGuildChannel;
                foreach (var discordChannel in guildChannel.Guild.TextChannels)
                {
                    if (discordChannel.Name == split[2])
                    {
                        channel.channelId = discordChannel.Id;
                        break;
                    }
                }

                Program.settings.twitchSettings.RequiresSaving();
                Program.settings.SerializeAsync(false);
                await message.Channel.SendMessageAsync("added channel to follow!");
                return;
            }

            await message.Channel.SendMessageAsync("missing arguments");
        }
    }
}
