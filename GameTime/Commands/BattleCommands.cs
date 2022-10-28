using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity.Extensions;
using GameTime.Models;

namespace GameTime.Commands
{
    public class ItemCommands : BaseCommandModule
    {
        [Command("list"), Description("list [item type]~Lists all items based on the type given. Will give a menu of all item types if no parameters are given.")]
        public async Task List(CommandContext ctx)
        {
            var options = new List<DiscordSelectComponentOption>
            {
                new DiscordSelectComponentOption("Guns", "guns"),
                new DiscordSelectComponentOption("Ammunition", "ammo"),
                new DiscordSelectComponentOption("Melee Weapons", "melee"),
                new DiscordSelectComponentOption("Armors", "armor"),
                new DiscordSelectComponentOption("Shields", "shield"),
                new DiscordSelectComponentOption("Units", "unit"),
                new DiscordSelectComponentOption("Parts", "part"),
                new DiscordSelectComponentOption("Attachments", "attachment"),
                new DiscordSelectComponentOption("Utilities", "utility"),
            };
            var componentId = "listMenu";
            var selectMenu = new DiscordSelectComponent(componentId, null, options, false, 1, 1);
            var embed = new DiscordEmbedBuilder()
            {
                Title = "Items",
                Description = "Select an option from the menu or use the command\ng/list [item type]",
            }.WithFooter("Menu expires in 2 minute").Build();
            var messageBuilder = new DiscordMessageBuilder().AddComponents(selectMenu).WithEmbed(embed);
            var message = await ctx.Channel.SendMessageAsync(embed);

            var response = await ctx.Client.GetInteractivity().WaitForSelectAsync(message, ctx.User, componentId, timeoutOverride: TimeSpan.FromMinutes(2));
            if(!response.TimedOut)
            {
                await message.ModifyAsync(new DiscordMessageBuilder().WithEmbed(message.Embeds[0]));
            }
            else
            {
                CreateListEmbed(response.Result.Values[0]);
            }
        }
        [Command("list"), Description("Lists all items based on the type given. Will give a menu of all item types if no parameters are given.")]
        public async Task List(CommandContext ctx, [RemainingText] string type)
        {
            
        }
        private DiscordEmbed CreateListEmbed(string type)
        {
            var title = GenerateListEmbedTitle(type);
            if (title == "") //If item request is invalid
                return null;

            var common = "";
            var uncommon = "";
            var rare = "";
            var unique = "";
            ItemType itemType = title switch
            {
                "Ranged Weapons" => ItemType.Firearm,
                "Ammunition" => ItemType.Ammo,
                "Melee Weapons" => ItemType.Melee,
                "Armors" => ItemType.Armor,
                "Shields" => ItemType.Shield,
                "Troops" => ItemType.Unit,
                "Parts" => ItemType.Part,
                "Attachments" => ItemType.Attachment,
                "Utilities" => ItemType.Utility,
                _ => ItemType.None,
            };

            foreach (var item in GetItemsByType(itemType))
            {
                switch(item.Rarity)
                {
                    case Rarity.Common:
                        common += item.Name + "\n";
                        break;
                    case Rarity.Uncommon:
                        uncommon += item.Name + "\n";
                        break;
                    case Rarity.Rare:
                        rare += item.Name + "\n";
                        break;
                    case Rarity.Unique:
                        unique += item.Name + "\n";
                        break;
                }
            }
            return new DiscordEmbedBuilder()
            {
                Title = title
            }.AddField("Common", common, true).AddField("Uncommon", uncommon, true).
            AddField("Rare", rare, true).AddField("Unique", unique, true);
        }
        private string GenerateListEmbedTitle(string type)
        {
            if (type.Contains("gun") || type.Contains("firearm") || type.Contains("range"))
                return "Ranged Weapons";
            else if (type.Contains("ammo") || type.Contains("ammu"))
                return "Ammunition";
            else if (type.Contains("melee") || type.Contains("hand"))
                return "Melee Weapons";
            else if (type.Contains("arm") || type.Contains("vest")) //armor
                return "Armors";
            else if (type.Contains("shield"))
                return "Shields";
            else if (type.Contains("sold") || type.Contains("tro") || type.Contains("unit"))
                return "Troops";
            else if (type.Contains("part"))
                return "Parts";
            else if (type.Contains("att") || type.Contains("add"))
                return "Attachments";
            else if (type.Contains("util"))
                return "Utilities";
            else
                return "";
        }
        private List<Item> GetItemsByType(ItemType type)
        {
            var items = new List<Item>();
            foreach(var it in Bot.GameItems.Items)
            {
                if(it.Value.Type == type)
                {
                    items.Add(it.Value);
                }
            }
            return items;
        }


        [Command("info")]
        public async Task Info(CommandContext ctx, [RemainingText] string itemName)
        {
            if (!Bot.GameItems.Items.ContainsKey(itemName.ToLower()))
            {
                await ctx.Channel.SendMessageAsync("No Item Found");
            }
            else
            {
                var embed = GenerateInfo(Bot.GameItems.Items[itemName]);
                await ctx.Channel.SendMessageAsync(embed);
            }
        }
        private DiscordEmbed GenerateInfo(Item item)
        {
            switch (item.Type)
            {
                case ItemType.Firearm:
                    var i = item as Firearm;
                    return new DiscordEmbedBuilder
                    {
                        Title = i.Name,
                        Color = DiscordColor.Orange,
                    }.AddField("Parts Required", i.GetPartSlotsString())
                    .AddField("Damage", $"{i.MinDamage}-{i.MaxDamage}").Build();
                default:
                    return new DiscordEmbedBuilder().Build();
            }
        }
    }
}
