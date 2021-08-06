using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using System.IO;
using DiscordBot._555Design;
using DiscordBot.Serialization;
using DiscordBot.Scripting;
using DiscordBot.RSSFeedLink;
using DiscordBot.CustomCommands;
using DiscordBot.TwitchNotify;

namespace DiscordBot
{
    class Program
    {
        public static DiscordSocketClient _client;
        public static SettingsSerialization settings;
        public static CommandManager commandManager;
        public static TwitchNotifier twitchNotifier;

        static string token;
        

        SystemCommands systemCommands;
        TimerCommands timerCommands;
        ScriptingCommands scriptingCommands;
        RSSFeedCommands rssCommands;
        CustomResponseCommands customResponseCommands;
        TwitchNotifierCommands twitchNotifierCommands;



        public Program()
        {
            settings = new SettingsSerialization();

            settings.Deserialize();

            // It is recommended to Dispose of a client when you are finished
            // using it, at the end of your app's lifetime.
            _client = new DiscordSocketClient();

            commandManager = new CommandManager(_client, settings.systemSettings.commandCode);

            _client.Log += LogAsync;
            _client.Ready += ReadyAsync;
            _client.MessageReceived += commandManager.MessageReceivedAsync;

            systemCommands = new SystemCommands();
            systemCommands.RegisterCommands(commandManager);

            timerCommands = new TimerCommands();
            timerCommands.RegisterCommands(commandManager);

            scriptingCommands = new ScriptingCommands();
            scriptingCommands.RegisterCommands(commandManager);

            rssCommands = new RSSFeedCommands();
            rssCommands.RegisterCommands(commandManager);

            twitchNotifierCommands = new TwitchNotifierCommands();
            twitchNotifierCommands.RegisterCommands(commandManager);

            customResponseCommands = new CustomResponseCommands();
            customResponseCommands.RegisterCommands(commandManager);

            twitchNotifier = new TwitchNotifier();
        }

        // Discord.Net heavily utilizes TAP for async, so we create
        // an asynchronous context from the beginning.
        static void Main(string[] args)
        {
            try
            {
                using (var reader = new StreamReader($"{Path.GetDirectoryName(typeof(Program).Assembly.Location)}\\token.token"))
                {
                    token = reader.ReadLine();
                }
            } catch (FileNotFoundException) {
                Console.WriteLine("Input Token:");
                token = Console.ReadLine();

                using (var writer = new StreamWriter($"{Path.GetDirectoryName(typeof(Program).Assembly.Location)}\\token.token"))
                {
                    writer.WriteLine(token);
                }
            }

            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync()
        {
            // Tokens should be considered secret data, and never hard-coded.
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            // Block the program until it is closed.
            await Task.Delay(Timeout.Infinite);
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        // The Ready event indicates that the client has opened a
        // connection and it is now safe to access the cache.
        private Task ReadyAsync()
        {
            Console.WriteLine($"{_client.CurrentUser} is connected!");

            return Task.CompletedTask;
        }

        ~Program()
        {
            _client.Dispose();
        }
    }
}