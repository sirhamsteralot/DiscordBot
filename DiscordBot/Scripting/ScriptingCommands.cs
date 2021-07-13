using Discord.WebSocket;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Scripting;

namespace DiscordBot.Scripting
{
    public class ScriptingCommands :ICommandGroup
    {
        public void RegisterCommands(CommandManager manager)
        {
            manager.AddCommand("eval", Eval);
        }

        public async Task Eval(SocketMessage message)
        {
            string arg = message.Content[(Program.commandManager.CommandStart.Length)..];

            try
            {
                var result = await ScriptManager.ExecuteScript(arg);
                await message.Channel.SendMessageAsync(result?.ToString());

            }
            catch (CompilationErrorException ex)
            {
                string response = $"Error executing script!\n" +
                        $"```{ex.Message}```";

                await message.Channel.SendMessageAsync(response);
            }
        }
    }
}
