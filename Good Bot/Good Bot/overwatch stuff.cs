using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Newtonsoft.Json.Linq;

namespace Good_Bot
{
    public class OverwatchStuff : ModuleBase<SocketCommandContext>
    {
        String[] x={"<:bronze:768478213084020756>","<:silver:768478213076156427>","<:gold:768478213743050822>","<:platinum:768478215970095124>","<:diamond:768478213541593129>","<:master:768478215659585567>","<:grandmaster:768478215932477491>",};
        private List<string> _overwatchHeroes = new List<string>
        {
            "Ana",
            "         Ashe",
            "     Baptiste",
            "      Bastion",
            "     Brigitte",
            "         D.Va",
            "     Doomfist",
            "         Echo",
            "        Genji",
            "        Hanzo",
            "      Junkrat",
            "        Lúcio",
            "       McCree",
            "          Mei",
            "        Mercy",
            "        Moira",
            "        Orisa",
            "       Pharah",
            "       Reaper",
            "    Reinhardt",
            "      Roadhog",
            "        Sigma",
            "  Soldier: 76",
            "       Sombra",
            "     Symmetra",
            "     Torbjörn",
            "       Tracer",
            "   Widowmaker",
            "      Winston",
            "Wrecking Ball",
            "        Zarya"
        };

        [Command("rank")]
        public async Task Profile([Remainder] string tag)
        {
            tag = tag.Replace('#', '-');
            Console.WriteLine(tag);
            var url = $"https://ow-api.com/v1/stats/pc/us/{tag}/profile";

            // var embedBuilder = new EmbedBuilder()
            //     .WithDescription(
            //         "Moderation: type `moderation` for moderation commands\n\nFun: type `fun` for fun commands\n\nUtil: type `util` for utility commands ~~Yes the bot is useful and not completely useless~~")
            //     .WithFooter(footer =>
            //     {
            //         footer
            //             .WithIconUrl("https://i.imgur.com/6Bi17B3.png");
            //     });
            // var embed = embedBuilder.Build();
            // await ReplyAsync(embed: embed);
            var json = new WebClient().DownloadString(url);
            // var json1 = new WebClient().DownloadString(url1);
            dynamic data = JObject.Parse(json);
            Console.WriteLine(data.ratings[0]);
            // Console.WriteLine(data.ratings[1].level);
            // Console.WriteLine(data.ratings[2].level);
            if (data.ratings[0].level.ToString() != null)
                await Send(data.ratings[0]);
            if (data.ratings[1].level.ToString() != null)
                await Send(data.ratings[1]);
            if (data.ratings[2].level.ToString() != null)
                await Send(data.ratings[2]);

            // Console.WriteLine(json1);
            // await ReplyAsync(json1);
        }

        [Command("complete")]
        public async Task Complete([Remainder] string tag)
        {
            tag = tag.Replace('#', '-');
            Console.WriteLine(tag);

            var url1 = $"https://ow-api.com/v1/stats/pc/us/{tag}/complete";
            var json1 = new WebClient().DownloadString(url1);

            Console.WriteLine(json1);
            await ReplyAsync(json1);
        }

        [Command("heroes")]
        public async Task Heroes([Remainder] string tag)
        {
            tag = tag.Replace('#', '-');
            Console.WriteLine(tag);
            // var url = $"https://ow-api.com/v1/stats/pc/us/{tag}/profile";
            // var url1 = $"https://ow-api.com/v1/stats/pc/us/{tag}/complete";

            // foreach (var v in overwatchHeroes)
            // {  
            var url2 = $"https://ow-api.com/v1/stats/pc/us/{tag}/heroes/lucio";
            var json2 = new WebClient().DownloadString(url2);
            Console.WriteLine(json2);
            await ReplyAsync(json2);
            // }

            // var json = new WebClient().DownloadString(url);
            // var json1 = new WebClient().DownloadString(url1);

            // Console.WriteLine(json);
            // await ReplyAsync(json);
            // Console.WriteLine(json1);
            // await ReplyAsync(json1);
        }

        [Command("team")]
        public async Task Team(IGuildUser user1, [Remainder] string tag = null)
        {
            await user1.SendMessageAsync($"You are a member of {tag} team");
        }

        private async Task Send(dynamic s)
        {
            var embedBuilde = new EmbedBuilder()
                .WithDescription(
                    $"Imagine being {s.level.ToString()}  on {s.role.ToString()}")
                ;
            var embed = embedBuilde.Build();
            await ReplyAsync(embed:embed);
        }
    }
}