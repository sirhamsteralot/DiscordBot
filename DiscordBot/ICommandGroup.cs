using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    interface ICommandGroup
    {
        void RegisterCommands(CommandManager commandManager);
    }
}
