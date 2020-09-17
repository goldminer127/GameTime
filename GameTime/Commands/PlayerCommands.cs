using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus;
using GameTime.Extensions;
using GameTime.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices.ComTypes;

namespace GameTime.Commands
{
    public class PlayerCommands : BaseCommandModule
    {
        [Command("inventory"), Aliases("i", "inv")]
        [Description("Displays the user's inventory.")]
        public async Task Inventory(CommandContext ctx, Int64 id = 0)
        {
            var embed = new DiscordEmbedBuilder();
            id = id == 0 ? Convert.ToInt64(ctx.Member.Id) : id;
            var user = Bot.PlayerDatabase.GetPlayerByID(Convert.ToInt64(id));
            var member = await ctx.Guild.GetMemberAsync(Convert.ToUInt64(id));
            embed.Title = $"{ member.Username }'s Inventory";
            if (user == null && ctx.Member.IsBot != true)
            {
                embed = NewPlayer(ctx, embed, member.Id);
                await ctx.Channel.SendMessageAsync(embed: embed);
            }
            else if (user == null && id != Convert.ToInt64(ctx.Member.Id))
            {
                embed.Title = "No user can be found";
                embed.Color = DiscordColor.Red;
                await ctx.Channel.SendMessageAsync(embed: embed);
            }
            else if (ctx.Member.IsBot != true)
            {
                if (user.Name != ctx.Member.Username)
                {
                    user.Name = ctx.Member.Username;
                }
                Bot.PlayerDatabase.UpdatePlayer(user);
                var totalItems = 0;
                string common = "", uncommon = "", rare = "", epic = "", unique = "";
                decimal inventoryValue = 0;
                foreach (var item in user.Inventory.OrderBy(y => y.Name).OrderBy(x => x.Rarity))
                {
                    switch (item.Rarity)
                    {
                        case Rarity.Common:
                            common += $"{item.Name} x{item.Multiple}\n";
                            break;
                        case Rarity.Uncommon:
                            uncommon += $"{item.Name} x{item.Multiple}\n";
                            break;
                        case Rarity.Rare:
                            rare += $"{item.Name} x{item.Multiple}\n";
                            break;
                        case Rarity.Epic:
                            epic += $"{item.Name} x{item.Multiple}\n";
                            break;
                        case Rarity.Unique:
                            unique += $"{item.Name} x{item.Multiple}\n";
                            break;
                    }
                    inventoryValue += item.Value;
                    totalItems += item.Multiple;
                }
                var isOptedIn = "No";
                foreach (var guild in user.GuildsOptedIn)
                {
                    if (guild == Convert.ToInt64(ctx.Guild.Id))
                    {
                        isOptedIn = "Yes";
                    }
                }
                common = String.IsNullOrEmpty(common) ? "None" : common;
                uncommon = String.IsNullOrEmpty(uncommon) ? "None" : uncommon;
                rare = String.IsNullOrEmpty(rare) ? "None" : rare;
                epic = String.IsNullOrEmpty(epic) ? "None" : epic;
                unique = String.IsNullOrEmpty(unique) ? "None" : unique;
                embed.AddField("Balance:", $"${user.Balance.ToString("###,###,###,###,###,##0.#0")}", false);
                embed.AddField("Health:", user.Health.ToString(), false);
                embed.AddField("Active Protection:", user.ProtectionName, false);
                embed.AddField("Protection Health:", user.Protection.ToString(), false);
                embed.AddField("Opted In:", $"{isOptedIn}", false);
                embed.AddField("Commons:", common, true);
                embed.AddField("Uncommons:", uncommon, true);
                embed.AddField("Rares:", rare, true);
                embed.AddField("Epics:", epic, true);
                embed.AddField("Uniques:", unique, true);
                embed.WithThumbnail(member.AvatarUrl);

                embed.WithFooter($"Item Count: {totalItems}\nInventory Value: ${inventoryValue.ToString("###,###,###,###,###,##0.#0")}");
                await ctx.Channel.SendMessageAsync(embed: embed);
            }
            else
            { 

            }
        }

        [Command("use"), Aliases("u", "utilize")]
        [Description("Allows the user to use an item.")]
        public async Task UseItem(CommandContext ctx, [RemainingText, Description("Item you want to use.")] string itemName)
        {
            var embed = new DiscordEmbedBuilder();
            var user = Bot.PlayerDatabase.GetPlayerByID(Convert.ToInt64(ctx.Member.Id));
            if (user == null)
            {
                embed = NewPlayer(ctx, embed, ctx.Member.Id);
                embed.Color = DiscordColor.Purple;
            }
            else if (user.IsBanned == true)
            {
                embed.Title = "You Are Banned";
                embed.Description += " You are currently banned form using the main feature of this bot.";
                embed.Color = DiscordColor.Red;
            }
            else if(ctx.Member.IsBot != true)
            {
                if (user.Name != ctx.Member.Username)
                {
                    user.Name = ctx.Member.Username;
                }
                var obtained = "";
                var interactivity = ctx.Client.GetInteractivity();
                if (String.IsNullOrEmpty(itemName))
                {
                    embed.Title = "Error";
                    embed.AddField("Invalid Name", "Please provide an item name to use.");
                    embed.Color = DiscordColor.Red;
                }
                else
                {
                    var amount = 1;
                    var split = itemName.ToLower().Split(" x");
                    if (split.Count() > 1)
                    {
                        if (Int32.Parse(split[1]) > 0)
                        {
                            amount = Int32.Parse(split[1]);
                        }
                        itemName = split[0];
                    }
                    if (amount > 100)
                    {
                        embed.Title = "Too many Items";
                        embed.Description = "You are attempting to use too many items. 100 items is the limit.";
                        embed.Color = DiscordColor.Red;
                    }
                    else
                    {
                        Item item = null;
                        foreach (var i in user.Inventory)
                        {
                            if (i.Name.ToLower() == itemName.ToLower())
                            {
                                item = i;
                            }
                            else if (i.Subname.ToLower() == itemName.ToLower())
                            {
                                item = i;
                            }
                        }
                        if (item == null)
                        {
                            embed.Title = "Error";
                            embed.AddField("Invalid Item", "No item with that name exists in your inventory.");
                            embed.Color = DiscordColor.Red;
                        }
                        else if (item.Multiple < amount)
                        {
                            embed.Title = "Not Enough Items";
                            embed.AddField("Error", "You do not have enough of that item in your inventory.");
                            embed.Color = DiscordColor.Red;
                        }
                        else
                        {
                            if (item is Crate crate)
                            {
                                decimal value = 0m;
                                for (var i = 0; i < amount; i++)
                                {
                                    var crateItem = Crate.OpenCrate(crate.Rarity);
                                    var isItemInInventory = false;
                                    foreach (var thing in user.Inventory)
                                    {
                                        if (thing.ID == crateItem.ID)
                                        {
                                            isItemInInventory = true;
                                            crateItem = thing;
                                            break;
                                        }
                                    }
                                    if (isItemInInventory == false)
                                    {
                                        user.Inventory.Add(crateItem);
                                        value += crateItem.Value;
                                    }
                                    else
                                    {
                                        crateItem.Multiple++;
                                        value += crateItem.Value;
                                    }
                                    embed.Title = $"You got:";
                                    obtained += $"[{crateItem.Rarity}] {crateItem.Name}\n";
                                    embed.Color = crate.Rarity switch
                                    {
                                        Rarity.Common => DiscordColor.Gray,
                                        Rarity.Uncommon => DiscordColor.DarkGreen,
                                        Rarity.Rare => DiscordColor.Blue,
                                        Rarity.Epic => DiscordColor.Gold,
                                        _ => DiscordColor.Orange
                                    };
                                    if (crate.Multiple > 1)
                                    {
                                        crate.Multiple--;
                                    }
                                    else
                                    {
                                        user.Inventory.Remove(crate);
                                    }
                                    Bot.PlayerDatabase.UpdatePlayer(user);
                                }
                                obtained += $"Total value: ${value.ToString("###,###,###,###,###,##0.#0")}";
                                embed.Description = $"{obtained}";
                            }
                            else if (item is Weapon weapon)
                            {
                                if (DateTime.Now - user.CooldownStartTime >= user.CooldownLength)
                                {
                                    var opedIn = 0;
                                    var isOptedIn = false;
                                    foreach (var player in Bot.PlayerDatabase.Players)
                                    {
                                        foreach (var guild in player.GuildsOptedIn)
                                        {
                                            if (guild == Convert.ToInt64(ctx.Guild.Id))
                                            {
                                                opedIn++;
                                            }
                                        }
                                    }
                                    foreach (var guild in user.GuildsOptedIn)
                                    {
                                        if (guild == Convert.ToInt64(ctx.Guild.Id))
                                        {
                                            isOptedIn = true;
                                        }
                                    }
                                    if (isOptedIn == true)
                                    {
                                        if (opedIn >= 4)
                                        {
                                            var hasAmmo = false;
                                            Item i = null;
                                            foreach (var i2 in user.Inventory)
                                            {
                                                if (i2 is Ammo ammo && ammo.Type == weapon.Ammo)
                                                {
                                                    hasAmmo = true;
                                                    i = i2;
                                                }
                                            }
                                            embed.Title = $"You used {weapon.Name}";
                                            if (weapon.WeaponType == WeaponType.Melee || (weapon.WeaponType == WeaponType.Firearm || weapon.WeaponType == WeaponType.HeavyFirearm) && hasAmmo == true)
                                            {
                                                user.CooldownStartTime = DateTime.Now;
                                                user.CooldownLength = weapon.Cooldown;
                                                user.CooldownEndTime = user.CooldownStartTime.Add(user.CooldownLength);
                                                Bot.PlayerDatabase.UpdatePlayer(user);
                                                if (weapon.Special == Special.SpreadShot)
                                                {
                                                    var allTargets = new List<Player>();
                                                    Player target = null;
                                                    DiscordMember targetAsMember = null;
                                                    for (var loop = 0; loop < 3; loop++)
                                                    {
                                                        var randomNum = new Random().Next(0, Bot.PlayerDatabase.Players.Count);
                                                        target = Bot.PlayerDatabase.Players[randomNum];
                                                        var OptedIn = false;
                                                        foreach (var guild in target.GuildsOptedIn)
                                                        {
                                                            if (guild == Convert.ToInt64(ctx.Guild.Id))
                                                            {
                                                                OptedIn = true;
                                                            }
                                                        }
                                                        var validTarget = !allTargets.Contains(target) && target.ID != user.ID && OptedIn == true;
                                                        while (!validTarget)
                                                        {
                                                            OptedIn = false;
                                                            randomNum = new Random().Next(0, Bot.PlayerDatabase.Players.Count);
                                                            target = Bot.PlayerDatabase.Players[randomNum];
                                                            foreach (var guild in target.GuildsOptedIn)
                                                            {
                                                                if (guild == Convert.ToInt64(ctx.Guild.Id))
                                                                {
                                                                    OptedIn = true;
                                                                }
                                                            }
                                                            validTarget = !allTargets.Contains(target) && target.ID != user.ID && OptedIn == true;
                                                        }
                                                        allTargets.Add(target);
                                                    }
                                                    foreach (var targets in allTargets)
                                                    {
                                                        targetAsMember = await ctx.Guild.GetMemberAsync(Convert.ToUInt64(targets.ID));
                                                        embed.Description += Attack(embed, ctx, user, targets, weapon, targetAsMember, i) + "\n";
                                                    }
                                                }
                                                else
                                                {
                                                    var targets = "";
                                                    var allTargets = new List<Player>();
                                                    Player target = null;
                                                    DiscordMember targetAsMember = null;
                                                    for (var loop = 0; loop < 3; loop++)
                                                    {
                                                        var randomNum = new Random().Next(0, Bot.PlayerDatabase.Players.Count);
                                                        target = Bot.PlayerDatabase.Players[randomNum];
                                                        var OptedIn = false;
                                                        foreach (var guild in target.GuildsOptedIn)
                                                        {
                                                            if (guild == Convert.ToInt64(ctx.Guild.Id))
                                                            {
                                                                OptedIn = true;
                                                            }
                                                        }
                                                        var validTarget = !allTargets.Contains(target) && target.ID != user.ID && OptedIn == true;
                                                        while (!validTarget)
                                                        {
                                                            OptedIn = false;
                                                            randomNum = new Random().Next(0, Bot.PlayerDatabase.Players.Count);
                                                            target = Bot.PlayerDatabase.Players[randomNum];
                                                            foreach (var guild in target.GuildsOptedIn)
                                                            {
                                                                if (guild == Convert.ToInt64(ctx.Guild.Id))
                                                                {
                                                                    OptedIn = true;
                                                                }
                                                            }
                                                            validTarget = !allTargets.Contains(target) && target.ID != user.ID && OptedIn == true;
                                                        }
                                                        allTargets.Add(target);
                                                        targetAsMember = await ctx.Guild.GetMemberAsync(Convert.ToUInt64(target.ID));
                                                        targets += $"{loop + 1}. {targetAsMember.Username}\n\n";
                                                    }
                                                    if (user.ActiveUtility != null && user.ActiveUtility.Special == Special.Scan)
                                                    {
                                                        embed.Title = "Target List:";
                                                        embed.Description = $"A random target will be chosen for you if you do not answer in 60 seconds.\nRespond with a number like a normal attack. The inventory closest to this message is 1 and continues to 3.";
                                                        await ctx.Channel.SendMessageAsync(embed: embed);
                                                        await Inventory(ctx, allTargets[0].ID);
                                                        await Inventory(ctx, allTargets[1].ID);
                                                        await Inventory(ctx, allTargets[2].ID);
                                                        var selected = false;
                                                        while (selected == false)
                                                        {
                                                            var response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(60));
                                                            if (response.TimedOut)
                                                            {
                                                                target = allTargets[new Random().Next(1, 4)];
                                                                targetAsMember = await ctx.Guild.GetMemberAsync(Convert.ToUInt64(target.ID));
                                                                embed.Title = $"You targetted {targetAsMember.Username}";
                                                                embed.Description = "";
                                                                selected = true;
                                                            }
                                                            else if (response.Result.Content == "1")
                                                            {
                                                                target = allTargets[0];
                                                                targetAsMember = await ctx.Guild.GetMemberAsync(Convert.ToUInt64(target.ID));
                                                                embed.Title = $"You targetted {targetAsMember.Username}";
                                                                embed.Description = "";
                                                                selected = true;
                                                            }
                                                            else if (response.Result.Content == "2")
                                                            {
                                                                target = allTargets[1];
                                                                targetAsMember = await ctx.Guild.GetMemberAsync(Convert.ToUInt64(target.ID));
                                                                embed.Title = $"You targetted {targetAsMember.Username}";
                                                                embed.Description = "";
                                                                selected = true;
                                                            }
                                                            else if (response.Result.Content == "3")
                                                            {
                                                                target = allTargets[2];
                                                                targetAsMember = await ctx.Guild.GetMemberAsync(Convert.ToUInt64(target.ID));
                                                                embed.Title = $"You targetted {targetAsMember.Username}";
                                                                embed.Description = "";
                                                                selected = true;
                                                            }
                                                            else
                                                            {
                                                                embed.Title = $"{response.Result.Content} is not a target";
                                                                embed.Description = "Choose a listed target by sending the number to the left of the name";
                                                                await ctx.Channel.SendMessageAsync(embed: embed);
                                                            }
                                                        }
                                                        targetAsMember = await ctx.Guild.GetMemberAsync(Convert.ToUInt64(target.ID));
                                                        embed.Description += Attack(embed, ctx, user, target, weapon, targetAsMember, i);
                                                    }
                                                    else
                                                    {
                                                        embed.Title = "Target List:";
                                                        embed.Description = $"{targets}\n\n A random target will be chosen for you if you do not answer in 15 seconds.";
                                                        await ctx.Channel.SendMessageAsync(embed: embed);
                                                        var selected = false;
                                                        while (selected == false)
                                                        {
                                                            var response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(15));
                                                            if (response.TimedOut)
                                                            {
                                                                target = allTargets[new Random().Next(1, 4)];
                                                                targetAsMember = await ctx.Guild.GetMemberAsync(Convert.ToUInt64(target.ID));
                                                                embed.Title = $"You targetted {targetAsMember.Username}";
                                                                embed.Description = "";
                                                                selected = true;
                                                            }
                                                            else if (response.Result.Content == "1")
                                                            {
                                                                target = allTargets[0];
                                                                targetAsMember = await ctx.Guild.GetMemberAsync(Convert.ToUInt64(target.ID));
                                                                embed.Title = $"You targetted {targetAsMember.Username}";
                                                                embed.Description = "";
                                                                selected = true;
                                                            }
                                                            else if (response.Result.Content == "2")
                                                            {
                                                                target = allTargets[1];
                                                                targetAsMember = await ctx.Guild.GetMemberAsync(Convert.ToUInt64(target.ID));
                                                                embed.Title = $"You targetted {targetAsMember.Username}";
                                                                embed.Description = "";
                                                                selected = true;
                                                            }
                                                            else if (response.Result.Content == "3")
                                                            {
                                                                target = allTargets[2];
                                                                targetAsMember = await ctx.Guild.GetMemberAsync(Convert.ToUInt64(target.ID));
                                                                embed.Title = $"You targetted {targetAsMember.Username}";
                                                                embed.Description = "";
                                                                selected = true;
                                                            }
                                                            else
                                                            {
                                                                embed.Title = $"{response.Result.Content} is not a target";
                                                                embed.Description = "Choose a listed target by sending the number to the left of the name";
                                                                await ctx.Channel.SendMessageAsync(embed: embed);
                                                            }
                                                        }
                                                        targetAsMember = await ctx.Guild.GetMemberAsync(Convert.ToUInt64(target.ID));
                                                        embed.Description += Attack(embed, ctx, user, target, weapon, targetAsMember, i);
                                                    }
                                                }
                                                if (user.ActiveUtility != null && user.ActiveUtility.Special == Special.Scan && user.RadarUse <= user.ActiveUtility.Uses)
                                                {
                                                    user.RadarUse++;
                                                    if (user.RadarUse >= user.ActiveUtility.Uses)
                                                    {
                                                        var embed2 = new DiscordEmbedBuilder();
                                                        embed2.Title = "Your radar has been exhausted";
                                                        embed2.Color = DiscordColor.Red;
                                                        await ctx.Channel.SendMessageAsync(embed: embed2);
                                                        user.RadarUse = 0;
                                                        user.ActiveUtility = null;
                                                    }
                                                }
                                                Bot.PlayerDatabase.UpdatePlayer(user);
                                                embed.Color = DiscordColor.DarkBlue;
                                            }
                                            else if (!hasAmmo)
                                            {
                                                foreach (var i2 in Bot.Items.AllItems)
                                                {
                                                    if (i2 is Ammo ammo && ammo.Type == weapon.Ammo)
                                                    {
                                                        i = i2;
                                                    }
                                                }
                                                embed.Title = "No ammunition";
                                                embed.Description = $"You do not have ammo for your weapon. {weapon.Name} requires {i.Name}.";
                                                embed.Color = DiscordColor.Red;
                                            }
                                        }
                                        else
                                        {
                                            embed.Title = "Not Enough Players";
                                            embed.Description = "There must be at least 4 players opted in to use weapons.";
                                            embed.Color = DiscordColor.Red;
                                        }
                                    }
                                    else
                                    {
                                        embed.Title = "Must Be Opted In";
                                        embed.Description = "You must be opted into this server to use weapons.";
                                        embed.Color = DiscordColor.Red;
                                    }
                                }
                                else
                                {
                                    embed.Title = "Cooldown Still Active";
                                    var cooldown = user.GetWeaponCooldown();
                                    embed.Description = $"{cooldown}";
                                    embed.Color = DiscordColor.Red;
                                    Bot.PlayerDatabase.UpdatePlayer(user);
                                }
                            }
                            else if (item is Healing healing)
                            {
                                if (user.Health == 100)
                                {
                                    embed.Title = "You already have max health";
                                    embed.Color = DiscordColor.Red;
                                }
                                else
                                {
                                    if (DateTime.Now - user.HealCooldownStart >= user.HealCooldownLength)
                                    {
                                        var healed = healing.MaxHealingRate;
                                        if (healing.LowestHealingRate != -1)
                                        {
                                            healed = new Random().Next(healing.LowestHealingRate, healing.MaxHealingRate);
                                        }
                                        user.Health = healed + user.Health > 100 ? 100 : user.Health + healed;
                                        if (healing.Multiple > 1)
                                        {
                                            healing.Multiple--;
                                        }
                                        else
                                        {
                                            user.Inventory.Remove(healing);
                                        }
                                        user.HealCooldownLength = healing.Cooldown;
                                        user.HealCooldownEnd = user.HealCooldownStart.Add(user.HealCooldownLength);
                                        Bot.PlayerDatabase.UpdatePlayer(user);
                                        embed.Title = $"You used {healing.Name}";
                                        embed.AddField($"{ctx.Member.Username}:", $"Health {user.Health}");
                                        embed.Color = DiscordColor.Green;
                                    }
                                    else
                                    {
                                        embed.Title = "Cooldown Still Active:";
                                        var cooldown = user.GetHealthCooldown();
                                        embed.Description = $"{cooldown}";
                                        embed.Color = DiscordColor.Red;
                                        Bot.PlayerDatabase.UpdatePlayer(user);
                                    }
                                }
                            }
                            else if (item is Armor armor)
                            {
                                if (user.Protection > 0)
                                {
                                    embed.Title = $"You already equipped {armor.Name}";
                                    embed.Description = $"Your armor will protect you for {user.Protection} damage";
                                    embed.Color = DiscordColor.Black;
                                }
                                else
                                {
                                    user.Protection = armor.ProtectionRate;
                                    user.ProtectionName = armor.Name;
                                    if (armor.Multiple > 1)
                                    {
                                        armor.Multiple--;
                                    }
                                    else
                                    {
                                        user.Inventory.Remove(armor);
                                    }
                                    Bot.PlayerDatabase.UpdatePlayer(user);
                                    embed.Title = $"You equipped {armor.Name}";
                                    embed.Description = $"Your armor will protect you for {armor.ProtectionRate} damage";
                                    embed.Color = DiscordColor.Black;
                                }
                            }
                            else if (item is Ammo ammo)
                            {
                                embed.Title = "Error";
                                embed.Description = ("Invalid Usable item");
                                embed.Color = DiscordColor.Red;
                            }
                            else if (item is Attachment attachment)
                            {
                                if (user.ActiveAttachment == null)
                                {
                                    user.ActiveAttachment = attachment;
                                    if (attachment.Ability == Addon.HighestDamageIncrease)
                                    {
                                        embed.Title = $"You readied your {attachment.Name}";
                                        embed.Description = ($"Your next weapon will do {attachment.Intensity} more damage.");
                                    }
                                    else if (attachment.Ability == Addon.ReduceCooldown)
                                    {
                                        embed.Title = $"You readied your {attachment.Name}";
                                        embed.Description = ($"Your next weapon will have {attachment.Intensity}% less cooldown.");
                                    }
                                    else if (attachment.Ability == Addon.IncreaseHitRate)
                                    {
                                        embed.Title = $"You readied your {attachment.Name}";
                                        embed.Description = ($"Your next weapon will do {attachment.Intensity} more hit.");
                                    }
                                    embed.Color = DiscordColor.Goldenrod;
                                    Bot.PlayerDatabase.UpdatePlayer(user);
                                }
                                else
                                {
                                    embed.Title = $"You Already Have an Active Attachment";
                                    embed.Description = ($"{user.ActiveAttachment.Name} is still readied");
                                    embed.Color = DiscordColor.Red;
                                }
                                if (attachment.Multiple > 1)
                                {
                                    attachment.Multiple--;
                                }
                                else
                                {
                                    user.Inventory.Remove(attachment);
                                }
                                Bot.PlayerDatabase.UpdatePlayer(user);
                            }
                            else if (item is Money money)
                            {
                                decimal total = 0m;
                                for (var loop = 0; loop < amount; loop++)
                                {
                                    embed.Title = amount > 1 ? $"You used {amount} {money.Name} and got:" : $"You used {money.Name} and got:";
                                    decimal moneyObtained = new Random().Next((int)money.LowestAmount * 100, (int)money.HighestAmount * 100 + 1);
                                    moneyObtained /= 100;
                                    total += moneyObtained;
                                    if (money.Multiple > 1)
                                    {
                                        money.Multiple--;
                                    }
                                    else
                                    {
                                        user.Inventory.Remove(money);
                                    }
                                    user.Balance += moneyObtained;
                                }
                                embed.Description = $"${total.ToString("###,###,###,###,###,##0.#0")}\n";
                                embed.Color = DiscordColor.Gold;
                                Bot.PlayerDatabase.UpdatePlayer(user);
                            }
                            else if (item is Utility utility)
                            {
                                if (utility.Special == Special.SupplyCall)
                                {
                                    embed.Title = amount > 1 ? $"Your {amount} {utility.Name}s Got" : $"Your {utility.Name}s Got";
                                    Item copy = null;
                                    var isInInventory = false;
                                    for (var loops = 0; loops < amount; loops++)
                                    {
                                        for (var loop = 0; loop < 5; loop++)
                                        {
                                            var c = Crate.GetRandomCrate();
                                            obtained += $"{c.Name}\n";
                                            foreach (var thing in user.Inventory)
                                            {
                                                if (thing.ID == c.ID)
                                                {
                                                    copy = thing;
                                                    isInInventory = true;
                                                    break;
                                                }
                                            }
                                            if (isInInventory == false)
                                            {
                                                user.Inventory.Add(c);
                                            }
                                            else
                                            {
                                                copy.Multiple++;
                                            }
                                            isInInventory = false;
                                            Bot.PlayerDatabase.UpdatePlayer(user);
                                        }
                                        if (utility.Multiple > 1)
                                        {
                                            utility.Multiple--;
                                        }
                                        else
                                        {
                                            user.Inventory.Remove(utility);
                                        }
                                        Bot.PlayerDatabase.UpdatePlayer(user);
                                    }
                                    embed.Description = $"{obtained}";
                                }
                                else if (utility.Special == Special.Scan)
                                {
                                    user.ActiveUtility = utility;
                                    embed.Title = "You equipped your radar";
                                    embed.Description = "Your next 3 attacks will reveal your target's inventory.";
                                    if (utility.Multiple > 1)
                                    {
                                        utility.Multiple--;
                                    }
                                    else
                                    {
                                        user.Inventory.Remove(utility);
                                    }
                                    Bot.PlayerDatabase.UpdatePlayer(user);
                                }
                                embed.Color = DiscordColor.Orange;
                            }
                            //MAKE EMBED CONFIRMING THE ITEM'S USE
                        }
                    }
                }
            }
            await ctx.Channel.SendMessageAsync(embed: embed);
        }

