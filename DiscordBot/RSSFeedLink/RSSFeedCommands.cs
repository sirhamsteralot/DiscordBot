using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace DiscordBot.RSSFeedLink
{
    public class RSSFeedCommands
    {
        RSSReader rssreader = new RSSReader();


        public void RegisterCommands(CommandManager manager)
        {
            manager.AddCommand("registerrss", RegisterRSSLinkCommand);
            manager.AddCommand("triggerrss", TriggerRSS);
            manager.AddCommand("postlastrss", PostLastRss);
        }

        public async Task RegisterRSSLinkCommand(SocketMessage message)
        {
            if (!PermissionsChecker.IsMessageFromTrustedUser(message))
                return;

            RSSFeed feed = new RSSFeed();

            string[] arguments = message.Content.Split(' ');

            if (arguments.Length < 3)
            {
                await message.Channel.SendMessageAsync("Not enough arguments!");
            }

            var guildChannel = message.Channel as SocketGuildChannel;
            foreach (var channel in guildChannel.Guild.TextChannels)
            {
                if (channel.Name == arguments[1])
                {
                    feed.channelId = channel.Id;
                    break;
                }
            }

            feed.rssURL = arguments[2];
            Program.settings.rssFeeds.trackedFeeds.Add(feed);
            Program.settings.SerializeAsync();

            await message.Channel.SendMessageAsync($"RSS Feed added\n channel: {message.Channel.Name}\n URL: {arguments[2]}");
        }

        public async Task TriggerRSS(SocketMessage message)
        {
            if (!PermissionsChecker.IsMessageFromTrustedUser(message))
                return;

            await message.Channel.SendMessageAsync("Triggering!");
            rssreader.CheckRSS(null);
        }

        public async Task PostLastRss(SocketMessage message)
        {
            if (!PermissionsChecker.IsMessageFromTrustedUser(message))
                return;

            string[] split = message.Content.Split(' ');

            await message.Channel.SendMessageAsync("Checking RSS!");

            rssreader.PostLastRSS(split[1], message);
        }
    }
}
