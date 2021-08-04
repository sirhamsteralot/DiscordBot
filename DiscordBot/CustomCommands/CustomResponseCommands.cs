using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DiscordBot.Serialization;

namespace DiscordBot.CustomCommands
{
    public class CustomResponseCommands : ICommandGroup
    {
        StringBuilder sb = new StringBuilder();

        public void RegisterCommands(CommandManager manager)
        {
            manager.AddCommand("addresponse", AddResponseCommand, "adds a response, usage: addresponse *response_name* *response_content*");
            manager.AddCommand("removeresponse", RemoveResponseCommand, "removes a response, usage: removeresponse *response_name*");
            manager.AddCommand("listcustom", ListCustomCommandsCommand, "lists the custom commands");

        }

        public async Task ListCustomCommandsCommand(SocketMessage message)
        {
            sb.AppendLine("custom responses:");

            foreach (var response in Program.settings.customResponses.responses)
            {
                sb.AppendLine(response.Name);
            }

            await message.Channel.SendMessageAsync(sb.ToString());

            sb.Clear();
        }

        public async Task AddResponseCommand(SocketMessage message)
        {
            if (!PermissionsChecker.IsMessageFromTrustedUser(message))
                return;

            string[] splitCommand = message.Content.Split(' ');

            if (splitCommand.Length < 3)
                await message.Channel.SendMessageAsync("Not enough arguments!");

            CustomResponse response = new CustomResponse();
            response.Name = splitCommand[1];

            for (int i = 2; i < splitCommand.Length; i++) {
                sb.Append(splitCommand[i]).Append(' ');
            }

            response.Content = sb.ToString();
            sb.Clear();

            Program.settings.customResponses.responses.Add(response);

            Program.settings.SerializeAsync();
            await message.Channel.SendMessageAsync("Response Added!");
        }

        public async Task RemoveResponseCommand(SocketMessage message)
        {
            if (!PermissionsChecker.IsMessageFromTrustedUser(message))
                return;

            string[] splitCommand = message.Content.Split(' ');

            if (splitCommand.Length < 2)
                await message.Channel.SendMessageAsync("Not enough arguments!");

            CustomResponse toRemove = null;

            foreach (var response in Program.settings.customResponses.responses)
            {
                if (response.Name == splitCommand[1])
                {
                    toRemove = response;
                    break;
                }
            }

            if (toRemove != null)
            {
                Program.settings.customResponses.responses.RemoveWhere(x => x.Name == splitCommand[1]);
                await message.Channel.SendMessageAsync("Removed!");
                return;
            }

            await message.Channel.SendMessageAsync("Could not find message to remove!");
        }
    }
}
