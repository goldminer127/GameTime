using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus;
using System;
using System.Threading.Tasks;

namespace GameTime
{
    public static class Events
    {
        public static async Task Client_Ready(DiscordClient client, ReadyEventArgs e)
        {
            try
            {
                foreach (var user in Bot.PlayerDatabase.Players)
                {
                    user.InMinigame = false;
                    Bot.PlayerDatabase.UpdatePlayer(user);
                }
                Console.WriteLine("All User Status Reset");
            }
            catch
            {
                Console.WriteLine("All User Status Reset Failed");
            }
            var channel = Bot.HomeGuild.GetChannel(729501695934988300);
            await channel.SendMessageAsync("GameTime Started").ConfigureAwait(false);
            Console.WriteLine("GameTime Ready");
        }
        public static async Task GuildMemberLeave(DiscordClient client, GuildMemberRemoveEventArgs e)
        {
            var user = Bot.PlayerDatabase.GetPlayerByID(Convert.ToInt64(e.Member.Id));
            if (user != null)
            {
                foreach (var guild in user.GuildsOptedIn)
                {
                    if (guild == Convert.ToInt64(e.Guild.Id))
                    {
                        user.GuildsOptedIn.Remove(guild);
                        Bot.PlayerDatabase.UpdatePlayer(user);
                        break;
                    }
                }
                Console.WriteLine($"{e.Member.Username} left {e.Guild.Name}");
            }
            var channel = Bot.HomeGuild.GetChannel(729501695934988300);
            await channel.SendMessageAsync($"{e.Member.Username} left {e.Guild.Name}").ConfigureAwait(false);
        }
        public static async Task CommandErrored(CommandsNextExtension cmd, CommandErrorEventArgs e)
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
                var channel = Bot.HomeGuild.GetChannel(729501695934988300);
                embed.AddField(e.Exception.Message, e.Exception.StackTrace);
                await channel.SendMessageAsync(embed: embed).ConfigureAwait(false);
            }
            else
            {
                var user = Bot.PlayerDatabase.GetPlayerByID((long)e.Context.Member.Id);
                user.InMinigame = false;
                Bot.PlayerDatabase.UpdatePlayer(user);
                Console.WriteLine($"{user.Name} status emergancy reset");
                try
                {
                    var channel = Bot.HomeGuild.GetChannel(729501695934988300);
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
                    await channel.SendMessageAsync(embed: embed).ConfigureAwait(false);
                    embed.ClearFields();
                    embed.Title = "Critical Error";
                    embed.AddField("GameTime Error", "An error has occured when attempting to execute the command. For more information and assistance please join GameTime's discord server.");
                    embed.AddField("Error Report Code", errorCode);
                    //embed.AddField("GameTime Discord Server", "https://discord.gg/vxPTsqW"); Keep disabled until 3.0
                    embed.Color = DiscordColor.Red;
                    await e.Context.Channel.SendMessageAsync(embed: embed).ConfigureAwait(false);
                    Console.WriteLine($"Error report {errorCode} sent successfully");
                }
                catch
                {
                    await e.Context.Channel.SendMessageAsync("A critical error occured but the error message could not be sent to site.").ConfigureAwait(false);
                    Console.WriteLine($"Error report could not be sent.\n\nReport:\n{e.Exception.Message}\n{e.Exception.StackTrace}");
                }
            }
        }
    }
}