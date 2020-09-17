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

namespace GameTime
{
    public class Bot
    {
        public static string Version { get; private set; }
        public static Config Config { get; private set; }
        public static PlayerDatabase PlayerDatabase { get; private set; }
        public static ItemList Items { get; private set; }
        public static InteractivityExtension Interactivity { get; private set; }
        public static WordList Words { get; private set; }
        public static PuzzleList DotPuzzles { get; private set; }
        public static DiscordGuild HomeGuild { get; set; }

        public static void Main(string[] args)
        {
            new Bot().Run();
        }

        public Bot()
        {
            Version = "1.0.0";
            PlayerDatabase = new PlayerDatabase("player.db");
            Items = new ItemList();
            Words = new WordList();
            DotPuzzles = new PuzzleList();
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
            }
            DiscordAsync().GetAwaiter().GetResult();
        }

        private async Task DiscordAsync()
        {
            var client = new DiscordClient(new DiscordConfiguration()
            {
                Token = Config.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                UseInternalLogHandler = true
            });
            HomeGuild = await client.GetGuildAsync(749832562779750430);
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
            commands.CommandErrored += Events.CommandErrored;
            commands.RegisterCommands<AdminCommands>();
            commands.RegisterCommands<PlayerCommands>();
            commands.RegisterCommands<WordScramblerCommands>();
            commands.RegisterCommands<MoveToDotCommands>();
            await client.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
