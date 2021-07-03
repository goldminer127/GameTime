using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using GameTime.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus.Interactivity.Extensions;

namespace GameTime.Commands
{
    class CasinoGames : BaseCommandModule
    {
        private DiscordEmbed builtEmbed;
        [Command("casino")]
        [Description("Enter the GameTime casino")]
        public async Task Casino(CommandContext ctx)
        {
            var embed = new DiscordEmbedBuilder();
            var user = Bot.PlayerDatabase.GetPlayerByID(Convert.ToInt64(ctx.Member.Id));
            if (GeneralFunctions.ValidatePlayer(ctx, user, true))
            {
                GeneralFunctions.UpdatePlayerDisplayInfo(ctx, user);
                if (!user.InMinigame)
                {
                    user.InMinigame = true;
                    user.NameOfMinigame = "Casino";
                    Bot.PlayerDatabase.UpdatePlayer(user);
                    var interactivity = ctx.Client.GetInteractivity();
                    var exit = false;
                    while (!exit)
                    {
                        embed.ClearFields();
                        embed.Title = "GameTime Casino";
                        embed.Description = $"Welcome to GameTime Casino! Choose a game by __responding with the name__ of the game you want to play! **Do not use g/ for casino commands.**";
                        embed.AddField("Balance", $"{user.Credits.ToString("###,###,###,###,###,##0")} credits");
                        embed.AddField("Shop", "Convert cash and credits.", false);
                        embed.AddField("Blackjack", "Get close to 21 to win!", true);
                        embed.AddField("Slots", "Get certain symbol combinations!", true);
                        embed.AddField("HighLow", "Guess if the number generated is higher or lower than the displayed number!", true);
                        embed.AddField("Coinflip", "Guess which side the coin will land", true);
                        embed.WithFooter("Respond with exit to leave the casino");
                        embed.Color = DiscordColor.Blurple;
                        var displayMessage = ctx.Channel.SendMessageAsync(embed: embed);
                        var response = await interactivity.WaitForMessageAsync(x => (x.Channel == ctx.Channel && x.Author == ctx.Member) && (x.Content.ToLower() == "shop"|| x.Content.ToLower() == "exit" || x.Content.ToLower() == "blackjack"|| x.Content.ToLower() == "slot"|| x.Content.ToLower() == "slots" || x.Content.ToLower() == "highlow"|| x.Content.ToLower() == "coinflip"), TimeSpan.FromSeconds(60));
                        if (response.TimedOut) //First If 
                        {
                            await displayMessage.Result.DeleteAsync();
                            await ctx.Channel.SendMessageAsync($"Casino exited");
                            exit = true;
                        }
                        else if (response.Result.Content.ToLower() == "blackjack")  //Second If (BlackJack)
                        {
                            const long MAX_BLACKJACK_BET = 500;
                            try
                            {
                                await response.Result.DeleteAsync();
                            }
                            catch
                            {

                            }
                            long bet = 0;
                            var cancel = false;
                            var hasCredits = false;
                            DiscordMessage message1 = null;
                            embed.ClearFields();
                            embed.Title = "Blackjack";
                            embed.Description = $"Welcome to blackjack! In order to win you need to either hit 21 or get as close to 21 as possible. Get over 21 and you've lost!";
                            embed.AddField("How much will you bet?", $"Respond with the amount of credits you would like to bet.\nMax bet is {MAX_BLACKJACK_BET} credits", true);
                            embed.WithFooter("Respond with exit to exit blackjack");
                            embed.AddField("Balance", $"{user.Credits.ToString("###,###,###,###,###,##0")} credits", true);
                            embed.WithFooter("Respond with exit to exit BlackJack.");
                            embed.Color = DiscordColor.Blurple;
                            await displayMessage.Result.DeleteAsync();
                            var displayMessage1 = await ctx.Channel.SendMessageAsync(embed: embed);
                            while (!cancel && !hasCredits)
                            {
                                var responseJ = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(60));
                                if (responseJ.TimedOut)
                                {
                                    await ctx.Channel.SendMessageAsync($"Casino exited");
                                    user.InMinigame = false;
                                    Bot.PlayerDatabase.UpdatePlayer(user);
                                    exit = true;
                                    cancel = true;
                                }
                                else if (responseJ.Result.Content.ToLower() == "exit")
                                {
                                    await ctx.Channel.SendMessageAsync($"Exited Blackjack");
                                    cancel = true;
                                }
                                else
                                {
                                    try
                                    {
                                        await message1.DeleteAsync();
                                    }
                                    catch { }
                                    try
                                    {
                                        bet = Int64.Parse(responseJ.Result.Content);
                                        if (bet <= MAX_BLACKJACK_BET)
                                        {
                                            if (bet > user.Credits)
                                            {
                                                message1 = await ctx.Channel.SendMessageAsync($"You don't have {bet} credits.");
                                            }
                                            else
                                            {
                                                hasCredits = true;
                                            }
                                        }
                                        else
                                        {
                                            message1 = await ctx.Channel.SendMessageAsync($"The max bet for blackjack is {MAX_BLACKJACK_BET} credits.");
                                        }
                                        await responseJ.Result.DeleteAsync();
                                    }
                                    catch
                                    {

                                    }
                                }
                            }
                            await displayMessage1.DeleteAsync();
                            if (!cancel)
                            {
                                var playAgain = true;
                                while (playAgain)
                                {
                                    await BlackJack(ctx, user, bet);
                                    var messageAG = ctx.Channel.SendMessageAsync($"Play again with a bet of {bet} credits? Yes or no");
                                    var responseAG = await interactivity.WaitForMessageAsync(x => (x.Channel == ctx.Channel && x.Author == ctx.Member) && (x.Content.ToLower() == "yes" || x.Content.ToLower() == "no"), TimeSpan.FromSeconds(60));
                                    if (responseAG.TimedOut)
                                    {
                                        await messageAG.Result.DeleteAsync();
                                        await ctx.Channel.SendMessageAsync($"Casino exited");
                                        exit = true;
                                        playAgain = false;
                                    }
                                    else if (responseAG.Result.Content.ToLower() == "yes")
                                    {
                                        await messageAG.Result.DeleteAsync();
                                    }
                                    else if (responseAG.Result.Content.ToLower() == "no")
                                    {
                                        await messageAG.Result.DeleteAsync();
                                        playAgain = false;
                                    }
                                }
                            }
                        }
                        else if (response.Result.Content.ToLower() == "slots" || response.Result.Content.ToLower() == "slot") //Third If (Slots)
                        {
                            const long MAX_SLOT_BET = 600;
                            try
                            {
                                await response.Result.DeleteAsync();
                            }
                            catch
                            {

                            }
                            long bet = 0;
                            var cancel = false;
                            var hasCredits = false;
                            DiscordMessage message1 = null;
                            embed.ClearFields();
                            embed.Title = "Slots";
                            embed.Description = $"Welcome to the slot machine! Obtain valid symbol combinations to win! ";
                            embed.AddField("How much will you bet?", $"Respond with the amount of credits you would like to bet (your bet will apply to all spins).\nMax bet is {MAX_SLOT_BET} credits", true);
                            embed.WithFooter("Respond with exit to exit slots.");
                            embed.AddField("Balance", $"{user.Credits.ToString("###,###,###,###,###,##0")} credits", true);
                            embed.Color = DiscordColor.Blurple;
                            await displayMessage.Result.DeleteAsync();
                            var displayMessage1 = await ctx.Channel.SendMessageAsync(embed: embed);
                            while (!cancel && !hasCredits)
                            {
                                var responseJ = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(60));
                                if (responseJ.TimedOut)
                                {
                                    await ctx.Channel.SendMessageAsync($"Casino exited");
                                    exit = true;
                                }
                                else if (responseJ.Result.Content.ToLower() == "exit")
                                {
                                    await ctx.Channel.SendMessageAsync($"Exited Slots");
                                    cancel = true;
                                }
                                else
                                {
                                    try
                                    {
                                        await message1.DeleteAsync();
                                    }
                                    catch { }
                                    try
                                    {
                                        bet = Int64.Parse(responseJ.Result.Content);
                                        if (bet <= MAX_SLOT_BET)
                                        {
                                            if (bet > user.Credits)
                                            {
                                                message1 = await ctx.Channel.SendMessageAsync($"You don't have {bet} credits.");
                                            }
                                            else
                                            {
                                                hasCredits = true;
                                            }
                                        }
                                        else if(bet > MAX_SLOT_BET)
                                        {
                                            message1 = await ctx.Channel.SendMessageAsync($"The max bet for slots is {MAX_SLOT_BET} credits.");
                                        }
                                        await responseJ.Result.DeleteAsync();
                                    }
                                    catch
                                    {

                                    }
                                }
                            }
                            await displayMessage1.DeleteAsync();
                            if(hasCredits)
                            await Slots(ctx, user, bet); //change to slots
                        }
                        else if(response.Result.Content.ToLower() == "shop")
                        {
                            await displayMessage.Result.DeleteAsync();
                            await Shop(ctx, user);
                            user = Bot.PlayerDatabase.GetPlayerByID(user.ID);
                        }
                        else if (response.Result.Content.ToLower() == "highlow")
                        {
                            await displayMessage.Result.DeleteAsync();
                            await HighLow(ctx, user);
                        }
                        else if (response.Result.Content.ToLower() == "coinflip")
                        {
                            await displayMessage.Result.DeleteAsync();
                            await Coinflip(ctx, user);
                        }
                        else if (response.Result.Content.ToLower() == "exit") //Last If (Exit)
                        {
                            await displayMessage.Result.DeleteAsync();
                            await ctx.Channel.SendMessageAsync($"Casino exited");
                            exit = true;
                        }
                        else //Else ignores other inputs
                        {

                        }
                    }
                    user.InMinigame = false;
                    Bot.PlayerDatabase.UpdatePlayer(user);
                }
                else
                {
                    await ctx.Channel.SendMessageAsync($"You are still in {user.NameOfMinigame}");
                }
            }
        }
        private async Task Shop(CommandContext ctx, Player user)
        {
            var interactivity = ctx.Client.GetInteractivity();
            var exit = false;
            const decimal CREDIT_VALUE = 0.10m;
            DiscordMessage message = null;
            var embed = new DiscordEmbedBuilder
            {
                Title = "Casino Shop",
            };
            embed.WithFooter("Respond with exit to exit the shop");
            embed.AddField("Cash Conversion", "Convert cash to credit by responding with \"convert cash {amount of cash to convert}\"\nConversion Rate: $1 = 10 credits\n\nExample: convert cash 1000");
            embed.AddField("Credit Conversion", "Convert credit to cash by responding with \"convert credit {amount of credits to convert}\"\nConversion Rate: 10 credits = $1\n\nExample: convert credit 1000");
            embed.WithFooter("Respond with exit to exit shop");
            while (!exit)
            {
                user = Bot.PlayerDatabase.GetPlayerByID(Convert.ToInt64(ctx.Member.Id));
                embed.Description = $"Welcome to the casino's shop. Here you can transfer money into credits which are used to play casino games.\n\n**{user.Name}'s Balance:** {(user.Balance).ToString("###,###,###,###,###,##0.#0")}\n**{user.Name}'s Credits:** {user.Credits.ToString("###,###,###,###,###,##0")}";
                message = await ctx.Channel.SendMessageAsync(embed: embed);
                var response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member && (x.Content.ToLower().Contains("convert credit") || x.Content.ToLower().Contains("convert cash")|| x.Content.ToLower().Contains("convert money") || x.Content.ToLower().Contains("exit")), TimeSpan.FromSeconds(60));
                if (response.TimedOut)
                {
                    await ctx.Channel.SendMessageAsync("No response given. Returning to casino.");
                    exit = true;
                }
                else if(response.Result.Content.ToLower() == "exit")
                {
                    await ctx.Channel.SendMessageAsync("Exiting casino shop");
                    exit = true;
                }
                else if (response.Result.Content.ToLower().Contains("convert cash") || response.Result.Content.ToLower().Contains("convert money"))
                {
                    try
                    {
                        var processedMessage = response.Result.Content.Split(" ");
                        var num = Decimal.Parse(processedMessage[2]);
                        if (user.Balance < num)
                        {
                            await ctx.Channel.SendMessageAsync($"You do not have ${num}");
                        }
                        else if (num < 0)
                        {
                            await ctx.Channel.SendMessageAsync("Value cannot be negative");
                        }
                        else if (num < CREDIT_VALUE)
                        {
                            await ctx.Channel.SendMessageAsync($"Value must be over {CREDIT_VALUE}");
                        }
                        else
                        {
                            user.Balance -= num;
                            user.Credits += (long)(num * 10);
                            Bot.PlayerDatabase.UpdatePlayer(user);
                        }
                    }
                    catch
                    {
                        await ctx.Channel.SendMessageAsync("Please enter a valid value");
                    }
                }
                else if(response.Result.Content.ToLower().Contains("convert credit"))
                {
                    try
                    {
                        var processedMessage = response.Result.Content.Split(" ");
                        var num = Int64.Parse(processedMessage[2]);
                        if (user.Credits < num)
                        {
                            await ctx.Channel.SendMessageAsync($"You do not have {num} credits");
                        }
                        else if (num < 0)
                        {
                            await ctx.Channel.SendMessageAsync("Value cannot be negative");
                        }
                        else
                        {
                            user.Credits -= num;
                            user.Balance += (decimal)((double)(num / (CREDIT_VALUE * 100)));
                            Bot.PlayerDatabase.UpdatePlayer(user);
                        }
                    }
                    catch
                    {

                        await ctx.Channel.SendMessageAsync("Please enter a valid value");
                    }
                }
                else
                {

                }
                await message.DeleteAsync();
            }
        }
        private async Task Coinflip(CommandContext ctx, Player user)
        {
            var interactivity = ctx.Client.GetInteractivity();
            var embed = new DiscordEmbedBuilder();
            const int MAXBET = 50000;
            var exit = false;
            embed.Title = "Coinflip";
            embed.WithFooter("Respond with exit to exit coinflip");
            while(!exit)
            {
                embed.ClearFields();
                embed.AddField($"{user.Name}'s Balance", $"{user.Credits.ToString("###,###,###,###,###,##0")} credits");
                var win = false;
                var validBet = false;
                long bet = 0;
                embed.Description = $"How much will you bet? The max bet is {MAXBET}";
                embed.Color = DiscordColor.Orange;
                await ctx.Channel.SendMessageAsync(embed: embed);
                var response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(60));
                if(response.TimedOut)
                {
                    await ctx.Channel.SendMessageAsync("No response given. Returning to casino");
                    exit = true;
                    break;
                }
                else if(response.Result.Content.ToLower() == "exit")
                {
                    await ctx.Channel.SendMessageAsync("Returning to casino");
                    exit = true;
                    break;
                }
                else
                {
                    try
                    {
                        bet = Int64.Parse(response.Result.Content);
                        if(bet < 0)
                        {
                            await ctx.Channel.SendMessageAsync("Your bet cannot be a negative value");
                        }
                        else if (bet > MAXBET)
                        {
                            await ctx.Channel.SendMessageAsync($"The max bet for coinflip is {MAXBET} credits");
                        }
                        else if(bet > user.Credits)
                        {
                            await ctx.Channel.SendMessageAsync($"You do not have {bet} credits");
                        }
                        else
                        {
                            validBet = true;
                        }
                    }
                    catch
                    {

                    }
                }

                //Coinflip
                if(validBet)
                {
                    embed.Description = "Heads or tails?";
                    embed.Color = DiscordColor.Yellow;
                    await ctx.Channel.SendMessageAsync(embed: embed);
                    response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member && (x.Content.ToLower() == "heads" || x.Content.ToLower() == "head" || x.Content.ToLower() == "tails" || x.Content.ToLower() == "tail" || x.Content.ToLower() == "exit"), TimeSpan.FromSeconds(60));
                    int side = new Random().Next(1, 3); //1-2
                    string coinside = side switch
                    {
                        1=>"heads",
                        _=>"tails"
                    };
                    if (response.TimedOut)
                    {
                        await ctx.Channel.SendMessageAsync("No response given. Returning to casino");
                        exit = true;
                        break;
                    }
                    else if (response.Result.Content.ToLower() == "exit")
                    {
                        await ctx.Channel.SendMessageAsync("Returning to casino");
                        exit = true;
                        break;
                    }
                    else if (response.Result.Content.ToLower() == "head" || response.Result.Content.ToLower() == "heads")
                    {
                        if (side == 1)
                            win = true;
                    }
                    else
                    {
                        if (side == 2)
                            win = true;
                    }

                    if(win)
                    {
                        embed.Description = $"The coin landed on {coinside}! You won {bet} credits!";
                        embed.Color = DiscordColor.Green;
                        user.Credits += bet * 2;
                        await ctx.Channel.SendMessageAsync(embed: embed);
                    }
                    else
                    {
                        embed.Description = $"The coin landed on {coinside}, better luck next time. You lost {bet} credits.";
                        embed.Color = DiscordColor.Red;
                        user.Credits -= bet;
                        await ctx.Channel.SendMessageAsync(embed: embed);
                    }
                    Bot.PlayerDatabase.UpdatePlayer(user);
                }
            }
        }
        private async Task HighLow(CommandContext ctx, Player user)
        {
            var interactivity = ctx.Client.GetInteractivity();
            var exit = false;
            var validBet = false;
            const int MIN = 1;
            const int SPECIALWIN = 10;
            const int MAXBET = 200;
            long bet = 0;
            var embed = new DiscordEmbedBuilder
            {
                Title = "Higher or Lower",
                Description = $"How much do you want to bet? (Your bet will apply to all current games of Higher or Lower). The max bet is {MAXBET}",
                Color = DiscordColor.Blurple
            };
            embed.AddField($"{user.Name}'s Balance", $"{user.Credits.ToString("###,###,###,###,###,##0")} credits");
            embed.WithFooter("Respond with exit to exit higher or lower. You will not lose any credits if you exit before giving a guess.");
            var message = await ctx.Channel.SendMessageAsync(embed: embed);
            while(!validBet)
            {
                var response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(60));
                if (response.TimedOut)
                {
                    await ctx.Channel.SendMessageAsync("No response given. Returning to casino");
                    exit = true;
                    break;
                }
                else if(response.Result.Content.ToLower() == "exit")
                {
                    await ctx.Channel.SendMessageAsync("Returning to casino");
                    exit = true;
                    break;
                }
                else
                {
                    try
                    {
                        bet = Int64.Parse(response.Result.Content);
                        if(bet < 0)
                        {
                            await ctx.Channel.SendMessageAsync("Your bet cannot be a negative number");
                        }
                        else if(bet > MAXBET)
                        {
                            await ctx.Channel.SendMessageAsync($"You cannot bet over {MAXBET} credits");
                        }
                        else if(bet > user.Credits)
                        {
                            await ctx.Channel.SendMessageAsync($"You do not have {bet} credits");
                        }
                        else
                        {
                            validBet = true;
                        }
                    }
                    catch
                    {

                    }
                }
            }
            await message.DeleteAsync();
            while (!exit)
            {
                var max = new Random().Next(25, 101);
                embed.ClearFields();
                embed.Color = DiscordColor.Blurple;
                embed.AddField($"{user.Name}'s Balance", $"{user.Credits.ToString("###,###,###,###,###,##0")} credits");
                var number = new Random().Next(MIN, max + 1);
                var mid = max / 2;
                var displayNumber = new Random().Next(mid - 10, mid + 10);
                var win = false;
                var specialWin = false;
                var responseGiven = false;
                embed.Description = $"A number between 1-{max} has been generated. Is the number bigger or lower than **{displayNumber}**? You can also guess the number to earn {SPECIALWIN}x the reward.";
                await ctx.Channel.SendMessageAsync(embed: embed);
                var response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member && (x.Content.ToLower() == "lower" || x.Content.ToLower() == "low" || x.Content.ToLower() == "higher" || x.Content.ToLower() == "high" || x.Content.ToLower() == "exit"), TimeSpan.FromSeconds(60));
                if(response.TimedOut)
                {
                    await ctx.Channel.SendMessageAsync($"Returning to casino.");
                    exit = true;
                }
                else if (response.Result.Content.ToLower() == "exit")
                {
                    await ctx.Channel.SendMessageAsync("Returning to casino");
                    exit = true;
                }
                else
                {
                    if(response.Result.Content.ToLower() == "higher" || response.Result.Content.ToLower() == "high")
                    {
                        if (number > displayNumber)
                            win = true;
                        responseGiven = true;
                    }
                    else if (response.Result.Content.ToLower() == "lower" || response.Result.Content.ToLower() == "low")
                    {
                        if (number < displayNumber)
                            win = true;
                        responseGiven = true;
                    }
                    else
                    {
                        try
                        {
                            var guess = Int32.Parse(response.Result.Content);
                            if (guess == number)
                            {
                                specialWin = true;
                                win = true;
                            }
                            responseGiven = true;
                        }
                        catch
                        {

                        }
                    }
                    if(responseGiven)
                    {
                        if(win)
                        {
                            embed.ClearFields();
                            if(specialWin)
                            {
                                embed.Description = $"You guessed the exact number! Your reward has increased {SPECIALWIN}x";
                                user.Credits += bet * SPECIALWIN - bet;
                                embed.Color = DiscordColor.Green;
                                await ctx.Channel.SendMessageAsync(embed: embed);
                            }
                            else
                            {
                                embed.Description = $"You guessed correctly! The number was {number}";
                                user.Credits += bet * 2;
                                embed.Color = DiscordColor.Green;
                                await ctx.Channel.SendMessageAsync(embed: embed);
                            }
                        }
                        else
                        {
                            embed.Description = $"Your guess was incorrect, better luck next time. The number was {number}";
                            user.Credits -= bet;
                            embed.Color = DiscordColor.Red;
                            await ctx.Channel.SendMessageAsync(embed: embed);
                        }
                        Bot.PlayerDatabase.UpdatePlayer(user);
                    }
                }
            }
        }
        private async Task Slots(CommandContext ctx, Player user, long bet)
        {
            var interactivity = ctx.Client.GetInteractivity();
            var embed = new DiscordEmbedBuilder();
            var symbols = new List<string>()
            {
                ":gem:",
                ":gem:",
                ":star:",
                ":star:",
                ":star:",
                ":apple:",
                ":apple:",
                ":apple:",
                ":apple:",
                ":apple:",
                ":gift:",
                ":gift:",
                ":gift:",
                ":gift:",
                ":cherries:",
                ":moneybag:",
            };
            var display = "";
            var exit = false;
            var firstDisplay = true;
            DiscordMessage message = null;
            embed.Title = "Slot Machine";
            embed.AddField("Controls", "Respond with **spin** to spin the slot machine. You can also do **spin {number of spins}** to spin multiple times. You can only do __10__ multiple spins at a time!");
            embed.AddField($"{user.Name}'s Balance", $"{user.Credits.ToString("###,###,###,###,###,##0")}");
            embed.AddField("Win Combinations", ":cherries:                       - .5x\n:cherries::cherries:           - 1x\n:apple::apple::apple: - 5x\n:gift::gift::gift: - 20x\n:star::star::star: - 50x\n:gem::gem::gem: - 75x\n:cherries::cherries::cherries: - 100x\n:moneybag::moneybag::moneybag: - 500x");
            embed.WithFooter("Respond with exit to leave slots");
            while (!exit)
            {
                if (firstDisplay)
                {
                    display = "|";
                    for (int slot = 0; slot < 3; slot++)
                    {
                        var rnum = new Random().Next(0, symbols.Count);
                        display += $" {symbols[rnum]} |";
                    }
                    embed.Description = display;
                    message = await ctx.Channel.SendMessageAsync(embed: embed);
                    firstDisplay = false;
                }
                else if(user.Credits > 0)
                {
                    var response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(60));
                    if(response.TimedOut)
                    {
                        //await message.DeleteAsync();
                        await ctx.Channel.SendMessageAsync("Slot machine exited");
                        exit = true;
                    }
                    else if(response.Result.Content.ToLower().Contains("spin"))
                    {
                        //await message.DeleteAsync();
                        display = "";
                        var splitResponse = response.Result.Content.Split(" ");
                        try
                        {
                            try
                            {
                                await message.DeleteAsync();
                            }
                            catch
                            {}
                            var totalSpins = Int32.Parse(splitResponse[1]);
                            if (totalSpins > 10)
                            {
                                await ctx.Channel.SendMessageAsync($"Spinning 10 times instead of {totalSpins} spins due to max limit");
                                totalSpins = 10;
                                for(int i = 0; i < totalSpins; i++)
                                {
                                    display += $"Spin {i + 1}\n";
                                    display = SpinSlots(symbols, user, bet, display);
                                    display += "\n";
                                    display += "\n";
                                }
                            }
                            else
                            {
                                for (int i = 0; i < totalSpins; i++)
                                {
                                    display += $"Spin {i + 1}\n";
                                    display = SpinSlots(symbols, user, bet, display);
                                    display += "\n";
                                    display += "\n";
                                }
                            }
                            embed.ClearFields();
                            embed.Description = $"{display}";
                            embed.AddField($"{user.Name}'s Balance", $"{user.Credits.ToString("###,###,###,###,###,##0")}");
                            embed.AddField("Win Combinations", ":cherries:                       - .5x\n:cherries::cherries:           - 1x\n:apple::apple::apple: - 5x\n:gift::gift::gift: - 20x\n:star::star::star: - 50x\n:gem::gem::gem: - 75x\n:cherries::cherries::cherries: - 100x\n:moneybag::moneybag::moneybag: - 500x");
                            message = await ctx.Channel.SendMessageAsync(embed: embed);
                        }
                        catch
                        {
                            try
                            {
                                await message.DeleteAsync();
                            }
                            catch
                            {}
                            display = "";
                            embed.Description = SpinSlots(symbols, user, bet, display);
                            embed.ClearFields();
                            embed.AddField($"{user.Name}'s Balance", $"{user.Credits.ToString("###,###,###,###,###,##0")}");
                            embed.AddField("Win Combinations", ":cherries:                       - .5x\n:cherries::cherries:           - 1x\n:apple::apple::apple: - 5x\n:gift::gift::gift: - 20x\n:star::star::star: - 50x\n:gem::gem::gem: - 75x\n:cherries::cherries::cherries: - 100x\n:moneybag::moneybag::moneybag: - 500x");
                            message = await ctx.Channel.SendMessageAsync(embed: embed);
                        }
                        try
                        {
                            await response.Result.DeleteAsync();
                        }
                        catch
                        {

                        }
                    }
                    else if(response.Result.Content.ToLower() == "exit")
                    {
                        await response.Result.DeleteAsync();
                        //await message.DeleteAsync();
                        await ctx.Channel.SendMessageAsync("Slots exited.");
                        exit = true;
                    }
                    else
                    {

                    }
                }
                else
                {
                    await ctx.Channel.SendMessageAsync("You ran out of credits! Slots exited");
                    exit = true;
                }
            }
        }
        private async Task BlackJack(CommandContext ctx, Player user, long bet)
        {
            var interactivity = ctx.Client.GetInteractivity();
            var embed = new DiscordEmbedBuilder();
            var stand = false;
            var canSplit = false;
            var isDouble = false;
            var isDouble2 = false;
            var canDouble = true;
            var didSplit = false;
            long bet2 = 0;
            Card[] deck = new Card[52];
            for(int num = 1; num < 14; num++)
            {
                deck = num switch
                {
                    1 => ShuffleDeck(deck, "A", 11, 1),
                    2 => ShuffleDeck(deck, "2", 2),
                    3 => ShuffleDeck(deck, "3", 3),
                    4 => ShuffleDeck(deck, "4", 4),
                    5 => ShuffleDeck(deck, "5", 5),
                    6 => ShuffleDeck(deck, "6", 6),
                    7 => ShuffleDeck(deck, "7", 7),
                    8 => ShuffleDeck(deck, "8", 8),
                    9 => ShuffleDeck(deck, "9", 9),
                    10 => ShuffleDeck(deck, "10", 10),
                    11 => ShuffleDeck(deck, "J", 10),
                    12 => ShuffleDeck(deck, "Q", 10),
                    13 => ShuffleDeck(deck, "K", 10),
                };
            }
            var dealerCards = new List<Card>();
            var playerCards = new List<Card>();
            var playerCards2 = new List<Card>();
            var dealer = "";
            var player = "";
            var player2 = "";
            int draw = 0;
            for(int i = 0; i < 2; i++)
            {
                dealerCards.Add(deck[draw]);
                draw++;
                playerCards.Add(deck[draw]);
                draw++;
            }
            dealer += $"{dealerCards[0].Symbol} ? ";
            foreach (Card c in playerCards)
            {
                player += $"{c.Symbol} ";
            }
            var dealerValue = dealerCards[0].MaxValue;
            var playerValue = CountCards(playerCards);
            var playerValue2 = 0;
            embed.Title = "BlackJack";
            embed.Description = "Hit 21 or be closest to 21 to win. If you get over 21 you lose. Respond with any actions listed in the __What will you do__ section";
            embed.AddField("Dealer", $"{dealer}\nValue: {dealerValue}");
            embed.AddField($"{user.Name}", $"{player}\nValue: {playerValue}");
            embed.Color = DiscordColor.Blurple;
            if (playerCards[0].Symbol == playerCards[1].Symbol || playerCards[0].MaxValue == playerCards[1].MaxValue)
            {
                embed.AddField("What will you do?", "Hit, Double, Split or Stay?");
                canSplit = true;
            }
            else if (playerValue > 21)
            {

            }
            else
            {
                embed.AddField("What will you do?", "Hit, Double or Stay?");
            }
            var message = await ctx.Channel.SendMessageAsync(embed: embed);
            while (!stand && playerValue < 21 && dealerValue < 21)
            {
                var response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(120));
                if (response.TimedOut)
                {
                    break;
                }
                else if (response.Result.Content.ToLower() == "hit")
                {
                    canDouble = false;
                    player = "";
                    playerCards.Add(deck[draw]);
                    draw++;
                    foreach (Card c in playerCards)
                    {
                        player += $"{c.Symbol} ";
                    }
                    playerValue = CountCards(playerCards);
                    embed.ClearFields();
                    embed.AddField("Dealer", $"{dealer}\nValue: {dealerValue}");
                    embed.AddField($"{user.Name}", $"{player}\nValue: {playerValue}");
                    embed.AddField("What will you do?", "Hit or Stay");
                    builtEmbed = embed.Build();
                    await message.ModifyAsync(null, builtEmbed);
                    try
                    {
                        await response.Result.DeleteAsync();
                    }
                    catch
                    {

                    }
                }
                else if (response.Result.Content.ToLower() == "stay" || response.Result.Content.ToLower() == "stand")
                {
                    dealerValue = CountCards(dealerCards);
                    stand = true;
                    dealer = "";
                    if (dealerValue < 17)
                    {
                        while (dealerValue < 17)
                        {
                            dealerCards.Add(deck[draw]);
                            dealerValue = CountCards(dealerCards);
                            draw++;
                        }
                    }
                    foreach (Card c in dealerCards)
                    {
                        dealer += $"{c.Symbol} ";
                    }
                    embed.ClearFields();
                    embed.AddField("Dealer", $"{dealer}\nValue: {dealerValue}");
                    embed.AddField($"{user.Name}", $"{player}\nValue: {playerValue}");
                    builtEmbed = embed.Build();
                    await message.ModifyAsync(null, builtEmbed);
                    try
                    {
                        await response.Result.DeleteAsync();
                    }
                    catch
                    {

                    }
                }
                else if (response.Result.Content.ToLower() == "double" && canDouble)
                {
                    dealerValue = CountCards(dealerCards);
                    canDouble = false;
                    isDouble = true;
                    stand = true;
                    player = "";
                    playerCards.Add(deck[draw]);
                    draw++;
                    foreach (Card c in playerCards)
                    {
                        player += $"{c.Symbol} ";
                    }
                    playerValue = CountCards(playerCards);
                    dealer = "";
                    if (dealerValue < 17)
                    {
                        while (dealerValue < 17)
                        {
                            dealerCards.Add(deck[draw]);
                            dealerValue = CountCards(dealerCards);
                            draw++;
                        }
                    }
                    foreach (Card c in dealerCards)
                    {
                        dealer += $"{c.Symbol} ";
                    }
                    embed.ClearFields();
                    embed.AddField("Dealer", $"{dealer}\nValue: {dealerValue}");
                    embed.AddField($"{user.Name}", $"{player}\nValue: {playerValue}");
                    builtEmbed = embed.Build();
                    await message.ModifyAsync(null, builtEmbed);
                    try
                    {
                        await response.Result.DeleteAsync();
                    }
                    catch
                    {

                    }
                }
                else if (response.Result.Content.ToLower() == "split" && canSplit)
                {
                    if (user.Credits - bet * 2 < 0)
                    {
                        await ctx.Channel.SendMessageAsync("You don't have enough credits to split");
                        canSplit = false;
                    }
                    else
                    {
                        bet2 = bet;
                        didSplit = true;
                        playerCards2.Add(playerCards[1]);
                        playerCards.Remove(playerCards[1]);
                        playerValue = CountCards(playerCards);
                        player = "";
                        foreach (Card c in playerCards)
                        {
                            player += $"{c.Symbol} ";
                        }
                        foreach (Card c in playerCards2)
                        {
                            player2 += $"{c.Symbol} ";
                        }
                        playerValue2 = CountCards(playerCards2);
                        embed.ClearFields();
                        embed.AddField("Dealer", $"{dealer}\nValue: {dealerValue}");
                        embed.AddField($"{user.Name}", $"{player} <- Selected\nValue: {playerValue}\n{player2}\nValue: {playerValue2}");
                        embed.AddField("What will you do?", "Hit, Double or Stay");
                        builtEmbed = embed.Build();
                        await message.ModifyAsync(null, builtEmbed);
                        try
                        {
                            await response.Result.DeleteAsync();
                        }
                        catch
                        {

                        }
                        var selectedDeck = 1;
                        while (selectedDeck <= 2)
                        {
                            switch (selectedDeck)
                            {
                                case 1: //Start of case 1
                                    while (!stand && playerValue < 21)
                                    {
                                        response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(120));
                                        if (response.TimedOut)
                                        {
                                            break;
                                        }
                                        else if (response.Result.Content.ToLower() == "hit")
                                        {
                                            canDouble = false;
                                            player = "";
                                            playerCards.Add(deck[draw]);
                                            draw++;
                                            foreach (Card c in playerCards)
                                            {
                                                player += $"{c.Symbol} ";
                                            }
                                            playerValue = CountCards(playerCards);
                                            embed.ClearFields();
                                            embed.AddField("Dealer", $"{dealer}\nValue: {dealerValue}");
                                            embed.AddField($"{user.Name}", $"{player} <- Selected\nValue: {playerValue}\n{player2}\nValue: {playerValue2}");
                                            embed.AddField("What will you do?", "Hit or Stay");
                                            builtEmbed = embed.Build();
                                            await message.ModifyAsync(null, builtEmbed);
                                            await response.Result.DeleteAsync();
                                        }
                                        else if (response.Result.Content.ToLower() == "stay" || response.Result.Content.ToLower() == "stand")
                                        {
                                            stand = true;
                                        }
                                        else if (response.Result.Content.ToLower() == "double" && canDouble)
                                        {
                                            canDouble = false;
                                            isDouble = true;
                                            stand = true;
                                            player = "";
                                            playerCards.Add(deck[draw]);
                                            draw++;
                                            foreach (Card c in playerCards)
                                            {
                                                player += $"{c.Symbol} ";
                                            }
                                            playerValue = CountCards(playerCards);
                                            embed.ClearFields();
                                            embed.AddField("Dealer", $"{dealer}\nValue: {dealerValue}");
                                            embed.AddField($"{user.Name}", $"{player}\nValue: {playerValue}\n{player2} <- Selected\nValue: {playerValue2}");
                                            embed.AddField("What will you do?", "Hit, Double or Stay");
                                            builtEmbed = embed.Build();
                                            await message.ModifyAsync(null, builtEmbed);
                                            try
                                            {
                                                await response.Result.DeleteAsync();
                                            }
                                            catch
                                            {

                                            }
                                        }
                                    }
                                    canDouble = true;
                                    stand = false;
                                    selectedDeck++;
                                    break; //End of case 1
                                case 2: //Start of case 2
                                    embed.ClearFields();
                                    embed.AddField("Dealer", $"{dealer}\nValue: {dealerValue}");
                                    embed.AddField($"{user.Name}", $"{player}\nValue: {playerValue}\n{player2} <- Selected\nValue: {playerValue2}");
                                    embed.AddField("What will you do?", "Hit, Double or Stay");
                                    builtEmbed = embed.Build();
                                    embed.AddField("What will you do?", "Hit, Double or Stay");
                                    await message.ModifyAsync(null, builtEmbed);
                                    while (!stand && playerValue2 < 21)
                                    {
                                        response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author == ctx.Member, TimeSpan.FromSeconds(120));
                                        if (response.TimedOut)
                                        {
                                            break;
                                        }
                                        else if (response.Result.Content.ToLower() == "hit")
                                        {
                                            canDouble = false;
                                            player2 = "";
                                            playerCards2.Add(deck[draw]);
                                            draw++;
                                            foreach (Card c in playerCards2)
                                            {
                                                player2 += $"{c.Symbol} ";
                                            }
                                            playerValue2 = CountCards(playerCards2);
                                            embed.ClearFields();
                                            embed.AddField("Dealer", $"{dealer}\nValue: {dealerValue}");
                                            embed.AddField($"{user.Name}", $"{player}\nValue: {playerValue}\n{player2} <- Selected\nValue: {playerValue2}");
                                            embed.AddField("What will you do?", "Hit or Stay");
                                            builtEmbed = embed.Build();
                                            await message.ModifyAsync(null, builtEmbed);
                                            try
                                            {
                                                await response.Result.DeleteAsync();
                                            }
                                            catch
                                            {

                                            }
                                        }
                                        else if (response.Result.Content.ToLower() == "stay" || response.Result.Content.ToLower() == "stand")
                                        {
                                            stand = true;
                                        }
                                        else if (response.Result.Content.ToLower() == "double" && canDouble)
                                        {
                                            canDouble = false;
                                            isDouble2 = true;
                                            stand = true;
                                            player2 = "";
                                            playerCards2.Add(deck[draw]);
                                            draw++;
                                            foreach (Card c in playerCards2)
                                            {
                                                player2 += $"{c.Symbol} ";
                                            }
                                            playerValue2 = CountCards(playerCards2);
                                        }
                                    }
                                    selectedDeck++;
                                    break;
                                default:
                                    break;
                            }
                        }
                        dealerValue = CountCards(dealerCards);
                        dealer = "";
                        if (dealerValue < 17)
                        {
                            while (dealerValue < 17)
                            {
                                dealerCards.Add(deck[draw]);
                                dealerValue = CountCards(dealerCards);
                                draw++;
                            }
                        }
                        foreach (Card c in dealerCards)
                        {
                            dealer += $"{c.Symbol} ";
                        }
                        embed.ClearFields();
                        embed.AddField("Dealer", $"{dealer}\nValue: {dealerValue}");
                        embed.AddField($"{user.Name}", $"{player}\nValue: {playerValue}");
                        builtEmbed = embed.Build();
                        await message.ModifyAsync(null, builtEmbed);
                        try
                        {
                            await response.Result.DeleteAsync();
                        }
                        catch
                        {

                        }
                    }
                }
                else
                {

                }
            }
            long outcome = 0;
            if (didSplit)
            {
                embed.ClearFields();
                embed.AddField("Dealer", $"{dealer}\nValue: {dealerValue}");
                embed.AddField($"{user.Name}", $"{player}\nValue: {playerValue}\n{player2}\nValue: {playerValue2}");
                embed.Title = "BlackJack";

                //For first split
                if (playerValue == 21)
                {
                    embed.Color = DiscordColor.Green;
                    embed.Description = $"{user.Name} got blackjack on their first line and won!";
                    if (isDouble)
                    {
                        user.Credits += 2 * (bet * 2);
                        outcome += 2 * (bet * 2);
                    }
                    else
                    {
                        user.Credits += bet * 2;
                        outcome += bet * 2;
                    }
                }
                else if (dealerValue == 21)
                {
                    embed.Color = DiscordColor.Red;
                    embed.Description = $"Dealer got blackjack. {user.Name} lost on their first line.";
                    if (isDouble)
                    {
                        user.Credits -= bet * 2;
                        outcome -= bet * 2;
                    }
                    else
                    {
                        user.Credits -= bet;
                        outcome -= bet;
                    }
                }
                else if (playerValue == dealerValue)
                {
                    embed.Color = DiscordColor.Yellow;
                    embed.Description = $"Dealer and {user.Name}'s first line tied.";
                }
                else if (playerValue > 21)
                {
                    embed.Color = DiscordColor.Red;
                    embed.Description = $"{user.Name} busted and lost on their first line.";
                    if (isDouble)
                    {
                        user.Credits -= bet * 2;
                        outcome -= bet * 2;
                    }
                    else
                    {
                        user.Credits -= bet;
                        outcome -= bet;
                    }
                }
                else if (dealerValue > 21)
                {
                    embed.Color = DiscordColor.Green;
                    embed.Description = $"Dealer busted! {user.Name} won on their first line!";
                    if (isDouble)
                    {
                        user.Credits += 2 * (bet * 2);
                        outcome += 2 * (bet * 2);
                    }
                    else
                    {
                        user.Credits += bet * 2;
                        outcome += bet * 2;
                    }
                }
                else if (playerValue > dealerValue)
                {
                    embed.Color = DiscordColor.Green;
                    embed.Description = $"{user.Name} won on their first line!";
                    if (isDouble)
                    {
                        user.Credits += 2 * (bet * 2);
                        outcome += 2 * (bet * 2);
                    }
                    else
                    {
                        user.Credits += bet * 2;
                        outcome += bet * 2;
                    }
                }
                else
                {
                    embed.Color = DiscordColor.Red;
                    embed.Description = $"{user.Name} lost on their first line.";
                    if (isDouble)
                    {
                        user.Credits -= bet * 2;
                        outcome -= bet * 2;
                    }
                    else
                    {
                        user.Credits -= bet;
                        outcome -= bet;
                    }
                }

                //For second split
                if (playerValue2 == 21)
                {
                    embed.Color = DiscordColor.Green;
                    embed.Description += $"\n{user.Name} got blackjack on their second line and won!";
                    if (isDouble2)
                    {
                        user.Credits += 2 * (bet2 * 2);
                        outcome += 2 * (bet2 * 2);
                    }
                    else
                    {
                        user.Credits += bet2 * 2;
                        outcome += bet2 * 2;
                    }
                }
                else if (dealerValue == 21)
                {
                    embed.Color = DiscordColor.Red;
                    embed.Description += $"\nDealer got blackjack. {user.Name} lost on their second line.";
                    if (isDouble2)
                    {
                        user.Credits -= bet2 * 2;
                        outcome -= bet2 * 2;
                    }
                    else
                    {
                        user.Credits -= bet2;
                        outcome -= bet2;
                    }
                }
                else if (playerValue2 == dealerValue)
                {
                    embed.Color = DiscordColor.Yellow;
                    embed.Description += $"\nDealer and {user.Name}'s second line tied.";
                }
                else if (playerValue2 > 21)
                {
                    embed.Color = DiscordColor.Red;
                    embed.Description += $"\n{user.Name} busted and lost on their second line.";
                    if (isDouble2)
                    {
                        user.Credits -= bet2 * 2;
                        outcome -= bet2 * 2;
                    }
                    else
                    {
                        user.Credits -= bet2;
                        outcome -= bet2;
                    }
                }
                else if (dealerValue > 21)
                {
                    embed.Color = DiscordColor.Green;
                    embed.Description += $"\nDealer busted! {user.Name} won on their second line!";
                    if (isDouble2)
                    {
                        user.Credits += 2 * (bet2 * 2);
                        outcome += 2 * (bet2 * 2);
                    }
                    else
                    {
                        user.Credits += bet * 2;
                        outcome += bet2 * 2;
                    }
                }
                else if (playerValue2 > dealerValue)
                {
                    embed.Color = DiscordColor.Green;
                    embed.Description += $"\n{user.Name} won on their second line!";
                    if (isDouble2)
                    {
                        user.Credits += 2 * (bet2 * 2);
                        outcome += 2 * (bet2 * 2);
                    }
                    else
                    {
                        user.Credits += bet2 * 2;
                        outcome += bet2 * 2;
                    }
                }
                else
                {
                    embed.Color = DiscordColor.Red;
                    embed.Description += $"\n{user.Name} lost on their second line.";
                    if (isDouble2)
                    {
                        user.Credits -= bet2 * 2;
                        outcome -= bet2 * 2;
                    }
                    else
                    {
                        user.Credits -= bet2;
                        outcome -= bet2;
                    }
                }
            }
            else
            {
                embed.ClearFields();
                embed.AddField("Dealer", $"{dealer}\nValue: {dealerValue}");
                embed.AddField($"{user.Name}", $"{player}\nValue: {playerValue}");
                embed.Title = "BlackJack";
                if (playerValue == 21)
                {
                    embed.Color = DiscordColor.Green;
                    embed.Description = $"{user.Name} got blackjack and won!";
                    if (isDouble)
                    {
                        user.Credits += 2 * (bet * 2);
                        outcome += 2 * (bet * 2);
                    }
                    else
                    {
                        user.Credits += bet * 2;
                        outcome += bet * 2;
                    }
                }
                else if (dealerValue == 21)
                {
                    embed.Color = DiscordColor.Red;
                    embed.Description = $"Dealer got blackjack. {user.Name} lost.";
                    if (isDouble)
                    {
                        user.Credits -= bet * 2;
                        outcome -= bet * 2;
                    }
                    else
                    {
                        user.Credits -= bet;
                        outcome -= bet;
                    }
                }
                else if (playerValue == dealerValue)
                {
                    embed.Color = DiscordColor.Yellow;
                    embed.Description = $"Dealer and {user.Name} tied.";
                }
                else if (playerValue > 21)
                {
                    embed.Color = DiscordColor.Red;
                    embed.Description = $"{user.Name} busted and lost.";
                    if (isDouble)
                    {
                        user.Credits -= bet * 2;
                        outcome -= bet * 2;
                    }
                    else
                    {
                        user.Credits -= bet;
                        outcome -= bet;
                    }
                }
                else if (dealerValue > 21)
                {
                    embed.Color = DiscordColor.Green;
                    embed.Description = $"Dealer busted! {user.Name} won!";
                    if (isDouble)
                    {
                        user.Credits += 2 * (bet * 2);
                        outcome += 2 * (bet * 2);
                    }
                    else
                    {
                        user.Credits += bet * 2;
                        outcome += bet * 2;
                    }
                }
                else if (playerValue > dealerValue)
                {
                    embed.Color = DiscordColor.Green;
                    embed.Description = $"{user.Name} won!";
                    if (isDouble)
                    {
                        user.Credits += 2 * (bet * 2);
                        outcome += 2 * (bet * 2);
                    }
                    else
                    {
                        user.Credits += bet * 2;
                        outcome += bet * 2;
                    }
                }
                else
                {
                    embed.Color = DiscordColor.Red;
                    embed.Description = $"{user.Name} lost.";
                    if (isDouble)
                    {
                        user.Credits -= bet * 2;
                        outcome -= bet * 2;
                    }
                    else
                    {
                        user.Credits -= bet;
                        outcome -= bet;
                    }
                }
            }
            var displayOutcome = "";
            if (outcome > 0)
                displayOutcome = $"{outcome} credits won!";
            else
                displayOutcome = $"{Math.Abs(outcome)} credits lost, better luck next time.";
            embed.AddField("Outcome", displayOutcome);
            builtEmbed = embed.Build();
            await message.ModifyAsync(null, builtEmbed);
            Bot.PlayerDatabase.UpdatePlayer(user);
            embed = new DiscordEmbedBuilder();
        }
        private byte CountCards(List<Card> cards)
        {
            byte totalValue = 0;
            var hasAce = false;
            var totalAces = 0;
            foreach(Card c in cards)
            {
                totalValue += c.MaxValue;
                if(c.Symbol == "A")
                {
                    hasAce = true;
                    totalAces++;
                }
            }
            if(totalValue > 21 && hasAce == true)
            {
                totalValue = 0;
                foreach(Card c in cards)
                {
                    if(c.Symbol == "A")
                    {

                    }
                    else
                    {
                        totalValue += c.MaxValue;
                    }
                }
                for(int i = 0; i < totalAces; i++)
                {
                    if(totalValue + 11 > 21)
                    {
                        totalValue++;
                    }
                    else
                    {
                        totalValue += 11;
                    }
                }
                return totalValue;
            }
            else
            {
                return totalValue;
            }
        }
        private string SpinSlots(List<string> symbols, Player user, long bet, string display)
        {
            var rawResult = "";
            int outcome = 0;
            display += "|";
            for (int slot = 0; slot < 3; slot++)
            {
                var rnum = new Random().Next(0, symbols.Count);
                display += $" {symbols[rnum]} |";
                rawResult += $"{symbols[rnum]} ";
            }
            var processedResult = rawResult.Split(" ");
            if(processedResult[0] == processedResult[1] && processedResult[1] == processedResult[2])
            {
                switch(processedResult[0])
                {
                    case ":cherries:":
                        outcome = (int)(bet * 100);
                        user.Credits += outcome;
                        break;
                    case ":apple:":
                        outcome = (int)(bet * 5);
                        user.Credits += outcome;
                        break;
                    case ":gift:":
                        outcome = (int)(bet * 20);
                        user.Credits += outcome;
                        break;
                    case ":star:":
                        outcome = (int)(bet * 50);
                        user.Credits += outcome;
                        break;
                    case ":gem:":
                        outcome = (int)(bet * 75);
                        user.Credits += outcome;
                        break;
                    case ":moneybag:":
                        outcome = (int)(bet * 500);
                        user.Credits += outcome;
                        break;
                    default:
                        break;
                }
                display += $"\n{outcome} credits won";
            }
            else if ((processedResult[0] == ":cherries:" && processedResult[1] == ":cherries:") || (processedResult[0] == ":cherries:" && processedResult[2] == ":cherries:") || (processedResult[1] == ":cherries:" && processedResult[2] == ":cherries:"))
            {
                outcome = (int)(bet * 1);
                user.Credits += outcome - bet;
                display += $"\nNo credits lost";
            }
            else if (processedResult[0] == ":cherries:" || processedResult[1] == ":cherries:" || processedResult[2] == ":cherries:")
            {
                outcome = (int)(bet * .5);
                user.Credits += outcome - bet;
                display += $"\n{Math.Abs(outcome - bet)} credits lost";
            }
            else
            {
                display += $"\n{bet} credits lost";
                user.Credits -= bet;
            }
            Bot.PlayerDatabase.UpdatePlayer(user);
            return display;
        }
        private Card[] ShuffleDeck(Card[] deck, string symbole, byte max, byte min = 0)
        {
            var deckIndex = -1;
            for (int l = 0; l < 4; l++)
            {
                while (deckIndex == -1 || !(deck[deckIndex] == null))
                {
                    deckIndex = new Random().Next(0, 52);
                }
                deck[deckIndex] = new Card { MaxValue = max, MinValue = (min != 0) ? min : max, Symbol = symbole };
            }
            return deck;
        }
    }
}
