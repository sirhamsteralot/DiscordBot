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

        public void RegisterCommands(CommandManager manager)
        {
            manager.AddCommand("ping", PongCommand);
            manager.AddCommand("ly", LYCommand);
            manager.AddCommand("save", SaveSettingsCommand);
        }

        public async Task PongCommand(SocketMessage message)
        {
            await message.Channel.SendMessageAsync("Pong!");
        }

        public async Task LYCommand(SocketMessage message)
        {
            await message.Channel.SendMessageAsync("I love you");
        }

        public async Task SaveSettingsCommand(SocketMessage message)
        {
            Program.settings.SerializeAsync();
        }
    }
}
