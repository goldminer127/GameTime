using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using GameTime.MinesweeperModels;
using GameTime.DotPuzzleModels;
using GameTime.ScramblerModels;
using GameTime.Extensions;
using GameTime.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using DSharpPlus.Interactivity;
using System.Threading.Tasks;
using DSharpPlus.Interactivity.Extensions;

namespace GameTime.Commands
{
    class GauntletCommands : BaseCommandModule
    {
        private DiscordEmbedBuilder embed;
        public static bool PuzzleStatus { get; set; }
        [Command("gauntlet")]
        [Description("")]
        public async Task Gauntlet(CommandContext ctx)
        {
            var interactivity = ctx.Client.GetInteractivity();
            embed = new DiscordEmbedBuilder();
            var MoveToDotCommands = new MoveToDotCommands();
            var user = Bot.PlayerDatabase.GetPlayerByID(Convert.ToInt64(ctx.Member.Id));
            if (user == null)
            {
                embed = Bot.PlayerDatabase.NewPlayer(ctx, embed, ctx.Member.Id);
                embed.Color = DiscordColor.Blurple;
            }
            else
            {
                Item userKey = null;
                var totalKeys = 0;
                foreach(var item in user.Inventory)
                {
                    if (item.ID == 113)
                    {
                        userKey = item;
                        totalKeys = item.Multiple;
                        break;
                    }
                }
                PuzzleStatus = true;
                embed.Title = "Gauntlet List";
                embed.Description = "Choose a gauntlet by responding with the gauntlet name or number";
                embed.AddField("1. Novice Gauntlet", "5 Puzzles Total\n4x Connectx\n1x Minesweeper\n\nReward: 12 Uncommon Crates");
                embed.AddField("2. Expert Gauntlet", "7 Puzzles Total\n5x Connectx\n1x Minesweeper\n1x Word Scrambler\n\nReward: 6 Epic Crates");
                embed.WithFooter($"{user.Name}'s total keys {totalKeys}");
                var message = await ctx.Channel.SendMessageAsync(embed: embed);
                var response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(120));
                if(response.TimedOut)
                {
                    
                }
                else if (userKey == null)
                {
                    await message.DeleteAsync();
                    embed.ClearFields();
                    embed.Title = "Unable to access gauntlet";
                    embed.Description = "You have no gauntlet key.";
                    embed.WithFooter(" ");
                }
                else if(response.Result.Content.ToLower() == "novice gauntlet" || response.Result.Content.ToLower() == "1")
                {
                    GeneralFunctions.RemoveFromInventory(user, userKey, 1);
                    await message.DeleteAsync();
                    await NoviceGauntlet(ctx, user);
                }
                else if (response.Result.Content.ToLower() == "expert gauntlet" || response.Result.Content.ToLower() == "2")
                {
                    GeneralFunctions.RemoveFromInventory(user, userKey, 1);
                    await message.DeleteAsync();
                    await ExpertGauntlet(ctx, user);
                }
                else
                {

                }
            }
            await ctx.Channel.SendMessageAsync(embed: embed);
        }
        private async Task ExpertGauntlet(CommandContext ctx, Player user)
        {
            embed = new DiscordEmbedBuilder();
            var completed = false;
            var stage = 0;
            while(PuzzleStatus && !completed)
            {
                stage++;
                switch(stage)
                {
                    case 1:
                        await GauntletScrambler(ctx, user, Difficulty.Easy, stage);
                        break;
                    case 2:
                        await GauntletConnectx(ctx, user, DotDifficulty.Medium, stage);
                        break;
                    case 3:
                        await GauntletMinesweeper(ctx, user, MineDifficulty.Medium, stage);
                        break;
                    case 4:
                        await GauntletConnectx(ctx, user, DotDifficulty.Medium, stage);
                        break;
                    case 5:
                        await GauntletConnectx(ctx, user, DotDifficulty.Medium, stage);
                        break;
                    case 6:
                        await GauntletMinesweeper(ctx, user, MineDifficulty.Hard, stage);
                        break;
                    case 7:
                        await GauntletConnectx(ctx, user, DotDifficulty.Hard, stage);
                        completed = true;
                        break;
                };
            }
            if(PuzzleStatus == false)
            {
                embed.Title = ("Gauntlet Failed");
                embed.Description = $"You got to stage {stage}";
            }
            else
            {
                embed.Title = ("Gauntlet Passed!");
                embed.Description = $"You got 6 epic crates!";
                GeneralFunctions.AddToInventory(user, Crate.GetGauntletReward("expert"), 6);
            }
            await ctx.Channel.SendMessageAsync(embed: embed);
            Bot.PlayerDatabase.UpdatePlayer(user);
        }
        private async Task NoviceGauntlet(CommandContext ctx, Player user)
        {
            embed = new DiscordEmbedBuilder();
            var completed = false;
            var stage = 0;
            while (PuzzleStatus && !completed)
            {
                stage++;
                switch (stage)
                {
                    case 1:
                        await GauntletConnectx(ctx, user, DotDifficulty.Easy, stage);
                        break;
                    case 2:
                        await GauntletMinesweeper(ctx, user, MineDifficulty.Easy, stage);
                        break;
                    case 3:
                        await GauntletMinesweeper(ctx, user, MineDifficulty.Medium, stage);
                        break;
                    case 4:
                        await GauntletConnectx(ctx, user, DotDifficulty.Medium, stage);
                        break;
                    case 5:
                        await GauntletConnectx(ctx, user, DotDifficulty.Hard, stage);
                        completed = true;
                        break;
                }
            }
            if (PuzzleStatus == false)
            {
                embed.Title = ("Gauntlet Failed");
                embed.Description = $"You got to stage {stage}";
            }
            else
            {
                embed.Title = ("Gauntlet Passed");
                embed.Description = $"You got 12 uncommon crates!";
                GeneralFunctions.AddToInventory(user, Crate.GetGauntletReward( "novice"), 12);
            }
            await ctx.Channel.SendMessageAsync(embed: embed);
            Bot.PlayerDatabase.UpdatePlayer(user);
        }
        private async Task GauntletConnectx(CommandContext ctx, Player user, DotDifficulty difficulty, int stage)
        {
            MoveToDotCommands connectx = new MoveToDotCommands();
            await connectx.DotPuzzle(ctx, user, true, difficulty, stage);
        }
        private async Task GauntletMinesweeper(CommandContext ctx, Player user, MineDifficulty difficulty, int stage)
        {
            MinesweeperCommands minesweeper = new MinesweeperCommands();
            await minesweeper.Minesweeper(ctx, user, true, difficulty, stage);
        }
        private async Task GauntletScrambler(CommandContext ctx, Player user, Difficulty difficulty, int stage)
        {
            WordScramblerCommands scrambler = new WordScramblerCommands();
            await scrambler.Scrambler(ctx, user, true, difficulty, stage);
        }
    }
}
