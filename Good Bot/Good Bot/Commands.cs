using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global


namespace Good_Bot
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        private readonly string[] _replies = {"Cap! he said a bad word", "Good boys dont say FUCK"};

        private readonly string[] _replies1 =
        {
            "You are the reason they put instructions on shampoo.", "You fight like a dairy farmer!", "",
            "You suck more than a suck machine set on 'suck a lot'", "Man, you're like school on Sunday. No Class"
        };

        [Command("commands")]
        public async Task Help()
        {
            var embedBuilder = new EmbedBuilder()
                .WithDescription(
                    "Moderation: type `moderation` for moderation commands\n\nFun: type `fun` for fun commands\n\nUtil: type `util` for utility commands ~~Yes the bot is useful and not completely useless~~")
                .WithFooter(footer =>
                {
                    footer
                        .WithIconUrl("https://i.imgur.com/6Bi17B3.png");
                });
            var embed = embedBuilder.Build();
            await ReplyAsync(embed: embed);
        }

        [Command("moderation")]
        public async Task Mod()
        {
            var embedBuilder = new EmbedBuilder()
                .WithDescription(
                    "Kick: you want me to explain what `kick` is?\n\nBan: Your really reading description for `ban`,bruh.")
                .WithFooter(footer =>
                {
                    footer
                        .WithIconUrl("https://i.imgur.com/6Bi17B3.png");
                });
            var embed = embedBuilder.Build();
            await ReplyAsync(embed: embed);
        }

        [Command("util")]
        public async Task Utility()
        {
            var embedBuilder = new EmbedBuilder()
                .WithDescription(
                    "`say`: umm so this is a long explanation:\n`say whatever text (name of emote)  whatever text`\nThe `say` can be used to make the bot repeat whatever is typed after the command" +
                    "Where it comes in use is that you can use nitro emotes from the current server by typing them in `()`.\nExample:`say (hackerman)`  \n\n")
                .WithFooter(footer =>
                {
                    footer
                        .WithIconUrl("https://i.imgur.com/6Bi17B3.png");
                });
            var embed = embedBuilder.Build();
            await ReplyAsync(embed: embed);
        }

        // [Command("mute")]
        // public async Task Mute(IGuildUser user = null)
        // {
        //     if (user == null)
        //     {
        //         await ReplyAsync("Please specify a user!");
        //         return;
        //     }
        //     await user.SendMessageAsync("byeeee");
        //     var r = user.RoleIds;
        //     var roles = r.ToImmutableArray();
        //     foreach (var variable in roles)
        //     {
        //         IRole role=Context.Guild.GetRole(variable);
        //         if (role.Id==Context.Guild.Id)
        //             continue;
        //         await user.RemoveRoleAsync(role);
        //     }
        //     await user.AddRoleAsync(Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == "mute"));
        // }
        [Command("ban")]
        [RequireUserPermission(GuildPermission.BanMembers,
            ErrorMessage = "You don't have the permission ``ban_member``!")]
        public async Task BanMember(IGuildUser user = null, [Remainder] string reason = null)
        {
            if (user == null)
            {
                await ReplyAsync("Please specify a user!");
                return;
            }

            reason ??= "Not specified";
            await user.SendMessageAsync("fuck u");
            await Context.Guild.AddBanAsync(user, 1, reason);

            var embedBuilder = new EmbedBuilder()
                .WithDescription($":white_check_mark: {user.Mention} was banned\n**Reason** {reason}")
                .WithFooter(footer =>
                {
                    footer
                        .WithText("Woof woof!")
                        .WithIconUrl("https://i.imgur.com/6Bi17B3.png");
                });
            var embed = embedBuilder.Build();
            await ReplyAsync(embed: embed);

            var logChannel = Context.Client.GetChannel(642698444431032330) as ITextChannel;
            var embedBuilderLog = new EmbedBuilder()
                .WithDescription(
                    $"{user.Mention} was banned\n**Reason** {reason}\n**Moderator** {Context.User.Mention}")
                .WithFooter(footer =>
                {
                    footer
                        .WithText("User Ban Log")
                        .WithIconUrl("https://i.imgur.com/6Bi17B3.png");
                });
            var embedLog = embedBuilderLog.Build();
            if (logChannel != null) await logChannel.SendMessageAsync(embed: embedLog);
        }

        [Command("kick")]
        [RequireBotPermission(GuildPermission.KickMembers)]
        public async Task KickAsync(IGuildUser user)
        {
            var guildUser = Context.Guild.GetUser(Context.User.Id);

            if (!guildUser.GuildPermissions.KickMembers)
            {
                await Context.Message.DeleteAsync();
                await ReplyAsync(":warning: `No permissions to kick players`");
            }
            else
            {
                await user.SendMessageAsync("Good thing u left , I was gonna bite u for the stuff u said");
                await user.KickAsync();
                await Context.Channel.SendMessageAsync($" `{user.Username} has been kicked from the server`");
                // var guild = Context.Guild;
                // var channel = guild.GetTextChannel(609086251978457098); //582790350620327952
                //
                // var builder = new EmbedBuilder();
                //
                // builder.WithTitle("Logged Information")
                //     .AddField("User", $"{user.Mention}")
                //     .AddField("Moderator", $"{Context.User.Username}")
                //     .AddField("Other Information", "Can be invited back")
                //     .AddField("Command", $"``.kick {user.Username}``")
                //     .WithDescription(
                //         $"This player has been kicked from {Context.Guild.Name} by {Context.User.Username}")
                //     .WithFooter($"{Context.User.Username}", Context.User.GetAvatarUrl())
                //     .WithCurrentTimestamp()
                //     .WithColor(new Color(54, 57, 62));
            }
        }
        //
        // [Command("I")]
        // // ReSharper disable once UnusedParameter.Global
        // public async Task Says([Remainder] string echo)
        // {
        //     if (Context.Message.Author.Id != 292149859459399680)
        //     {
        //         // ReplyAsync is a method on ModuleBase
        //         var userInfo = Context.User;
        //         var id = Context.Message.Author.Id;
        //         await ReplyAsync($"<@{id}> There is no I in team");
        //         // var avatar = userInfo.GetAvatarUrl();
        //         // Console.WriteLine(avatar);
        //     }
        // }

        [Command("pls beg")]
        public async Task Begger()
        {
            // ReplyAsync is a method on ModuleBase
            // var userInfo = Context.User;
            var id = Context.Message.Author.Id;
            await ReplyAsync($"<@{id}> stop begging");
            // var chan = Context.Channel as IGuildChannel;
            Console.WriteLine("c");
            var cat = Context.Message.Channel as IGuildChannel;
            Console.WriteLine("b");
            Console.WriteLine(cat);
        }

        [Command("fuck")]
        // ReSharper disable once UnusedParameter.Global
        public async Task Saya(IGuildUser user = null, [Remainder] string echo = null)
        {
            if (user == null)
            {
                var id = Context.Message.Author.Id;
                var r = new Random();
                var rInt = r.Next(2); //for ints
                await ReplyAsync($"<@{id}> " + _replies[rInt]);
            }
            else if (user.Id != 292149859459399680)
            {
                var messages = Context.Channel.GetMessagesAsync(1).Flatten();
                foreach (var h in await messages.ToArrayAsync()) await Context.Channel.DeleteMessageAsync(h);
                var id = user.Id;
                await ReplyAsync($" Yeah! fuck <@{id}> ");
            }
        }

        [Command("trash talk")]
        // ReSharper disable once UnusedParameter.Global
        public async Task Trash(IGuildUser user = null, [Remainder] string echo = null)
        {
            if (user == null)
            {
            }
            else
            {
                var r = new Random();
                var rInt = r.Next(4); //for ints
                var messages = Context.Channel.GetMessagesAsync(1).Flatten();
                foreach (var h in await messages.ToArrayAsync()) await Context.Channel.DeleteMessageAsync(h);
                var id = user.Id;
                await ReplyAsync($"<@{id}> " + _replies1[rInt]);
            }
        }

        [Command("meme")]
        public async Task Meme(int x = 0)
        {
            Console.WriteLine(x);
            for (var i = 0; i <= x; i++)
            {
                var url = "https://apis.duncte123.me/meme";
                var webClient = new WebClient();
                webClient.Headers.Add("user-agent", "Only a test!");

                var json = webClient.DownloadString(url);
                dynamic data = JObject.Parse(json);
                Console.WriteLine(data.data.title);
                EmbedBuilder embedBuilder = new EmbedBuilder().WithColor(Color.Blue)
                    .WithTitle($"{data.data.title}")
                    .WithUrl(data.data.url.ToString())
                    .WithImageUrl(data.data.image.ToString());
                var embed = embedBuilder.Build();
                await ReplyAsync(embed: embed);
            }
        }

        [Command("ping")]
        public async Task Ping(IGuildUser user = null, [Remainder] string echo = null)
        {
            Currency.get_Balance(Context.Message.Author.Id);
            if (true) //_bal >= 50
            {
                var embedBuilder = new EmbedBuilder()
                    .WithDescription(echo);
                var embed = embedBuilder.Build();
                // ReplyAsync is a method on ModuleBase
                await user.SendMessageAsync("Someone sent u a secret ping");
                await user.SendMessageAsync(embed: embed);
                var messages = Context.Channel.GetMessagesAsync(1).Flatten();
                foreach (var h in await messages.ToArrayAsync()) await Context.Channel.DeleteMessageAsync(h);
                await ReplyAsync("yeah pinged");
            }
        }

        [Command("catto")]
        public async Task Account()
        {
            var url = "https://aws.random.cat/meow";
            var webClient = new WebClient();
            webClient.Headers.Add("user-agent", "Only a test!");

            var json = webClient.DownloadString(url);
            dynamic data = JObject.Parse(json);
            Console.WriteLine(data.file);
            EmbedBuilder embedBuilder = new EmbedBuilder().WithColor(Color.Blue)
                // .WithAuthor($"{data.file}")
                .WithImageUrl(data.file.ToString());
            var embed = embedBuilder.Build();
            await ReplyAsync(embed: embed);
        }

        [Command("doggo")]
        public async Task Doggo()
        {
            var url = "https://dog.ceo/api/breeds/image/random";
            var webClient = new WebClient();
            webClient.Headers.Add("user-agent", "Only a test!");

            var json = webClient.DownloadString(url);
            dynamic data = JObject.Parse(json);
            Console.WriteLine(data.message);
            EmbedBuilder embedBuilder = new EmbedBuilder().WithColor(Color.Blue)
                // .WithAuthor($"{data.file}")
                .WithImageUrl(data.message.ToString());
            var embed = embedBuilder.Build();
            await ReplyAsync(embed: embed);
        }

        // ReSharper disable once StringLiteralTypo
        [Command("sadge")]
        // ReSharper disable once IdentifierTypo
        public async Task Sadge()
        {
            await Context.Message.DeleteAsync();
            await ReplyAsync("https://imgur.com/NAN66eB");
        }

        [Command("search")]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        public async Task Search([Remainder] string term)
        {
            // var url = $"https://aws.random.cat/meow";
            // WebClient webClient=new WebClient();
            // webClient.Headers.Add("user-agent", "Only a test!");
            //
            // var json =  webClient.DownloadString(url);
            // dynamic data = JObject.Parse(json);

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://mashape-community-urban-dictionary.p.rapidapi.com/define?term={term}"),
                Headers =
                {
                    {"x-rapidapi-key", "388e24cd8cmsh8b7f8a4aae55578p14eb40jsnbd65a0d5bfa4"},
                    {"x-rapidapi-host", "mashape-community-urban-dictionary.p.rapidapi.com"}
                }
            };
            using var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadAsStringAsync();
            dynamic data = JObject.Parse(body);
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (data != null)
            {
                var items = (JArray) data["list"];
                var length = items.Count;
                var r = new Random();
                var rInt = r.Next(length - 1); //for ints
                // Console.WriteLine(length);
                Console.WriteLine(data.list[rInt].definition);
                await ReplyAsync(data.list[rInt].definition.ToString().Replace("[", "**")
                    .Replace("]", "**"));
                
            }
        }

        // ReSharper disable once StringLiteralTypo
        [Command("searcht")]
        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        // ReSharper disable once IdentifierTypo
        public async Task Searcht([Remainder] string term)
        {
            // var url = $"https://aws.random.cat/meow";
            // WebClient webClient=new WebClient();
            // webClient.Headers.Add("user-agent", "Only a test!");
            //
            // var json =  webClient.DownloadString(url);
            // dynamic data = JObject.Parse(json);

            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://mashape-community-urban-dictionary.p.rapidapi.com/define?term={term}"),
                Headers =
                {
                    {"x-rapidapi-key", "388e24cd8cmsh8b7f8a4aae55578p14eb40jsnbd65a0d5bfa4"},
                    {"x-rapidapi-host", "mashape-community-urban-dictionary.p.rapidapi.com"}
                }
            };
            using var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadAsStringAsync();
            dynamic data = JObject.Parse(body);

            var items = (JArray) data["list"];
            var length = items.Count;
            var r = new Random();
            var rInt = r.Next(length - 1); //for ints
            // Console.WriteLine(length);
            Console.WriteLine(data.list[rInt].definition);

            await ReplyAsync($" {data.list[rInt].definition.ToString().Replace("[", "**").Replace("]", "**")}", true);
        }

        [Command("user")]
        public async Task blizz_con(IGuildUser user)
        {
            await ReplyAsync(user.Id.ToString());
            await user.KickAsync();
        }
        
    }
}