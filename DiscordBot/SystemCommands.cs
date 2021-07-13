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
            manager.AddCommand("addtrusteduser", AddTrustedUserCommand);
            manager.AddCommand("getauthorid", GetAuthorIDCommand);
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
        public async Task GetAuthorIDCommand(SocketMessage message)
        {
            await message.Channel.SendMessageAsync(message.Author.Id.ToString());
        }


        public async Task AddTrustedUserCommand(SocketMessage message)
        {
            if (message.Author.Id == 171198424996249601)
            {
                string[] split = message.Content.Split(' ');
                ulong result;

                if (split.Length < 2)
                {
                    await message.Channel.SendMessageAsync("missing argument!");
                    return;
                }

                if (!ulong.TryParse(split[1], out result))
                {
                    await message.Channel.SendMessageAsync($"failed to parse argument [{split[1]}]!");
                    return;
                }

                Program.settings.systemSettings.trustedUsers.Add(result);
                Program.settings.SerializeAsync();
                await message.Channel.SendMessageAsync("added trusted user and saved!");
                return;
            }

            await message.Channel.SendMessageAsync("You are not hamster!");
        }

        public async Task SetCommandCode(SocketMessage message) {
            if (!PermissionsChecker.IsMessageFromTrustedUser(message))
                return;

            string[] parts = message.Content.Split(' ');

            if (parts.Length > 1)
            {
                Program.settings.systemSettings.commandCode = parts[1];

                Program.commandManager.CommandStart = Program.settings.systemSettings.commandCode;

                Program.settings.SerializeAsync();

                await message.Channel.SendMessageAsync("Changed command code and saved!");
                return;
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
