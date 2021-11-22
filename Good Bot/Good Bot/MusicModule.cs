using System;
using System.Threading.Tasks;
using Discord.Commands;
using Lavalink4NET;
using Lavalink4NET.DiscordNet;
using Lavalink4NET.Player;
using Lavalink4NET.Rest;

namespace Good_Bot
{
    /// <summary>
    ///     Presents some of the main features of the Lavalink4NET-Library.
    /// </summary>
    [Name("Music")]
    [RequireContext(ContextType.Guild)]
    public sealed class MusicModule : ModuleBase<SocketCommandContext>
    {
        private readonly IAudioService _audioService;


        /// <summary>
        ///     Initializes a new instance of the <see cref="MusicModule" /> class.
        /// </summary>
        /// <param name="audioService">the audio service</param>
        /// <exception cref="ArgumentNullException">
        ///     thrown if the specified <paramref name="audioService" /> is <see langword="null" />.
        /// </exception>
        public MusicModule(IAudioService audioService)
        {
            _audioService = audioService ?? throw new ArgumentNullException(nameof(audioService));
        }

        /// <summary>
        ///     Disconnects from the current voice channel connected to asynchronously.
        /// </summary>
        /// <returns>a task that represents the asynchronous operation</returns>
        [Command("disconnect", RunMode = RunMode.Async)]
        public async Task Disconnect()
        {
            var player = await GetPlayerAsync();
            if (player == null) return;
            await player.StopAsync(true);
            await ReplyAsync("Disconnected.");
        }

        /// <summary>
        ///     Plays music from YouTube asynchronously.
        /// </summary>
        /// <param name="query">the search query</param>
        /// <returns>a task that represents the asynchronous operation</returns>
        [Command("play", RunMode = RunMode.Async)]
        public async Task Play([Remainder] string query)
        {
            // await Context.Guild.CurrentUser.ModifyAsync(x => x.Deaf =true);
            Console.WriteLine(Context);
            Console.WriteLine("abcd");
            var player = await GetPlayerAsync();
            // await bot.ModifyAsync(x => { x.Deaf = true; });
            if (player == null) return;
            Console.WriteLine("hello");
            var track = await _audioService.GetTrackAsync(query, SearchMode.YouTube);

            if (track == null)
            {
                await ReplyAsync("😖 No results. ");
                return;
            }
            if (player.IsLooping)
                player.IsLooping = false;
            var position = await player.PlayAsync(track, true);

            if (position == 0)
                await ReplyAsync("🔈 Playing: " + track.Source);
            else
                await ReplyAsync("🔈 Added to queue: " + track.Source);
        }

        [Command("playlist", RunMode = RunMode.Async)]
        public async Task Playlist([Remainder] string query)
        {
            // await Context.Guild.CurrentUser.ModifyAsync(x => x.Deaf =true);
            Console.WriteLine(Context);
            Console.WriteLine("abcd");
            var player = await GetPlayerAsync();
            if (player == null) return;

            var track = await _audioService.GetTracksAsync(query);
            Console.WriteLine(track);
            var inq = 0;
            foreach (var cool in track)
            {
                var position = await player.PlayAsync(cool, true);
                if (position == 0)
                    await ReplyAsync("🔈 Playing: ");
                else
                    inq++;
            }

            await ReplyAsync("Added " + inq + " songs to q ");
        }

        /// <summary>
        ///     Shows the track position asynchronously.
        /// </summary>
        /// <returns>a task that represents the asynchronous operation</returns>
        [Command("position", RunMode = RunMode.Async)]
        public async Task Position()
        {
            var player = await GetPlayerAsync();
            if (player == null) return;
            if (player.CurrentTrack == null)
            {
                await ReplyAsync("Nothing playing!");
                return;
            }
            await ReplyAsync($"Position: {player.TrackPosition} / {player.CurrentTrack.Duration}.");
        }

        [Command("seek", RunMode = RunMode.Async)]
        public async Task Seek(double seekSeconds)
        {
            var player = await GetPlayerAsync();
            if (player == null) return;
            if (player.CurrentTrack == null)
            {
                await ReplyAsync("Nothing playing!");
                return;
            }
            await ReplyAsync($"Position: {player.TrackPosition} / {player.CurrentTrack.Duration}.");
            if (player.TrackPosition.TotalSeconds + seekSeconds ! > player.CurrentTrack.Duration.TotalSeconds)
            {
                // player.SeekPositionAsync(seekSeconds);
            }
        }
        /// <summary>
        ///     Stops the current track asynchronously.
        /// </summary>
        /// <returns>a task that represents the asynchronous operation</returns>
        [Command("stop", RunMode = RunMode.Async)]
        public async Task Stop()
        {
            var player = await GetPlayerAsync();
            if (player == null) return;
            if (player.CurrentTrack == null)
            {
                await ReplyAsync("Nothing playing!");
                return;
            }
            await player.StopAsync();
            await ReplyAsync("Stopped playing.");
        }
        [Command("pause", RunMode = RunMode.Async)]
        public async Task Pause()
        {
            var player = await GetPlayerAsync();
            if (player == null) return;
            if (player.CurrentTrack == null)
            {
                await ReplyAsync("Nothing playing!");
                return;
            }
            await player.PauseAsync();
            await ReplyAsync("Paused.");
        }

