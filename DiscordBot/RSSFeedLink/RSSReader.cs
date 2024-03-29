﻿using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
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

                foreach (SyndicationItem item in feed.Items)
                {
                    if (item.PublishDate <= trackedFeed.lastPostDate)
                        break;
                    else
                    {
                        trackedFeed.lastPostDate = item.PublishDate;
                        Program.settings.rssFeeds.RequiresSaving();
                        Program.settings.SerializeAsync(false);
                    }

                    await PrintRSS(item, trackedFeed);
                    await Task.Delay(1000);
                }
            }
        }

        public async void PostLastRSS(string url, SocketMessage message)
        {
            XmlReader reader = XmlReader.Create(url);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();

            foreach (var item in feed.Items)
            {
                messageBuilder.AppendLine($"**{item.Title.Text}**");
                messageBuilder.AppendLine($"{SanetizeHTML(item.Summary.Text)}");
                messageBuilder.AppendLine();

                foreach (var link in item.Links)
                    messageBuilder.AppendLine($"{link.Uri}");

                break;
            }

            await message.Channel.SendMessageAsync(messageBuilder.ToString());
            messageBuilder.Clear();
        }

        public async void PostLastRSS(RSSFeed rss)
        {
            XmlReader reader = XmlReader.Create(rss.rssURL);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();

            foreach (var item in feed.Items)
            {
                await PrintRSS(item, rss);
                rss.lastPostDate = item.PublishDate;
                break;
            }
        }

        public async Task PrintRSS(SyndicationItem item, RSSFeed feed)
        {
            messageBuilder.AppendLine($"**{SanetizeHTML(item.Title.Text)}**");
            messageBuilder.AppendLine($"{SanetizeHTML(item.Summary.Text)}");
            messageBuilder.AppendLine();

            foreach( var link in item.Links)
                messageBuilder.AppendLine($"{link.Uri}");

            var channel = Program._client.GetChannel(feed.channelId) as ISocketMessageChannel;
            await channel.SendMessageAsync(messageBuilder.ToString());
            messageBuilder.Clear();
        }

        public string SanetizeHTML(string text)
        {
            
            messageBuilder.Append(HttpUtility.HtmlDecode(text));

            messageBuilder.Replace("<br>", "\n");
            messageBuilder.Replace("<li>", "- ");
            string output = messageBuilder.ToString();
            messageBuilder.Clear();

            output = Regex.Replace(output, "<.*?>", String.Empty);

            return output;
        }
    }
}
