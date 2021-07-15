using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class CommandManager
    {
        private readonly DiscordSocketClient _client;

        public string CommandStart { get; set; }

        public Dictionary<string, Func<SocketMessage, Task>> Commands { get; set; }
        public Dictionary<string, string> HelpText { get; set; }


        public CommandManager(DiscordSocketClient client, string commandStart)
        {
            Commands = new Dictionary<string, Func<SocketMessage, Task>>();
            HelpText = new Dictionary<string, string>();
            CommandStart = commandStart;

            _client = client;
        }

        public void AddCommand(string commandName, Func<SocketMessage, Task> action)
        {
            Commands.Add(commandName, action);
        }
        public void AddCommand(string commandName, Func<SocketMessage, Task> action, string helpText)
        {
            AddCommand(commandName, action);
            AddHelpText(commandName, helpText);
        }

        public void AddHelpText(string commandName, string helpText)
        {
            HelpText.Add(commandName, helpText);
        }

        public async Task MessageReceivedAsync(SocketMessage message)
        {
            // The bot should never respond to itself.
            if (message.Author.Id == _client.CurrentUser.Id)
                return;

            if (message.Content.StartsWith(CommandStart))
            {
                try
                {
                    await ProcessCommandMessage(message);
                } catch (Exception e)
                {
                    string response = $"Error executing command!\n" +
                        $"```{e.Message}```";

                    await message.Channel.SendMessageAsync(response);
                }
                
            }
        }

        private async Task ProcessCommandMessage(SocketMessage message)
        {
            string command = message.Content[(CommandStart.Length)..];
            command = command.Split(' ')[0];
            command = command.ToLower();

            Func<SocketMessage, Task> commandAction;
            if (!Commands.TryGetValue(command, out commandAction))
            {
                await message.Channel.SendMessageAsync($"{message.Author.Mention}, Command \"{command}\" not found!");
                return;
            }

            await commandAction(message);
        }
    }
}
