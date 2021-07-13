using Discord.WebSocket;
using Discord.Commands;
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
            manager.AddCommand("setcommandcode", SetCommandCode);
        }

        public async Task PongCommand(SocketMessage message)
        {
            await message.Channel.SendMessageAsync("Pong!");
        }

        public async Task LYCommand(SocketMessage message)
        {
            if (message.Author.Id == 302917497437290496)
                await message.Channel.SendMessageAsync("I love you");
        }

        public async Task SetCommandCode(SocketMessage message) {
            if (!PermissionsChecker.IsSentByDiscordAdministrator(message))
                return;

            string[] parts = message.Content.Split(' ');

            if (parts.Length > 1)
            {
                Program.settings.systemSettings.commandCode = parts[1];

                Program.commandManager.CommandStart = Program.settings.systemSettings.commandCode;

                Program.settings.SerializeAsync();

                await message.Channel.SendMessageAsync("Changed command code and saved!");
            }

            await message.Channel.SendMessageAsync("Arguments missing!");
        }

        public async Task SaveSettingsCommand(SocketMessage message)
        {
            if (!PermissionsChecker.IsSentByDiscordAdministrator(message))
                return;


            Program.settings.SerializeAsync();
            await message.Channel.SendMessageAsync("Saved!");
        }
    }
}
