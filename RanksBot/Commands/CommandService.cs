using Discord;
using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RanksBot.Commands
{
    public class CommandService
    {
        private DiscordSocketClient _client = null!;
        private SlashCommandBuilder[] _commands = null!;

        public CommandService(DiscordSocketClient client)
        {
            _client = client;
            _client.Ready += Client_Ready;
            _client.SlashCommandExecuted += SlashCommandHandler;
        }

        public async Task Client_Ready()
        {
            var guild = _client.GetGuild(1031999653094363177);

            var ranksCommand = new SlashCommandBuilder()
                .WithName("ranks")
                .WithDescription("fetch the specified ranks")
                    .AddOption(new SlashCommandOptionBuilder()
                    .WithName("member-ranks")
                    .WithDescription("select the rank to query")
                    .WithRequired(true)
                        .AddChoice("recruit", "Recruit")
                        .AddChoice("corporal", "Corporal")
                        .AddChoice("donator", "Sapphire")
                    .WithType(ApplicationCommandOptionType.String));
          
            var globalCommand = new SlashCommandBuilder();

            globalCommand.WithName("hi");
            globalCommand.WithDescription("Hello");

            try
            {
                await guild.CreateApplicationCommandAsync(ranksCommand.Build());
                await guild.CreateApplicationCommandAsync(globalCommand.Build());
            }
            catch (HttpException e)
            {
                var json = JsonConvert.SerializeObject(e.Errors, Formatting.Indented);

                Console.WriteLine(json);
            }

        }

        private async Task SlashCommandHandler(SocketSlashCommand command)
        {
            switch (command.Data.Name)
            {
                case "ping":
                    await CommandHandler.Ping(command);
                    break;
                case "pong":
                    await command.RespondAsync("ping!");
                    break;
                case "ranks":
                    // Sheets.Connect();
                    await CommandHandler.FetchRanks(command);
                    break;
                case "hi":
                    await command.RespondAsync("Howdy!");
                    break;
                default:
                    await command.RespondAsync("Invalid Command!");
                    break;
            }
        }
    }
}
