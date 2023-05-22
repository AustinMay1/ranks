using Discord;
using Discord.Net;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RanksBot
{
    public class Commands
    {
        private DiscordSocketClient _client = null!;

        public Commands(DiscordSocketClient client)
        {
            _client = client;
            _client.Ready += Client_Ready;
        }

        public async Task Client_Ready()
        {
            var guild = _client.GetGuild(1031999653094363177);

            var guildCommand1 = new SlashCommandBuilder();
            var guildCommand2 = new SlashCommandBuilder();

            guildCommand1.WithName("ping");
            guildCommand1.WithDescription("A game of ping-pong!");

            guildCommand2.WithName("fetch-ranks");
            guildCommand2.WithDescription("retrieve all current ranks");

            var globalCommand = new SlashCommandBuilder();

            globalCommand.WithName("hi");
            globalCommand.WithDescription("Hello");

            try
            {
                 await guild.CreateApplicationCommandAsync(guildCommand1.Build());
                 await guild.CreateApplicationCommandAsync(guildCommand2.Build());
                 await guild.CreateApplicationCommandAsync(globalCommand.Build());
            }
            catch (HttpException e)
            {
                var json = JsonConvert.SerializeObject(e.Errors, Formatting.Indented);

                Console.WriteLine(json);
            }

        }
    }
}
