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
            IDMChannel dmChannel = await message.Author.GetOrCreateDMChannelAsync();

            await dmChannel.SendMessageAsync("Test!");
        }
    }
}
