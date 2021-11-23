using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using Lavalink4NET;
using Lavalink4NET.DiscordNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Victoria;


namespace Good_Bot
{
    class Program
    {
        private static LavaNode _lavaNode;
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IConfiguration _config;

        private IServiceProvider _services;
        public static async Task Main()=>
            await new Program().RunBotAsync();
        

        private async Task RunBotAsync()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _config = BuildConfig();
            // I added how it shows in the "Getting Ready" section
            // you add the LavalinkNode and it's configuration in the dependency injection
            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .AddSingleton<IDiscordClientWrapper, DiscordClientWrapper>()
                .AddSingleton<IAudioService, LavalinkNode>()
                .AddSingleton<LavaNode>()
                .AddSingleton<LavaConfig>()
                .AddSingleton(new LavalinkNodeOptions // consider using a configuration
                {
                    RestUri = "http://localhost:2333/",
                    WebSocketUri = "ws://localhost:2333/",
                    // ReSharper disable once StringLiteralTypo
                    Password = "youshallnotpass"
                })
                // [ Other services ]
                .BuildServiceProvider();

            var token = _config["token"];
            _client.Log += _client_Log;
            _client.Ready += OnReadyAsync;

            await RegisterCommandsAsync();
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            await Task.Delay(-1);
        }
        private async Task OnReadyAsync()
        {
            // Avoid calling ConnectAsync again if it's already connected 
            // (It throws InvalidOperationException if it's already connected).
            _lavaNode = _services.GetRequiredService<LavaNode>();
            var audioService = _services.GetRequiredService<IAudioService>();
            await audioService.InitializeAsync();
            if (!_lavaNode.IsConnected) await _lavaNode.ConnectAsync();
        }

        private Task _client_Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        private async Task RegisterCommandsAsync()
        {
            _client.JoinedGuild += HandleJoinAsync;
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            // UserStatus status = "stays";
        }

        private Task HandleJoinAsync(SocketGuild arg)
        {
            var guild = arg as IGuild;
            var channelsAsync = guild.GetTextChannelsAsync();
            foreach (var collection in channelsAsync.Result)
            {
                Console.WriteLine(collection.Name);
                collection.CreateWebhookAsync("gb");
            }

          
            throw new NotImplementedException();
        }

        private IConfiguration BuildConfig()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json")
                .Build();
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            // Console.WriteLine(_client.Guilds.Count);
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);
            if (message != null && message.Author.IsBot) return;
            if (message != null)
            {
                // await Context.Message.DeleteAsync();
                var l = "";
                // ReplyAsync is a method on ModuleBase

                var s = 0;
                var sample = message.Content;
                if (sample.Contains(':'))
                    for (var i = 0; i < sample.Length; i++)
                        //get the first :
                        if (sample[i] == ':')
                            if (i != 0 && sample[i - 1] == '<')
                            {
                                break;
                            }
                            else
                            {
                                Console.WriteLine("1");
                                //for loop for the word after that
                                for (var j = i + 1; j < sample.Length; j++)
                                {
                                    s++;
                                    if (sample[j] == ':')
                                    {
                                        Console.WriteLine("3");
                                        string n;
                                        Console.WriteLine(n = sample.Substring(i, s + 1));
                                        foreach (var guilds in message.Author.MutualGuilds.AsEnumerable())
                                        {
                                            // Console.WriteLine(guilds.Name);
                                            // Console.WriteLine("hi");
                                            var finalEmote = guilds.Emotes.FirstOrDefault(o =>
                                                o.Name.ToString() == n.Replace(":", ""));
                                            if (finalEmote != null)
                                            {
                                                Console.WriteLine(finalEmote.Name);
                                                l = sample.Replace(n, $"{finalEmote}");
                                                break;
                                            }
                                        }

                                        if (sample[i] == ':')
                                            if (i != 0 && sample[i - 1] != '<')
                                                break;
                                        var guildUser = (IGuildUser) message.Author;
                                        using var client = new WebClient();
                                        var values = new NameValueCollection();
                                        string nickname;
                                        if (guildUser.Nickname != null)
                                        {
                                            Console.WriteLine("3");
                                            nickname = guildUser.Nickname;
                                        }
                                        else
                                        {
                                            nickname = guildUser.Username;
                                        }

                                        values["username"] = nickname;
                                        values["content"] = $"{l}";
                                        var avatarUrl = message.Author.GetAvatarUrl();

                                        values["avatar_url"] = $"{avatarUrl}";
                                        RestWebhook webhook = null;
                                        var chan = message.Channel as ITextChannel;
                                        Debug.Assert(chan != null, nameof(chan) + " != null");
                                        var webhooks = chan.GetWebhooksAsync();
                                        foreach (var y in webhooks.Result)
                                            if (y.Name == "gb")
                                            {
                                                webhook = y as RestWebhook;
                                                break;
                                            }

                                        if (webhook == null)
                                        {
                                            var textChannel = message.Channel as ITextChannel;
                                            Debug.Assert(textChannel != null, nameof(textChannel) + " != null");

                                            await textChannel.CreateWebhookAsync("gb");
                                            webhooks = chan.GetWebhooksAsync();
                                            foreach (var y in webhooks.Result)
                                                if (y.Name == "gb")
                                                {
                                                    webhook = y as RestWebhook;
                                                    break;
                                                }
                                        }

                                        Console.WriteLine($"{webhook}");
                                        Debug.Assert(webhook != null, nameof(webhook) + " != null");
                                        var response = client.UploadValues(
                                            $"https://discord.com/api/webhooks/{webhook.Id}/{webhook.Token}", values);

                                        Console.WriteLine("ok");
                                        var responseString = Encoding.Default.GetString(response);
                                        Console.WriteLine(responseString);
                                        // await ReplyAsync(l);
                                        await message.DeleteAsync();
                                        break;
                                    }
                                }
                            }
            }
            var argPos = 0;
            if (message.HasStringPrefix("", ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess)
                {
                    Console.WriteLine(message);
                    Console.WriteLine(result.ErrorReason);
                }

                if (result.Error.Equals(CommandError.UnmetPrecondition))
                    if (message != null)
                        await message.Channel.SendMessageAsync(result.ErrorReason);
            }
        }
    }
}