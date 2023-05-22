using Discord;
using Discord.WebSocket;

namespace RanksBot
{
    public class Program
    {
        private DiscordSocketClient _client = null!;
        private LoggingService _log = null!;
        private Commands _commands = null!;
        
        
        public static Task Main(string[] args) => new Program().MainAsync();

        private async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            _log = new LoggingService(_client);
            _commands = new Commands(_client);
            
            _client.SlashCommandExecuted += SlashCommandHandler;

            var token = File.ReadAllText("token.txt");

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
                                            
            await Task.Delay(-1);
        }

        private async Task SlashCommandHandler(SocketSlashCommand command)
        {
            if (command.Data.Name == "ping")
            {
                await command.RespondAsync($"You executed {command.Data.Name}\npong!");
            }
            else if (command.Data.Name == "fetch-ranks")
            {
                Sheets.Connect();
                await command.RespondAsync("Ranks fetched to console!");
            }
            else
            {
                await command.RespondAsync("Howdy!");
            }
        }
    }
}