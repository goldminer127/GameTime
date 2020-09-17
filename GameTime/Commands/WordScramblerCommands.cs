﻿using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using GameTime.Extensions;
using GameTime.Models;
using GameTime.ScramblerModels;
using System;
using System.Threading.Tasks;

namespace GameTime.Commands
{
    class WordScramblerCommands : BaseCommandModule
    {
        [Command("scrambler"), Aliases("scram", "sc")]
        [Description("Descramble the word to get a crate.")]
        public async Task Scrambler(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder();
            var user = Bot.PlayerDatabase.GetPlayerByID(Convert.ToInt64(ctx.Member.Id));
            if (user == null)
            {
                embed = NewPlayer(ctx, embed, ctx.Member.Id);
                embed.Color = DiscordColor.Yellow;
            }
            else
            {
                if (user.Name != ctx.Member.Username)
                {
                    user.Name = ctx.Member.Username;
                }
                Bot.PlayerDatabase.UpdatePlayer(user);
                if (DateTime.Now - user.ScramblerStart >= user.ScramblerCooldown)
                {
                    var interactivity = ctx.Client.GetInteractivity();
                    var randomNum = new Random().Next(1, Bot.Words.AllWords.Count);
                    var word = Bot.Words.AllWords[randomNum];
                    var scrambledWord = WordList.ScrambleWord(word.Word);
                    var crate = Crate.GetScrambleReward(word);
                    var multiple = 1;
                    if (word.Difficulty == Difficulty.Moderate || word.Difficulty == Difficulty.Hard)
                    {
                        multiple = 2;
                    }
                    else if (word.Difficulty == Difficulty.Impossible)
                    {
                        multiple = 4;
                    }
                    var difficulty = word.Difficulty switch
                    {
                        Difficulty.Easy => "E",
                        Difficulty.Moderate => "M",
                        Difficulty.Hard => "H",
                        Difficulty.Extreme => "EXR",
                        Difficulty.Insane => "INA",
                        _ => "IMB"
                    };
                    embed.Title = "Word Scrambler";
                    embed.WithFooter("You have 20 seconds to answer.");
                    embed.AddField("Unscramble the following word", $"{scrambledWord}");
                    embed.AddField("Reward: ", $"{crate.Name} x{multiple}");
                    embed.Color = DiscordColor.Aquamarine;
                    await ctx.Channel.SendMessageAsync(embed: embed);
                    embed = embed.ClearFields();
                    var response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(20));
                    if (response.TimedOut)
                    {
                        embed.Title = "Time Ran Out";
                        embed.Description = "You ran out of time.";
                        embed.AddField("Answer", $"{word.Word.ToLower()}");
                        embed.WithFooter($"#SC{difficulty}{randomNum}");
                        embed.Color = DiscordColor.Red;
                    }
                    else if (response.Result.Content.ToLower() == word.Word.ToLower() && user.IsBanned == false)
                    {
                        for (var i = 0; i < multiple; i++)
                        {
                            var isInInventory = false;
                            Item copy = null;
                            foreach (var thing in user.Inventory)
                            {
                                if (thing.ID == crate.ID)
                                {
                                    copy = thing;
                                    isInInventory = true;
                                    break;
                                }
                            }
                            if (isInInventory == false)
                            {
                                user.Inventory.Add(crate);
                            }
                            else
                            {
                                copy.Multiple++;
                            }
                        }
                        embed.Title = "Correct Answer!";
                        embed.AddField("Answer", $"{word.Word.ToLower()}");
                        embed.AddField("Reward: ", $"{crate.Name} x{multiple}");
                        embed.WithFooter($"#SC{difficulty}{randomNum}");
                        embed.Color = DiscordColor.Gold;
                    }
                    else if (response.Result.Content.ToLower() == word.Word.ToLower() && user.IsBanned == true)
                    {
                        embed.Title = "Correct Answer!";
                        embed.Description = "You are not eligible to revieve rewards from this mini-game because you are banned from using the main feature of this bot.";
                        embed.AddField("Answer", $"{word.Word.ToLower()}");
                        embed.WithFooter($"#SC{difficulty}{randomNum}");
                        Bot.PlayerDatabase.UpdatePlayer(user);
                    }
                    else if (response.Result.Content.ToLower() != word.Word)
                    {
                        embed.Title = "Incorrect Answer";
                        embed.AddField("Answer", $"{word.Word.ToLower()}");
                        embed.AddField("Your Answer", $"{response.Result.Content.ToLower()}");
                        embed.Description = "Better luck next time.";
                        embed.WithFooter($"#SC{difficulty}{randomNum}");
                        embed.Color = DiscordColor.Red;
                    }
                    user.ScramblerCooldown = TimeSpan.FromMinutes(30);
                    user.ScramblerStart = DateTime.Now;
                    user.ScramblerEnd = user.ScramblerStart.Add(user.ScramblerCooldown);
                }
                else
                {
                    embed.Title = "Cooldown Still Active";
                    embed.Description = $"{user.GetScremblerCooldown()}";
                    embed.Color = DiscordColor.Red;
                }
                Bot.PlayerDatabase.UpdatePlayer(user);
            }
            await ctx.Channel.SendMessageAsync(embed: embed);
        }
        private DiscordEmbedBuilder NewPlayer(CommandContext ctx, DiscordEmbedBuilder embed, ulong id)
        {
            embed.Title = "New User Detected";
            embed.Description = "Here is a flare to start you off. Use g/inventory again to view your inventory. Use g/help to view all the commands. ";
            Bot.PlayerDatabase.AddPlayer(new Player() { ID = Convert.ToInt64(id) });
            return embed;
        }
    }
}
