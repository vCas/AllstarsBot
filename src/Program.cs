using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

namespace AllstarsBot {
    internal class Program {
        static void Main() {

            var token = Environment.GetEnvironmentVariable("DISCORDTOKEN")?.Trim();
            new Program().RunBotAsync(token).GetAwaiter().GetResult();
        }
        
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

        private async Task RunBotAsync(string token) {
            _client = new DiscordSocketClient();
            _commands = new CommandService();

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            _client.Log += _client_Log;

            await RegisterCommandsAsync();
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }

        private Task _client_Log(LogMessage arg) {
            Console.WriteLine(arg.Message);
            return Task.CompletedTask;
        }

        private async Task RegisterCommandsAsync() {
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task HandleCommandAsync(SocketMessage arg) {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);
            if (message.Author.IsBot) return;

            var argPos = 0;
            if (message.HasStringPrefix("!", ref argPos)) {
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess) Console.WriteLine(result.ErrorReason);
                if (result.Error.Equals(CommandError.UnmetPrecondition))
                    await message.Channel.SendMessageAsync(result.ErrorReason);
            }
        }
    }
}