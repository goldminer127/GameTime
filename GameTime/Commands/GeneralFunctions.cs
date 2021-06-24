﻿using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using GameTime.Models;
using System;
using System.Threading.Tasks;

namespace GameTime.Commands
{
    class GeneralFunctions
    {
        public static void AddToInventory(Player user, Item item, int amount)
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
                item.Multiple += amount - 1;
            }
            else
            {
                copy.Multiple += amount;
            }
        }
        public static void UpdatePlayerDisplayInfo(CommandContext ctx, Player user)
        {
            user.Name = ctx.Member.Username;
            user.Image = ctx.Member.AvatarUrl;
            Bot.PlayerDatabase.UpdatePlayer(user);
        }
        public static void RemoveFromInventory(Player user, Item item, int amount)
        {
            if (item.Multiple - amount > 1)
            {
                item.Multiple -= amount;
            }
            else
            {
                user.Inventory.Remove(item);
            }
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
            else if (player.IsBanned == true || isNotCoreFunction == true)
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
        public static DiscordColor RarityColor(Item item)
        {
            switch (item.Rarity)
            {
                case Rarity.Common:
                    return DiscordColor.Gray;
                case Rarity.Uncommon:
                    return DiscordColor.DarkGreen;
                case Rarity.Rare:
                    return DiscordColor.Blue;
                case Rarity.Epic:
                    return DiscordColor.Gold;
                default: //Unique
                    return DiscordColor.Orange;
            }
        }
    }
}
