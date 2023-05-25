using Discord;
using Discord.WebSocket;
using RanksBot.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Discord.Interactions;

namespace RanksBot
{
    public class Program
    {
        private readonly IServiceProvider _services;
        private readonly IConfiguration _configuration;

        public Program() 
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("settings.json", optional: false)
                .Build();

            _services = new ServiceCollection()
                .AddSingleton(_configuration)
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
                .AddSingleton<InteractionHandler>()
                .BuildServiceProvider();
        }
        
        static void Main(string[] args) => new Program().RunAsync().GetAwaiter().GetResult();

        private async Task RunAsync()
        {
            var client = _services.GetRequiredService<DiscordSocketClient>();

            await _services.GetRequiredService<InteractionHandler>().InitializeAsync();

            new LoggingService(client);
            
            await client.LoginAsync(TokenType.Bot, _configuration["token"]);
            await client.StartAsync();
                                            
            await Task.Delay(Timeout.Infinite);
        }

        public static bool IsDebug()
        {
            #if DEBUG
                return true;
            #else
                return false;
            #endif
        }
    }
}