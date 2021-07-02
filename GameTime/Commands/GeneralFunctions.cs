using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using GameTime.Models;
using System;
using System.Threading.Tasks;

namespace GameTime.Commands
{
    class GeneralFunctions
    {
        public static void AddToInventory(Player user, Item item, int amount = 1)
        {
            var isInInventory = false;
            Item copy = null;
            foreach (var it in user.Inventory)
            {
                if (it.ID == item.ID)
                {
                    copy = it;
                    isInInventory = true;
                    break;
                }
            }
            if (isInInventory == false)
            {
                item = Bot.Items.GetItem(item);
                user.Inventory.Add(item);
                item.Multiple += amount - 1;
            }
            else
            {
                copy.Multiple += amount;
            }
        }
        public static void RemoveFromInventory(Player user, Item item, int amount)
        {
            if (item.Multiple - amount >= 1)
            {
                item.Multiple -= amount;
            }
            else
            {
                user.Inventory.Remove(item);
            }
        }
        //Searches for specified item in inventory. Returns null if it does not exist
        public static Item SearchItemInInventory(string itemName, Player user)
        {
            foreach (var i in user.Inventory)
            {
                if (i.Name.ToLower() == itemName.ToLower())
                {
                    return i;
                }
                else if (i.Subname.ToLower() == itemName.ToLower())
                {
                    return i;
                }
            }
            return null;
        }
        public static void UpdatePlayerDisplayInfo(CommandContext ctx, Player user)
        {
            user.Name = ctx.Member.Username;
            user.Image = ctx.Member.AvatarUrl;
            Bot.PlayerDatabase.UpdatePlayer(user);
        }
        public static async Task DelayMessageDeletion(DiscordMessage message, int length = 1000)
        {
            await Task.Delay(length);
            await message.DeleteAsync();
        }
        /*
        public static async Task MailPlayer(int length, long playerId, Mail mail)
        {
            player.Mail.Add(mail);
            Bot.PlayerDatabase.UpdatePlayer(player);
            AutoDeleteMail(length, player, mail);
        }
        */
        public static async Task AutoDeleteMail(CommandContext ctx, int length, long playerId, int mailId)
        {
            await Task.Delay(length);
            try
            {
                var player = Bot.PlayerDatabase.GetPlayerByID(playerId);
                Mail mail = null;
                foreach (var m in player.Mail)
                {
                    if (m.Id == mailId)
                    {
                        mail = m;
                    }
                }
                player.Mail.Remove(mail);
                Bot.PlayerDatabase.UpdatePlayer(player);
                int a = player.Mail.Count;
                await ctx.Channel.SendMessageAsync("Mail Deleted");
            }
            catch
            {

            }
        }
        //Checks if player is a valid player
        public static bool ValidatePlayer(CommandContext ctx, Player player, bool isNotCoreFunction = false)
        {
            var embed = new DiscordEmbedBuilder();
            if (player == null)
            {
                embed = Bot.PlayerDatabase.NewPlayer(ctx.Member, embed);
                embed.Color = DiscordColor.Blurple;
                ctx.Channel.SendMessageAsync(embed: embed);
                return false;
            }
            else if (player.IsBanned == true && isNotCoreFunction == false)
            {
                embed.Title = "You Are Banned";
                embed.Description += " You are currently banned form using the main feature of this bot.";
                embed.Color = DiscordColor.Red;
                ctx.Channel.SendMessageAsync(embed: embed);
                return false;
            }
            //Will always return true if discord user is not a bot
            return !(ctx.Member.IsBot);
        }
        //Get player from database via name or id
        public static Player GetRequestePlayer(string player)
        {
            try
            {
                var id = Convert.ToInt64(player);
                return Bot.PlayerDatabase.GetPlayerByID(Convert.ToInt64(id));
            }
            catch
            {
                var user = Bot.PlayerDatabase.GetPlayerByName(player);
                return user;
            }
        }
        //Assigns rarity colors to embeds
        public static DiscordColor RarityColor(Item item)
        {
            return item.Rarity switch
            {
                Rarity.Common => DiscordColor.Gray,
                Rarity.Uncommon => DiscordColor.DarkGreen,
                Rarity.Rare => DiscordColor.Blue,
                Rarity.Epic => DiscordColor.Gold,
                _ => DiscordColor.Orange
            };
        }
        //Returns item amounts from commands. Limits at 100
        //name looks like ([item name] x[num])
        public static byte GetItemAmount(string itemName)
        {
            var split = itemName.ToLower().Split(" x");
            if (split.Length > 1 && byte.Parse(split[1]) > 0)
            {
                try
                {
                    if (byte.Parse(split[1]) <= 100)
                        return byte.Parse(split[1]);
                    else
                        return 101;
                }
                catch
                {
                    return 1;
                }
            }
            else
            {
                return 1;
            }
        }
        //Returns item name from commands
        //name looks like ([item name] x[num])
        public static string GetItemName(string itemName)
        {
            return itemName.ToLower().Split(" x")[0];
        }
    }
}
