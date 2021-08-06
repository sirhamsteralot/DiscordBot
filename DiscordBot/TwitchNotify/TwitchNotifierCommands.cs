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
                    Program.settings.SerializeAsync();

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

                Program.settings.SerializeAsync();
                await message.Channel.SendMessageAsync("added channel to follow!");
                return;
            }

            await message.Channel.SendMessageAsync("missing arguments");
        }
    }
}
