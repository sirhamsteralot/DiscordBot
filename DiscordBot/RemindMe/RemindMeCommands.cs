using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.RemindMe
{
    public class RemindMeCommands : ICommandGroup
    {
        public void RegisterCommands(CommandManager commandManager)
        {
            commandManager.AddCommand("remindme", RemindMeCommand, "reminds a user of something after a certain timespan, usage: !remindme {time} {message}");

        }

        public async Task RemindMeCommand(SocketMessage message)
        {
            string[] split = message.Content.Split(' ');

            TimeSpan timeSpan;
            int lastArgumentIndex;

            if (!RemindMe.ParseTime(split, out timeSpan, out lastArgumentIndex))
            {
                await message.Channel.SendMessageAsync("Could not parse the timespan of your message!\nMake sure every part of the timespan is split like: `!remindme 10 d 5 h 20 m whatever the reminder is for`");
                return;
            }


            string remindermessage = "";
            for (int i = lastArgumentIndex + 1; i < split.Length; i++)
            {
                remindermessage += split[i] + " ";
            }

            remindermessage = remindermessage.Trim();
            

            IDMChannel dmChannel = await message.Author.GetOrCreateDMChannelAsync();

            RemindMeItem reminder = new RemindMeItem()
            {
                reminder = remindermessage,
                responseChannel = message.Author.Id,
                reminderTime = DateTime.UtcNow + timeSpan
            };

            Program.settings.remindMeSettings.remindMeItems.Add(reminder);
            Program.settings.remindMeSettings.RequiresSaving();
            Program.settings.SerializeAsync(false);


            await dmChannel.SendMessageAsync($"Reminding you of `{remindermessage}` after\n{timeSpan}");
        }
    }
}
