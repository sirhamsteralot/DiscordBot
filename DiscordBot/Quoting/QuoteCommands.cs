using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Quoting
{
    public class QuoteCommands : ICommandGroup
    {
        Random r =  new Random();

        public void RegisterCommands(CommandManager commandManager)
        {
            commandManager.AddCommand("quote", QuoteCommand, "gets a quote or adds a quote, usage: quote / quote {quote}");
            commandManager.AddCommand("removequote", RemoveQuoteCommand, "removes a quote, usage: removequote {quote}");
        }

        public async Task QuoteCommand(SocketMessage message)
        {
            string argumentPart = message.Content.Substring("quote".Length + Program.settings.systemSettings.commandCode.Length);

            if (argumentPart != "")
            {
                Quote toadd = new Quote
                {
                    quote = argumentPart,
                    date = message.Timestamp
                };
                Program.settings.quoteSettings.quotes.Add(toadd);
                Program.settings.SerializeAsync();

                await message.Channel.SendMessageAsync("Quote Added!");
                return;
            }

            Quote quote = Program.settings.quoteSettings.quotes[r.Next(Program.settings.quoteSettings.quotes.Count)];

            await message.Channel.SendMessageAsync(quote.quote);
        }

        public async Task RemoveQuoteCommand (SocketMessage message)
        {
            if (!PermissionsChecker.IsMessageFromTrustedUser(message))
                return;

            string argumentPart = message.Content.Substring("removequote".Length + Program.settings.systemSettings.commandCode.Length);

            if (argumentPart != "")
            {
                int count = Program.settings.quoteSettings.quotes.RemoveAll(x => x.quote == argumentPart);
                Program.settings.SerializeAsync();

                if (count > 0)
                {
                    await message.Channel.SendMessageAsync($"{count} matching quotes removed!");
                    return;
                }

                await message.Channel.SendMessageAsync("No matching quotes found!");
                return;
            }

            await message.Channel.SendMessageAsync("Missing Argument!");
        }
    }
}
