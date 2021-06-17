using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using GameTime.Models;
using System.Threading.Tasks;
using System;
using GameTime.Extensions;
using GameTime.DotPuzzleModels;
using DSharpPlus.Interactivity.Extensions;

namespace GameTime.Commands
{
    public class MoveToDotCommands : BaseCommandModule
    {
        [Command("connectx"), Aliases("cx", "connect", "dotpuzzle", "dp")]
        [Description("Play a game of ConnectX.")]
        public async Task Connectx(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder();
            var user = Bot.PlayerDatabase.GetPlayerByID(Convert.ToInt64(ctx.Member.Id));
            if (user == null)
            {
                embed = Bot.PlayerDatabase.NewPlayer(ctx, embed, ctx.Member.Id);
                embed.Color = DiscordColor.Blurple;
                await ctx.Channel.SendMessageAsync(embed: embed);
            }
            else
            {
                if (!user.InMinigame)
                {
                    if ((DateTime.Now - user.DotPuzzleStart >= user.DotPuzzleCooldown) || user.GameCooldownIgnore == true)
                    {
                        user.NameOfMinigame = "Connectx";
                        user.InMinigame = true;
                        Bot.PlayerDatabase.UpdatePlayer(user);
                        await DotPuzzle(ctx, user, false);
                        user.InMinigame = false;
                        user.DotPuzzleStart = DateTime.Now;
                        user.DotPuzzleCooldown = TimeSpan.FromMinutes(30);
                        user.DotPuzzleEnd = user.DotPuzzleStart.Add(user.DotPuzzleCooldown);
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
                else
                {
                    await ctx.Channel.SendMessageAsync($"You are still in {user.NameOfMinigame}");
                }
            }
        }
        public async Task DotPuzzle(CommandContext ctx, Player user, bool isGauntlet, DotDifficulty difficulty = DotDifficulty.Easy, int stage = 0)
        {
            var embed = new DiscordEmbedBuilder();
            if (((DateTime.Now - user.DotPuzzleStart >= user.DotPuzzleCooldown) || user.GameCooldownIgnore == true) || isGauntlet)
            {
                var interactivity = ctx.Client.GetInteractivity();
                var puzzle = Bot.DotPuzzles.AllPuzzles[new Random().Next(0, Bot.DotPuzzles.AllPuzzles.Count)];
                if (isGauntlet)
                {
                    if (difficulty == DotDifficulty.Easy)
                    {
                        while (puzzle.DotDifficulty != DotDifficulty.Easy)
                        {
                            puzzle = Bot.DotPuzzles.AllPuzzles[new Random().Next(0, Bot.DotPuzzles.AllPuzzles.Count)];
                        }
                    }
                    else if (difficulty == DotDifficulty.Medium)
                    {
                        while (puzzle.DotDifficulty != DotDifficulty.Medium)
                        {
                            puzzle = Bot.DotPuzzles.AllPuzzles[new Random().Next(0, Bot.DotPuzzles.AllPuzzles.Count)];
                        }
                    }
                    else if (difficulty == DotDifficulty.Hard)
                    {
                        while (puzzle.DotDifficulty != DotDifficulty.Hard)
                        {
                            puzzle = Bot.DotPuzzles.AllPuzzles[new Random().Next(0, Bot.DotPuzzles.AllPuzzles.Count)];
                        }
                    }
                    else if (difficulty == DotDifficulty.Extreme)
                    {
                        while (puzzle.DotDifficulty != DotDifficulty.Extreme)
                        {
                            puzzle = Bot.DotPuzzles.AllPuzzles[new Random().Next(0, Bot.DotPuzzles.AllPuzzles.Count)];
                        }
                    }
                    embed.Title = $"Connectx (Gauntlet stage: {stage})";
                }
                else
                {
                    embed.Title = "Connectx";
                }
                var puzzleConstructed = Bot.DotPuzzles.ConstructPuzzle(puzzle.Puzzle);
                var end = DateTime.Now + puzzle.TimeLimit;
                var timer = false;
                var index = 1;
                var multiple = 1;
                var numMoves = puzzle.Moves;
                var crate = Crate.GetDotPuzzleReward(puzzle);
                if (puzzle.DotDifficulty == DotDifficulty.Hard)
                {
                    multiple = 2;
                }
                embed.Color = DiscordColor.Blurple;
                embed.AddField("Controls:", $"Up = w Down = s Left = a Right = d\nClear all the X's. You have {Convert.ToInt32(puzzle.TimeLimit.TotalSeconds)} seconds to complete this puzzle.");
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
                embed.Description += $"Moves left: {numMoves}";
                var dembed = embed.Build();
                var msg = await ctx.RespondAsync(embed: dembed);
                var cooldown = DateTime.Now + TimeSpan.FromSeconds(1);
                while (timer != true && numMoves > 0)
                {
                    if (puzzleConstructed.Exists(x => x.Contains("X")) != true)
                    {
                        break;
                    }
                    else
                    {
                        var response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(.01));
                        if (response.TimedOut)
                        {
                            timer = Timer(end);
                        }
                        else if (Timer(cooldown) == false)
                        {
                            var embed2 = new DiscordEmbedBuilder();
                            embed2.Description = "Slow down, there is a 1 second cooldown between moves!";
                            embed2.Color = DiscordColor.Red;
                            await ctx.Channel.SendMessageAsync(embed: embed2);
                            try
                            {
                                await response.Result.DeleteAsync();
                            }
                            catch
                            {

                            }
                        }
                        else
                        {
                            var rawmessage = response.Result.Content.ToLower();
                            var moves = rawmessage.Split(" ");
                            foreach (var move in moves)
                            {
                                var position = puzzleConstructed.FindIndex(x => x.Contains("+"));
                                if (numMoves <= 0)
                                {
                                    break;
                                }
                                else if (move.ToLower() == "w" || move.ToLower() == "up")
                                {
                                    var newPosition = position - puzzle.Splitter;
                                    if (newPosition + 1 <= 0)
                                    {
                                        var embed3 = new DiscordEmbedBuilder();
                                        embed3.Title = "Cannot Move To That Position";
                                        embed3.Color = DiscordColor.Red;
                                        var invalidPosMessage = await ctx.Channel.SendMessageAsync(embed: embed3);
                                        GeneralFunctions.DelayMessageDeletion(invalidPosMessage, 3000);
                                        break;
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
                                        numMoves--;
                                    }
                                    embed.Description += $"Moves left: {numMoves}";
                                    timer = Timer(end);
                                    cooldown = DateTime.Now + TimeSpan.FromSeconds(1);
                                }
                                else if (move.ToLower() == "s" || move.ToLower() == "down")
                                {
                                    var newPosition = position + puzzle.Splitter;
                                    if (newPosition + 1 > puzzleConstructed.Count - 1)
                                    {
                                        var embed3 = new DiscordEmbedBuilder();
                                        embed3.Title = "Cannot Move To That Position";
                                        embed3.Color = DiscordColor.Red;
                                        var invalidPosMessage = await ctx.Channel.SendMessageAsync(embed: embed3);
                                        GeneralFunctions.DelayMessageDeletion(invalidPosMessage, 3000);
                                        break;
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
                                        numMoves--;
                                    }
                                    embed.Description += $"Moves left: {numMoves}";
                                    timer = Timer(end);
                                    cooldown = DateTime.Now + TimeSpan.FromSeconds(1);
                                }
                                else if (move.ToLower() == "a" || move.ToLower() == "left")
                                {
                                    var newPosition = position - 1;
                                    if (((newPosition + 1) % puzzle.Splitter == 0) || newPosition < -1)
                                    {
                                        var embed3 = new DiscordEmbedBuilder();
                                        embed3.Title = "Cannot Move To That Position";
                                        embed3.Color = DiscordColor.Red;
                                        var invalidPosMessage = await ctx.Channel.SendMessageAsync(embed: embed3);
                                        GeneralFunctions.DelayMessageDeletion(invalidPosMessage, 3000);
                                        break;
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
                                        numMoves--;
                                    }
                                    embed.Description += $"Moves left: {numMoves}";
                                    timer = Timer(end);
                                    cooldown = DateTime.Now + TimeSpan.FromSeconds(1);
                                }
                                else if (move.ToLower() == "d" || move.ToLower() == "right")
                                {
                                    var newPosition = position + 1;
                                    if (((newPosition + 1) % puzzle.Splitter == 1) || newPosition > puzzleConstructed.Count - 1)
                                    {
                                        var embed3 = new DiscordEmbedBuilder();
                                        embed3.Title = "Cannot Move To That Position";
                                        embed3.Color = DiscordColor.Red;
                                        var invalidPosMessage = await ctx.Channel.SendMessageAsync(embed: embed3);
                                        GeneralFunctions.DelayMessageDeletion(invalidPosMessage, 3000);
                                        break;
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
                                        numMoves--;
                                    }
                                    embed.Description += $"Moves left: {numMoves}";
                                    timer = Timer(end);
                                    cooldown = DateTime.Now + TimeSpan.FromSeconds(1);
                                }
                                else
                                {
                                    var embed2 = new DiscordEmbedBuilder();
                                    embed2.Title = "Invalid input";
                                    embed2.Color = DiscordColor.Red;
                                    var message = await ctx.Channel.SendMessageAsync(embed: embed2);
                                    cooldown = DateTime.Now + TimeSpan.FromSeconds(1);
                                    GeneralFunctions.DelayMessageDeletion(message, 3000);
                                    break;
                                }
                            }
                            dembed = embed.Build();
                            await msg.ModifyAsync(null, dembed);
                            try
                            {
                                await response.Result.DeleteAsync();
                            }
                            catch
                            {

                            }
                        }
                    }
                }
                if (puzzleConstructed.Exists(x => x.Contains("X")) != true)
                {
                    if (isGauntlet == true)
                    {
                        await msg.DeleteAsync();
                        GauntletCommands.PuzzleStatus = true;
                    }
                    else
                    {
                        embed.Title = "Connectx (Win)";
                        embed.Color = DiscordColor.Gold;
                        dembed = embed.Build();
                        if (user.GameCooldownIgnore == true || user.IsBanned == true)
                        {
                            var embed2 = new DiscordEmbedBuilder();
                            embed2.Title = "Reward Not Recieved";
                            if (user.IsBanned == true)
                            {
                                embed2.Description = "You are not eligible to revieve rewards from this mini-game because you are banned from using the main feature of this bot.";
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
                }
                else
                {
                    if (isGauntlet == true)
                    {
                        GauntletCommands.PuzzleStatus = false;
                    }
                    embed.Title = "Connectx (Failed)";
                    embed.Color = DiscordColor.Red;
                    dembed = embed.Build();
                    await msg.ModifyAsync(null, dembed);
                }
            }
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
