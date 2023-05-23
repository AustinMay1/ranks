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
            
            DateTime today = DateTime.Now;
            // Calculate difference of today - joinedDate
            TimeSpan diff;
            // Returns an IList<IList<object>> for each row in a sheet
            var ranks = Sheets.Connect((string) command.Data.Options.First().Value);

            var embed = new EmbedBuilder()
                .WithAuthor(command.User)
                .WithTitle($"Rank: {command.Data.Options.First().Value}")
                .WithDescription("Members up for promotion!")
                .WithColor(Color.Purple)
                .WithCurrentTimestamp();

            foreach (var row in ranks)
            {
                diff = today - DateTime.Parse((string) row[2]);
                var timeInClan = (int)diff.TotalDays; 
                
                if ((string)row[1] == (string)command.Data.Options.First().Value)
                {
                    if (diff.TotalDays >= Ranks.ranks[(string)command.Data.Options.First().Value])
                    {
                        embed.AddField(new EmbedFieldBuilder() { Name = row[0].ToString(), Value = $"Total Days: {timeInClan.ToString()}", IsInline = true});
                    }
                }
            }

            await command.RespondAsync(embed: embed.Build());
        }
    }
}
