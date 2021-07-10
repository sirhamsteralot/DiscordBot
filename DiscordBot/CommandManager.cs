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
        string CommandStart = "H@";


        public CommandManager(DiscordSocketClient client)
        {
            _client = client;
        }

        public async Task MessageReceivedAsync(SocketMessage message)
        {
            // The bot should never respond to itself.
            if (message.Author.Id == _client.CurrentUser.Id)
                return;

            if (message.Content.StartsWith(CommandStart))
            {

            }
        }
    }
}
