﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using System.IO;
using DiscordBot._555Design;

namespace DiscordBot
{
    class Program
    {
        private readonly DiscordSocketClient _client;
        static string token;
        CommandManager commandManager;

        SystemCommands systemCommands;
        TimerCommands timerCommands;


        public Program()
        {
            

            // It is recommended to Dispose of a client when you are finished
            // using it, at the end of your app's lifetime.
            _client = new DiscordSocketClient();

            commandManager = new CommandManager(_client);

            _client.Log += LogAsync;
            _client.Ready += ReadyAsync;
            _client.MessageReceived += commandManager.MessageReceivedAsync;

            systemCommands = new SystemCommands();
            systemCommands.RegisterCommands(commandManager);

            timerCommands = new TimerCommands();
            timerCommands.RegisterCommands(commandManager);
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
            } catch (FileNotFoundException e) {
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