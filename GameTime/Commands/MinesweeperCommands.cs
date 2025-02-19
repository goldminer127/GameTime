﻿using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using GameTime.Extensions;
using GameTime.MinesweeperModels;
using GameTime.Models;
using System;
using System.Threading.Tasks;
using DSharpPlus.Interactivity.Extensions;

namespace GameTime.Commands
{
    class MinesweeperCommands : BaseCommandModule
    {
        private DiscordEmbedBuilder embed;
        private DiscordEmbed BuiltEmbed;
        [Command("minesweeper")]
        [Description("Admin Command\nCommand Type: Connection Test\nReturns Ping")]
        public async Task MinesweeperCommand(CommandContext ctx)
        {
            embed = new DiscordEmbedBuilder();
            var user = Bot.PlayerDatabase.GetPlayerByID(Convert.ToInt64(ctx.Member.Id));
            if (GeneralFunctions.ValidatePlayer(ctx, user, true))
            {
                if (!user.InMinigame)
                {
                    if ((DateTime.Now - user.MinesweeperStart >= user.MinesweeperCooldown) || user.GameCooldownIgnore == true)
                    {
                        user.InMinigame = true;
                        user.NameOfMinigame = "Minesweeper";
                        Bot.PlayerDatabase.UpdatePlayer(user);
                        await Minesweeper(ctx, user, false);
                        user.MinesweeperStart = DateTime.Now;
                        user.MinesweeperCooldown = TimeSpan.FromMinutes(30);
                        user.MinesweeperEnd = user.MinesweeperStart.Add(user.MinesweeperCooldown);
                        user.InMinigame = false;
                        Bot.PlayerDatabase.UpdatePlayer(user);
                    }
                    else
                    {
                        embed.Title = "Cooldown Still Active";
                        embed.Description = $"{user.GetMinesweeperCooldown()}";
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
        public async Task Minesweeper(CommandContext ctx, Player user, bool isGauntlet = false, MineDifficulty difficulty = MineDifficulty.Easy, int stage = 0)
        {
            var interactivity = ctx.Client.GetInteractivity();
            embed = new DiscordEmbedBuilder();
            MinesweeperBoard board = null;
            var puzzleFinished = false;
            var failed = false;
            var timedOut = false;
            embed.AddField("Controls:", "Choose a square by entering a letter followed by a number.\nInputs can be sent in a comma separated list.\nExample: A2 or A2,C4\nFlag A2 or Flag A2,B4");
            if (!isGauntlet)
            {
                embed.Title = "Minesweeper";
                int rNum = new Random().Next(1, 3);
                difficulty = rNum switch
                {
                    1 => MineDifficulty.Easy,
                    2 => MineDifficulty.Medium,
                    _ => MineDifficulty.Hard
                };
                board = new MinesweeperBoard(difficulty);
                var rewardDisplay = difficulty switch
                {
                    MineDifficulty.Easy => "Common Crate x3",
                    MineDifficulty.Medium => "Uncommon Crate x2",
                    _ => "Rare Crate x2",
                };
                embed.AddField("Reward:", $"{rewardDisplay}");
            }
            else
            {
                embed.Title = $"Minesweeper (Gauntlet stage: {stage})";
                board = new MinesweeperBoard(difficulty);
            }
            var display = MakeDisplay(board);
            embed.Description = $"```\n{display}\n```";
            embed.Color = DiscordColor.Blurple;
            embed.WithFooter($"Board will time out after 2 minutes of no actions.\nDifficulty: {board.Difficulty}");
            BuiltEmbed = embed.Build();
            var message = await ctx.RespondAsync(embed: BuiltEmbed);
            DiscordMessage wrongInputMessage = null;
            int totalMoves = 0;
            while (puzzleFinished != true)
            {
                var response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(120));
                if (response.TimedOut)
                {
                    timedOut = true;
                    break;
                }
                else
                {
                    var rawInput = response.Result.Content.ToLower();
                    if((rawInput.Length > 8 && rawInput.Contains(",") == false && !rawInput.Contains("unflag") && rawInput.Contains("flag") == true) || (rawInput.Length > 10 && rawInput.Contains(",") == false && rawInput.Contains("unflag") == true))
                    {
                        wrongInputMessage = await ctx.Channel.SendMessageAsync("A list of inputs must be in a comma separated list.");
                    }
                    else if (rawInput.Length > 2 && rawInput.Contains(",") == false && rawInput.Contains("flag") == false)
                    {
                        wrongInputMessage = await ctx.Channel.SendMessageAsync("A list of inputs must be in a comma separated list.");
                    }
                    else
                    {
                        //Response processing
                        string[] moves = null;
                        if (response.Result.Content.ToLower().Contains("flag"))//change to rawinput
                        {
                            var commandRemovedArray = rawInput.Split("flag");
                            var spaceRemovedArray = commandRemovedArray[1].Split(" ");
                            var spaceRemoved = "";
                            foreach (string s in spaceRemovedArray)
                            {
                                spaceRemoved += s;
                            }
                            moves = spaceRemoved.Split(",");
                        }
                        else
                        {
                            var spaceRemovedArray = rawInput.Split(" ");
                            var spaceRemoved = "";
                            foreach (string s in spaceRemovedArray)
                            {
                                spaceRemoved += s;
                            }
                            moves = spaceRemoved.Split(",");
                        }
                        //Finished response processing
                        foreach (string move in moves)
                        {
                            board = move[0] switch
                            {
                                'a' => PerformMove(board, ctx, rawInput, move, 0),
                                'b' => PerformMove(board, ctx, rawInput, move, 1),
                                'c' => PerformMove(board, ctx, rawInput, move, 2),
                                'd' => PerformMove(board, ctx, rawInput, move, 3),
                                'e' => PerformMove(board, ctx, rawInput, move, 4),
                                'f' => PerformMove(board, ctx, rawInput, move, 5),
                                'g' => PerformMove(board, ctx, rawInput, move, 6),
                                'h' => PerformMove(board, ctx, rawInput, move, 7),
                                'i' => PerformMove(board, ctx, rawInput, move, 8),
                                'j' => PerformMove(board, ctx, rawInput, move, 9),
                                _ => board
                            };
                        }
                        display = MakeDisplay(board);
                    }
                    //Decides if puzzle is finished or failed
                    puzzleFinished = true;
                    for (int row = 0; row < board.Board.GetLength(0); row++)
                    {
                        for (int col = 0; col < board.Board.GetLength(1); col++)
                        {
                            if (board.Board[row, col].IsMine && board.Board[row, col].IsRevealed)
                            {
                                puzzleFinished = true;
                                failed = true;
                                break;
                            }
                            else if (!board.Board[row, col].IsRevealed && !board.Board[row, col].IsMine)
                            {
                                puzzleFinished = false;
                            }
                        }
                        if (failed)
                            break;
                    }
                    embed.Description = $"```\n{display}\n```";
                    BuiltEmbed = embed.Build();
                    message = await message.ModifyAsync(embed: BuiltEmbed);
                    try
                    {
                        await response.Result.DeleteAsync();
                    }
                    catch
                    {

                    }
                    if(wrongInputMessage != null)
                    {
                        await wrongInputMessage.DeleteAsync();
                        wrongInputMessage = null;
                    }
                }
            }
            if (timedOut)
            {
                embed.Color = DiscordColor.Red;
                embed.Title = "Minesweeper (Timed Out)";
                BuiltEmbed = embed.Build();
            }
            else if (failed)
            {
                if (isGauntlet == true)
                {
                    GauntletCommands.PuzzleStatus = false;
                }
                else if (totalMoves == 1 && user.GameCooldownIgnore == false)
                {
                    user.MinesweeperStart = DateTime.Now;
                    user.MinesweeperCooldown = TimeSpan.FromMinutes(0);
                    user.MinesweeperEnd = DateTime.Now;
                    embed.ClearFields();
                    embed.AddField("Cooldown Ignored", "You were given another chance because you hit a mine on your first move.");
                    Bot.PlayerDatabase.UpdatePlayer(user);
                }
                embed.Title = "Minesweeper (Game Failed)";
                embed.Color = DiscordColor.Red;
                display = "";
                for (int row = 0; row < board.Board.GetLength(0); row++)
                {
                    display += row switch
                    {
                        0 => "A",
                        1 => "B",
                        2 => "C",
                        3 => "D",
                        4 => "E",
                        5 => "F",
                        6 => "G",
                        7 => "H",
                        8 => "I",
                        _ => "J",
                    };
                    for (int col = 0; col < board.Board.GetLength(1); col++)
                    {
                        if (board.Board[row, col].IsRevealed == false && !board.Board[row, col].IsMine)
                        {
                            display += $" ‏‏‎#";
                        }
                        else if (!board.Board[row, col].IsMine)
                        {
                            if (board.Board[row, col].TotalMines == 0)
                                display += " ‏‏‎ ‏‏‎";
                            else
                                display += $" ‏‏‎{board.Board[row, col].TotalMines}";
                        }
                        else
                        {
                            display += $" ‏‏‎@";
                        }
                    }
                    display += " ‏‏‎|\n";
                }
                embed.Description = $"```\n{display}\n```";
                BuiltEmbed = embed.Build();
            }
            else
            {
                if (isGauntlet == true)
                {
                    await message.DeleteAsync();
                    message = null;
                    GauntletCommands.PuzzleStatus = true;
                }
                else
                {
                    embed.Title = "Minesweeper (Game Win)";
                    embed.Color = DiscordColor.Green;
                    BuiltEmbed = embed.Build();
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
                        var crate = Crate.GetMinesweeperReward(board.Difficulty);
                        switch (board.Difficulty)
                        {
                            case MineDifficulty.Easy:
                                GeneralFunctions.AddToInventory(user, crate, 3);
                                break;
                            case MineDifficulty.Medium:
                                GeneralFunctions.AddToInventory(user, crate, 2);
                                break;
                            case MineDifficulty.Hard:
                                GeneralFunctions.AddToInventory(user, crate, 2);
                                break;
                        }
                    }
                }
            }
            try { message = await message.ModifyAsync(embed: BuiltEmbed); }
            catch { }
            Bot.PlayerDatabase.UpdatePlayer(user);        
        }
        private string MakeDisplay(MinesweeperBoard board)
        {
            var display = "  0 1 2 3 4 5 6 7 8 9\n";
            for (int row = 0; row < board.Board.GetLength(0); row++)
            {
                display += row switch
                {
                    0 => "A",
                    1 => "B",
                    2 => "C",
                    3 => "D",
                    4 => "E",
                    5 => "F",
                    6 => "G",
                    7 => "H",
                    8 => "I",
                    _ => "J",
                };
                for (int col = 0; col < board.Board.GetLength(1); col++)
                {
                    if (board.Board[row, col].IsRevealed == false && !board.Board[row, col].Flagged)
                    {
                        display += $" ‏‏‎#";
                    }
                    else if (board.Board[row, col].IsRevealed == false && board.Board[row, col].Flagged)
                    {
                        display += $" ‏‏‎X";
                    }
                    else if (!board.Board[row, col].IsMine)
                    {
                        if (board.Board[row, col].TotalMines == 0)
                            display += " ‏‏‎ ‏‏‎";
                        else
                            display += $" ‏‏‎{board.Board[row, col].TotalMines}";
                    }
                    else
                    {
                        display += $" ‏‏‎@";
                    }
                }
                display += " ‏‏‎|\n";
            }
            return display;
        }
        private MinesweeperBoard RevealEmpty(MinesweeperBoard board, int x, int y)
        {
            for(int row = -1; row < 2; row++)
            {
                for(int col = -1; col < 2; col++)
                {
                    try
                    {
                        board.Board[x + row, y + col].IsRevealed = true;
                    }
                    catch { }
                }
            }
            for (int row = -1; row < 2; row++)
            {
                for (int col = -1; col < 2; col++)
                {
                    try
                    {
                        if ((board.Board[x + row, y + col].TotalMines == 0) && !ScanAdjacent(board, x + row, y + col))
                        {
                            board = RevealEmpty(board, x + row, y + col);
                        }
                    }
                    catch { }
                }
            }
            return board;
        }
        private MinesweeperBoard Chording(MinesweeperBoard board, int x, int y)
        {
            var totalFlags = 0;
            var totalCorrectFlagged = 0;
            for (int row = -1; row < 2; row++)
            {
                for (int col = -1; col < 2; col++)
                {
                    try
                    {
                        if (board.Board[x + row, y + col].Flagged && board.Board[x + row, y + col].IsMine)
                        {
                            totalCorrectFlagged++;
                        }
                        if (board.Board[x + row, y + col].Flagged)
                            totalFlags++;
                    }
                    catch { }
                }
            }
            if (totalCorrectFlagged == totalFlags)
            {
                for (int row = -1; row < 2; row++)
                {
                    for (int col = -1; col < 2; col++)
                    {
                        try
                        {
                            if (!board.Board[x + row, y + col].IsMine)
                            {
                                board.Board[x + row, y + col].IsRevealed = true;
                                if (board.Board[x + row, y + col].TotalMines == 0)
                                    RevealEmpty(board, x + row, y + col);
                            }
                        }
                        catch { }
                    }
                }
            }
            return board;
        }
        private bool ScanAdjacent(MinesweeperBoard board, int x, int y)
        {
            for (int row = -1; row < 2; row++)
            {
                for (int col = -1; col < 2; col++)
                {
                    try
                    {
                        if (board.Board[x + row, y + col].IsRevealed == false)
                        {
                            return false;
                        }
                    }
                    catch { }
                }
            }
            return true;
        }
        private MinesweeperBoard PerformMove(MinesweeperBoard board, CommandContext ctx, string rawInput, string move, int xPosition)
        {
            var yPosition = 0;
            if (rawInput.Contains("unflag"))
            {
                try
                {
                    yPosition = Int32.Parse(move[1].ToString());
                    board.Board[xPosition, yPosition].Flagged = false;
                }
                catch
                {
                    var msg = ctx.Channel.SendMessageAsync("An input is invalid, make sure you input a letter and a number.");
                    Task.Delay(5000);
                    msg.Result.DeleteAsync();
                }
            }
            else if (rawInput.Contains("flag"))
            {
                try
                {
                    yPosition = Int32.Parse(move[1].ToString());
                    board.Board[xPosition, yPosition].Flagged = true;
                }
                catch
                {
                    var msg = ctx.Channel.SendMessageAsync("An input is invalid, make sure you input a letter and a number.");
                    Task.Delay(5000);
                    msg.Result.DeleteAsync();
                }
            }
            else
            {
                try
                {
                    yPosition = Int32.Parse(move[1].ToString());
                    if (board.Board[xPosition, yPosition].IsRevealed)
                        Chording(board, xPosition, yPosition);
                    else
                        board.Board[xPosition, yPosition].IsRevealed = true;
                    if (board.Board[xPosition, yPosition].TotalMines == 0)
                    {
                        board = RevealEmpty(board, xPosition, yPosition);
                    }
                }
                catch
                {
                    var msg = ctx.Channel.SendMessageAsync("An input is invalid, make sure you input a letter and a number.");
                    Task.Delay(5000);
                    msg.Result.DeleteAsync();
                }
            }
            return board;
        }
    }
}