        [Command("sell"), Aliases("s", "se")]
        [Description("Sells sellable items for money.")]
        public async Task SellItem(CommandContext ctx, [RemainingText, Description("Item you want to sell.")] string itemName)
        {
            var embed = new DiscordEmbedBuilder();
            var user = Bot.PlayerDatabase.GetPlayerByID(Convert.ToInt64(ctx.Member.Id));
            if (user == null)
            {
                embed = NewPlayer(ctx, embed, ctx.Member.Id);
                embed.Color = DiscordColor.Purple;
            }
            else if (user.IsBanned == true)
            {
                embed.Title = "You Are Banned";
                embed.Description += " You are currently banned form using the main feature of this bot.";
                embed.Color = DiscordColor.Red;
            }
            else
            {
                if (user.Name != ctx.Member.Username)
                {
                    user.Name = ctx.Member.Username;
                }
                if (String.IsNullOrEmpty(itemName))
                {
                    embed.Title = "Error";
                    embed.AddField("Invalid Name", "Please provide an item name to use.");
                    embed.Color = DiscordColor.Red;
                }
                else
                {
                    var amount = 1;
                    var split = itemName.ToLower().Split(" x");
                    if (split.Count() > 1)
                    {
                        if (Int32.Parse(split[1]) > 0)
                        {
                            amount = Int32.Parse(split[1]);
                        }
                        itemName = split[0];
                    }
                    var gained = 0m;
                    Item item = null;
                    foreach (var i in user.Inventory)
                    {
                        if (i.Name.ToLower() == itemName.ToLower())
                        {
                            item = i;
                        }
                        else if (i.Subname.ToLower() == itemName.ToLower())
                        {
                            item = i;
                        }
                    }
                    if (item == null)
                    {
                        embed.Title = "Error";
                        embed.AddField("Invalid Item", "No item with that name exists in your inventory.");
                        embed.Color = DiscordColor.Red;
                    }
                    else if (item.IsSellable == true)
                    {
                        if (item.Multiple < amount)
                        {
                            embed.Title = "Not Enough Items";
                            embed.AddField("Error", "You do not have enough of that item in your inventory.");
                            embed.Color = DiscordColor.Red;
                        }
                        else
                        {
                            user.Balance += item.Value * amount;
                            gained += item.Value * amount;
                            if (item.Multiple >= 1 && item.Multiple - amount > 0)
                            {
                                item.Multiple -= amount;
                            }
                            else if (item.Multiple - amount == 0)
                            {
                                user.Inventory.Remove(item);
                            }
                            Bot.PlayerDatabase.UpdatePlayer(user);
                            embed.Title = "Item Sold!";
                            embed.AddField("Item", $"{amount} {item.Name}s sold for ${gained.ToString("###,###,###,###,###,##0.#0")}");
                            embed.Color = DiscordColor.Gold;
                        }
                    }
                    else
                    {
                        embed.Title = "Item is not sellable";
                        embed.Description = $"{item.Name} is not sellable";
                        embed.Color = DiscordColor.Red;
                    }
                }
            }
            await ctx.Channel.SendMessageAsync(embed: embed);
        }

        [Command("balance"), Aliases("bal")]
        [Description("Displays a user's balance")]
        public async Task Balance(CommandContext ctx)
        {
            var user = Bot.PlayerDatabase.GetPlayerByID(Convert.ToInt64(ctx.Member.Id));
            var embed = new DiscordEmbedBuilder();
            if (user == null)
            {
                embed = NewPlayer(ctx, embed, ctx.Member.Id);
                embed.Color = DiscordColor.Purple;
            }
            else if (user.IsBanned == true)
            {
                embed.Title = "You Are Banned";
                embed.Description += " You are currently banned form using the main feature of this bot.";
                embed.Color = DiscordColor.Red;
            }
            else
            {
                if (user.Name != ctx.Member.Username)
                {
                    user.Name = ctx.Member.Username;
                }
                embed.Title = $"{ctx.Member.Username}'s Balance";
                embed.Description = $"${user.Balance.ToString("###,###,###,###,###,##0.#0")}";
            }
            await ctx.Channel.SendMessageAsync(embed: embed);
        }

        [Command("info"), Aliases("inf")]
        [Description("Displays information about an item.")]
        public async Task Info(CommandContext ctx, [RemainingText, Description("Item you want to find info on.")] string itemName)
        {
            var embed = new DiscordEmbedBuilder();
            if (String.IsNullOrEmpty(itemName))
            {
                embed.Title = "Error";
                embed.AddField("Invalid Name", "Please provide a valid item name.");
                embed.Color = DiscordColor.Red;
            }
            else
            {
                var info = "";
                Item item = null;
                foreach (var i in Bot.Items.AllItems)
                {
                    if (i.Name.ToLower() == itemName.ToLower())
                    {
                        item = i;
                    }
                    else if (i.Subname.ToLower() == itemName.ToLower())
                    {
                        item = i;
                    }
                }
                if (item == null)
                {
                    embed.Title = "Invalid Item";
                    embed.AddField("Invalid Name", $"{itemName} is not an item.");
                    embed.Color = DiscordColor.Red;
                }
                else if (item is Crate crate)
                {
                    embed.Title = $"{crate.Name}";
                    embed.Description = $"Tags:\n{crate.Name}\n{crate.Subname}";
                    if (crate.Rarity != Rarity.Unique)
                    {
                        embed.AddField("Info", $"Gives you a {crate.Rarity.ToString().ToLower()}-epic item.");
                    }
                    else
                    {
                        embed.AddField("Info", $"Gives you a {crate.Rarity.ToString().ToLower()} item.");
                    }
                    embed.AddField("Value", $"${crate.Value.ToString("###,###,###,###,###,##0.#0")}");
                }
                else if (item is Weapon weapon)
                {
                    Item i = null;
                    foreach (var i2 in Bot.Items.AllItems)
                    {
                        if (i2 is Ammo ammo && ammo.Type == weapon.Ammo)
                        {
                            i = i2;
                        }
                    }
                    embed.Title = $"[{weapon.Rarity}] {weapon.Name}";
                    if (weapon.Subname != "None")
                    {
                        embed.Description = $"Tags:\n{weapon.Name}\n{weapon.Subname}";
                    }
                    else
                    {
                        embed.Description = $"Tags:\n{weapon.Name}";
                    }
                    if (weapon.WeaponType == WeaponType.Firearm || weapon.WeaponType == WeaponType.HeavyFirearm)
                    {
                        embed.AddField("Ammunition:", $"[{i.Rarity}] {i.Name}", true);
                        if (weapon.LowestDamage == -1)
                        {
                            embed.AddField("Damage:", $"{weapon.HighestDamage}", true);
                        }
                        else
                        {
                            embed.AddField("Damage:", $"{weapon.LowestDamage}-{weapon.HighestDamage}", true);
                        }
                        embed.AddField("Hit Rate:", $"{weapon.FireRate}", true);
                        embed.AddField("Value:", $"${weapon.Value.ToString("###,###,###,###,###,##0.#0")}", true);
                        embed.AddField("Cooldown:", $"{weapon.Cooldown.ToString(@"hh\:mm\:ss")}");
                        if (weapon.Special == Special.ArmorBust)
                        {
                            embed.AddField("Special:", "Destroys your target's armor. If your target has no armor active, your weapon will damage your target for 50% of their health.");
                        }
                        else if (weapon.Special == Special.SpreadShot)
                        {
                            embed.AddField("Special:", "Hits 3 targets at the same time.");
                        }
                    }
                    else
                    {
                        if (weapon.LowestDamage == -1)
                        {
                            embed.AddField("Damage:", $"{weapon.HighestDamage}", true);
                        }
                        else
                        {
                            embed.AddField("Damage:", $"{weapon.LowestDamage}-{weapon.HighestDamage}", true);
                        }
                        embed.AddField("Value:", $"${weapon.Value.ToString("###,###,###,###,###,##0.#0")}", true);
                        embed.AddField("Cooldown:", $"{weapon.Cooldown.ToString(@"hh\:mm\:ss")}");
                    }
                }
                else if (item is Ammo ammo)
                {
                    embed.Title = $"[{ammo.Rarity}] {ammo.Name}";
                    if (ammo.Subname != "None")
                    {
                        embed.Description = $"Tags:\n{ammo.Name}\n{ammo.Subname}";
                    }
                    else
                    {
                        embed.Description = $"Tags:\n{ammo.Name}";
                    }
                    foreach (var i2 in Bot.Items.AllItems)
                    {
                        if (i2 is Weapon weapon2 && weapon2.Ammo == ammo.Type)
                        {
                            info += $"[{i2.Rarity}] {i2.Name}\n";
                        }
                    }
                    embed.AddField("Used By:", $"{info}", true);
                    embed.AddField("Value:", $"${ammo.Value.ToString("###,###,###,###,###,##0.#0")}", true);
                }
                else if (item is Armor armor)
                {
                    embed.Title = $"[{armor.Rarity}] {armor.Name}";
                    if (armor.Subname != "None")
                    {
                        embed.Description = $"Tags:\n{armor.Name}\n{armor.Subname}";
                    }
                    else
                    {
                        embed.Description = $"Tags:\n{armor.Name}";
                    }
                    embed.AddField("Info:", $"Protects the user from {armor.ProtectionRate} damage", true);
                    embed.AddField("Value:", $"${armor.Value.ToString("###,###,###,###,###,##0.#0")}", true);
                }
                else if (item is Attachment attachment)
                {
                    embed.Title = $"[{attachment.Rarity}] {attachment.Name}";
                    if (attachment.Subname != "None")
                    {
                        embed.Description = $"Tags:\n{attachment.Name}\n{attachment.Subname}";
                    }
                    else
                    {
                        embed.Description = $"Tags:\n{attachment.Name}";
                    }
                    if (attachment.Ability == Addon.HighestDamageIncrease)
                    {
                        embed.AddField("Boost:", $"Increases the user's attack damage by {attachment.Intensity}.\nOnly works on guns.");
                    }
                    else if (attachment.Ability == Addon.IncreaseHitRate)
                    {
                        embed.AddField("Boost:", $"Adds {attachment.Intensity} more strike to the user's attack.\nOnly works on guns.");
                    }
                    else if (attachment.Ability == Addon.ReduceCooldown)
                    {
                        embed.AddField("Boost:", $"Reduces the user's attack cooldown by {attachment.Intensity}%\nOnly works on guns.");
                    }
                    embed.AddField("Value:", $"${attachment.Value.ToString("###,###,###,###,###,##0.#0")}", true);
                }
                else if (item is Healing healing)
                {
                    embed.Title = $"[{healing.Rarity}] {healing.Name}";
                    if (healing.Subname != "None")
                    {
                        embed.Description = $"Tags:\n{healing.Name}\n{healing.Subname}";
                    }
                    else
                    {
                        embed.Description = $"Tags:\n{healing.Name}";
                    }
                    if (healing.LowestHealingRate == -1)
                    {
                        embed.AddField("Heals:", $"{healing.MaxHealingRate}", true);
                    }
                    else
                    {
                        embed.AddField("Heals:", $"{healing.MaxHealingRate}-{healing.LowestHealingRate}", true);
                    }
                    embed.AddField("Value:", $"${healing.Value.ToString("###,###,###,###,###,##0.#0")}", true);
                    embed.AddField("Cooldown:", $"{healing.Cooldown.ToString(@"hh\:mm\:ss")}");
                }
                else if (item is Money money)
                {
                    embed.Title = $"[{money.Rarity}] {money.Name}";
                    if (money.Subname != "None")
                    {
                        embed.Description = $"Tags:\n{money.Name}\n{money.Subname}";
                    }
                    else
                    {
                        embed.Description = $"Tags:\n{money.Name}";
                    }
                    embed.AddField("Grants:", $"${money.HighestAmount.ToString("###,###,###,###,###,##0.#0")}-${money.LowestAmount.ToString("###,###,###,###,###,##0.#0")}", true);
                    embed.AddField("Value:", $"${money.Value.ToString("###,###,###,###,###,##0.#0")}", true);
                }
                else if (item is Utility utility)
                {
                    embed.Title = $"[{utility.Rarity}] {utility.Name}";
                    if (utility.Subname != "None")
                    {
                        embed.Description = $"Tags:\n{utility.Name}\n{utility.Subname}";
                    }
                    else
                    {
                        embed.Description = $"Tags:\n{utility.Name}";
                    }
                    if (utility.Special == Special.Scan)
                    {
                        embed.AddField("Special:", "Reveals the inventories of the user's target. Good for 3 uses.");
                    }
                    else if (utility.Special == Special.SupplyCall)
                    {
                        embed.AddField("Special:", "Calls in 5 random crates of common-epic rarity.");
                    }
                    embed.AddField("Value:", $"${utility.Value.ToString("###,###,###,###,###,##0.#0")}", true);
                }
                if (item != null)
                {
                    embed.Color = item.Rarity switch
                    {
                        Rarity.Common => DiscordColor.Gray,
                        Rarity.Uncommon => DiscordColor.DarkGreen,
                        Rarity.Rare => DiscordColor.Blue,
                        Rarity.Epic => DiscordColor.Gold,
                        _ => DiscordColor.Orange
                    };
                }
            }
            await ctx.Channel.SendMessageAsync(embed: embed);
        }

        [Command("items"), Aliases("its", "all")]
        [Description("Displays all items.")]
        public async Task Items(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder();
            embed.Title = "Item List:";
            string common = "", uncommon = "", rare = "", epic = "", unique = "";
            foreach (var item in Bot.Items.AllItems.OrderBy(y => y.Name).OrderBy(x => x.Rarity))
            {
                if (item.Rarity == Rarity.Common)
                {
                    if (item.IsPurchaseable == false && item.IsSellable == false)
                    {
                        common += $"{item.Name}\n";
                    }
                    else if (item.IsPurchaseable == true && item.IsSellable == false)
                    {
                        common += $"{item.Name} *b*\n";
                    }
                    else if (item.IsPurchaseable == true && item.IsSellable == true)
                    {
                        common += $"{item.Name} *b* **s**\n";
                    }
                    else if (item.IsPurchaseable == false && item.IsSellable == true)
                    {
                        common += $"{item.Name} **s**\n";
                    }
                }
                else if (item.Rarity == Rarity.Uncommon)
                {
                    if (item.IsPurchaseable == false && item.IsSellable == false)
                    {
                        uncommon += $"{item.Name}\n";
                    }
                    else if (item.IsPurchaseable == true && item.IsSellable == false)
                    {
                        uncommon += $"{item.Name} *b*\n";
                    }
                    else if (item.IsPurchaseable == true && item.IsSellable == true)
                    {
                        uncommon += $"{item.Name} *b* **s**\n";
                    }
                    else if (item.IsPurchaseable == false && item.IsSellable == true)
                    {
                        uncommon += $"{item.Name} **s**\n";
                    }
                }
                else if (item.Rarity == Rarity.Rare)
                {
                    if (item.IsPurchaseable == false && item.IsSellable == false)
                    {
                        rare += $"{item.Name}\n";
                    }
                    else if (item.IsPurchaseable == true && item.IsSellable == false)
                    {
                        rare += $"{item.Name} *b*\n";
                    }
                    else if (item.IsPurchaseable == true && item.IsSellable == true)
                    {
                        rare += $"{item.Name} *b* **s**\n";
                    }
                    else if (item.IsPurchaseable == false && item.IsSellable == true)
                    {
                        rare += $"{item.Name} **s**\n";
                    }
                }
                else if (item.Rarity == Rarity.Epic)
                {
                    if (item.IsPurchaseable == false && item.IsSellable == false)
                    {
                        epic += $"{item.Name}\n";
                    }
                    else if (item.IsPurchaseable == true && item.IsSellable == false)
                    {
                        epic += $"{item.Name} *b*\n";
                    }
                    else if (item.IsPurchaseable == true && item.IsSellable == true)
                    {
                        epic += $"{item.Name} *b* **s**\n";
                    }
                    else if (item.IsPurchaseable == false && item.IsSellable == true)
                    {
                        epic += $"{item.Name} **s**\n";
                    }
                }
                else if (item.Rarity == Rarity.Unique)
                {
                    if (item.IsPurchaseable == false && item.IsSellable == false)
                    {
                        unique += $"{item.Name}\n";
                    }
                    else if (item.IsPurchaseable == true && item.IsSellable == false)
                    {
                        unique += $"{item.Name} *b*\n";
                    }
                    else if (item.IsPurchaseable == true && item.IsSellable == true)
                    {
                        unique += $"{item.Name} *b* **s**\n";
                    }
                    else if (item.IsPurchaseable == false && item.IsSellable == true)
                    {
                        unique += $"{item.Name} **s**\n";
                    }
                }
            }
            embed.AddField("Commons:", common, true);
            embed.AddField("Uncommons:", uncommon, true);
            embed.AddField("Rares:", rare, true);
            embed.AddField("Epics:", epic, true);
            embed.AddField("Uniques:", unique, true);
            embed.AddField("Key:", "*b* = buyable\n**s** = sellable");
            await ctx.Channel.SendMessageAsync(embed: embed);
        }

        [Command("buy"), Aliases("purchase", "b")]
        [Description("Buy buyable items.")]
        public async Task Buy(CommandContext ctx, [RemainingText] string itemName)
        {
            var embed = new DiscordEmbedBuilder();
            var user = Bot.PlayerDatabase.GetPlayerByID(Convert.ToInt64(ctx.Member.Id));
            if (user == null)
            {
                embed = NewPlayer(ctx, embed, ctx.Member.Id);
                embed.Color = DiscordColor.Purple;
            }
            else if (user.IsBanned == true)
            {
                embed.Title = "You Are Banned";
                embed.Description += " You are currently banned form using the main feature of this bot.";
                embed.Color = DiscordColor.Red;
            }
            else
            {
                if (user.Name != ctx.Member.Username)
                {
                    user.Name = ctx.Member.Username;
                }
                if (String.IsNullOrEmpty(itemName))
                {
                    embed.Title = "Error";
                    embed.AddField("Invalid Name", "Please provide an item name to use.");
                    embed.Color = DiscordColor.Red;
                }
                else
                {
                    var amount = 1;
                    var split = itemName.ToLower().Split(" x");
                    if (split.Count() > 1)
                    {
                        if (Int32.Parse(split[1]) > 0)
                        {
                            amount = Int32.Parse(split[1]);
                        }
                        itemName = split[0];
                    }
                    Item item = null;
                    foreach (var i in Bot.Items.AllItems)
                    {
                        if (i.Name.ToLower() == itemName.ToLower())
                        {
                            item = i;
                        }
                        else if (i.Subname.ToLower() == itemName.ToLower())
                        {
                            item = i;
                        }
                    }
                    if (item != null)
                    {
                        if (item.IsPurchaseable == true)
                        {
                            if (user.Balance > item.Value * amount)
                            {
                                if (item is Crate)
                                {
                                    var isInInventory = false;
                                    Item copy = null;
                                    foreach (var thing in user.Inventory)
                                    {
                                        if (thing.ID == item.ID)
                                        {
                                            copy = thing;
                                            isInInventory = true;
                                            break;
                                        }
                                    }
                                    if (isInInventory == false)
                                    {
                                        item = Bot.Items.GetItem(item);
                                        user.Inventory.Add(item);
                                        item.Multiple += amount-1;
                                    }
                                    else
                                    {
                                        copy.Multiple += amount;
                                    }
                                    user.Balance -= item.Value * amount;
                                    Bot.PlayerDatabase.UpdatePlayer(user);
                                    embed.Title = $"You bought {amount} {item.Name}s";
                                    embed.Description = $"You bought {amount} {item.Name} for ${(item.Value * amount).ToString("###,###,###,###,###,##0.#0")}";
                                    embed.Color = DiscordColor.Green;
                                }
                                else
                                {
                                    var isInInventory = false;
                                    Item copy = null;
                                    foreach (var thing in user.Inventory)
                                    {
                                        if (thing.ID == item.ID)
                                        {
                                            copy = thing;
                                            isInInventory = true;
                                            break;
                                        }
                                    }
                                    if (isInInventory == false)
                                    {
                                        item = Bot.Items.GetItem(item);
                                        user.Inventory.Add(item);
                                        item.Multiple += amount-1;
                                    }
                                    else
                                    {
                                        copy.Multiple += amount;
                                    }
                                    user.Balance -= item.Value * 1.25m * amount;
                                    Bot.PlayerDatabase.UpdatePlayer(user);
                                    embed.Title = $"You bought {amount} {item.Name}s";
                                    embed.Description = $"You bought {amount} {item.Name} for ${((item.Value * 1.25m) * amount).ToString("###,###,###,###,###,##0.#0")}";
                                    embed.Color = DiscordColor.Green;
                                }
                            }
                            else
                            {
                                embed.Title = "You don't have enough money";
                                if (item is Crate)
                                {
                                    embed.Description = $"You are ${((item.Value * amount) - user.Balance).ToString("###,###,###,###,###,##0.#0")} short.";
                                }
                                else
                                {
                                    embed.Description = $"You are ${((item.Value * 1.25m * amount) - user.Balance).ToString("###,###,###,###,###,##0.#0")} short.";
                                }
                                embed.Color = DiscordColor.Red;
                            }
                        }
                        else
                        {
                            embed.Title = "Item not purchasable";
                            embed.Description = $"{item.Name} is not purchasable";
                            embed.Color = DiscordColor.Red;
                        }
                    }
                    else
                    {
                        embed.Title = "Item does not exist";
                        embed.Description = $"{itemName} is not a valid item.";
                        embed.Color = DiscordColor.Red;
                    }
                }
            }
            await ctx.Channel.SendMessageAsync(embed: embed);
        }

        [Command("hourly"), Aliases("h", "hour")]
        [Description("Collect hourly crate.")]
        public async Task Hour(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder();
            var user = Bot.PlayerDatabase.GetPlayerByID(Convert.ToInt64(ctx.Member.Id));
            if (user == null)
            {
                embed = NewPlayer(ctx, embed, ctx.Member.Id);
                embed.Description += " Use g/hour again or g/h to claim your crate.";
                embed.Color = DiscordColor.Purple;
            }
            else if (user.IsBanned == true)
            {
                embed.Title = "You Are Banned";
                embed.Description += " You are currently banned form using the main feature of this bot.";
                embed.Color = DiscordColor.Red;
            }
            else
            {
                if (user.Name != ctx.Member.Username)
                {
                    user.Name = ctx.Member.Username;
                }
                if (DateTime.Now - user.CooldownHourStartTime >= user.HourCooldown)
                {
                    var item = Crate.GetRandomCrate();
                    var isInInventory = false;
                    Item copy = null;
                    foreach (var thing in user.Inventory)
                    {
                        if (thing.ID == item.ID)
                        {
                            copy = thing;
                            isInInventory = true;
                            break;
                        }
                    }
                    if (isInInventory == false)
                    {
                        user.Inventory.Add(item);
                    }
                    else
                    {
                        copy.Multiple++;
                    }
                    user.HourCooldown = TimeSpan.FromHours(1);
                    user.CooldownHourStartTime = DateTime.Now;
                    user.CooldownHourEndTime = user.CooldownHourStartTime.Add(user.HourCooldown);
                    Bot.PlayerDatabase.UpdatePlayer(user);
                    embed.Title = $"You got {item.Name}";
                    if (item.Rarity == Rarity.Common)
                    {
                        embed.Color = DiscordColor.Gray;
                    }
                    else if (item.Rarity == Rarity.Uncommon)
                    {
                        embed.Color = DiscordColor.DarkGreen;
                    }
                    else if (item.Rarity == Rarity.Rare)
                    {
                        embed.Color = DiscordColor.Blue;
                    }
                    else if (item.Rarity == Rarity.Epic)
                    {
                        embed.Color = DiscordColor.Gold;
                    }
                    embed.Description = "You can do this again in the next hour";
                }
                else
                {
                    embed.Title = "Cooldown Still Active";
                    var cooldown = user.GetHourCooldown();
                    embed.Description = $"{cooldown}";
                    embed.Color = DiscordColor.Red;
                    Bot.PlayerDatabase.UpdatePlayer(user);
                }
            }
            await ctx.Channel.SendMessageAsync(embed: embed);
        }

        [Command("daily"), Aliases("d", "day")]
        [Description("Collect daily crate.")]
        public async Task Daily(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder();
            var user = Bot.PlayerDatabase.GetPlayerByID(Convert.ToInt64(ctx.Member.Id));
            if (user == null)
            {
                embed = NewPlayer(ctx, embed, ctx.Member.Id);
                embed.Description += " Use g/daily again or g/d or g/day to claim your crate.";
                embed.Color = DiscordColor.Purple;
            }
            else if (user.IsBanned == true)
            {
                embed.Title = "You Are Banned";
                embed.Description += " You are currently banned form using the main feature of this bot.";
                embed.Color = DiscordColor.Red;
            }
            else
            {
                if (user.Name != ctx.Member.Username)
                {
                    user.Name = ctx.Member.Username;
                }
                if (DateTime.Now - user.DailyCooldownEnd >= user.DailyCooldown)
                {
                    var item = Crate.GetRandomCrate();
                    var isInInventory = false;
                    Item copy = null;
                    foreach (var thing in user.Inventory)
                    {
                        if (thing.ID == item.ID)
                        {
                            copy = thing;
                            isInInventory = true;
                            break;
                        }
                    }
                    if (isInInventory == false)
                    {
                        user.Inventory.Add(item);
                    }
                    else
                    {
                        copy.Multiple++;
                    }
                    user.DailyCooldown = TimeSpan.FromHours(24);
                    user.DailyCooldownStart = DateTime.Now;
                    user.DailyCooldownEnd = user.DailyCooldownStart.Add(user.DailyCooldown);
                    Bot.PlayerDatabase.UpdatePlayer(user);
                    embed.Title = $"You got {item.Name}";
                    if (item.Rarity == Rarity.Common)
                    {
                        embed.Color = DiscordColor.Gray;
                    }
                    else if (item.Rarity == Rarity.Uncommon)
                    {
                        embed.Color = DiscordColor.DarkGreen;
                    }
                    else if (item.Rarity == Rarity.Rare)
                    {
                        embed.Color = DiscordColor.Blue;
                    }
                    else if (item.Rarity == Rarity.Epic)
                    {
                        embed.Color = DiscordColor.Gold;
                    }
                    embed.Description = "You can do this again in the next hour";
                }
                else
                {
                    embed.Title = "Cooldown Still Active";
                    var cooldown = user.GetDailyCooldown();
                    embed.Description = $"{cooldown}";
                    embed.Color = DiscordColor.Red;
                    Bot.PlayerDatabase.UpdatePlayer(user);
                }
            }
            await ctx.Channel.SendMessageAsync(embed: embed);
        }

        [Command("optin"), Aliases("oi", "opt")]
        [Description("Opt into the server.")]
        public async Task OptIn(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder();
            var user = Bot.PlayerDatabase.GetPlayerByID(Convert.ToInt64(ctx.Member.Id));
            var isOptedIn = false;
            if (user == null)
            {
                embed = NewPlayer(ctx, embed, ctx.Member.Id);
                embed.Color = DiscordColor.Purple;
                await ctx.Channel.SendMessageAsync(embed: embed);
            }
            else if (user.IsBanned == true)
            {
                embed.Title = "You Are Banned";
                embed.Description += " You are currently banned form using the main feature of this bot.";
                embed.Color = DiscordColor.Red;
            }
            else
            {
                if (user.Name != ctx.Member.Username)
                {
                    user.Name = ctx.Member.Username;
                }
                foreach (var guild in user.GuildsOptedIn)
                {
                    if (guild == Convert.ToInt64(ctx.Guild.Id))
                    {
                        isOptedIn = true;
                    }
                }
            }
            if (isOptedIn == false && user.IsBanned == false)
            {
                var interactivity = ctx.Client.GetInteractivity();
                embed.Title = "Confirmation";
                embed.Description = "Are you sure you want to opt into the server? You will be able to attack and will be attacked until you opt out. Reply yes or no in the next 10 seconds.";
                embed.Color = DiscordColor.White;
                await ctx.Channel.SendMessageAsync(embed: embed);
                var response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(10));
                if (response.TimedOut)
                {
                    embed.Title = $"Not Opted In";
                    embed.Description = "You chose to stay opted out.";
                    embed.Color = DiscordColor.White;
                    goto done;
                }
                else if (response.Result.Content.ToLower() != "yes" && response.Result.Content.ToLower() != "no")
                {
                    var select = false;
                    while (select == false)
                    {
                        embed.Title = $"Choose a Valid Response";
                        embed.Description = "Reply yes or no";
                        embed.Color = DiscordColor.Red;
                        await ctx.Channel.SendMessageAsync(embed: embed);
                        response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel, TimeSpan.FromSeconds(10));
                        if (response.TimedOut)
                        {
                            embed.Title = $"Not Opted In";
                            embed.Description = "You chose to stay opted out.";
                            embed.Color = DiscordColor.White;
                        }
                        else if ((response.Result.Content.ToLower() == "yes" || response.Result.Content.ToLower() == "no"))
                        {
                            select = true;
                        }
                    }
                }
                if (response.Result.Content != null && response.Result.Content.ToLower() == "yes")
                {
                    embed.Title = $"You Opted In";
                    embed.Description = "You are now opted into the server. You may opt out in 24 hours.";
                    embed.Color = DiscordColor.Orange;
                    user.GuildsOptedIn.Add(Convert.ToInt64(ctx.Guild.Id));
                    user.OptCooldownStart = DateTime.Now;
                    user.OptCooldown = TimeSpan.FromHours(24);
                    user.OptCooldownEnd = user.OptCooldownStart.Add(user.OptCooldown);
                }
                else if (response.Result.Content != null && response.Result.Content.ToLower() == "no")
                {
                    embed.Title = $"Not Opted In";
                    embed.Description = "You chose to stay opted out.";
                    embed.Color = DiscordColor.White;
                }
            done:
                Bot.PlayerDatabase.UpdatePlayer(user);
            }
            else if (user.IsBanned == true)
            {
                embed.Title = "You Are Banned";
                embed.Description += " You are currently banned form using the main feature of this bot.";
                embed.Color = DiscordColor.Red;
            }
            else
            {
                embed.Title = "You Are Opted In";
                embed.Description = "You are already opted in.";
                embed.Color = DiscordColor.Red;
            }
            await ctx.Channel.SendMessageAsync(embed: embed);
        }

        [Command("optout"), Aliases("ou")]
        [Description("Opt out of the server.")]
        public async Task OptOut(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder();
            var user = Bot.PlayerDatabase.GetPlayerByID(Convert.ToInt64(ctx.Member.Id));
            var isOptedIn = false;
            if (user == null)
            {
                embed = NewPlayer(ctx, embed, ctx.Member.Id);
                embed.Color = DiscordColor.Purple;
                await ctx.Channel.SendMessageAsync(embed: embed);
            }
            else
            {
                if (user.Name != ctx.Member.Username)
                {
                    user.Name = ctx.Member.Username;
                }
                foreach (var guild in user.GuildsOptedIn)
                {
                    if (guild == Convert.ToInt64(ctx.Guild.Id))
                    {
                        isOptedIn = true;
                    }
                }
            }
            if (isOptedIn == true)
            {
                if (DateTime.Now - user.OptCooldownStart >= user.OptCooldown)
                {
                    var interactivity = ctx.Client.GetInteractivity();
                    embed.Title = "Confirmation";
                    embed.Description = "Are you sure you want to opt out of the server? You will not be able to attack and will be safe from all attacks. Reply yes or no in the next 10 seconds.";
                    embed.Color = DiscordColor.White;
                    await ctx.Channel.SendMessageAsync(embed: embed);
                    var response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel, TimeSpan.FromSeconds(10));
                    if (response.TimedOut)
                    {
                        embed.Title = $"Not Opted Out";
                        embed.Description = "You chose to stay opted in.";
                        embed.Color = DiscordColor.White;
                        goto done;
                    }
                    else if (response.Result.Content.ToLower() != "yes" && response.Result.Content.ToLower() != "no")
                    {
                        var select = false;
                        while (select == false)
                        {
                            embed.Title = $"Choose a Valid Response";
                            embed.Description = "Reply yes or no";
                            embed.Color = DiscordColor.Red;
                            await ctx.Channel.SendMessageAsync(embed: embed);
                            response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(10));
                            if (response.TimedOut)
                            {
                                embed.Title = $"Not Opted In";
                                embed.Description = "You chose to stay opted out.";
                                embed.Color = DiscordColor.White;
                            }
                            else if (response.Result.Content.ToLower() == "yes" || response.Result.Content.ToLower() == "no")
                            {
                                select = true;
                            }
                        }
                    }
                    if (response.Result.Content != null && response.Result.Content.ToLower() == "yes")
                    {
                        embed.Title = $"You Opted Out";
                        embed.Description = "You are now opted out of server.";
                        embed.Color = DiscordColor.Green;
                        user.GuildsOptedIn.Remove(Convert.ToInt64(ctx.Guild.Id));
                    }
                    else if (response.Result.Content != null && response.Result.Content.ToLower() == "no")
                    {
                        embed.Title = $"Not Opted Out";
                        embed.Description = "You chose to stay opted in.";
                        embed.Color = DiscordColor.White;
                    }
                done:
                    Bot.PlayerDatabase.UpdatePlayer(user);
                }
                else
                {
                    embed.Title = "Opt Cooldown Still Active";
                    var cooldown = user.GetOptCooldown();
                    embed.Description = $"{cooldown}";
                    embed.Color = DiscordColor.Red;
                    Bot.PlayerDatabase.UpdatePlayer(user);
                }
            }
            else
            {
                embed.Title = "You Aren't Opted In";
                embed.Description = "You aren't opted into the server.";
                embed.Color = DiscordColor.Red;
            }
            await ctx.Channel.SendMessageAsync(embed: embed);
        }

        [Command("weekly"), Aliases("w", "week")]
        [Description("Collect weekly crate.")]
        public async Task Weekly(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder();
            var user = Bot.PlayerDatabase.GetPlayerByID(Convert.ToInt64(ctx.Member.Id));
            if (user == null)
            {
                embed = NewPlayer(ctx, embed, ctx.Member.Id);
                embed.Description += " Use g/weekly again or g/w or g/week to claim your crate.";
                embed.Color = DiscordColor.Purple;
            }
            else if (user.IsBanned == true)
            {
                embed.Title = "You Are Banned";
                embed.Description += " You are currently banned form using the main feature of this bot.";
                embed.Color = DiscordColor.Red;
            }
            else
            {
                if (user.Name != ctx.Member.Username)
                {
                    user.Name = ctx.Member.Username;
                }
                if (DateTime.Now - user.WeeklyCooldownStart >= user.WeeklyCooldown)
                {
                    var item = Crate.GetWeekly();
                    var isInInventory = false;
                    Item copy = null;
                    foreach (var thing in user.Inventory)
                    {
                        if (thing.ID == item.ID)
                        {
                            copy = thing;
                            isInInventory = true;
                            break;
                        }
                    }
                    if (isInInventory == false)
                    {
                        user.Inventory.Add(item);
                    }
                    else
                    {
                        copy.Multiple++;
                    }
                    user.WeeklyCooldown = TimeSpan.FromDays(7);
                    user.WeeklyCooldownEnd = user.WeeklyCooldownStart.Add(user.WeeklyCooldown);
                    Bot.PlayerDatabase.UpdatePlayer(user);
                    embed.Title = $"You got {item.Name}";
                    embed.Color = DiscordColor.Orange;
                    embed.Description = "You can do this again in the next month";
                }
                else
                {
                    embed.Title = "Cooldown Still Active:";
                    var cooldown = user.GetWeeklyCooldown();
                    embed.Description = $"{cooldown}";
                    embed.Color = DiscordColor.Red;
                    Bot.PlayerDatabase.UpdatePlayer(user);
                }
            }
            await ctx.Channel.SendMessageAsync(embed: embed);
        }

        [Command("monthly"), Aliases("m", "month")]
        [Description("Collect monthly crate.")]
        public async Task Monthly(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder();
            var user = Bot.PlayerDatabase.GetPlayerByID(Convert.ToInt64(ctx.Member.Id));
            if (user == null)
            {
                embed = NewPlayer(ctx, embed, ctx.Member.Id);
                embed.Description += " Use g/monthly again or g/m or g/month to claim your crate.";
                embed.Color = DiscordColor.Purple;
            }
            else if(user.IsBanned == true)
            {
                embed.Title = "You Are Banned";
                embed.Description += " You are currently banned form using the main feature of this bot.";
                embed.Color = DiscordColor.Red;
            }
            else
            {
                if (user.Name != ctx.Member.Username)
                {
                    user.Name = ctx.Member.Username;
                }
                Bot.PlayerDatabase.UpdatePlayer(user);
                if (DateTime.Now - user.MonthlyCooldownStart >= user.MonthlyCooldown)
                {
                    var item = Crate.GetMonthly();
                    var isInInventory = false;
                    Item copy = null;
                    foreach (var thing in user.Inventory)
                    {
                        if (thing.ID == item.ID)
                        {
                            copy = thing;
                            isInInventory = true;
                            break;
                        }
                    }
                    if (isInInventory == false)
                    {
                        user.Inventory.Add(item);
                    }
                    else
                    {
                        copy.Multiple++;
                    }
                    user.MonthlyCooldown = TimeSpan.FromDays(30);
                    user.MonthlyCooldownEnd = user.MonthlyCooldownStart.Add(user.MonthlyCooldown);
                    Bot.PlayerDatabase.UpdatePlayer(user);
                    embed.Title = $"You got {item.Name}";
                    embed.Color = DiscordColor.Orange;
                    embed.Description = "You can do this again in the next month";
                }
                else
                {
                    embed.Title = "Cooldown Still Active:";
                    var cooldown = user.GetMonthCooldown();
                    embed.Description = $"{cooldown}";
                    embed.Color = DiscordColor.Red;
                    Bot.PlayerDatabase.UpdatePlayer(user);
                }
            }
            await ctx.Channel.SendMessageAsync(embed: embed);
        }

        [Command("cooldown"), Aliases("c")]
        [Description("Displays all cooldowns.")]
        public async Task Cooldown(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder();
            var user = Bot.PlayerDatabase.GetPlayerByID(Convert.ToInt64(ctx.Member.Id));
            if (user == null)
            {
                embed = NewPlayer(ctx, embed, ctx.Member.Id);
                embed.Color = DiscordColor.Purple;
            }
            else
            {
                if (user.Name != ctx.Member.Username)
                {
                    user.Name = ctx.Member.Username;
                }
                Bot.PlayerDatabase.UpdatePlayer(user);
                embed.Title = "Cooldowns:";
                embed.AddField("Hourly", $"{user.GetHourCooldown()}", true);
                embed.AddField("Daily", $"{user.GetDailyCooldown()}", true);
                embed.AddField("Weekly", $"{user.GetWeeklyCooldown()}", true);
                embed.AddField("Monthly", $"{user.GetMonthCooldown()}", true);
                embed.AddField("Scrambler", $"{user.GetScremblerCooldown()}", true);
                embed.AddField("ConnectX", $"{user.GetDotPuzzleCooldown()}", true);
                embed.AddField("Optout", $"{user.GetOptCooldown()}", true);
                embed.AddField("Attack", $"{user.GetWeaponCooldown()}", true);
                embed.AddField("Healing", $"{user.GetHealthCooldown()}", true);
                embed.Color = DiscordColor.CornflowerBlue;
            }
            await ctx.Channel.SendMessageAsync(embed: embed);
        }
        [Command("imc"), Aliases("ignore", "imgc", "optimc")]
        [Description("Opt for ignoring minigame cooldowns. Rewards will not be obtainable.")]
        public async Task IgnoreMinigameCooldown(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder();
            var user = Bot.PlayerDatabase.GetPlayerByID(Convert.ToInt64(ctx.Member.Id));
            if (user.Name != ctx.Member.Username)
            {
                user.Name = ctx.Member.Username;
            }
            Bot.PlayerDatabase.UpdatePlayer(user);
            if (user == null)
            {
                embed = NewPlayer(ctx, embed, ctx.Member.Id);
                embed.Color = DiscordColor.Purple;
            }
            else if(user.GameCooldownIgnore == false)
            {
                embed.Title = "Minigame Cooldown Removed";
                embed.Description = "Minigame cooldowns will no longer be applied to you. You will not be able to collect rewards from minigames. Only ConnectX cooldowns will be ignored.";
                embed.Color = DiscordColor.Green;
                user.GameCooldownIgnore = true;
                Bot.PlayerDatabase.UpdatePlayer(user);
            }
            else
            {
                embed.Title = "Cooldown Already Ignored";
                embed.Description = "You are already ignoring minigame cooldowns.";
                embed.Color = DiscordColor.Red;
            }
            await ctx.Channel.SendMessageAsync(embed: embed);
        }

        [Command("amc"), Aliases("amgc", "optoutimg", "optamc")]
        [Description("Opt out of ignoring minigame cooldowns. Rewards will be obtainable and cooldowns will apply.")]
        public async Task AcceptMinigameCooldown(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder();
            var user = Bot.PlayerDatabase.GetPlayerByID(Convert.ToInt64(ctx.Member.Id));
            if (user.Name != ctx.Member.Username)
            {
                user.Name = ctx.Member.Username;
            }
            Bot.PlayerDatabase.UpdatePlayer(user);
            if (user == null)
            {
                embed = NewPlayer(ctx, embed, ctx.Member.Id);
                embed.Color = DiscordColor.Purple;
            }
            else if(user.GameCooldownIgnore == true)
            {
                embed.Title = "Minigame Cooldown Applied";
                embed.Description = "Minigame cooldowns will now be applied to you. You can now get rewards from minigames.";
                embed.Color = DiscordColor.Green;
                user.GameCooldownIgnore = false;
                Bot.PlayerDatabase.UpdatePlayer(user);
            }
            else
            {
                embed.Title = "Cooldown Already Applied";
                embed.Description = "You are already not ignoring minigame cooldowns.";
                embed.Color = DiscordColor.Red;
            }
            await ctx.Channel.SendMessageAsync(embed: embed);
        }

        [Command("trade"), Aliases("t", "tr")]
        [Description("Trade items between players. Trade rules apply.")]
        public async Task Trade(CommandContext ctx, DiscordMember partner)
        {
            var embed = new DiscordEmbedBuilder();
            var user = Bot.PlayerDatabase.GetPlayerByID(Convert.ToInt64(ctx.Member.Id));
            var partnerInv = Bot.PlayerDatabase.GetPlayerByID(Convert.ToInt64(partner.Id));
            var interactivity = ctx.Client.GetInteractivity();
            if (user.Name != ctx.Member.Username)
            {
                user.Name = ctx.Member.Username;
            }
            if (user == null)
            {
                embed = NewPlayer(ctx, embed, ctx.Member.Id);
                embed.Color = DiscordColor.Purple;
                await ctx.Channel.SendMessageAsync(embed: embed);
            }
            else if (user.TradeBan == true)
            {
                embed.Title = "You Currently Have A Trade Ban";
                embed.Description = "A trade ban prevents you from being able to trade items with GameTime. This ban can be appealed in the trade appeal thread under the appeal section.";
                embed.Color = DiscordColor.Red;
                await ctx.Channel.SendMessageAsync(embed: embed);
            }
            else if (partner == null)
            {
                embed.Title = "Could Not Locate Player";
                embed.Description = "Could not find the player you attempted to trade with.";
                embed.Color = DiscordColor.Red;
                await ctx.Channel.SendMessageAsync(embed: embed);
            }
            else if (partnerInv == null)
            {
                embed.Title = "Could Not Locate Player";
                embed.Description = "Could not find the player you attempted to trade with.";
                embed.Color = DiscordColor.Red;
                await ctx.Channel.SendMessageAsync(embed: embed);
            }
            else if (user.IsBanned == true)
            {
                embed.Title = "You Are Banned";
                embed.Description += " You are currently banned form using the main feature of this bot.";
                embed.Color = DiscordColor.Red;
                await ctx.Channel.SendMessageAsync(embed: embed);
            }
            else
            {
                embed.Description = $"{partner.Mention} do you agree to trade(Yes/No)?\nYou have 60 seconds to answer.";
                embed.Color = DiscordColor.Gold;
                var message = await ctx.Channel.SendMessageAsync(embed: embed);
            repeat:
                var response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == partner, TimeSpan.FromSeconds(60));
                await response.Result.DeleteAsync();
                if (response.TimedOut)
                {
                    embed.Description = $"{partner.Username} did not answer. Trade has been cancelled.";
                    embed.Color = DiscordColor.Red;
                    await ctx.Channel.SendMessageAsync(embed: embed);
                }
                else if (response.Result.Content.ToLower() == "no")
                {
                    embed.Description = $"{partner.Username} declined. Trade has been cancelled.";
                    embed.Color = DiscordColor.Red;
                    await ctx.Channel.SendMessageAsync(embed: embed);
                }
                else if (response.Result.Content.ToLower() == "yes")
                {
                    embed.Title = "Trade";
                    embed.Description = $"{ctx.Member.Username} & {partner.Username}\n{ctx.Member.Id} & {partner.Id}\nType the name of the item you would like to add\nExample: +add staff\nExample: +add staff x5\nType remove before the item to remove an item from the trade\nExample: +remove staff\nExample: +remove staff x5\nRespond with +confirm to lock your end.\nOther commands: +unconfirm, +cancel";
                    embed.AddField($"{ctx.Member.Username}", "None");
                    embed.AddField($"{partner.Username}", "None");
                    DiscordEmbed bembed = embed;
                    await message.ModifyAsync(null, bembed);
                    var userConfirm = false;
                    var partnerConfirm = false;
                    var userItems = new List<Item>();
                    var partnerItems = new List<Item>();
                    var display = "";
                    var userStatus = "Unconfirmed";
                    var partnerStatus = "Unconfirmed";
                    while (userConfirm == false || partnerConfirm == false)
                    {
                        display = "";
                        var name = "";
                        response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member || x.Author == partner, TimeSpan.FromSeconds(180));
                        if (response.Result.Content.ToLower().Contains("+add"))
                        {
                            var Name = response.Result.Content.Split("+add ");
                            if (Name.Length > 1)
                            {
                                name = Name[1];
                            }
                        }
                        else if (response.Result.Content.ToLower().Contains("+remove"))
                        {
                            var Name = response.Result.Content.Split("+remove ");
                            if (Name.Length > 1)
                            {
                                name = Name[1];
                            }
                        }
                        if (response.TimedOut)
                        {
                            embed.ClearFields();
                            embed.Description = "Trade timed out. Trade has been cancelled.";
                            embed.Color = DiscordColor.Red;
                            bembed = embed;
                            await message.ModifyAsync(null, bembed);
                        }
                        else if (response.Result.Author == ctx.Member)
                        {
                            if (response.Result.Content.ToLower() == "+unconfirm")
                            {
                                userConfirm = false;
                                userStatus = "Unconfirmed";
                                var embed2 = new DiscordEmbedBuilder();
                                embed2.Title = $"{ctx.Member.Username} unconfirmed.";
                                embed2.Color = DiscordColor.Red;
                                await ctx.Channel.SendMessageAsync(embed: embed2);
                                await response.Result.DeleteAsync();
                            }
                            else if (response.Result.Content.ToLower() == "g/i" || response.Result.Content.ToLower() == "g/inv" || response.Result.Content.ToLower() == "g/inventory")
                            {

                            }
                            else if (response.Result.Content.ToLower() == "+cancel")
                            {
                                embed.ClearFields();
                                embed.Title = "Trade Cancelled";
                                embed.Description = $"{ctx.Member.Username} cancelled the trade.";
                                embed.Color = DiscordColor.Red;
                                bembed = embed;
                                await message.ModifyAsync(null, bembed);
                                await response.Result.DeleteAsync();
                                break;
                            }
                            else if (userConfirm == true)
                            {
                                var embed2 = new DiscordEmbedBuilder();
                                embed2.Title = $"{ctx.Member.Username} cannot modify their end.";
                                embed2.Color = DiscordColor.Red;
                                await ctx.Channel.SendMessageAsync(embed: embed2);
                                await response.Result.DeleteAsync();
                            }
                            else if (response.Result.Content.ToLower() == "+confirm")
                            {
                                userConfirm = true;
                                if (userConfirm == true)
                                {
                                    userStatus = "Confirmed";
                                }
                                if (partnerConfirm == true)
                                {
                                    partnerStatus = "Confirmed";
                                }
                                embed.ClearFields();
                                foreach (var it in userItems)
                                {
                                    display += $"{it.Name} x{it.Multiple}\n";
                                }
                                if (display == "")
                                {
                                    display = "None";
                                }
                                embed.AddField($"{ctx.Member.Username} {userStatus}", $"{display}");
                                display = "";
                                foreach (var it in partnerItems)
                                {
                                    display += $"{it.Name} x{it.Multiple}\n";
                                }
                                if (display == "")
                                {
                                    display = "None";
                                }
                                embed.AddField($"{partner.Username} {partnerStatus}", $"{display}");
                                bembed = embed;
                                await message.ModifyAsync(null, bembed);
                                await response.Result.DeleteAsync();
                            }
                            else if (response.Result.Content.ToLower().Contains("+remove"))
                            {
                                var amount = 1;
                                var itemName = name.ToLower();
                                var split = name.ToLower().Split(" x");
                                if (split.Count() > 1)
                                {
                                    if (Int32.Parse(split[1]) > 0)
                                    {
                                        amount = Int32.Parse(split[1]);
                                    }
                                    itemName = split[0];
                                }
                                Item item = null;
                                foreach (var i in userItems)
                                {
                                    if (i.Name.ToLower() == itemName.ToLower())
                                    {
                                        item = i;
                                    }
                                    else if (i.Subname.ToLower() == itemName.ToLower())
                                    {
                                        item = i;
                                    }
                                }
                                if (item != null)
                                {
                                    if (item.Multiple + 1 < amount)
                                    {
                                        var embed2 = new DiscordEmbedBuilder();
                                        embed2.Title = "Not Enough Items";
                                        embed2.AddField("Error", "You are attempting to remove too many items.");
                                        embed2.Color = DiscordColor.Red;
                                        await ctx.Channel.SendMessageAsync(embed: embed2);
                                    }
                                    else
                                    {
                                        display = "";
                                        if (item.Multiple >= 1 && item.Multiple - amount > 0)
                                        {
                                            item.Multiple -= amount;
                                        }
                                        else if (item.Multiple - amount == 0)
                                        {
                                            userItems.Remove(item);
                                        }
                                        else
                                        {
                                            var embed2 = new DiscordEmbedBuilder();
                                            embed2.Title = "Not Enough Items";
                                            embed2.AddField("Error", "You are attempting to remove too many items.");
                                            embed2.Color = DiscordColor.Red;
                                            await ctx.Channel.SendMessageAsync(embed: embed2);
                                        }
                                        embed.ClearFields();
                                        foreach (var it in userItems)
                                        {
                                            display += $"{it.Name} x{it.Multiple}\n";
                                        }
                                        if (display == "")
                                        {
                                            display = "None";
                                        }
                                        embed.AddField($"{ctx.Member.Username} {userStatus}", $"{display}");
                                        display = "";
                                        foreach (var it in partnerItems)
                                        {
                                            display += $"{it.Name} x{it.Multiple}\n";
                                        }
                                        if (display == "")
                                        {
                                            display = "None";
                                        }
                                        embed.AddField($"{partner.Username} {partnerStatus}", $"{display}");
                                        bembed = embed;
                                        await message.ModifyAsync(null, bembed);
                                        await response.Result.DeleteAsync();
                                    }
                                }
                                else if (response.Result.Content.ToLower().Contains("+remove"))
                                {
                                    var embed2 = new DiscordEmbedBuilder();
                                    embed2.Title = "Item Not Found";
                                    embed2.AddField("Error", "Item is not being used in trade.");
                                    embed2.Color = DiscordColor.Red;
                                    await ctx.Channel.SendMessageAsync(embed: embed2);
                                    await response.Result.DeleteAsync();
                                }
                                else
                                {

                                }
                            }
                            else
                            {
                                var amount = 1;
                                var itemName = name.ToLower();
                                var split = name.ToLower().Split(" x");
                                if (split.Count() > 1)
                                {
                                    if (Int32.Parse(split[1]) > 0)
                                    {
                                        amount = Int32.Parse(split[1]);
                                    }
                                    itemName = split[0];
                                }
                                Item item = null;
                                foreach (var i in user.Inventory)
                                {
                                    if (i.Name.ToLower() == itemName.ToLower())
                                    {
                                        item = i;
                                    }
                                    else if (i.Subname.ToLower() == itemName.ToLower())
                                    {
                                        item = i;
                                    }
                                }
                                if (item != null)
                                {
                                    if (item.Multiple < amount)
                                    {
                                        var embed2 = new DiscordEmbedBuilder();
                                        embed2.Title = "Not Enough Items";
                                        embed2.AddField("Error", "You do not have enough of that item in your inventory.");
                                        embed2.Color = DiscordColor.Red;
                                        await ctx.Channel.SendMessageAsync(embed: embed2);
                                    }
                                    else
                                    {
                                        foreach (var i in Bot.Items.AllItems)
                                        {
                                            if (i.Name.ToLower() == itemName.ToLower())
                                            {
                                                item = i;
                                            }
                                            else if (i.Subname.ToLower() == itemName.ToLower())
                                            {
                                                item = i;
                                            }
                                        }
                                        display = "";
                                        var isInList = false;
                                        Item copy = null;
                                        foreach (var it in userItems)
                                        {
                                            if (it.ID == item.ID)
                                            {
                                                copy = it;
                                                isInList = true;
                                                break;
                                            }
                                        }
                                        if (isInList == false)
                                        {
                                            item = Bot.Items.GetItem(item);
                                            userItems.Add(item);
                                            item.Multiple = amount;
                                        }
                                        else
                                        {
                                            var invalid = false;
                                            foreach (var it in userItems)
                                            {
                                                invalid = false;
                                                foreach (var invItem in user.Inventory)
                                                {
                                                    if (it.ID == invItem.ID && it.Multiple + amount > invItem.Multiple)
                                                    {
                                                        invalid = true;
                                                    }
                                                }
                                            }
                                            if (invalid != true)
                                            {
                                                copy.Multiple += amount;
                                            }
                                            else
                                            {
                                                var embed2 = new DiscordEmbedBuilder();
                                                embed2.Title = "Not Enough Items";
                                                embed2.AddField("Error", "You do not have enough of that item in your inventory.");
                                                embed2.Color = DiscordColor.Red;
                                                await ctx.Channel.SendMessageAsync(embed: embed2);
                                            }
                                        }
                                        embed.ClearFields();
                                        foreach (var it in userItems)
                                        {
                                            display += $"{it.Name} x{it.Multiple}\n";
                                        }
                                        if (display == "")
                                        {
                                            display = "None";
                                        }
                                        embed.AddField($"{ctx.Member.Username} {userStatus}", $"{display}");
                                        display = "";
                                        foreach (var it in partnerItems)
                                        {
                                            display += $"{it.Name} x{it.Multiple}\n";
                                        }
                                        if (display == "")
                                        {
                                            display = "None";
                                        }
                                        embed.AddField($"{partner.Username} {partnerStatus}", $"{display}");
                                        bembed = embed;
                                        await message.ModifyAsync(null, bembed);
                                        await response.Result.DeleteAsync();
                                    }
                                }
                                else if (response.Result.Content.ToLower().Contains("+add") && item == null)
                                {
                                    var embed2 = new DiscordEmbedBuilder();
                                    embed2.Title = $"{ctx.Member.Username} entered an invalid item or does not have the item.";
                                    embed2.Color = DiscordColor.Red;
                                    await ctx.Channel.SendMessageAsync(embed: embed2);
                                    await response.Result.DeleteAsync();
                                }
                                else
                                {

                                }
                            }
                        }
                        else if (response.Result.Author == partner)
                        {
                            if (response.Result.Content.ToLower() == "+unconfirm")
                            {
                                userConfirm = false;
                                userStatus = "Unconfirmed";
                                var embed2 = new DiscordEmbedBuilder();
                                embed2.Title = $"{ctx.Member.Username} unconfirmed.";
                                embed2.Color = DiscordColor.Red;
                                await ctx.Channel.SendMessageAsync(embed: embed2);
                                await response.Result.DeleteAsync();
                            }
                            else if (response.Result.Content.ToLower() == "g/i" || response.Result.Content.ToLower() == "g/inv" || response.Result.Content.ToLower() == "g/inventory")
                            {

                            }
                            else if (response.Result.Content.ToLower() == "+cancel")
                            {
                                embed.ClearFields();
                                embed.Title = "Trade Cancelled";
                                embed.Description = $"{partner.Username} cancelled the trade.";
                                embed.Color = DiscordColor.Red;
                                bembed = embed;
                                await message.ModifyAsync(null, bembed);
                                await response.Result.DeleteAsync();
                                break;
                            }
                            else if (partnerConfirm == true)
                            {
                                var embed2 = new DiscordEmbedBuilder();
                                embed2.Title = $"{partner.Username} cannot modify their end.";
                                embed2.Color = DiscordColor.Red;
                                await ctx.Channel.SendMessageAsync(embed: embed2);
                                await response.Result.DeleteAsync();
                            }
                            else if (response.Result.Content.ToLower() == "+unconfirm")
                            {
                                partnerConfirm = false;
                                partnerStatus = "Unconfirmed";
                                var embed2 = new DiscordEmbedBuilder();
                                embed2.Title = $"{partner.Username} unconfirmed.";
                                embed2.Color = DiscordColor.Red;
                                await ctx.Channel.SendMessageAsync(embed: embed2);
                                await response.Result.DeleteAsync();
                            }
                            else if (response.Result.Content.ToLower() == "+confirm")
                            {
                                partnerConfirm = true;
                                if (userConfirm == true)
                                {
                                    userStatus = "Confirmed";
                                }
                                if (partnerConfirm == true)
                                {
                                    partnerStatus = "Confirmed";
                                }
                                embed.ClearFields();
                                foreach (var it in userItems)
                                {
                                    display += $"{it.Name} x{it.Multiple}\n";
                                }
                                if (display == "")
                                {
                                    display = "None";
                                }
                                embed.AddField($"{ctx.Member.Username} {userStatus}", $"{display}");
                                display = "";
                                foreach (var it in partnerItems)
                                {
                                    display += $"{it.Name} x{it.Multiple}\n";
                                }
                                if (display == "")
                                {
                                    display = "None";
                                }
                                embed.AddField($"{partner.Username} {partnerStatus}", $"{display}");
                                bembed = embed;
                                await message.ModifyAsync(null, bembed);
                                await response.Result.DeleteAsync();
                            }
                            else if (response.Result.Content.ToLower().Contains("+remove"))
                            {
                                var amount = 1;
                                var itemName = name.ToLower();
                                var split = name.ToLower().Split(" x");
                                if (split.Count() > 1)
                                {
                                    if (Int32.Parse(split[1]) > 0)
                                    {
                                        amount = Int32.Parse(split[1]);
                                    }
                                    itemName = split[0];
                                }
                                Item item = null;
                                foreach (var i in partnerItems)
                                {
                                    if (i.Name.ToLower() == itemName.ToLower())
                                    {
                                        item = i;
                                    }
                                    else if (i.Subname.ToLower() == itemName.ToLower())
                                    {
                                        item = i;
                                    }
                                }
                                if (item != null)
                                {
                                    if (item.Multiple + 1 < amount)
                                    {
                                        var embed2 = new DiscordEmbedBuilder();
                                        embed2.Title = "Not Enough Items";
                                        embed2.AddField("Error", "You are attempting to remove too many items.");
                                        embed2.Color = DiscordColor.Red;
                                        await ctx.Channel.SendMessageAsync(embed: embed2);
                                    }
                                    else
                                    {
                                        display = "";
                                        if (item.Multiple >= 1 && item.Multiple - amount > 0)
                                        {
                                            item.Multiple -= amount;
                                        }
                                        else if (item.Multiple - amount == 0)
                                        {
                                            partnerItems.Remove(item);
                                        }
                                        else
                                        {
                                            var embed2 = new DiscordEmbedBuilder();
                                            embed2.Title = "Not Enough Items";
                                            embed2.AddField("Error", "You are attempting to remove too many items.");
                                            embed2.Color = DiscordColor.Red;
                                            await ctx.Channel.SendMessageAsync(embed: embed2);
                                        }
                                        embed.ClearFields();
                                        foreach (var it in userItems)
                                        {
                                            display += $"{it.Name} x{it.Multiple}\n";
                                        }
                                        if (display == "")
                                        {
                                            display = "None";
                                        }
                                        embed.AddField($"{ctx.Member.Username} {userStatus}", $"{display}");
                                        display = "";
                                        foreach (var it in partnerItems)
                                        {
                                            display += $"{it.Name} x{it.Multiple}\n";
                                        }
                                        if (display == "")
                                        {
                                            display = "None";
                                        }
                                        embed.AddField($"{partner.Username} {partnerStatus}", $"{display}");
                                        bembed = embed;
                                        await message.ModifyAsync(null, bembed);
                                        await response.Result.DeleteAsync();
                                    }
                                }
                                else if (response.Result.Content.ToLower().Contains("+remove"))
                                {
                                    var embed2 = new DiscordEmbedBuilder();
                                    embed2.Title = "Item Not Found";
                                    embed2.AddField("Error", "Item is not being used in trade.");
                                    embed2.Color = DiscordColor.Red;
                                    await ctx.Channel.SendMessageAsync(embed: embed2);
                                    await response.Result.DeleteAsync();
                                }
                                else
                                {

                                }
                            }
                            else
                            {
                                var amount = 1;
                                var itemName = name.ToLower();
                                var split = name.ToLower().Split(" x");
                                if (split.Count() > 1)
                                {
                                    if (Int32.Parse(split[1]) > 0)
                                    {
                                        amount = Int32.Parse(split[1]);
                                    }
                                    itemName = split[0];
                                }
                                Item item = null;
                                foreach (var i in partnerInv.Inventory)
                                {
                                    if (i.Name.ToLower() == itemName.ToLower())
                                    {
                                        item = i;
                                    }
                                    else if (i.Subname.ToLower() == itemName.ToLower())
                                    {
                                        item = i;
                                    }
                                }
                                if (item != null)
                                {
                                    if (item.Multiple < amount)
                                    {
                                        var embed2 = new DiscordEmbedBuilder();
                                        embed2.Title = "Not Enough Items";
                                        embed2.AddField("Error", "You do not have enough of that item in your inventory.");
                                        embed2.Color = DiscordColor.Red;
                                        await ctx.Channel.SendMessageAsync(embed: embed2);
                                    }
                                    else
                                    {
                                        foreach (var i in Bot.Items.AllItems)
                                        {
                                            if (i.Name.ToLower() == itemName.ToLower())
                                            {
                                                item = i;
                                            }
                                            else if (i.Subname.ToLower() == itemName.ToLower())
                                            {
                                                item = i;
                                            }
                                        }
                                        display = "";
                                        var isInList = false;
                                        Item copy = null;
                                        foreach (var it in partnerItems)
                                        {
                                            if (it.ID == item.ID)
                                            {
                                                copy = it;
                                                isInList = true;
                                                break;
                                            }
                                        }
                                        if (isInList == false)
                                        {
                                            item = Bot.Items.GetItem(item);
                                            partnerItems.Add(item);
                                            item.Multiple = amount;
                                        }
                                        else
                                        {
                                            var invalid = false;
                                            foreach (var it in partnerItems)
                                            {
                                                invalid = false;
                                                foreach (var invItem in partnerInv.Inventory)
                                                {
                                                    if (it.ID == invItem.ID && it.Multiple + amount > invItem.Multiple)
                                                    {
                                                        invalid = true;
                                                    }
                                                }
                                            }
                                            if (invalid != true)
                                            {
                                                copy.Multiple += amount;
                                            }
                                            else
                                            {
                                                var embed2 = new DiscordEmbedBuilder();
                                                embed2.Title = "Not Enough Items";
                                                embed2.AddField("Error", "You do not have enough of that item in your inventory.");
                                                embed2.Color = DiscordColor.Red;
                                                await ctx.Channel.SendMessageAsync(embed: embed2);
                                            }
                                        }
                                        embed.ClearFields();
                                        foreach (var it in userItems)
                                        {
                                            display += $"{it.Name} x{it.Multiple}\n";
                                        }
                                        if (display == "")
                                        {
                                            display = "None";
                                        }
                                        embed.AddField($"{ctx.Member.Username} {userStatus}", $"{display}");
                                        display = "";
                                        foreach (var it2 in partnerItems)
                                        {
                                            display += $"{it2.Name} x{it2.Multiple}\n";
                                        }
                                        if (display == "")
                                        {
                                            display = "None";
                                        }
                                        embed.AddField($"{partner.Username} {partnerStatus}", $"{display}");
                                        bembed = embed;
                                        await message.ModifyAsync(null, bembed);
                                    }
                                    await response.Result.DeleteAsync();
                                }
                                else if (response.Result.Content.ToLower().Contains("+add") && item == null)
                                {
                                    var embed2 = new DiscordEmbedBuilder();
                                    embed2.Title = $"{partner.Username} entered an invalid item or does not have the item.";
                                    embed2.Color = DiscordColor.Red;
                                    await ctx.Channel.SendMessageAsync(embed: embed2);
                                }
                                else
                                {

                                }
                            }
                        }
                    }
                    var finalTrade = bembed;
                    if (userConfirm == true && partnerConfirm == true)
                    {
                        userConfirm = false;
                        partnerConfirm = false;
                        var status = $"Both parties must confirm the trade with +accept. Use +refuse to cancel. Once you accept you cannot use +refuse. \n{ctx.Member.Username} Agreement: {userConfirm}\n{partner.Username} Agreement: {partnerConfirm}";
                        embed.ClearFields();
                        embed.Title = "Confirmation";
                        embed.Description = status;
                        embed.Color = DiscordColor.Yellow;
                        message = await ctx.Channel.SendMessageAsync(embed: embed);
                        while (userConfirm == false || partnerConfirm == false)
                        {
                            response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member || x.Author == partner, TimeSpan.FromSeconds(60));
                            if (response.TimedOut)
                            {
                                var player = "";
                                if (userConfirm == false)
                                {
                                    player += ctx.Member.Username;
                                }
                                else if (partnerConfirm == false)
                                {
                                    player += partner.Username;
                                }
                                else
                                {
                                    player += $"{ctx.Member.Username} & {partner.Username}";
                                }
                                embed.Title = "Trade Failed";
                                embed.Description = $"{partner.Username} did not respond to the trade.";
                                embed.Color = DiscordColor.Red;
                                bembed = embed;
                                await message.ModifyAsync(null, bembed);
                                break;
                            }
                            else if (response.Result.Author == ctx.Member && response.Result.Content.ToLower() == "+accept" && userConfirm != true)
                            {
                                userConfirm = true;
                                status = $"Both parties must confirm the trade with +accept.\n{ctx.Member.Username} Agreement: {userConfirm}\n{partner.Username} Agreement: {partnerConfirm}";
                                embed.Description = status;
                                bembed = embed;
                                await message.ModifyAsync(null, bembed);
                            }
                            else if (response.Result.Author == partner && response.Result.Content.ToLower() == "+accept" && partnerConfirm != true)
                            {
                                partnerConfirm = true;
                                status = $"Both parties must confirm the trade with +accept.\n{ctx.Member.Username} Agreement: {userConfirm}\n{partner.Username} Agreement: {partnerConfirm}";
                                embed.Description = status;
                                bembed = embed;
                                await message.ModifyAsync(null, bembed);
                            }
                            else if (response.Result.Author == ctx.Member && response.Result.Content.ToLower() == "+refuse" && partnerConfirm != true)
                            {
                                var player = "";
                                if (userConfirm == false)
                                {
                                    player += ctx.Member.Username;
                                }
                                else if (partnerConfirm == false)
                                {
                                    player += partner.Username;
                                }
                                else
                                {
                                    player += $"{ctx.Member.Username} & {partner.Username}";
                                }
                                embed.Title = "Trade Failed";
                                embed.Description = $"{partner.Username} did not agree to the trade.";
                                embed.Color = DiscordColor.Red;
                                bembed = embed;
                                await message.ModifyAsync(null, bembed);
                                break;
                            }
                            else if (response.Result.Author == partner && response.Result.Content.ToLower() == "+refuse" && partnerConfirm != true)
                            {
                                var player = "";
                                if (userConfirm == false)
                                {
                                    player += ctx.Member.Username;
                                }
                                else if (partnerConfirm == false)
                                {
                                    player += partner.Username;
                                }
                                else
                                {
                                    player += $"{ctx.Member.Username} & {partner.Username}";
                                }
                                embed.Title = "Trade Failed";
                                embed.Description = $"{partner.Username} did not agree to the trade.";
                                embed.Color = DiscordColor.Red;
                                bembed = embed;
                                await message.ModifyAsync(null, bembed);
                                break;
                            }
                            else
                            { }
                        }
                        if (userConfirm == true && partnerConfirm == true)
                        {
                            await message.DeleteAsync();
                            foreach (var tItem in partnerItems)
                            {
                                var amount2 = tItem.Multiple;
                                var isInInventory = false;
                                Item copy2 = null;
                                foreach (var thing in user.Inventory)
                                {
                                    if (thing.ID == tItem.ID)
                                    {
                                        copy2 = thing;
                                        isInInventory = true;
                                        break;
                                    }
                                }
                                for (var loop = 0; loop < amount2; loop++)
                                {
                                    if (isInInventory == false)
                                    {
                                        user.Inventory.Add(tItem);
                                        break;
                                    }
                                    else
                                    {
                                        copy2.Multiple++;
                                    }
                                }
                                foreach (var thing in partnerInv.Inventory)
                                {
                                    if (thing.ID == tItem.ID)
                                    {
                                        copy2 = thing;
                                        break;
                                    }
                                }
                                for (var loop = 0; loop < amount2; loop++)
                                {
                                    if (copy2.Multiple > 1)
                                    {
                                        copy2.Multiple--;
                                    }
                                    else
                                    {
                                        partnerInv.Inventory.Remove(copy2);
                                    }
                                }
                            }
                            Bot.PlayerDatabase.UpdatePlayer(partnerInv);
                            Bot.PlayerDatabase.UpdatePlayer(user);
                            foreach (var tItem in userItems)
                            {
                                var amount2 = tItem.Multiple;
                                var isInInventory = false;
                                Item copy2 = null;
                                foreach (var thing in partnerInv.Inventory)
                                {
                                    if (thing.ID == tItem.ID)
                                    {
                                        copy2 = thing;
                                        isInInventory = true;
                                        break;
                                    }
                                }
                                for (var loop = 0; loop < amount2; loop++)
                                {
                                    if (isInInventory == false)
                                    {
                                        partnerInv.Inventory.Add(tItem);
                                        break;
                                    }
                                    else
                                    {
                                        copy2.Multiple++;
                                    }
                                }
                                foreach (var thing in user.Inventory)
                                {
                                    if (thing.ID == tItem.ID)
                                    {
                                        copy2 = thing;
                                        break;
                                    }
                                }
                                for (var loop = 0; loop < amount2; loop++)
                                {
                                    if (copy2.Multiple > 1)
                                    {
                                        copy2.Multiple--;
                                    }
                                    else
                                    {
                                        user.Inventory.Remove(copy2);
                                    }
                                }
                            }
                            Bot.PlayerDatabase.UpdatePlayer(partnerInv);
                            Bot.PlayerDatabase.UpdatePlayer(user);
                            var embed3 = new DiscordEmbedBuilder();
                            embed3.Title = $"Trade Successful";
                            embed3.Color = DiscordColor.Green;
                            var channel = Bot.HomeGuild.GetChannel(750195506130583652);
                            await ctx.Channel.SendMessageAsync(embed: embed3);
                            await channel.SendMessageAsync(embed: finalTrade);
                        }
                        else
                        { }
                    }
                    else
                    { }
                }
                else
                {
                    goto repeat;
                }
            }
        }

        [Command("players"), Aliases("p", "users")]
        [Description("Displays total players playing GameTime.")]
        public async Task Players(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder();
            var totalPlayers = 0;
            var optedPlayers = 0;
            List<Player> allPlayers = new List<Player>();
            foreach(var player in Bot.PlayerDatabase.Players)
            {
                totalPlayers++;
                foreach(var guild in player.GuildsOptedIn)
                {
                    if(guild == Convert.ToInt64(ctx.Guild.Id))
                    {
                        optedPlayers++;
                    }
                }
            }
            embed.Title = "Players";
            embed.AddField($"Total users in server {ctx.Guild.Name}", $"{ctx.Guild.MemberCount}");
            embed.AddField("Total users in GameTime", $"{totalPlayers}");
            embed.AddField("Total players opted in this server", $"{optedPlayers}");
            embed.Color = DiscordColor.Azure;
            await ctx.Channel.SendMessageAsync(embed: embed);
        }
        private async void Death(CommandContext ctx, Player user, Player target)
        {
            var embed = new DiscordEmbedBuilder();
            var targetAsMember = ctx.Guild.GetMemberAsync(Convert.ToUInt64(target.ID));
            var inventoryItems = -1;
            foreach (var item in target.Inventory)
            {
                inventoryItems += item.Multiple;
            }
            var moneyStolen = Math.Round(target.Balance * 0.2m, 2);
            target.Balance -= moneyStolen;
            user.Balance += moneyStolen;
            if (inventoryItems > 0)
            {
                var leftInventory = inventoryItems * 75 / 100;
                var itemsLooted = "";
                while (inventoryItems > leftInventory)
                {
                    var rNum = new Random().Next(0, target.Inventory.Count);
                    var lootedItem = target.Inventory[rNum];
                    var isInInventory = false;
                    Item copy = null;
                    if (lootedItem.Multiple > 1)
                    {
                        lootedItem.Multiple--;
                    }
                    else
                    {
                        target.Inventory.Remove(lootedItem);
                    }
                    inventoryItems--;
                    itemsLooted += $"[{lootedItem.Rarity}] {lootedItem.Name}\n";
                    Item copy2 = null;
                    foreach (var thing in user.Inventory)
                    {
                        if (thing.ID == lootedItem.ID)
                        {
                            copy2 = thing;
                            isInInventory = true;
                            break;
                        }
                    }
                    if (isInInventory == false)
                    {
                        Item item2 = null;
                        foreach (var i in Bot.Items.AllItems)
                        {
                            if (i.Name.ToLower() == lootedItem.Name.ToLower())
                            {
                                item2 = i;
                            }
                            else if (i.Subname.ToLower() == lootedItem.Name.ToLower())
                            {
                                item2 = i;
                            }
                        }
                        user.Inventory.Add(item2);
                    }
                    else
                    {
                        copy2.Multiple++;
                    }
                }
                target.Health = 100;
                embed.Title = $"You killed {targetAsMember.Result.Username}";
                embed.AddField("You took:", $"{itemsLooted}\n${moneyStolen.ToString("###,###,###,###,###,##0.#0")}");
                embed.Color = DiscordColor.VeryDarkGray;
                Bot.PlayerDatabase.UpdatePlayer(user);
                Bot.PlayerDatabase.UpdatePlayer(target);
            }
            else
            {
                target.Health = 100;
                embed.Title = $"You killed {targetAsMember.Result.Username}";
                embed.AddField("You took:", $"None\n${moneyStolen}");
                embed.Color = DiscordColor.VeryDarkGray;
                Bot.PlayerDatabase.UpdatePlayer(user);
                Bot.PlayerDatabase.UpdatePlayer(target);
            }
            await ctx.Channel.SendMessageAsync(embed: embed);
        }
        private DiscordEmbedBuilder NewPlayer(CommandContext ctx, DiscordEmbedBuilder embed, ulong id)
        {
            embed.Title = "New User Detected";
            embed.Description = "Here is a flare to start you off. Use g/inventory again to view your inventory. Use g/help to view all the commands. ";
            Bot.PlayerDatabase.AddPlayer(new Player() { ID = Convert.ToInt64(id), Name = ctx.Member.Username });
            return embed;
        }
        private string Attack(DiscordEmbedBuilder embed, CommandContext ctx, Player user, Player target, Weapon weapon, DiscordMember targetAsMember, Item i)
        {
            var obtained = "";
            if ((weapon.WeaponType == WeaponType.Firearm || weapon.WeaponType == WeaponType.HeavyFirearm) && target.Protection > 0)
            {
                int damage = 0, hit = 0, leftoverDamage = 0, num = 0, attachmentHit = 0;
                if (user.ActiveAttachment != null)
                {
                    if (user.ActiveAttachment.Ability == Addon.HighestDamageIncrease)
                    {
                        damage += user.ActiveAttachment.Intensity;
                        obtained = $"Your {user.ActiveAttachment.Name} added {user.ActiveAttachment.Intensity} damage\n";
                    }
                    else if (user.ActiveAttachment.Ability == Addon.ReduceCooldown)
                    {
                        user.CooldownLength += TimeSpan.FromSeconds(user.ActiveAttachment.Intensity);
                        obtained = $"Your {user.ActiveAttachment.Name} reduced your cooldown by {user.ActiveAttachment.Intensity}%\n";
                    }
                    else if (user.ActiveAttachment.Ability == Addon.IncreaseHitRate)
                    {
                        attachmentHit += user.ActiveAttachment.Intensity;
                        obtained = $"Your {user.ActiveAttachment.Name} added {user.ActiveAttachment.Intensity} more strike to your weapon\n";
                    }
                    user.ActiveAttachment = null;
                }
                if (weapon.Special != Special.None && weapon.Special == Special.ArmorBust)
                {
                    damage += weapon.HighestDamage;
                    obtained += $"You broke {targetAsMember.Username}'s {target.ProtectionName}!\nYou hit {targetAsMember.Username} for {weapon.HighestDamage}\n";
                    target.Protection = 0;
                    target.ProtectionName = null;
                }
                else
                {
                    for (int count = 0; count < weapon.FireRate + attachmentHit; count++)
                    {
                        hit += weapon.LowestDamage == -1 ? weapon.HighestDamage : new Random().Next(weapon.LowestDamage, weapon.HighestDamage + 1);
                        damage += hit;
                        if (target.Protection > 0)
                        {
                            obtained += $"You hit {targetAsMember.Username}'s {target.ProtectionName} for {hit}\n";
                            var armorHealth2 = target.Protection;
                            armorHealth2 -= hit;
                            if (damage < 0)
                            {
                                damage = 0;
                            }
                            else
                            {
                                if (target.Protection < hit)
                                {
                                    leftoverDamage = hit - target.Protection;
                                    target.Protection = 0;
                                    damage -= hit;
                                    damage += leftoverDamage;
                                }
                                else
                                {
                                    target.Protection -= hit;
                                    damage = armorHealth2 - damage;
                                }
                                if (damage < 0)
                                {
                                    damage = 0;
                                }
                            }
                            Bot.PlayerDatabase.UpdatePlayer(target);
                            Bot.PlayerDatabase.UpdatePlayer(target);
                        }
                        else
                        {
                            obtained += $"You hit {targetAsMember.Username} for {hit}\n";
                        }
                        if (target.Protection <= 0 && num == 0)
                        {
                            num++;
                            obtained += $"You broke {targetAsMember.Username}'s {target.ProtectionName}! {leftoverDamage} hit {targetAsMember.Nickname}\n";
                            target.Protection = 0;
                            target.ProtectionName = null;
                        }
                    }
                }
                target.Health -= damage;
                if (i.Multiple > 1)
                {
                    i.Multiple--;
                }
                else
                {
                    user.Inventory.Remove(i);
                }
                Bot.PlayerDatabase.UpdatePlayer(user);
                Bot.PlayerDatabase.UpdatePlayer(target);
                if(target.ProtectionName == null)
                {
                    embed.Description = $"{obtained}\n{targetAsMember.Mention}\nHealth: {target.Health}\nPlayer Protection: {target.Protection}";
                }
                else
                {
                    embed.Description = $"{obtained}\n{targetAsMember.Mention}\nHealth: {target.Health}\n{target.ProtectionName}: {target.Protection}";
                }
            }
            else if (weapon.WeaponType == WeaponType.Melee && target.Protection > 0)
            {
                int damage = 0, hit = 0, leftoverDamage = 0, num = 0;
                hit = weapon.LowestDamage == -1 ? weapon.HighestDamage : new Random().Next(weapon.LowestDamage, weapon.HighestDamage + 1);
                damage += hit;
                if (target.Protection > 0)
                {
                    obtained += $"You hit {targetAsMember.Username}'s {target.ProtectionName} for {hit}\n";
                    var armorHealth2 = target.Protection;
                    armorHealth2 -= hit;
                    if (damage < 0)
                    {
                        damage = 0;
                    }
                    else
                    {
                        if (target.Protection < hit)
                        {
                            leftoverDamage = hit - target.Protection;
                            target.Protection = 0;
                            damage -= hit;
                            damage += leftoverDamage;
                        }
                        else
                        {
                            target.Protection -= hit;
                            damage = armorHealth2 - damage;
                        }
                        if (damage < 0)
                        {
                            damage = 0;
                        }
                    }
                    Bot.PlayerDatabase.UpdatePlayer(target);
                    Bot.PlayerDatabase.UpdatePlayer(target);
                }
                else
                {
                    obtained += $"You hit {targetAsMember.Username} for {hit}\n";
                }
                if (target.Protection <= 0 && num == 0)
                {
                    num++;
                    obtained += $"You broke {targetAsMember.Username}'s {target.ProtectionName}! {leftoverDamage} hit {targetAsMember.Username}\n";
                    target.Protection = 0;
                    target.ProtectionName = null;
                }
                    target.Health -= damage;
                if (weapon.Multiple > 1)
                {
                    weapon.Multiple--;
                }
                else
                {
                    user.Inventory.Remove(weapon);
                }
                Bot.PlayerDatabase.UpdatePlayer(user);
                Bot.PlayerDatabase.UpdatePlayer(target);
                if (target.ProtectionName == null)
                {
                    embed.Description = $"{obtained}\n{targetAsMember.Mention}\nHealth: {target.Health}\nPlayer Protection: {target.Protection}";
                }
                else
                {
                    embed.Description = $"{obtained}\n{targetAsMember.Mention}\nHealth: {target.Health}\n{target.ProtectionName}: {target.Protection}";
                }
            }
            else if ((weapon.WeaponType == WeaponType.Firearm || weapon.WeaponType == WeaponType.HeavyFirearm) && target.Protection <= 0)
            {
                int damage = 0, hit = 0, attachmentHit = 0;
                user.CooldownStartTime = DateTime.Now;
                user.CooldownLength = weapon.Cooldown;
                if (user.ActiveAttachment != null)
                {
                    if (user.ActiveAttachment.Ability == Addon.HighestDamageIncrease)
                    {
                        damage += user.ActiveAttachment.Intensity;
                        obtained = $"Your {user.ActiveAttachment.Name} added {user.ActiveAttachment.Intensity} damage\n";
                    }
                    else if (user.ActiveAttachment.Ability == Addon.ReduceCooldown)
                    {
                        user.CooldownLength += TimeSpan.FromSeconds(user.ActiveAttachment.Intensity);
                        obtained = $"Your {user.ActiveAttachment.Name} reduced your cooldown by {user.ActiveAttachment.Intensity}%\n";
                    }
                    else if (user.ActiveAttachment.Ability == Addon.IncreaseHitRate)
                    {
                        attachmentHit += user.ActiveAttachment.Intensity;
                        obtained = $"Your {user.ActiveAttachment.Name} added {user.ActiveAttachment.Intensity} more strike to your weapon\n";
                    }
                    user.ActiveAttachment = null;
                }
                if (weapon.Special != Special.None && weapon.Special == Special.ArmorBust)
                {
                    damage += (int)(target.Health * .5);
                    obtained += $"{targetAsMember.Username} did not have any active protection.\nYou damaged {targetAsMember.Username} for 50% of their health ({damage})\n";
                }
                else
                {
                    for (int count = 0; count < weapon.FireRate + attachmentHit; count++)
                    {
                        hit = weapon.LowestDamage == -1 ? weapon.HighestDamage : new Random().Next(weapon.LowestDamage, weapon.HighestDamage + 1);
                        damage += hit;
                        obtained += $"You hit {targetAsMember.Username} for {hit}\n";
                    }
                }
                target.Health -= damage;
                if (i.Multiple > 1)
                {
                    i.Multiple--;
                }
                else
                {
                    user.Inventory.Remove(i);
                }
                Bot.PlayerDatabase.UpdatePlayer(user);
                Bot.PlayerDatabase.UpdatePlayer(target);
                embed.Description = $"{obtained}\n{ targetAsMember.Mention }\nHealth: {target.Health}";
                if (target.Health <= 0)
                {
                    Death(ctx, user, target);
                }
            }
            else if (weapon.WeaponType == WeaponType.Melee && target.Protection <= 0)
            {
                var damage = 0;
                var hit = 0;
                for (int count = 0; count < weapon.FireRate; count++)
                {
                    hit = weapon.LowestDamage == -1 ? weapon.HighestDamage : new Random().Next(weapon.LowestDamage, weapon.HighestDamage + 1);
                    damage += hit;
                    obtained += $"You hit {targetAsMember.Username} for {hit}\n";
                }
                target.Health -= damage;
                Bot.PlayerDatabase.UpdatePlayer(target);
                if (weapon.Multiple > 1)
                {
                    weapon.Multiple--;
                }
                else
                {
                    user.Inventory.Remove(weapon);
                }
                Bot.PlayerDatabase.UpdatePlayer(user);
                embed.Description = $"{obtained}\n{targetAsMember.Mention}\nHealth: {target.Health}";
            }
            if (target.Health <= 0)
            {
                Death(ctx, user, target);
            }
            return embed.Description;
        }
    }
}
