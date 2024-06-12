using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using GameTime.Commands;
//using GameTime.Models; Removed temp
using GameTime.Databases;
using GameTime.Models;

namespace GameTime
{
    public class Bot
    {
        private static Bot bot = null;
        public string Version { get; private set; }
        public Config Config { get; private set; }
        public static DiscordGuild HomeGuild { get; private set; }
        public static DiscordChannel LoggingChannel { get; set; }
        public static DiscordClient Client { get; private set; }
        public static InteractivityExtension Interactivity { get; private set; }
        public static ItemDatabase GameItems { get; private set; }
        public static MutliplayerGameSessions GameSessions { get; private set; }
        public static void Main(string[] args)
        {
            Console.WriteLine(new DetailedMap().ToString());
            //new Bot().Run();
        }
        //Change Version here every update
        private Bot()
        {
            Version = "1.0.0";
        }
        public static Bot BotInstance()
        {
            if(bot == null)
            {
                bot = new Bot();
            }
            return bot;
        }
        private void Run()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Clear();
            Console.Title = "GameTime";
            Console.WriteLine("GameTime Console");
            Console.WriteLine("Is this a test startup?(y/n)");
            string response = Console.ReadLine().ToLower();
            while(response != "y" && response != "n")
            {
                response = Console.ReadLine().ToLower();
            }
            if(response == "y")
            {
                Console.WriteLine("Launching as test startup...");
                Setup();
            }
            else
            {
                Console.WriteLine("Launching as main startup...");
                Setup();
            }
            
        }
        private void Setup()
        {
            Console.WriteLine("Loading Libraries");
            //GameItems = new ItemDatabase();
            Console.WriteLine("Items were not loaded.");
            GameSessions = new MutliplayerGameSessions();

            Console.WriteLine("Loading Config");
            Config = Config.LoadConfig();
            if(Config == null)
            {
                Console.WriteLine("First time setup required.");
                //Token and prefix setup
                var token = "";
                var prefix = "";
                while (string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("Input Bot Token");
                    token = Console.ReadLine();
                }
                while (string.IsNullOrEmpty(prefix))
                {
                    Console.WriteLine("Input Prefix");
                    prefix = Console.ReadLine();
                }
                //Home server setup
                Console.WriteLine("Input home guild id.");
                var id = Console.ReadLine();
                Config = new Config(token, prefix, id);
                Config.SaveConfig(Config);
                Console.WriteLine("Setup completed.");
            }
            else
            {
                Console.WriteLine("Config Loaded.");
                Console.WriteLine("Use designated home server?(y/n)");
                var response = Console.ReadLine().ToLower();
                while (response != "y" && response != "n")
                {
                    response = Console.ReadLine().ToLower();
                }
                if (response == "n")
                {
                    ChangeHomeGuild();
                }
            }
            DiscordAsync().GetAwaiter().GetResult();
        }
        private void ChangeHomeGuild()
        {
            Console.WriteLine("Input new home guild id.");
            var id = Console.ReadLine();
            Config = new Config(Config.Token, Config.Prefix, id);
            Config.SaveConfig(Config);
            Console.WriteLine("New guild id saved.");
        }
        private async Task DiscordAsync()
        {
            Console.WriteLine("Setting up discord configs...");
            var client = new DiscordClient(new DiscordConfiguration()
            {
                Token = Config.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                Intents = DiscordIntents.MessageContents | DiscordIntents.GuildMessages | DiscordIntents.Guilds
            });
            Interactivity = client.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromMinutes(5)
            });
            var commands = client.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = new List<string>() { Config.Prefix },
                EnableDms = true,
                EnableDefaultHelp = false,
                EnableMentionPrefix = false,
                IgnoreExtraArguments = true,
                CaseSensitive = false
            });
            var slash = client.UseSlashCommands();
            Client = client;
            HomeGuild = await client.GetGuildAsync(Config.HomeGuildId);
            while(HomeGuild == null)
            {
                Console.WriteLine("Home guild could not be found.");
                ChangeHomeGuild();
                HomeGuild = await client.GetGuildAsync(Config.HomeGuildId);
            }
            Console.WriteLine("Home guild loaded.");

            //Status channel
            var channels = Bot.HomeGuild.Channels.Values.GetEnumerator();
            channels.MoveNext(); //channels starts at null, this will skip that.
            while (channels.Current.Name != "gametime-status")
            {
                if (!channels.MoveNext())
                {
                    DefineStatusChannel();
                    break;
                }
                else if (channels.Current.Name == "gametime-status")
                {
                    Console.WriteLine("Status channel found, assigning channel as logging channel...");
                    LoggingChannel = channels.Current;
                    Console.WriteLine("Done.");
                }
            }

            client.Ready += Events.Client_Ready;
            client.ComponentInteractionCreated += Events.AcknowledgeComponentInteraction;
            commands.CommandErrored += Events.CommandErrored;
            //commands.RegisterCommands<HelpCommand>();
            //commands.RegisterCommands<ItemCommands>();
            commands.RegisterCommands<MinesweeperCommands>();
            commands.RegisterCommands<Connect4Commands>();
            //slash.RegisterCommands<Connect4Commands>();
            await client.ConnectAsync();
            await Task.Delay(-1);
        }
        private static void DefineStatusChannel()
        {
            Console.WriteLine("A status channel could not be found, define new channel with id...");
            while ((Bot.LoggingChannel = Bot.HomeGuild.GetChannel(UInt64.Parse(Console.ReadLine()))) == null)
            {
                Console.WriteLine("Channel could not be found, please try again...");
            }
            Console.WriteLine("Channel successfully loaded.");
        }
    }
}
