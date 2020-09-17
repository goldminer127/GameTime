using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Threading.Tasks;

namespace GameTime
{
    public static class Events
    {
        public static async Task Client_Ready(ReadyEventArgs e)
        {
            Console.WriteLine("Items Initialized");
            Console.WriteLine("Player Batabase Ready");
            Console.WriteLine("GameTime Ready");
        }
        public static async Task CommandErrored(CommandErrorEventArgs e)
        {
            await e.Context.Channel.TriggerTypingAsync().ConfigureAwait(false);
            var embed = new DiscordEmbedBuilder();
            if (e.Exception is CommandNotFoundException)
            {
                embed.Title = "Error";
                embed.AddField("Invalid Command", "Please check help for available commands");
                embed.Color = DiscordColor.Red;
                await e.Context.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);
            }
            else if (e.Exception is ArgumentException)
            {
                embed.Title = "Error";
                embed.AddField("Invalid Arguments", "Please check help for correct arguments needed");
                embed.Color = DiscordColor.Red;
                await e.Context.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);
            }
            else
            {
                var errorCode = "";
                for (int num = 0; num < 20; num++)
                {
                    errorCode += new Random().Next(1, 9);
                }
                embed.Title = "Error";
                embed.AddField("Command Executed", $"{e.Context.Message.Content}\nExecuted at: {e.Context.Message.Timestamp}");
                embed.AddField("Error Report Code", errorCode);
                embed.AddField(e.Exception.Message, e.Exception.StackTrace);
                embed.AddField("Guild Command Performed In", $"{e.Context.Guild.Banner}\n{e.Context.Guild.Name}\n{e.Context.Guild.Id}\nServer Owner: {e.Context.Guild.Owner.Username}#{e.Context.Guild.Owner.Discriminator}\n{e.Context.Guild.Owner.Id}");
                embed.AddField("User That Executed Command", $"{e.Context.Member.Username}#{e.Context.Member.Discriminator}\n{e.Context.Member.Id}");
                embed.Color = DiscordColor.Red;
                var channel = Bot.HomeGuild.GetChannel(750204377234669718);
                await channel.SendMessageAsync(embed: embed).ConfigureAwait(false);
                embed.ClearFields();
                embed.Title = "Error";
                embed.AddField("GameTime Error", "An error has occured when attempting to execute the command. For more information and assistance please join GameTime's discord server.");
                embed.AddField("Error Report Code", errorCode);
                embed.AddField("GameTime Discord Server", "https://discord.gg/vxPTsqW");
                embed.Color = DiscordColor.Red;
                await e.Context.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);
            }
        }
    }
}