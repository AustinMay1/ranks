using Discord;
using Discord.WebSocket;
using RanksBot.Logging;
using RanksBot.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace RanksBot
{
    public class Program
    {
        private readonly IServiceProvider _serviceProvider;

        public Program() 
        {
            _serviceProvider = CreateProvider();
        }
        
        static void Main(string[] args) => new Program().RunAsync(args).GetAwaiter().GetResult();

        static IServiceProvider CreateProvider()
        {
            var collection = new ServiceCollection();

            return collection.BuildServiceProvider();
        }

        static IServiceProvider CreateServices()
        {
            var config = new DiscordSocketConfig() {  };

            var collection = new ServiceCollection()
                .AddSingleton(config)
                .AddSingleton<DiscordSocketClient>()
                ;

            return collection.BuildServiceProvider();
        }

        private async Task RunAsync(string[] args)
        {
            var client = CreateServices().GetRequiredService<DiscordSocketClient>();

            new LoggingService(client);
            new CommandService(client);
            
            var token = File.ReadAllText("token.txt");

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
                                            
            await Task.Delay(Timeout.Infinite);
        }
    }
}