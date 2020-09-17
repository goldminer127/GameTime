using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using GameTime.Models;
using System.Threading.Tasks;
using DSharpPlus.Interactivity;
using System;

namespace GameTime.Commands
{
    public class AdminCommands :  BaseCommandModule
    {
        [Command ("ping")]
        [Description("Admin Command\nCommand Type: Connection Test\nReturns Ping")]
        [Hidden()]
        public async Task Ping(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder();
            embed.Title = "Ping";
            embed.AddField("Pong!", $"Response Time: {ctx.Client.Ping} ms");
            if (ctx.Client.Ping < 100 && ctx.Member.IsBot != true)
            {
                embed.Color = DiscordColor.Green;
            }
            else if (ctx.Client.Ping >= 100 && ctx.Client.Ping <= 1000 )
            {
                embed.Color = DiscordColor.Orange;
            }
            else
            {
                embed.Color = DiscordColor.Red;
            }
            await ctx.Channel.SendMessageAsync(embed: embed);
        }

        [Command("delete-player"), Aliases("delete")]
        [Description("Admin Command\nCommand Type: Mod\nDeletes Target")]
        [Hidden()]
        public async Task DeletePlayer(CommandContext ctx, [Description("User id")] long Id)
        {
            var embed = new DiscordEmbedBuilder();
            var authorized = AuthorizationAdmin(ctx);
            if (authorized == true)
            {
                var member = "";
                Player dPlayer = null;
                dPlayer = Bot.PlayerDatabase.GetPlayerByID(Id);
                var interactivity = ctx.Client.GetInteractivity();
                if (dPlayer == null)
                {
                    embed.Title = "User Not Found";
                }
                else
                {
                    member = dPlayer.Name;
                    embed.Description = $"Are you sure you want to delete {member} from the game?";
                    await ctx.Channel.SendMessageAsync(embed: embed);
                    var response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(60));
                    if (response.Result.Content.ToLower() == "yes" || response.Result.Content.ToLower() == "i am sure" || response.Result.Content.ToLower() == "affirmative" || response.Result.Content.ToLower() == "positive")
                    {
                        embed.Description = $"Understood...";
                        await ctx.Channel.SendMessageAsync(embed: embed);
                        Bot.PlayerDatabase.DeletePlayer(Convert.ToInt64(Id));
                        embed.Title = $"{member} Deleted";
                        embed.Description = $"{member} has been removed from the system";
                        embed.Color = DiscordColor.Green;
                    }
                    else if (response.Result.Content.ToLower() == "no" || response.Result.Content.ToLower() == "negative")
                    {
                        embed.Description = $"Understood...";
                        await ctx.Channel.SendMessageAsync(embed: embed);
                        embed.Title = $"No Action Taken";
                        embed.Description = $"No action has been done to {member}.";
                        embed.Color = DiscordColor.Green;
                    }
                    else if (response.TimedOut)
                    {
                        embed.Description = $"No response detected...";
                        await ctx.Channel.SendMessageAsync(embed: embed);
                        await Task.Delay(2000);
                        embed.Title = $"No Action Taken";
                        embed.Description = $"No action has been done to {member}.";
                        embed.Color = DiscordColor.Gray;
                    }
                    else
                    {
                        embed.Description = $"I did not understand. Please reply with a valid response.";
                        embed.Color = DiscordColor.Red;
                    }
                }
                await ctx.Channel.SendMessageAsync(embed: embed);
                await Thanks(ctx);
            }
            else
            {
                embed.Title = "Action Not Authorized";
                embed.Color = DiscordColor.Red;
                await ctx.Channel.SendMessageAsync(embed: embed);
            }

        }