        [Command("resume", RunMode = RunMode.Async)]
        public async Task Resume()
        {
            var player = await GetPlayerAsync();
            if (player == null) return;
            if (player.CurrentTrack == null)
            {
                await ReplyAsync("Nothing playing!");
                return;
            }
            await player.ResumeAsync();
            await ReplyAsync("Resumed.");
        }

        [Command("next", RunMode = RunMode.Async)]
        public async Task Next()
        {
            var player = await GetPlayerAsync();
            if (player == null) return;
            if (player.CurrentTrack == null)
            {
                await ReplyAsync("Nothing playing!");
                return;
            }
            await player.SkipAsync();
            await ReplyAsync("Next!");
        }

        /// <summary>
        ///     Updates the player volume asynchronously.
        /// </summary>
        /// <param name="volume">the volume (1 - 1000)</param>
        // /// <returns>a task that represents the asynchronous operation</returns>
        [Command("volume", RunMode = RunMode.Async)]
        [Alias("vol")]
        public async Task Volume(int volume = 100)
        {
            var user = Context.Guild.GetUser(Context.User.Id);
            if (!user.VoiceState.HasValue)
            {
                await ReplyAsync("You must be in a voice channel!");
                return;
            }
            if (volume > 1000 || volume < 0)
            {
                await ReplyAsync("Volume out of range: 0% - 1000%!");
                return;
            }
            var player = await GetPlayerAsync();
            if (player == null) return;
            await player.SetVolumeAsync(volume / 100f);
            await ReplyAsync($"Volume updated: {volume}%");
        }

        // [Command("loop")]
        // public async Task Loop()
        // {
        //     var player = await GetPlayerAsync();
        //     if (!player.IsLooping)
        //     {
        //         player.IsLooping = true;
        //         await ReplyAsync("looping");
        //     }
        //     else
        //     {
        //         await ReplyAsync("already looping");
        //     }
        // }

        [Command("latency")]
        public async Task Lat()
        {
            await ReplyAsync(Context.Client.Latency + "ms");
            await ReplyAsync((Context.Message.CreatedAt.Minute * 1000 + Context.Message.CreatedAt.Millisecond -
                              (DateTime.Now.Millisecond * 1000 + DateTime.Now.Millisecond)) + "ms");
        }

        [Command("cloud")]
        public async Task Something([Remainder] string query)
        {
            // await Context.Guild.CurrentUser.ModifyAsync(x => x.Deaf =true);
            Console.WriteLine(Context);
            Console.WriteLine("abcd");
            var player = await GetPlayerAsync();
            if (player == null) return;
            Console.WriteLine("hello");
            var track = await _audioService.GetTrackAsync(query, SearchMode.SoundCloud);

            if (track == null)
            {
                await ReplyAsync("😖 No results. ");
                return;
            }
            var position = await player.PlayAsync(track, true);

            if (position == 0)
                await ReplyAsync("🔈 Playing: " + track.Source);
            else
                await ReplyAsync("🔈 Added to queue: " + track.Source);
        }

        /// <summary>
        ///     Gets the guild player asynchronously.
        /// </summary>
        /// <param name="connectToVoiceChannel">
        ///     a value indicating whether to connect to a voice channel
        /// </param>
        /// <returns>
        ///     a task that represents the asynchronous operation. The task result is the lavalink player.
        /// </returns>
        private async Task<VoteLavalinkPlayer> GetPlayerAsync(bool connectToVoiceChannel = true)
        {
            var player = _audioService.GetPlayer<VoteLavalinkPlayer>(Context.Guild.Id);
            if (player != null
                && player.State != PlayerState.NotConnected
                && player.State != PlayerState.Destroyed)
                return player;
            var user = Context.Guild.GetUser(Context.User.Id);
            if (!user.VoiceState.HasValue)
            {
                await ReplyAsync("You must be in a voice channel!");
                return null;
            }
            if (!connectToVoiceChannel)
            {
                await ReplyAsync("The bot is not in a voice channel!");
                return null;
            }
            return await _audioService.JoinAsync<VoteLavalinkPlayer>(user.VoiceChannel);
        }
    }
}