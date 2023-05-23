using Discord;
using Discord.WebSocket;
using RanksBot.Logging;
using RanksBot.Commands;

namespace RanksBot
{
    public class Program
    {
        private DiscordSocketClient _client = null!;
        
        public static Task Main(string[] args) => new Program().MainAsync();

        private async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            new LoggingService(_client);
            new CommandService(_client);
            
            var token = File.ReadAllText("token.txt");

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
                                            
            await Task.Delay(-1);
        }
    }
}