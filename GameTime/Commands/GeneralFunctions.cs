using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using GameTime.Models;
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
    }
}
