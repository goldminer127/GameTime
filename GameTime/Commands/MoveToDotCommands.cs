using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using GameTime.Models;
using System.Threading.Tasks;
using System;
using GameTime.Extensions;
using DSharpPlus.Interactivity;
using GameTime.DotPuzzleModels;

namespace GameTime.Commands
{
    public class MoveToDotCommands : BaseCommandModule
    {
        [Command("connectx"), Aliases("cx", "connect", "dotpuzzle", "dp")]
        [Description("Play a game of ConnectX.")]
        public async Task DotPuzzle(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder();
            var user = Bot.PlayerDatabase.GetPlayerByID(Convert.ToInt64(ctx.Member.Id));
            if (user == null)
            {
                embed = NewPlayer(ctx, embed, ctx.Member.Id);
                embed.Color = DiscordColor.Yellow;
                await ctx.Channel.SendMessageAsync(embed: embed);
            }
            else
            {
                if (user.Name != ctx.Member.Username)
                {
                    user.Name = ctx.Member.Username;
                }
                Bot.PlayerDatabase.UpdatePlayer(user);
                if (DateTime.Now - user.DotPuzzleStart >= user.DotPuzzleCooldown || user.GameCooldownIgnore == true)
                {
                    user.DotPuzzleStart = DateTime.Now;
                    user.DotPuzzleCooldown = TimeSpan.FromMinutes(30);
                    user.DotPuzzleEnd = user.DotPuzzleStart.Add(user.DotPuzzleCooldown);
                    Bot.PlayerDatabase.UpdatePlayer(user);
                    var interactivity = ctx.Client.GetInteractivity();
                    var puzzle = Bot.DotPuzzles.AllPuzzles[new Random().Next(0 , Bot.DotPuzzles.AllPuzzles.Count)];
                    var puzzleConstructed = Bot.DotPuzzles.ConstructPuzzle(puzzle.Puzzle);
                    var end = DateTime.Now + puzzle.TimeLimit;
                    var timeIsUp = false;
                    var index = 1;
                    var crate = Crate.GetDotPuzzleReward(puzzle);
                    var multiple = 1;
                    if (puzzle.DotDifficulty == DotDifficulty.Hard)
                    {
                        multiple = 2;
                    }
                    embed.Title = "Complete The Puzzle";
                    embed.AddField("Instructions:", $"Up = w Down = s Left = a Right = d\nClear all the X's. You have {Convert.ToInt32(puzzle.TimeLimit.TotalSeconds)} seconds to complete this puzzle.");
                    embed.AddField("Reward:", $"{crate.Name} x{multiple}");
                    embed.Description = "```\n";
                    foreach (var s in puzzleConstructed)
                    {
                        if (index % puzzle.Splitter == 0 && index != puzzleConstructed.Count - 1)
                        {
                            embed.Description += (s) + "\n";
                        }
                        else
                        {
                            embed.Description += (s);
                        }
                        index++;
                    }
                    embed.Description += "\n```";
                    var dembed = embed.Build();
                    var msg = await ctx.RespondAsync(embed: dembed);
                    var cooldown = DateTime.Now + TimeSpan.FromSeconds(1);
                    while(timeIsUp != true)
                    {
                        if (puzzleConstructed.Exists(x => x.Contains("X")) != true)
                        {
                            break;
                        }
                        else
                        {
                            var response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(.01));
                            var position = puzzleConstructed.FindIndex(x => x.Contains("+"));
                            if (response.TimedOut)
                            {
                                timeIsUp = Timer(end);
                            }
                            else if (Timer(cooldown) == false)
                            {
                                var embed2 = new DiscordEmbedBuilder();
                                embed2.Description = "Slow down, there is a 1 second cooldown between moves!";
                                embed2.Color = DiscordColor.Red;
                                await ctx.Channel.SendMessageAsync(embed: embed2);
                                await response.Result.DeleteAsync();
                            }
                            else if (response.Result.Content.ToLower() == "w" || response.Result.Content.ToLower() == "up")
                            {
                                var newPosition = position - puzzle.Splitter;
                                if (newPosition < -1 || newPosition > puzzleConstructed.Count - 1)
                                {
                                    var embed3 = new DiscordEmbedBuilder();
                                    embed3.Title = "Cannot Move To That Position";
                                    embed3.Color = DiscordColor.Red;
                                    await ctx.Channel.SendMessageAsync(embed: embed3);
                                }
                                else
                                {
                                    puzzleConstructed[newPosition] = " +";
                                    puzzleConstructed[position] = " |";
                                    embed.Description = "```\n";
                                    index = 1;
                                    foreach (var s in puzzleConstructed)
                                    {
                                        if (index % puzzle.Splitter == 0 && index != puzzleConstructed.Count - 1)
                                        {
                                            embed.Description += (s) + "\n";
                                        }
                                        else
                                        {
                                            embed.Description += (s);
                                        }
                                        index++;
                                    }
                                    embed.Description += "```";
                                    dembed = embed.Build();
                                    await msg.ModifyAsync(null, dembed);
                                }
                                await response.Result.DeleteAsync();
                                timeIsUp = Timer(end);
                                cooldown = DateTime.Now + TimeSpan.FromSeconds(1);
                            }
                            else if (response.Result.Content.ToLower() == "s" || response.Result.Content.ToLower() == "down")
                            {
                                var newPosition = position + puzzle.Splitter;
                                if (newPosition <= -1 || newPosition >= puzzleConstructed.Count - 1)
                                {
                                    var embed3 = new DiscordEmbedBuilder();
                                    embed3.Title = "Cannot Move To That Position";
                                    embed3.Color = DiscordColor.Red;
                                    await ctx.Channel.SendMessageAsync(embed: embed3);
                                }
                                else
                                {
                                    puzzleConstructed[newPosition] = " +";
                                    puzzleConstructed[position] = " |";
                                    embed.Description = "```\n";
                                    index = 1;
                                    foreach (var s in puzzleConstructed)
                                    {
                                        if (index % puzzle.Splitter == 0 && index != puzzleConstructed.Count - 1)
                                        {
                                            embed.Description += (s) + "\n";
                                        }
                                        else
                                        {
                                            embed.Description += (s);
                                        }
                                        index++;
                                    }
                                    embed.Description += "```";
                                    dembed = embed.Build();
                                    await msg.ModifyAsync(null, dembed);
                                }
                                await response.Result.DeleteAsync();
                                timeIsUp = Timer(end);
                                cooldown = DateTime.Now + TimeSpan.FromSeconds(1);
                            }
                            else if (response.Result.Content.ToLower() == "a" || response.Result.Content.ToLower() == "left")
                            {
                                var newPosition = position - 1;
                                if (newPosition <= -1 || newPosition >= puzzleConstructed.Count - 1)
                                {
                                    var embed3 = new DiscordEmbedBuilder();
                                    embed3.Title = "Cannot Move To That Position";
                                    embed3.Color = DiscordColor.Red;
                                    await ctx.Channel.SendMessageAsync(embed: embed3);
                                }
                                else
                                {
                                    puzzleConstructed[newPosition] = " +";
                                    puzzleConstructed[position] = " -";
                                    embed.Description = "```\n";
                                    index = 1;
                                    foreach (var s in puzzleConstructed)
                                    {
                                        if (index % puzzle.Splitter == 0 && index != puzzleConstructed.Count - 1)
                                        {
                                            embed.Description += (s) + "\n";
                                        }
                                        else
                                        {
                                            embed.Description += (s);
                                        }
                                        index++;
                                    }
                                    embed.Description += "```";
                                    dembed = embed.Build();
                                    await msg.ModifyAsync(null, dembed);
                                }
                                await response.Result.DeleteAsync();
                                timeIsUp = Timer(end);
                                cooldown = DateTime.Now + TimeSpan.FromSeconds(1);
                            }
                            else if (response.Result.Content.ToLower() == "d" || response.Result.Content.ToLower() == "right")
                            {
                                var newPosition = position + 1;
                                if (newPosition < -1 || newPosition > puzzleConstructed.Count - 2)
                                {
                                    var embed3 = new DiscordEmbedBuilder();
                                    embed3.Title = "Cannot Move To That Position";
                                    embed3.Color = DiscordColor.Red;
                                    await ctx.Channel.SendMessageAsync(embed: embed3);
                                }
                                else
                                {
                                    puzzleConstructed[newPosition] = " +";
                                    puzzleConstructed[position] = " -";
                                    embed.Description = "```\n";
                                    index = 1;
                                    foreach (var s in puzzleConstructed)
                                    {
                                        if (index % puzzle.Splitter == 0 && index != puzzleConstructed.Count - 1)
                                        {
                                            embed.Description += (s) + "\n";
                                        }
                                        else
                                        {
                                            embed.Description += (s);
                                        }
                                        index++;
                                    }
                                    embed.Description += "```";
                                    dembed = embed.Build();
                                    await msg.ModifyAsync(null, dembed);
                                }
                                await response.Result.DeleteAsync();
                                timeIsUp = Timer(end);
                                cooldown = DateTime.Now + TimeSpan.FromSeconds(1);
                            }
                            else
                            {
                                var embed2 = new DiscordEmbedBuilder();
                                embed2.Title = "Invalid input";
                                embed2.Color = DiscordColor.Red;
                                await ctx.Channel.SendMessageAsync(embed: embed2);
                                cooldown = DateTime.Now + TimeSpan.FromSeconds(1);
                            }
                        }
                    }
                    if (puzzleConstructed.Exists(x => x.Contains("X")) != true)
                    {
                        embed.Title = "Puzzle Solved";
                        embed.Color = DiscordColor.Gold;
                        dembed = embed.Build();
                        if (user.GameCooldownIgnore == true || user.IsBanned == true)
                        {
                            var embed2 = new DiscordEmbedBuilder();
                            embed2.Title = "Reward Not Recieved";
                            if(user.IsBanned == true && user.GameCooldownIgnore == false)
                            {
                                embed2.Description = "You are not eligible to revieve rewards from this mini-game because you are banned from using the main feature of this bot.";
                            }
                            else if (user.IsBanned == true && user.GameCooldownIgnore == true)
                            {
                                embed2.Description = "You are not eligible to revieve rewards from this mini-game because you are banned from using the main feature of this bot.";
                                Bot.PlayerDatabase.UpdatePlayer(user);
                            }
                            else
                            {
                                embed2.Description = "You are not eligible to revieve rewards from this mini-game because you are in cooldown ignore mode.";
                                user.DotPuzzleStart = DateTime.Now;
                                user.DotPuzzleCooldown = TimeSpan.FromMinutes(0);
                                user.DotPuzzleEnd = DateTime.Now;
                                Bot.PlayerDatabase.UpdatePlayer(user);
                            }
                            embed2.Color = DiscordColor.Red;
                            await ctx.Channel.SendMessageAsync(embed: embed2);
                        }
                        else
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
                                Bot.PlayerDatabase.UpdatePlayer(user);
                            }
                        }
                        await msg.ModifyAsync(null, dembed);
                    }
                    else
                    {
                        embed.Title = "Puzzle Failed";
                        embed.Color = DiscordColor.Red;
                        dembed = embed.Build();
                        await msg.ModifyAsync(null, dembed);
                    }
                    Bot.PlayerDatabase.UpdatePlayer(user);
                }
                else
                {
                    embed.Title = "Cooldown Still Active";
                    embed.Description = $"{user.GetDotPuzzleCooldown()}";
                    embed.Color = DiscordColor.Red;
                    await ctx.Channel.SendMessageAsync(embed: embed);
                }
            }

        }
        private DiscordEmbedBuilder NewPlayer(CommandContext ctx, DiscordEmbedBuilder embed, ulong id)
        {
            embed.Title = "New User Detected";
            embed.Description = "Here is a flare to start you off. Use g/inventory again to view your inventory. Use g/help to view all the commands. ";
            Bot.PlayerDatabase.AddPlayer(new Player() { ID = Convert.ToInt64(id) });
            return embed;
        }
        private bool Timer(DateTime end)
        {
            if (DateTime.Now > end)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
