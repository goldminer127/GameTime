﻿using System;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity.Extensions;
using System.Collections.Generic;
using GameTime.MinesweeperModels;
using System.Diagnostics;
using GameTime.MultiplayerSessionModels.Enums;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace GameTime.Commands
{
    public class MinesweeperCommands : BaseCommandModule
    {
        [Command("Minesweeper"),Aliases("m", "ms", "mine"), Description("Starts a minesweeper game.")]
        public async Task NewGame(CommandContext ctx)
        {
            MinesweeperBoard board = DifficultySelectorHandler(ctx).Result;
            DiscordMessage message = await ctx.Channel.SendMessageAsync(MakeDisplay(board));
            while (board.Status == 0)
            {
                board = SinglePlayerActionHandler(ctx, message, board).Result;
                if(board.Status != MinesweeperStatus.Exited)
                    message = await message.ModifyAsync(MakeDisplay(board));
            }
            if(board.Status == MinesweeperStatus.Loose)
                await message.ModifyAsync(MakeResultDisplay(board));
            else if(board.Status == MinesweeperStatus.Win)
                await message.ModifyAsync(MakeResultDisplay(board));
        }
        /// <summary>
        /// It is dumb, but it also handles the first tile move. Fix this later, it is 5:40AM and I am too tired to think beyond the ability of an acorn.
        /// </summary>
        private async Task<MinesweeperBoard> DifficultySelectorHandler(CommandContext ctx)
        {
            var options = new List<DiscordSelectComponentOption>
            {
                new DiscordSelectComponentOption("Easy", "ms_easy"),
                new DiscordSelectComponentOption("Medium", "ms_moderate"),
                new DiscordSelectComponentOption("Hard", "ms_hard"),
                new DiscordSelectComponentOption("Suprise Me", "ms_random")
            };
            var component = new DiscordSelectComponent("minesweeper_selector", null, options, false, 1, 1);
            var message = await ctx.Channel.SendMessageAsync(new DiscordMessageBuilder()
            {
                Embed = new DiscordEmbedBuilder()
                {
                    Title = "Minesweeper",
                    Description = "Choose a difficulty"
                }.Build()
            }.AddComponents(component));
            var response = await ctx.Client.GetInteractivity().WaitForSelectAsync(message, ctx.User, "minesweeper_selector", timeoutOverride: TimeSpan.FromSeconds(30));
            if(response.TimedOut)
            {
                await message.DeleteAsync();
                return null;
            }
            else
            {
                await message.DeleteAsync();
                var board = GenerateBoard(ctx, response.Result.Values[0] switch
                {
                    "ms_easy" => Difficulty.Easy,
                    "ms_moderate" => Difficulty.Moderate,
                    "ms_hard" => Difficulty.Hard,
                    _ => Difficulty.Random
                }, out string firstTile);
                return PerformAction(new string[1] { firstTile }, board);
            }
        }
        private MinesweeperBoard GenerateBoard(CommandContext ctx, Difficulty difficulty, out string firstTile)
        {
            if(difficulty != Difficulty.Random)
            {
                firstTile = PreventFirstMine(ctx, 10, 10).Result;
                return new MinesweeperBoard(10, 10, difficulty, firstTile);
            }
            else
            {
                var col = new Random().Next(5, 21);
                var row = new Random().Next(5, 11);
                var ratio = new Random().Next(20, 50) / 100.0;
                var totalMines = (int)(row * col * ratio);
                firstTile = PreventFirstMine(ctx, col, row).Result;
                return new MinesweeperBoard(col, row, totalMines, firstTile);
            }
        }
        private async Task<string> PreventFirstMine(CommandContext ctx, int col, int row)
        {
            var message = await ctx.Channel.SendMessageAsync(MakeDisplay(null, col, row));
            while (true)
            {
                var response = await ctx.Client.GetInteractivity().WaitForMessageAsync(msg => msg.Author.Id == ctx.Member.Id && msg.Channel == ctx.Channel, TimeSpan.FromMinutes(5));
                if (response.TimedOut)
                {
                    await message.ModifyAsync(new DiscordEmbedBuilder()
                    {
                        Title = "Minesweeper",
                        Description = "Game has expired",
                    }.AddField("Map", MakeEmptyBoardDisplay(col, row)).Build());
                    return null;
                }
                else
                {
                    if (response.Result.Content.ToUpper().Contains("FLAG"))
                    {
                        await ctx.Channel.SendMessageAsync($"Must respond with a coord");
                    }
                    else
                    {
                        if (response.Result.Content.ToUpper().Contains("EXIT"))
                            await message.ModifyAsync(new DiscordEmbedBuilder()
                            {
                                Title = "Minesweeper",
                                Description = "Game has been exited",
                            }.AddField("Map", MakeEmptyBoardDisplay(col, row)).WithColor(DiscordColor.Orange).Build());
                       else if (VerifyTile(response.Result.Content))
                            return response.Result.Content;
                    }
                    await response.Result.DeleteAsync();
                }
            }
        }
        private async Task<MinesweeperBoard> SinglePlayerActionHandler(CommandContext ctx, DiscordMessage message, MinesweeperBoard board)
        {
            var response = await ctx.Client.GetInteractivity().WaitForMessageAsync(msg => msg.Author.Id == ctx.Member.Id && msg.Channel == ctx.Channel, TimeSpan.FromMinutes(5));
            if(response.TimedOut)
            {
                board.Status = MinesweeperStatus.Exited;
                await message.ModifyAsync(new DiscordEmbedBuilder()
                {
                    Title = "Minesweeper",
                    Description = "Game has expired",
                }.AddField("Map", board.ToString()).Build());
            }
            else
            {
                var command = response.Result.Content.ToUpper().Replace(" ", "");
                var log = "";
                if (command.Contains("EXIT"))
                {
                    board.Status = MinesweeperStatus.Exited;
                    await message.ModifyAsync(new DiscordEmbedBuilder()
                    {
                        Title = "Minesweeper",
                        Description = "Game has been exited",
                    }.AddField("Map", board.ToString()).WithColor(DiscordColor.Orange).Build());
                }
                else
                {
                    if (command.Contains("FLAG"))
                    {
                        var coords = command.Replace("FLAG", "").Split(',');
                        board = PerformAction(coords, board, true);
                    }
                    else
                    {
                        board = PerformAction(command.Split(','), board);
                    }
                    await response.Result.DeleteAsync();
                    Console.WriteLine(log);
                }
            }
            return board;
        }
        private MinesweeperBoard PerformAction(string[] coords, MinesweeperBoard board, bool isFlag = false)
        {
            for(int i = 0; i < coords.Length; i++)
            {
                var coord = coords[i].ToUpper();
                if (VerifyTile(coord))
                {
                    if (!isFlag)
                        board.RevealTile(coords[i][0], coords[i][1]);
                    else
                        board.FlagTile(coords[i][0], coords[i][1]);
                }
            }
            return board;
        }
        private bool VerifyTile(string coord)
        {
            if (coord[0] < 91 && coord[0] >= 65 && coord[1] >= 48 && coord[1] < 58 && coord.Length == 2)
                return true;
            else
                return false;
        }
        private DiscordEmbed MakeDisplay(MinesweeperBoard board, int length = 0, int width = 0)
        {
            return new DiscordEmbedBuilder()
            {
                Title = "Minesweeper",
                Description = "__Flag every mine__ without setting one off. Don't get boomed!\n" +
                "**How to play**\nTo reveal a tile respond with the coordinate of the tile you want to reveal\n" +
                "(\'Ex: A0\' will reveal tile A0)\n To flag a tile add the word \"flag\" before the tile you want " +
                "to flag\n(\'Ex: flag A0\' will flag tile A0)",
                Color = DiscordColor.Blurple
            }.AddField("Key","#: Hidden tile\n!: Flagged tile\n@: Mine\nNumbers indicate how many mines are " +
            "adjacent to that tile").AddField("Map", (board != null) ? board.ToString() : MakeEmptyBoardDisplay(length, width)).WithFooter("Game will expire after 5 minutes " +
            $"of no action\n" + ((board != null) ? $"Difficulty: {board.Difficulty}" : "")).Build();
        }
        private string MakeEmptyBoardDisplay(int length, int width)
        {
            string result = "``` ";
            for (int i = 0; i < width; i++)
            {
                result += $" {i}";
            }
            result += "\n";
            for (int len = 0; len < length; len++)
            {
                result += $"{char.ConvertFromUtf32(65 + len)}";
                for (int wid = 0; wid < width; wid++)
                {
                    result += " #";
                }
                result += "\n";
            }
            return result += "```";
        }
        private DiscordEmbed MakeResultDisplay(MinesweeperBoard board)
        {
            //Kept as switch because might make time out message
            return board.Status switch {
                MinesweeperStatus.Loose => GenerateLoseQuip(new DiscordEmbedBuilder()
                {
                    Title = "Minesweeper (Lost)",
                    Color = DiscordColor.Red
                }.AddField("Map", board.ToString())).Build(),
                MinesweeperStatus.Win => GenerateWinQuip(new DiscordEmbedBuilder()
                {
                    Title = "Minesweeper (Won)",
                    Description = "You didn't step on a mine? That is boring.",
                    Color = DiscordColor.Green
                }.AddField("Map", board.ToString())).Build(),
            };
        }
        private DiscordEmbedBuilder GenerateLoseQuip(DiscordEmbedBuilder embed)
        {
            return new Random().Next(0, 6) switch
            {
                0 => embed.WithDescription("You hit a mine! You lost your legs and the game. Good thing they were prosthetics so you can go right back in the fray!").WithFooter("Death by lack of legs"),
                1 => embed.WithDescription("You struck gold! Oh nevermind... it was a mine.").WithFooter("Death by being fooled"),
                2 => embed.WithDescription("You struck a mine! Who hired you to do this minesweeping?").WithFooter("Death by skipping sweep day"),
                3 => embed.WithDescription("You sweeped a mine! But in doing so you stood to close to it and now you are in bite size peices.").WithFooter("Death by Claymore Ramsay"),
                4 => embed.WithDescription("One hop this time!").WithFooter("Could not play hopscotch"),
                5 => embed.WithDescription("You hit a mine. Big deal, just walk it off and get back in there!").WithFooter("Death by blood loss"),
                _ => embed.WithDescription("You hit a mine! Congrats you've been mortally wounded and lost the game!").WithFooter("RIP")
            };
        }
        private DiscordEmbedBuilder GenerateWinQuip(DiscordEmbedBuilder embed)
        {
            return new Random().Next(0, 3) switch
            {
                0 => embed.WithDescription("You didn't step on a mine? That's boring.").WithFooter("Won by default"),
                1 => embed.WithDescription("Congrats! You did your job right! Now do it again.").WithFooter("Didn't die, but didn't get a raise"),
                2 => embed.WithDescription("You survived! The way is clear!").WithFooter("Joined the didn't explode club"),
                _ => embed.WithDescription("No explosions? Booooooooo").WithFooter("You cheated")
            };
        }
    }
}
