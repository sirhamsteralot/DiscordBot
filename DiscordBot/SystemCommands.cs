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
        CommandManager _manager;

        public void RegisterCommands(CommandManager manager)
        {
            manager.AddCommand("searchquotes", SearchQuotesInChannel, "searches quotes in this channel, usage: searchquotes *frommessageid*");
            manager.AddCommand("ping", PongCommand, "pongs");
            manager.AddCommand("save", SaveSettingsCommand, "saves settings to server");
            manager.AddCommand("setcommandcode", SetCommandCode, "sets a new command code, usage: setcommandcode *code*");
            manager.AddCommand("addtrusteduser", AddTrustedUserCommand, "Adds a trusted user, usage: addtrusteduser *id*");
            manager.AddCommand("botbanuser", AddBannedUserCommand, "Bans a user from using the bot, usage: banuser *id*");
            manager.AddCommand("botunbanuser", RemoveBannedUserCommand, "Unbans a user from the bot, usage: banuser *id*");
            manager.AddCommand("listbannedids", ListBannedUsersCommand, "Lists the banned user id's from the bot");
            manager.AddCommand("getauthorid", GetAuthorIDCommand, "Gets the id of the person using this command.");
            manager.AddCommand("shutdown", ShutdownCommand, "Shuts down the bot");
            manager.AddCommand("help", HelpCommand, "Shows a list of all commands, use Help *command name* to get detailed information about a command.");
            _manager = manager;
        }

        public async Task SearchQuotesInChannel(SocketMessage command)
        {
            if (!PermissionsChecker.IsMessageFromTrustedUser(command))
                return;

            ulong startMessageId = ulong.Parse(command.Content.Trim());

            responseBuilder.AppendLine("quotes added in this channel: ").AppendLine();

            await foreach (var page in command.Channel.GetMessagesAsync(startMessageId, Discord.Direction.After, int.MaxValue))
            {
                foreach (var message in page)
                {
                    if (message.Content.StartsWith("!quote "))
                        responseBuilder.AppendLine(message.Content);
                }
            }

            await command.Channel.SendMessageAsync(responseBuilder.ToString());
            responseBuilder.Clear();
        }

        public async Task HelpCommand(SocketMessage message)
        {
            string[] split = message.Content.Split(' ');

            if (split.Length > 1)
            {
                // return help for the commandname
                string helptext;
                if (_manager.HelpText.TryGetValue(split[1], out helptext))
                {
                    responseBuilder.Append("Showing Help for command: ").AppendLine(split[1]);
                    responseBuilder.AppendLine(helptext);
                } else
                {
                    responseBuilder.AppendLine($"Could not find help for command {split[1]}, command not registered or help does not exist for command");
                }

                await message.Channel.SendMessageAsync(responseBuilder.ToString());
                responseBuilder.Clear();
                return;
            }

            // return list of commands

            responseBuilder.AppendLine("Showing all available commands, use Help *command name* to get detailed information about a command.");

            foreach (var commandName in _manager.Commands.Keys)
            {
                responseBuilder.AppendLine($"{commandName}");
            }

            await message.Channel.SendMessageAsync(responseBuilder.ToString());
            responseBuilder.Clear();
            return;
        }

        public async Task PongCommand(SocketMessage message)
        {
            await message.Channel.SendMessageAsync("Pong!");
        }

        public async Task ShutdownCommand(SocketMessage message)
        {
            if (!PermissionsChecker.IsMessageFromTrustedUser(message))
                return;

            await message.Channel.SendMessageAsync("Bye!");
            Environment.Exit(0);
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
                Program.settings.systemSettings.RequiresSaving();
                Program.settings.SerializeAsync(false);
                await message.Channel.SendMessageAsync("added trusted user and saved!");
                return;
            }

            await message.Channel.SendMessageAsync("You are not hamster!");
        }

        public async Task AddBannedUserCommand(SocketMessage message)
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

                Program.settings.systemSettings.bannedUsers.Add(result);
                Program.settings.systemSettings.RequiresSaving();
                Program.settings.SerializeAsync(false);
                await message.Channel.SendMessageAsync("banned user and saved!");
                return;
            }

            await message.Channel.SendMessageAsync("You are not hamster!");
        }

        public async Task RemoveBannedUserCommand(SocketMessage message)
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

                if (Program.settings.systemSettings.bannedUsers.RemoveWhere(x => x == result) > 0)
                {
                    Program.settings.systemSettings.RequiresSaving();
                    Program.settings.SerializeAsync(false);
                    await message.Channel.SendMessageAsync("unbanned user and saved!");
                }

                await message.Channel.SendMessageAsync("failed to find and/or unban user!");

                return;
            }

            await message.Channel.SendMessageAsync("You are not hamster!");
        }

        public async Task ListBannedUsersCommand(SocketMessage message)
        {
            responseBuilder.AppendLine("List of user ID's banned from the bot:");

            foreach (var user in Program.settings.systemSettings.bannedUsers)
            {
                responseBuilder.AppendLine(user.ToString());
            }

            await message.Channel.SendMessageAsync(responseBuilder.ToString());
            responseBuilder.Clear();
        }

        public async Task SetCommandCode(SocketMessage message) {
            if (!PermissionsChecker.IsMessageFromTrustedUser(message))
                return;

            string[] parts = message.Content.Split(' ');

            if (parts.Length > 1)
            {
                Program.settings.systemSettings.commandCode = parts[1];

                Program.commandManager.CommandStart = Program.settings.systemSettings.commandCode;

                Program.settings.systemSettings.RequiresSaving();
                Program.settings.SerializeAsync(false);

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