        [Command("ban")]
        [Description("Admin Command\nCommand Type: Mod\nBans Target From Main Feature")]
        [Hidden()]
        public async Task Ban(CommandContext ctx, [Description("User id")]long Id)
        {
            var embed = new DiscordEmbedBuilder();
            var authorized = AuthorizationUser(ctx);
            if (authorized == false)
            {
                authorized = AuthorizationAdmin(ctx);
            }
            if (authorized == true)
            {
                var member = "";
                var dPlayer = Bot.PlayerDatabase.GetPlayerByID(Id);
                var interactivity = ctx.Client.GetInteractivity();
                if (dPlayer == null)
                {
                    embed.Title = "User Not Found";
                }
                else if (dPlayer.IsBanned == true)
                {
                    member = dPlayer.Name;
                    embed.Description = $"{member} is already banned.";
                    embed.Color = DiscordColor.Red;
                }
                else
                {
                    member = dPlayer.Name;
                    embed.Description = $"Are you sure you want to ban {member} from the game?";
                    await ctx.Channel.SendMessageAsync(embed: embed);
                    var response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(60));
                    if (response.Result.Content.ToLower() == "yes" || response.Result.Content.ToLower() == "i am sure" || response.Result.Content.ToLower() == "affirmative" || response.Result.Content.ToLower() == "positive")
                    {
                        embed.Description = $"Understood...";
                        await ctx.Channel.SendMessageAsync(embed: embed);
                        dPlayer.IsBanned = true;
                        dPlayer.GuildsOptedIn.Clear();
                        Bot.PlayerDatabase.UpdatePlayer(dPlayer);
                        embed.Title = $"{member} Banned";
                        embed.Description = $"{member} has been banned from playing the game of GameTime.";
                        embed.Color = DiscordColor.Green;
                    }
                    else if (response.Result.Content.ToLower() == "no" || response.Result.Content.ToLower() == "negative")
                    {
                        embed.Description = $"Understood...";
                        await ctx.Channel.SendMessageAsync(embed: embed);
                        embed.Title = $"No Action Taken";
                        embed.Description = $"No action has been done to {member}.";
                        embed.Color = DiscordColor.Green;
                    }
                    else if (response.TimedOut)
                    {
                        embed.Description = $"No response detected...";
                        await ctx.Channel.SendMessageAsync(embed: embed);
                        await Task.Delay(2000);
                        embed.Title = $"No Action Taken";
                        embed.Description = $"No action has been done to {member}.";
                        embed.Color = DiscordColor.Gray;
                    }
                    else
                    {
                        embed.Description = $"I did not understand. Please reply with a valid response.";
                        embed.Color = DiscordColor.Red;
                    }
                }
                await ctx.Channel.SendMessageAsync(embed: embed);
                await Thanks(ctx);
            }
            else
            {
                embed.Title = "Action Not Authorized";
                embed.Color = DiscordColor.Red;
                await ctx.Channel.SendMessageAsync(embed: embed);
            }
        }
        [Command("unban")]
        [Description("Admin Command\nCommand Type: Mod\nUnbans Target From Main Feature")]
        [Hidden()]
        public async Task Unban(CommandContext ctx, [Description("User id")] long Id)
        {
            var embed = new DiscordEmbedBuilder();
            var authorized = AuthorizationUser(ctx);
            if (authorized == false)
            {
                authorized = AuthorizationAdmin(ctx);
            }
            if (authorized == true)
            {
                var member = "";
                var dPlayer = Bot.PlayerDatabase.GetPlayerByID(Id);
                member = dPlayer.Name;
                var interactivity = ctx.Client.GetInteractivity();
                if (dPlayer == null)
                {
                    embed.Title = "User Not Found";
                }
                else if (dPlayer.IsBanned == false)
                {
                    embed.Description = $"{member} is already not banned.";
                    embed.Color = DiscordColor.Red;
                }
                else
                {
                    embed.Description = $"Are you sure you want to unban {member}?";
                    await ctx.Channel.SendMessageAsync(embed: embed);
                    var response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(60));
                    if (response.Result.Content.ToLower() == "yes" || response.Result.Content.ToLower() == "i am sure" || response.Result.Content.ToLower() == "affirmative" || response.Result.Content.ToLower() == "positive")
                    {
                        embed.Description = $"Understood...";
                        await ctx.Channel.SendMessageAsync(embed: embed);
                        dPlayer.IsBanned = false;
                        Bot.PlayerDatabase.UpdatePlayer(dPlayer);
                        embed.Title = $"{member} has been unbanned";
                        embed.Description = $"{member} has been unbanned from GameTime.";
                        embed.Color = DiscordColor.Green;
                    }
                    else if (response.Result.Content.ToLower() == "no" || response.Result.Content.ToLower() == "negative")
                    {
                        embed.Description = $"Understood...";
                        await ctx.Channel.SendMessageAsync(embed: embed);
                        embed.Title = $"No Action Taken";
                        embed.Description = $"No action has been done to {member}.";
                        embed.Color = DiscordColor.Green;
                    }
                    else if (response.TimedOut)
                    {
                        embed.Description = $"No response detected...";
                        await ctx.Channel.SendMessageAsync(embed: embed);
                        await Task.Delay(2000);
                        embed.Title = $"No Action Taken";
                        embed.Description = $"No action has been done to {member}.";
                        embed.Color = DiscordColor.Gray;
                    }
                    else
                    {
                        embed.Description = $"I did not understand. Please reply with a valid response.";
                        embed.Color = DiscordColor.Red;
                    }
                }
                await ctx.Channel.SendMessageAsync(embed: embed);
                await Thanks(ctx);
            }
            else
            {
                embed.Title = "Action Not Authorized";
                embed.Color = DiscordColor.Red;
                await ctx.Channel.SendMessageAsync(embed: embed);
            }
        }

        [Command("give")]
        [Description("Admin Command\nCommand Type: Admin\nGives Target Specified Item")]
        [Hidden()]
        public async Task Give(CommandContext ctx, [Description("Discord user")] DiscordMember member, [Description("Item name")][RemainingText]string item)
        {
            var embed = new DiscordEmbedBuilder();
            var authorized = AuthorizationAdmin(ctx);
            if (authorized == true)
            {
                var user = Bot.PlayerDatabase.GetPlayerByID(Convert.ToInt64(member.Id));
                if (user == null)
                {
                    embed.Title = "User Not Found";
                }
                else
                {
                    Item it = null;
                    foreach (var i in Bot.Items.AllItems)
                    {
                        if (i.Name.ToLower() == item.ToLower() || i.Subname.ToLower() == item.ToLower())
                        {
                            it = i;
                        }
                    }
                    if (it != null)
                    {
                        embed.Description = $"{it.Name} given to {member.Mention}";
                        var isInInventory = false;
                        GameTime.Models.Item copy = null;
                        foreach (var thing in user.Inventory)
                        {
                            if (thing.ID == it.ID)
                            {
                                copy = thing;
                                isInInventory = true;
                                break;
                            }
                        }
                        if (isInInventory == false)
                        {
                            user.Inventory.Add(it);
                        }
                        else
                        {
                            copy.Multiple++;
                        }
                    }
                    else
                    {
                        embed.Title = "Item not found";
                    }
                }
                Bot.PlayerDatabase.UpdatePlayer(user);
                await ctx.Channel.SendMessageAsync(embed: embed);
            }
            else
            {
                embed.Title = "Action Not Authorized";
                embed.Color = DiscordColor.Red;
                await ctx.Channel.SendMessageAsync(embed: embed);
            }
        }
        [Command("stop"), Aliases("shutdown", "close", "break")]
        [RequireOwner]
        [Hidden]
        [Description("Shut down the bot")]
        public async Task Stop(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder();
            embed.Title = "Shutdown Detected";
            embed.Description = "Shutdown order recieved. GameTime closing down...";
            embed.Color = DiscordColor.DarkRed;
            await ctx.Channel.SendMessageAsync(embed: embed);
            await Task.Delay(500);
            Environment.Exit(0);
        }
        [Command("add-authorize"), Aliases("authorize", "au")]
        [RequireOwner]
        [Hidden]
        [Description("Authorize a user")]
        public async Task AddAuhtorization(CommandContext ctx, long Id)
        {
            var embed = new DiscordEmbedBuilder();
            var interactivity = ctx.Client.GetInteractivity();
            embed.Title = "Branch";
            embed.Description = "Which branch is the user being authorized to?\n\nAdmin/Mod?\n\nType cancel to cancel action.";
            await ctx.Channel.SendMessageAsync(embed: embed);
            var response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(60));
            if(response.Result.Content.ToLower() == "admin")
            {
                embed.Title = "Confirmation";
                embed.Description = $"Are you sure you want to authorize {Id} as an admin?\nType \"yes\" to confirm. Type anything else to cancel.";
                await ctx.Channel.SendMessageAsync(embed: embed);
                response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(60));
                if(Confirmation(ctx, response.Result) == true)
                {
                    var user = Bot.PlayerDatabase.GetPlayerByID(Id);
                    user.AuthroizedAdmin = true;
                    Bot.PlayerDatabase.UpdatePlayer(user);
                    embed.Title = "Success";
                    embed.Description = $"User Id: {Id} has been authorized as an admin.";
                    embed.Color = DiscordColor.Green;
                }
                else
                {
                    embed.Title = "Canceled";
                    embed.Description = $"No action has been done.";
                    embed.Color = DiscordColor.Red;
                }
            }
            else if (response.Result.Content.ToLower() == "mod")
            {
                embed.Title = "Confirmation";
                embed.Description = $"Are you sure you want to authorize {Id} to use mod commands?\nType \"yes\" to confirm. Type anything else to cancel.";
                await ctx.Channel.SendMessageAsync(embed: embed);
                response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(60));
                if (Confirmation(ctx, response.Result) == true)
                {
                    var user = Bot.PlayerDatabase.GetPlayerByID(Id);
                    user.AuthroizedMod = true;
                    Bot.PlayerDatabase.UpdatePlayer(user);
                    embed.Title = "Success";
                    embed.Description = $"User Id: {Id} has been authorized to use mod commands.";
                    embed.Color = DiscordColor.Green;
                }
                else
                {
                    embed.Title = "Canceled";
                    embed.Description = $"No action has been done.";
                    embed.Color = DiscordColor.Red;
                }
            }
            else if (response.Result.Content.ToLower() == "cancel")
            {
                embed.Title = "Canceled";
                embed.Description = $"No action has been done.";
                embed.Color = DiscordColor.Red;
            }
            else
            {
                embed.Title = "Canceled";
                embed.Description = $"Invalid response detected, action terminated.";
                embed.Color = DiscordColor.Red;
            }
            await ctx.Channel.SendMessageAsync(embed: embed);
        }
        [Command("remove-authorized"), Aliases("unauthorize", "ua")]
        [RequireOwner]
        [Hidden]
        [Description("Unauthorize a user")]
        public async Task Unauhtorization(CommandContext ctx, long Id)
        {
            var embed = new DiscordEmbedBuilder();
            var interactivity = ctx.Client.GetInteractivity();
            embed.Title = "Branch";
            embed.Description = "Which branch is the user being unauthorized from?\n\nAdmin/Mod?\n\nType cancel to cancel action.";
            await ctx.Channel.SendMessageAsync(embed: embed);
            var response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(60));
            if (response.Result.Content.ToLower() == "admin")
            {
                embed.Title = "Confirmation";
                embed.Description = $"Are you sure you want to unauthorize {Id} from admin?\nType \"yes\" to confirm. Type anything else to cancel.";
                await ctx.Channel.SendMessageAsync(embed: embed);
                response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(60));
                if (Confirmation(ctx, response.Result) == true)
                {
                    var user = Bot.PlayerDatabase.GetPlayerByID(Id);
                    user.AuthroizedAdmin = false;
                    Bot.PlayerDatabase.UpdatePlayer(user);
                    embed.Title = "Success";
                    embed.Description = $"User Id: {Id} has been unauthorized.";
                    embed.Color = DiscordColor.Green;
                }
                else
                {
                    embed.Title = "Canceled";
                    embed.Description = $"No action has been done.";
                    embed.Color = DiscordColor.Red;
                }
            }
            else if (response.Result.Content.ToLower() == "mod")
            {
                embed.Title = "Confirmation";
                embed.Description = $"Are you sure you want to unauthorize {Id} from using mod commands?\nType \"yes\" to confirm. Type anything else to cancel.";
                await ctx.Channel.SendMessageAsync(embed: embed);
                response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(60));
                if (Confirmation(ctx, response.Result) == true)
                {
                    var user = Bot.PlayerDatabase.GetPlayerByID(Id);
                    user.AuthroizedMod = false;
                    Bot.PlayerDatabase.UpdatePlayer(user);
                    embed.Title = "Success";
                    embed.Description = $"User Id: {Id} has been unauthorized.";
                    embed.Color = DiscordColor.Green;
                }
                else
                {
                    embed.Title = "Canceled";
                    embed.Description = $"No action has been done.";
                    embed.Color = DiscordColor.Red;
                }
            }
            else if (response.Result.Content.ToLower() == "cancel")
            {
                embed.Title = "Canceled";
                embed.Description = $"No action has been done.";
                embed.Color = DiscordColor.Red;
            }
            else
            {
                embed.Title = "Canceled";
                embed.Description = $"Invalid response detected, action terminated.";
                embed.Color = DiscordColor.Red;
            }
            await ctx.Channel.SendMessageAsync(embed: embed);
        }
        private bool AuthorizationAdmin(CommandContext ctx)
        {
            var authorized = false;
            if (Bot.PlayerDatabase.GetPlayerByID(Convert.ToInt64(ctx.Member.Id)).AuthroizedAdmin == true)
            {
                authorized = true;
            }
            return authorized;
        }
        private bool AuthorizationUser(CommandContext ctx)
        {
            var authorized = false;
            if(Bot.PlayerDatabase.GetPlayerByID(Convert.ToInt64(ctx.Member.Id)).AuthroizedMod == true)
            {
                authorized = true;
            }
            return authorized;
        }
        private async Task Thanks(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder();
            var interactivity = ctx.Client.GetInteractivity();
            var response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(10));
            if (response.TimedOut)
            {

            }
            else if (response.Result.Content.ToLower() == "thank you" || response.Result.Content.ToLower() == "thankyou" || response.Result.Content.ToLower() == "thanks")
            {
                embed.Title = null;
                embed.ClearFields();
                embed.Color = DiscordColor.Black;
                embed.Description = $"You're welcome {ctx.Member.Username}";
                await ctx.Channel.SendMessageAsync(embed: embed);
            }
            else
            {

            }
        }
        private bool Confirmation(CommandContext ctx, DiscordMessage response)
        {
            var confirmed = false;
            if (response.Content.ToLower() == "yes")
            {
                confirmed = true;
            }
            return confirmed;
        }
    }
}

