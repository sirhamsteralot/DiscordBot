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
                await message.Channel.SendMessageAsync("Could not parse the timespan of your message!");
                return;
            }



            

            IDMChannel dmChannel = await message.Author.GetOrCreateDMChannelAsync();

            await dmChannel.SendMessageAsync($"Test!\n{timeSpan)}");
        }
    }
}
