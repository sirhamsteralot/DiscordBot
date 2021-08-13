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
            commandManager.AddCommand("remindme", RemindMeCommand, "reminds");

        }

        public async Task RemindMeCommand(SocketMessage message)
        {
            string[] split = message.Content.Split(' ');

            TimeSpan timeSpan;
            int lastArgumentIndex;

            if (!RemindMe.ParseTime(split, out timeSpan, out lastArgumentIndex))
            {
                await message.Channel.SendMessageAsync("Could not parse the timespan of your message!\nMake sure every part of the timespan is split like: ``!remindme 10 d 5 h 20 m whatever the reminder is for``");
                return;
            }



            

            IDMChannel dmChannel = await message.Author.GetOrCreateDMChannelAsync();

            await dmChannel.SendMessageAsync($"Test!\n{timeSpan}");
        }
    }
}
