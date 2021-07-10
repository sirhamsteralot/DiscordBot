using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class SystemCommands : ICommandGroup
    {
        StringBuilder responseBuilder = new StringBuilder();

        public void RegisterCommands(Dictionary<string, Func<SocketMessage, Task>> commands)
        {
            commands.Add("ping", PongCommand);
        }

        public async Task PongCommand(SocketMessage message)
        {
                await message.Channel.SendMessageAsync("Pong");
        }
    }
}
