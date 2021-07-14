using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Discord.WebSocket;

namespace DiscordBot.RSSFeedLink
{
    public class RSSReader
    {
        List<RSSFeed> TrackedFeeds { get => Program.settings.rssFeeds.trackedFeeds; }
        Timer checkTimer;
        StringBuilder messageBuilder = new StringBuilder();


        public RSSReader()
        {
            checkTimer = new Timer(CheckRSS, null, 6000, Program.settings.rssFeeds.CheckMillis);
        }

        public async void CheckRSS(object state)
        {
            foreach (var trackedFeed in TrackedFeeds)
            {
                XmlReader reader = XmlReader.Create(trackedFeed.rssURL);
                SyndicationFeed feed = SyndicationFeed.Load(reader);
                reader.Close();

                bool first = true;

                foreach (SyndicationItem item in feed.Items)
                {
                    if (item.Id == trackedFeed.lastGUID)
                        break;

                    if (first)
                    {
                        trackedFeed.lastGUID = item.Id;
                        first = false;
                    }

                    await PrintRSS(item, trackedFeed);
                    await Task.Delay(1000);
                }
            }
        }

        public async Task PrintRSS(SyndicationItem item, RSSFeed feed)
        {
            messageBuilder.AppendLine($"**{item.Title}**");
            messageBuilder.AppendLine($"{item.Summary}");
            messageBuilder.AppendLine();

            foreach( var link in item.Links)
                messageBuilder.AppendLine($"{link.Uri}");

            var channel = Program._client.GetChannel(feed.channelId) as ISocketMessageChannel;
            await channel.SendMessageAsync(messageBuilder.ToString());
            messageBuilder.Clear();
        }
    }
}
