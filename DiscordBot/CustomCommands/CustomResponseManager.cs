using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.CustomCommands
{
    public class CustomResponseManager
    {
        private HashSet<CustomResponse> CustomCommandSet => Program.settings.customResponses.responses;

        public async Task ProcessCommandMessage(SocketMessage message, string command)
        {

            foreach (var customCommand in CustomCommandSet)
            {
                if (command != customCommand.Name)
                    continue;

                await message.Channel.SendMessageAsync(customCommand.Content);
                return;
            }

            await message.Channel.SendMessageAsync($"{message.Author.Mention}, Command \"{command}\" not found!");
        }
    }
}
