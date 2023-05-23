using Discord;
using Discord.WebSocket;
using RanksBot.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RanksBot.Commands
{
    public class CommandHandler
    {
        public static async Task Ping(SocketSlashCommand command)
        {
            await command.RespondAsync("pong!");
        }

        public static async Task FetchRanks(SocketSlashCommand command)
        {
            Sheets.Connect((string) command.Data.Options.First().Value);

            var embed = new EmbedBuilder()
                .WithAuthor(command.User)
                .WithTitle($"Rank: {command.Data.Options.First().Value}\nCheck console.")
                .WithDescription("Members up for promotion!")
                .WithColor(Color.Purple)
                .WithCurrentTimestamp();


            await command.RespondAsync(embed: embed.Build());
        }
    }
}
