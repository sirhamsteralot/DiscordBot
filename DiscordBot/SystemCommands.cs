using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot
{
    public class SystemCommands
    {
        StringBuilder responseBuilder = new StringBuilder();


        public async void PongCommand(SocketMessage message)
        {
            if (message.Content == "!ping")
                await message.Channel.SendMessageAsync("Pong");
        }
    }
}
