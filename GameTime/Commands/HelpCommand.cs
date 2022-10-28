using System;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity.Extensions;
using System.Collections.Generic;

namespace GameTime.Commands
{
    public class HelpCommand : BaseCommandModule
    {
        /* Random shit that I keep here to remember so I don't go through 12 hours of testing again.
         * 
         * ctx.CommandsNext.RegisteredCommands gets all registered commands
         * Gets specific commands
         * string temp;
         * await Test(ctx, ctx.CommandsNext.FindCommand("help", out temp));
         */
        [Command("help"), Description("Gives more info about commands.")]
        public async Task Help(CommandContext ctx, [Description("Command to inspect")] string command)
        {
            await ctx.Channel.SendMessageAsync(command);
        }
        [Command("help"), Description("Gives more info about commands.")]
        public async Task Help(CommandContext ctx)
        {
            var options = new List<DiscordSelectComponentOption>
            {
                new DiscordSelectComponentOption("Game1 Commands", "1"),
                new DiscordSelectComponentOption("Test2 Commands", "2"),
            };
            var embedBuilder = new DiscordEmbedBuilder().WithTitle("Help Options").WithDescription("Select which section you would like to view. Or use the" +
                "respective commands for each section.").
                AddField("Game 1", "View all commands with \'g/help Game1\'").WithFooter("Menu expires in 2 minutes");
            var component = new DiscordSelectComponent("helpMenu", null, options, false, 1, 1);
            var messageBuilder = new DiscordMessageBuilder().WithEmbed(embedBuilder.Build()).AddComponents(component);
            var message = await ctx.Channel.SendMessageAsync(messageBuilder);
            await SelectHelpAsync(ctx, message, component.CustomId);
        }
        private async Task SelectHelpAsync(CommandContext ctx, DiscordMessage message, string componentId)
        {
            var response = await ctx.Client.GetInteractivity().WaitForSelectAsync(message, ctx.User, componentId, timeoutOverride: TimeSpan.FromMinutes(2));
            do
            {
                if (response.TimedOut)
                {
                    await message.ModifyAsync(new DiscordMessageBuilder().WithEmbed(message.Embeds[0]));
                }
                else
                {
                    switch (response.Result.Values[0])
                    {
                        case "1":
                            await message.ModifyAsync(Game1Commands(ctx));
                            break;
                        case "2":
                            await message.ModifyAsync("help");
                            break;
                        default:
                            await message.ModifyAsync("Go fuck yourself somehow");
                            break;
                    }
                    response = await ctx.Client.GetInteractivity().WaitForSelectAsync(message, ctx.User, componentId, timeoutOverride: TimeSpan.FromMinutes(2));
                }
            } while (!response.TimedOut);
        }
        private async Task Test(CommandContext ctx, Command command)
        {

        }

        private DiscordEmbed Game1Commands(CommandContext ctx)
        {
            return new DiscordEmbedBuilder()
            {
                Title = "Game1",
                Description = "All Game1 commands. Select a section with the menu to view other commands"
            }.
            WithFooter("Menu expires in 2 minutes").
            AddField("list [item type]", ctx.CommandsNext.RegisteredCommands["list"].Description).
            AddField("info <item name>", ctx.CommandsNext.RegisteredCommands["info"].Description).
            Build();
        }
    }
}
