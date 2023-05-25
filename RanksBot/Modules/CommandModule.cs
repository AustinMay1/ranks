using Discord;
using Discord.Interactions;
using RanksBot.Data;

namespace RanksBot.Modules
{
    public class CommandModule : InteractionModuleBase<SocketInteractionContext>
    {
        public InteractionService Commands { get; set; }
        private InteractionHandler _handler;

        public CommandModule(InteractionHandler handler) 
        {
            _handler = handler;
        }

        [SlashCommand("echo", "Repeats user input.")]
        public async Task Echo(string message, bool mention = false) => await RespondAsync(message);

        [SlashCommand("promotions", "List users up for promotion")]
        public async Task Promotions(Titles title)
        {
            DateTime today = DateTime.Now;
            // Calculate difference of today - joinedDate
            TimeSpan diff;
            // Returns an IList<IList<object>> for each row in a sheet
            var ranks = Sheets.Connect(title.ToString());

            var embed = new EmbedBuilder()
                .WithAuthor(Context.User)
                .WithTitle($"Rank: {title}")
                .WithDescription("Members up for promotion!")
                .WithColor(Color.DarkTeal)
                .WithCurrentTimestamp();

            foreach (var row in ranks)
            {
                diff = today - DateTime.Parse((string)row[2]);
                var timeInClan = (int)diff.TotalDays;

                if ((string)row[1] == title.ToString())
                {
                    if (diff.TotalDays >= Ranks.ranks[title.ToString()])
                    {
                        embed.AddField(new EmbedFieldBuilder() { Name = row[0].ToString(), Value = $"Total Days: {timeInClan}", IsInline = true });
                    }
                }
            }

            await RespondAsync(embed: embed.Build());
        }
    }
}
