using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using GameTime.Models;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using DSharpPlus.Interactivity.Extensions;
using System.IO;
using System.Text.Json;

namespace GameTime.Commands
{
    public class AdminCommands : BaseCommandModule
    {
        [Command("ping")]
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
            else if (ctx.Client.Ping >= 100 && ctx.Client.Ping <= 1000)
            {
                embed.Color = DiscordColor.Orange;
            }
            else
            {
                embed.Color = DiscordColor.Red;
            }
            await ctx.Channel.SendMessageAsync(embed: embed);
        }

        [Command("delete-all")]
        [Description("Admin Command\nCommand Type: Mod\nDeletes All Players")]
        [RequireOwner]
        [Hidden()]
        public async Task DeleteAll(CommandContext ctx)
        {
            foreach (var player in Bot.PlayerDatabase.Players)
            {
                Bot.PlayerDatabase.DeletePlayer(player.ID);
            }
            await ctx.Channel.SendMessageAsync("Databse Reset");
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
                    if (response.TimedOut)
                    {
                        embed.Description = $"No response detected...";
                        await ctx.Channel.SendMessageAsync(embed: embed);
                        await Task.Delay(2000);
                        embed.Title = $"No Action Taken";
                        embed.Description = $"No action has been done to {member}.";
                        embed.Color = DiscordColor.Gray;
                    }
                    else if (response.Result.Content.ToLower() == "yes" || response.Result.Content.ToLower() == "i am sure" || response.Result.Content.ToLower() == "affirmative" || response.Result.Content.ToLower() == "positive")
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
        public async Task Ban(CommandContext ctx, [Description("User id")] long Id)
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
        public async Task Give(CommandContext ctx, [Description("Discord user")] DiscordMember member, [Description("Item name")][RemainingText] string input)
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
                    var splitInput = input.Split(" x");
                    var amount = 1;
                    var item = "";
                    if (splitInput.Length == 1)
                    {
                        item = splitInput[0];
                    }
                    else
                    {
                        item = splitInput[0];
                        try
                        {
                            amount = int.Parse(splitInput[1]);

                        }
                        catch
                        {
                            embed.Title = "Input incorrect";
                            embed.Color = DiscordColor.Red;
                            goto msg;
                        }
                    }
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
                        embed.Description = $"{it.Name} x{amount} given to {member.Mention}";
                        GeneralFunctions.AddToInventory(user, it, amount);
                    }
                    else
                    {
                        embed.Title = "Item not found";
                    }
                }
                Bot.PlayerDatabase.UpdatePlayer(user);
            msg:
                await ctx.Channel.SendMessageAsync(embed: embed);
            }
            else
            {
                embed.Title = "Action Not Authorized";
                embed.Color = DiscordColor.Red;
                await ctx.Channel.SendMessageAsync(embed: embed);
            }
        }
        [Command("manage-metals"), Aliases("mm", "metals")]
        [Hidden]
        [Description("Admin Command\nCommand Type: Admin\nGives Target Specified Metal")]
        public async Task Metal(CommandContext ctx, [RemainingText] string playerName)
        {
            var interactivity = ctx.Client.GetInteractivity();
            var embed = new DiscordEmbedBuilder();
            var authorized = AuthorizationUser(ctx);
            if (authorized == false)
            {
                authorized = AuthorizationAdmin(ctx);
            }
            if (authorized == true)
            {
                Player user;
                long id;
                try
                {
                    id = Convert.ToInt64(playerName);
                    user = Bot.PlayerDatabase.GetPlayerByID(Convert.ToInt64(id));
                }
                catch
                {
                    user = Bot.PlayerDatabase.GetPlayerByName(playerName);
                }
                if (user == null)
                {
                    embed.Title = "No user can be found (Names are case sensitive)";
                    embed.Color = DiscordColor.Red;
                    await ctx.Channel.SendMessageAsync(embed: embed);
                }
                else
                {
                    embed.Title = "Metal List";
                    embed.Description = $"Do you want to give or remove a metal from {user.Name}? Type cancel to cancel command";
                    var metalList = "Veteran\nSwatter";
                    var userMetals = "";
                    if (user.Metals.Count > 0)
                    {
                        foreach (var metal in user.Metals)
                        {
                            userMetals += $"{metal}\n";
                        }
                    }
                    else
                    {
                        userMetals = " ‏‏‎ ";
                    }
                    embed.AddField("Metals", metalList);
                    embed.AddField($"{user.Name}'s Metals", userMetals);
                    await ctx.Channel.SendMessageAsync(embed: embed);
                    var response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member && (x.Content.ToLower() == "give" || x.Content.ToLower() == "remove" || x.Content.ToLower() == "cancel"), TimeSpan.FromSeconds(60));
                    if (response.TimedOut)
                    {
                        await ctx.Channel.SendMessageAsync("Command Timed Out");
                    }
                    else if (response.Result.Content.ToLower() == "give")
                    {
                        await ctx.Channel.SendMessageAsync("Which metal would you like to award from the list?");
                        response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(60));
                        var emoji = Bot.AllMetals.GetMetal(response.Result.Content.ToLower());
                        if (emoji == null)
                        {
                            await ctx.Channel.SendMessageAsync("Emoji not found");
                        }
                        else
                        {
                            user.Metals.Add(emoji);
                            await ctx.Channel.SendMessageAsync($"{emoji.Name} was awarded to {user.Name}");
                        }
                    }
                    else if (response.Result.Content.ToLower() == "remove")
                    {
                        await ctx.Channel.SendMessageAsync($"Which metal would you like to remove from {user.Name}?");
                        response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(60));
                        var emoji = Bot.AllMetals.GetMetal(response.Result.Content.ToLower());
                        if (emoji == null)
                        {
                            await ctx.Channel.SendMessageAsync("Emoji not found");
                        }
                        else
                        {
                            user.Metals.Remove(emoji);
                            await ctx.Channel.SendMessageAsync($"{emoji.Name} was removed from {user.Name}");
                        }
                    }
                    else
                    {
                        await ctx.Channel.SendMessageAsync("Command canceled");
                    }
                    Bot.PlayerDatabase.UpdatePlayer(user);
                }
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
            if (response.Result.Content.ToLower() == "admin")
            {
                embed.Title = "Confirmation";
                embed.Description = $"Are you sure you want to authorize {Id} as an admin?\nType \"yes\" to confirm. Type anything else to cancel.";
                await ctx.Channel.SendMessageAsync(embed: embed);
                response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(60));
                if (Confirmation(ctx, response.Result) == true)
                {
                    var user = Bot.PlayerDatabase.GetPlayerByID(Id);
                    user.AuthroizedAdmin = true;
                    user.AuthroizedMod = true;
                    Bot.PlayerDatabase.UpdatePlayer(user);
                    embed.Title = "Success";
                    embed.Description = $"User Id: {Id} has been authorized as an admin.";
                    embed.Color = DiscordColor.Green;
                }
                else
                {
                    embed.Title = "cancelled";
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
                    embed.Title = "cancelled";
                    embed.Description = $"No action has been done.";
                    embed.Color = DiscordColor.Red;
                }
            }
            else if (response.Result.Content.ToLower() == "cancel")
            {
                embed.Title = "cancelled";
                embed.Description = $"No action has been done.";
                embed.Color = DiscordColor.Red;
            }
            else
            {
                embed.Title = "cancelled";
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
                    embed.Title = "cancelled";
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
                    embed.Title = "cancelled";
                    embed.Description = $"No action has been done.";
                    embed.Color = DiscordColor.Red;
                }
            }
            else if (response.Result.Content.ToLower() == "cancel")
            {
                embed.Title = "cancelled";
                embed.Description = $"No action has been done.";
                embed.Color = DiscordColor.Red;
            }
            else
            {
                embed.Title = "cancelled";
                embed.Description = $"Invalid response detected, action terminated.";
                embed.Color = DiscordColor.Red;
            }
            await ctx.Channel.SendMessageAsync(embed: embed);
        }
        [Command("update-all"), Aliases("update")]
        [RequireOwner]
        [Hidden]
        [Description("Updates all players to newest version.")]
        public async Task UpdateAll(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder();
            embed.Title = "Players updated";
            var bugged = true;
            var updated = false;
            List<Item> buggedItems = new List<Item>();
            List<Item> oldItem = new List<Item>();
            foreach (var player in Bot.PlayerDatabase.Players)
            {
                foreach (var item in player.Inventory)
                {
                    var setItem = Bot.Items.GetItem(item);
                    if ((setItem != null) && (item.Name == setItem.Name && item.Rarity == setItem.Rarity && item.ID == setItem.ID))
                    {
                        bugged = false;
                        if ((setItem != null) && (item.Value == setItem.Value && item.ID == setItem.ID))
                        {
                            updated = true;
                        }
                    }
                    if (bugged == true)
                        buggedItems.Add(item);
                    if (updated == false)
                        oldItem.Add(item);
                    updated = false;
                    bugged = true;
                }
                foreach (var item in buggedItems)
                {
                    player.Inventory.Remove(item);
                }
                foreach (var item in oldItem)
                {
                    foreach (var pitem in player.Inventory)
                    {
                        if (pitem.ID == item.ID)
                        {
                            var setItem = Bot.Items.GetItem(item);
                            pitem.Value = setItem.Value;
                            break;
                        }
                    }
                }
                if(player.Mail == null)
                {
                    player.Mail = new List<Mail>();
                    player.AcceptMail = true;
                }
                try
                {
                    var member = ctx.Guild.GetMemberAsync((ulong)player.ID).Result;
                    player.Image = member.AvatarUrl;
                    player.Name = member.Username;
                    embed.Description += $"[{player.Name}]: Updated\n";
                }
                catch
                {
                    embed.Description += $"[{player.Name}]: Name and Image could not be found. Inventory updated\n";
                }
                Bot.PlayerDatabase.UpdatePlayer(player);
            }
            await MailAll(ctx);
            embed.Color = DiscordColor.Green;
            await ctx.Channel.SendMessageAsync(embed: embed);
        }
        [Command("give-credit")]
        [Description("Admin Command\nCommand Type: Admin\nGives target credits")]
        [RequireOwner]
        [Hidden()]
        public async Task GiveCredit(CommandContext ctx, long id)
        {
            var interactivity = ctx.Client.GetInteractivity();
            var user = Bot.PlayerDatabase.GetPlayerByID(id);
            if (user == null)
            {
                await ctx.Channel.SendMessageAsync("User does not exist");
            }
            else
            {
                await ctx.Channel.SendMessageAsync("How many credits do you want to give?");
                var response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(120));
                if (response.TimedOut)
                {
                    await ctx.Channel.SendMessageAsync("Command timed out");
                }
                else
                {
                    try
                    {
                        var credits = Int32.Parse(response.Result.Content);
                        user.Credits += credits;
                        Bot.PlayerDatabase.UpdatePlayer(user);
                        await ctx.Channel.SendMessageAsync("Success");
                    }
                    catch
                    {
                        await ctx.Channel.SendMessageAsync("Incorrect input");
                    }
                }
            }
        }
        [Command("write")]
        [Description("")]
        [RequireOwner]
        [Hidden()]
        public async Task WritePlayers(CommandContext ctx)
        {
            var json = new List<string>();
            var filePath = "PlayerLog.json";
            foreach (var player in Bot.PlayerDatabase.Players)
            {
                var information = "";
                information += $"{player.Name} Inventory:";
                json.Add(JsonSerializer.Serialize<string>(information));
                foreach (var item in player.Inventory)
                {
                    var items = $"{item.Name} {item.Multiple}x";
                    json.Add(JsonSerializer.Serialize<string>(items));
                }
                json.Add(JsonSerializer.Serialize<string>(" "));
            }
            File.WriteAllLines(filePath, json);
            await ctx.Channel.SendMessageAsync("Player database uploaded.");
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
            if (Bot.PlayerDatabase.GetPlayerByID(Convert.ToInt64(ctx.Member.Id)).AuthroizedMod == true)
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
        [Command("warn")]
        [Hidden]
        [Description("Warn a user")]
        public async Task Warn(CommandContext ctx, long Id, [RemainingText] string reason = null)
        {
            var interactivity = ctx.Client.GetInteractivity();
            var embed = new DiscordEmbedBuilder();
            var authorized = AuthorizationUser(ctx);
            var user = Bot.PlayerDatabase.GetPlayerByID(Id);
            var channel = Bot.HomeGuild.GetChannel(750036961447903324);
            if (authorized == false)
            {
                authorized = AuthorizationAdmin(ctx);
            }
            if (authorized == true && ctx.Channel.Id == channel.Id)
            {
                if(user == null)
                {
                    embed.Title = "No User Found";
                }
                else
                {
                    if(reason == null)
                    {
                        await ctx.Channel.SendMessageAsync($"Please provide a reason for warning {user.Name}");
                        var response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(120));
                        if (response.TimedOut)
                        {
                            goto end;
                        }
                        else
                        {
                            reason = response.Result.Content;
                        }
                    }
                    if (user.Warnings > 3)
                    {
                        await ctx.Channel.SendMessageAsync($"{user.Name} has {user.Warnings} warnings, would you like to ban them for exceeding the safe warning limit? Reply with yes or no");
                        var response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(120));
                        if(response.TimedOut)
                        {
                            goto end;
                        }
                        else if(response.Result.Content.ToLower() == "yes")
                        {
                            reason = "Recieved too many warnings";
                            user.IsBanned = true;
                            Bot.PlayerDatabase.UpdatePlayer(user);
                            embed.Title = "Ban successful";
                            embed.Description = $"{user.Name} was banned for:\n__{reason}__";
                            await channel.SendMessageAsync(embed: embed);
                        }
                        else
                        {
                            user.Warnings++;
                            Bot.PlayerDatabase.UpdatePlayer(user);
                            embed.Title = "Warning successful";
                            embed.Description = $"{user.Name} was warned for:\n__{reason}__";
                            await channel.SendMessageAsync(embed: embed);
                        }
                    }
                    else
                    {
                        user.Warnings++;
                        Bot.PlayerDatabase.UpdatePlayer(user);
                        embed.Title = "Warning successful";
                        embed.Description = $"{user.Name} was warned for:\n__{reason}__";
                        await channel.SendMessageAsync(embed: embed);
                    }
                }
            end:;
            }
            else
            {
                embed.Title = "Action Not Authorized";
                embed.Color = DiscordColor.Red;
                await ctx.Channel.SendMessageAsync(embed: embed);
            }
        }
        [Command("mail-all")]
        [Description("Admin Command\nCommand Type: Communication\nMails all players")]
        [RequireOwner]
        [Hidden()]
        public async Task MailAll(CommandContext ctx)
        {
            const int DELETION_TIME = 30000; //30 seconds
            var interactivity = ctx.Client.GetInteractivity();
            var embed = new DiscordEmbedBuilder();
            await ctx.Channel.SendMessageAsync("Title of mail");
            var response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(120));
            var title = response.Result.Content;
            await ctx.Channel.SendMessageAsync("What should be said in your mail?");
            response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(120));
            var content = response.Result.Content;
            foreach(var player in Bot.PlayerDatabase.Players)
            {
                if(player.AcceptMail)
                {
                    var mail = new Mail(title, content);
                    mail.Id = player.Mail.Count;
                    player.Mail.Add(mail);
                    Bot.PlayerDatabase.UpdatePlayer(player);
                    GeneralFunctions.AutoDeleteMail(ctx, DELETION_TIME, player.ID, mail.Id); //Do not apply await
                }
            }
            await ctx.Channel.SendMessageAsync("Mail sent!");
        }
        [Command("mail")]
        [Description("Admin Command\nCommand Type: Communication\nMails player")]
        [RequireOwner]
        [Hidden()]
        public async Task Mail(CommandContext ctx, long Id)
        {
            var player = Bot.PlayerDatabase.GetPlayerByID(Id);
            if (player == null)
            {
                await ctx.Channel.SendMessageAsync("No such player exists.");
            }
            else
            {
                const int DELETION_TIME = 30000; //30 seconds
                var interactivity = ctx.Client.GetInteractivity();
                var embed = new DiscordEmbedBuilder();
                await ctx.Channel.SendMessageAsync("Title of mail");
                var response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(120));
                var title = response.Result.Content;
                await ctx.Channel.SendMessageAsync("What should be said in your mail?");
                response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(120));
                var content = response.Result.Content;
                var mail = new Mail(title, content);
                mail.Id = player.Mail.Count;
                player.Mail.Add(mail);
                Bot.PlayerDatabase.UpdatePlayer(player);
                GeneralFunctions.AutoDeleteMail(ctx, DELETION_TIME, player.ID, mail.Id); //Do not apply await
                await ctx.Channel.SendMessageAsync("Mail sent!");
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