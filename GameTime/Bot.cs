using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using System.Collections.Generic;
using DSharpPlus;
using GameTime.Models;
using GameTime.Commands;
using DSharpPlus.Interactivity;
using GameTime.ScramblerModels;
using GameTime.DotPuzzleModels;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;

namespace GameTime
{
    public class Bot
    {
        public static string Version { get; private set; }
        public static Config Config { get; private set; }
        public static PlayerDatabase PlayerDatabase { get; private set; }
        public static ItemList Items { get; private set; }
        public static InteractivityExtension Interactivity { get; private set; }
        public static Metals AllMetals { get; private set; }
        public static WordList Words { get; private set; }
        public static PuzzleList DotPuzzles { get; private set; }
        public static DiscordGuild HomeGuild { get; private set; }
        public static DiscordClient Client { get; private set; }

        public static void Main(string[] args)
        {
            new Bot().Run();
        }

        public Bot()
        {
            Version = "0.2.1";
            PlayerDatabase = new PlayerDatabase("player.db");
            Items = new ItemList();
            Words = new WordList();
            DotPuzzles = new PuzzleList();
            AllMetals = new Metals();
        }

        ~Bot() 
        {
            PlayerDatabase.Dispose();
        }

        public void Run()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Clear();
            Console.WriteLine("GameTime Bot Console");
            Console.WriteLine($"Bot Version: {Version}");
            Config = Config.LoadConfig();
            if (Config != null)
            {
                Console.WriteLine($"Token: {Config.Token}");
                Console.WriteLine($"Prefix: {Config.Prefix}");
            }
            else
            {
                var token = "";
                var prefix = "";
                while (String.IsNullOrEmpty(token))
                {
                    Console.Write("Token: ");
                    token = Console.ReadLine();
                }
                while (String.IsNullOrEmpty(prefix))
                {
                    Console.Write("Prefix: ");
                    prefix = Console.ReadLine();
                }
                Config = new Config(token, prefix);
                Config.SaveConfig(Config);
                Console.Write("Settings Saved");
            }
            DiscordAsync().GetAwaiter().GetResult();
        }

        private async Task DiscordAsync()
        {
            var client = new DiscordClient(new DiscordConfiguration()
            {
                Token = Config.Token,
                TokenType = TokenType.Bot,
                //MinimumLogLevel = Microsoft.Extensions.Logging.LogLevel.Debug, //Unused
                AutoReconnect = true
            });
            HomeGuild = await client.GetGuildAsync(637049983916310558);
            Client = client;
            Interactivity = client.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromMinutes(5)
            });
            var commands = client.UseCommandsNext(new CommandsNextConfiguration()
            {
                StringPrefixes = new List<string>() { Config.Prefix },
                EnableDms = false,
                IgnoreExtraArguments = true,
                CaseSensitive = false
            });
            client.Ready += Events.Client_Ready;
            client.GuildMemberRemoved += Events.GuildMemberLeave;
            commands.CommandErrored += Events.CommandErrored;
            commands.RegisterCommands<AdminCommands>();
            Console.WriteLine("Admin Commands Ready");
            commands.RegisterCommands<PlayerCommands>();
            Console.WriteLine("Player Commands Ready");
            commands.RegisterCommands<WordScramblerCommands>();
            Console.WriteLine("Scrambler Initialized");
            commands.RegisterCommands<MoveToDotCommands>();
            Console.WriteLine("ConnectX Initialized");
            commands.RegisterCommands<MinesweeperCommands>();
            Console.WriteLine("Minesweeper Initialized");
            commands.RegisterCommands<GauntletCommands>();
            Console.WriteLine("Gauntlet Initialized");
            commands.RegisterCommands<CasinoGames>();
            Console.WriteLine("Casino Initialized");
            commands.RegisterCommands<BetaCommands>();
            Console.WriteLine("BetaCommands Initialized");
            commands.RegisterCommands<MultiplayerCommands>();
            Console.WriteLine("Connect4 Initialized");
            await client.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
