using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Exceptions;

namespace GameTime
{
    public static class Events
    {
        public static async Task Client_Ready(DiscordClient client, ReadyEventArgs e)
        {
            await Bot.LoggingChannel.SendMessageAsync("GameTime Started.");
            Console.WriteLine("GameTime Ready.");
            Console.WriteLine("Assigned prefix: \"g/\" case ignored.");
        }
        public static async Task CommandErrored(CommandsNextExtension cmd, CommandErrorEventArgs e)
        {
            await e.Context.Channel.TriggerTypingAsync();
            await e.Context.Channel.SendMessageAsync("Error\n" + e.Exception.Message);
        }

        public static async Task AcknowledgeComponentInteraction(DiscordClient client, ComponentInteractionCreateEventArgs e)
        {
            await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
        }
    }
}
