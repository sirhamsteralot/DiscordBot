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
                int nr;
                if (int.TryParse(argumentPart, out nr))
                {
                    if (nr >= Program.settings.quoteSettings.quotes.Count || nr < 0)
                    {
                        await message.Channel.SendMessageAsync("Quote doesnt exist!");
                        return;
                    }

                    await message.Channel.SendMessageAsync($"{nr}: {Program.settings.quoteSettings.quotes[nr].quote}");
                    return;
                }

                Quote toadd = new Quote
                {
                    quote = argumentPart,
                    date = message.Timestamp
                };

                var settings = Program.settings.quoteSettings;
                settings.quotes.Add(toadd);
                settings.RequiresSaving();
                Program.settings.SerializeAsync(false);

                await message.Channel.SendMessageAsync("Quote Added!");
                return;
            }

            int random = r.Next(Program.settings.quoteSettings.quotes.Count);
            Quote quote = Program.settings.quoteSettings.quotes[random];

            await message.Channel.SendMessageAsync($"{random}: {quote.quote}");
        }

        public async Task RemoveQuoteCommand (SocketMessage message)
        {
            if (!PermissionsChecker.IsMessageFromTrustedUser(message))
                return;

            string argumentPart = message.Content.Substring("removequote".Length + Program.settings.systemSettings.commandCode.Length);

            if (argumentPart != "")
            {
                int nr;
                if (int.TryParse(argumentPart, out nr))
                {
                    if (nr >= Program.settings.quoteSettings.quotes.Count || nr < 0)
                    {
                        await message.Channel.SendMessageAsync("Quote doesnt exist!");
                        return;
                    }
                    Program.settings.quoteSettings.quotes.RemoveAt(nr);
                    Program.settings.quoteSettings.RequiresSaving();
                    Program.settings.SerializeAsync(false);

                    await message.Channel.SendMessageAsync($"removed quote: {nr}");
                    return;
                }

                int count = Program.settings.quoteSettings.quotes.RemoveAll(x => x.quote == argumentPart);
                Program.settings.quoteSettings.RequiresSaving();
                Program.settings.SerializeAsync(false);

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
