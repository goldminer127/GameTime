using System;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity.Extensions;
using GameTime.Connect4Models;
using GameTime.MultiplayerSessionModels;

namespace GameTime.Commands
{
    public class Connect4Commands : BaseCommandModule
    {
        [Command("connect4"), Aliases("cn4j")]
        public async Task JoinConnect4(CommandContext ctx, string id)
        {
            if (Bot.GameSessions.GetSession(id) == null)
            {
                await ctx.Channel.SendMessageAsync($"Private session {id} does not exist");
            }
            var deleteInstance = false;
            if (Bot.GameSessions.IsPublicEmpty())
            {
                var session = new Connect4Session(false, 2);
                session.Join(ctx);
                Bot.GameSessions.AddSession(session);
                deleteInstance = true;
                await GameSearch(ctx, session, deleteInstance);
            }
            else
            {
                var session = Bot.GameSessions.SearchOpenPrivateSession(id);
                session.Join(ctx);
                Bot.GameSessions.UpdateSession(session);
                await GameSearch(ctx, session, deleteInstance);
            }
        }
        [Command("connect4"), Aliases("cn4")]
        public async Task JoinConnect4(CommandContext ctx)
        {
            var primaryInstance = false; //For removing session from database and single displays
            if (Bot.GameSessions.IsPublicEmpty())
            {
                var session = new Connect4Session(false, 2);
                session.Join(ctx);
                Bot.GameSessions.AddSession(session);
                primaryInstance = true;
                await GameSearch(ctx, session, primaryInstance);
            }
            else
            {
                var session = Bot.GameSessions.SearchOpenPublicSession();
                session.Join(ctx);
                Bot.GameSessions.UpdateSession(session);
                await GameSearch(ctx, session, primaryInstance);
            }
        }
        private async Task GameSearch(CommandContext ctx, GameSession session, bool primaryInstance)
        {
            var message = await ctx.Channel.SendMessageAsync("Searching for game...");
            while (session.GameStatus != Status.Close && session.GameStatus != Status.Exited)
            {
                var response = await ctx.Client.GetInteractivity().WaitForMessageAsync(msg => msg.Channel == ctx.Channel && msg.Author.Id == ctx.Member.Id, TimeSpan.FromSeconds(2));
                if (response.TimedOut)
                    session = Bot.GameSessions.GetSession(session.SessionId);
                else
                    session.SetStatus(Status.Exited);
            }
            await message.DeleteAsync();
            if(session.GameStatus != Status.Exited)
                await TurnHandler(ctx, session, primaryInstance);
        }
        private async Task TurnHandler(CommandContext ctx, GameSession session, bool primaryInstance)
        {
            /*Checks are to send a single message when both players are in the same text channel. Checks in this statement
              also alternates who to ping based on which player's turn it is. ALL MESSAGE STATEMENTS CONTAINS A CHECK WHETHER
              TO SEND A SINGLE MESSAGE IN 1 CHANNEL OR 2 MESSAGES TO SEPARATE CHANNELS*/
            var message = (session.PlayersInSameChannel(ctx.Channel.Id)) ? 
                (primaryInstance) ? 
                    await ctx.Channel.SendMessageAsync((ctx.User.Id == session.CurrentTurn.Id) ? 
                        $"{ctx.User.Mention} your move" : "", session.GameDisplay("No problems")) 
                : null
            : await ctx.Channel.SendMessageAsync((ctx.User.Id == session.CurrentTurn.Id) ? 
                $"{ctx.User.Mention} your move" : "", session.GameDisplay("No problems"));
            while (session.GameStatus != Status.Completed && session.GameStatus != Status.Exited)
            {
                //Allows user to exit while it isn't their turn
                while (session.CurrentTurn.Id != ctx.User.Id && (session.GameStatus != Status.Completed && session.GameStatus != Status.Exited))
                {
                    session = Bot.GameSessions.GetSession(session.SessionId);
                    var response = await ctx.Client.GetInteractivity().WaitForMessageAsync(msg => msg.Channel == ctx.Channel && msg.Author.Id == ctx.Member.Id, TimeSpan.FromSeconds(1.25));
                    if (response.TimedOut)
                    {
                        var waitUpdateMessage = new DiscordMessageBuilder().AddEmbed(session.Display).WithContent($"");
                        message = (session.PlayersInSameChannel(ctx.Channel.Id)) ? (primaryInstance) ? await message.ModifyAsync(waitUpdateMessage) : null : await message.ModifyAsync(waitUpdateMessage);
                    }
                    else if (response.Result.Content.ToLower().Contains("exit"))
                    {
                        session.SetStatus(Status.Exited);
                        session.GameStoppedDisplay($"{ctx.User.Username} conceeded");
                    }
                    Bot.GameSessions.UpdateSession(session);
                }
                if (session.GameStatus != Status.Completed && session.GameStatus != Status.Exited)
                {
                    var updateMessage = new DiscordMessageBuilder().AddEmbed(session.Display).WithContent($"{ctx.User.Mention} your move");
                    message = (session.PlayersInSameChannel(ctx.Channel.Id)) ? (primaryInstance) ? await message.ModifyAsync(updateMessage) : null : await message.ModifyAsync(updateMessage);
                    session = Bot.GameSessions.GetSession(session.SessionId);
                    session = ActionHandler(ctx, session.SessionId, message).Result;
                    Bot.GameSessions.UpdateSession(session);
                    updateMessage = new DiscordMessageBuilder().AddEmbed(session.Display).WithContent($"");
                    message = (session.PlayersInSameChannel(ctx.Channel.Id)) ? (primaryInstance) ? await message.ModifyAsync(updateMessage) : null : await message.ModifyAsync(updateMessage);
                }
            }
            session = Bot.GameSessions.GetSession(session.SessionId);
            //Clear the ping in message when game ends
            var newMessage = new DiscordMessageBuilder().AddEmbed(session.Display);
            message = (session.PlayersInSameChannel(ctx.Channel.Id)) ? (primaryInstance) ? await message.ModifyAsync(newMessage) : null : await message.ModifyAsync(newMessage);
            if (primaryInstance)
                Bot.GameSessions.RemoveSession(session.SessionId);
        }
        private async Task<GameSession> ActionHandler(CommandContext ctx, string sessionId, DiscordMessage message)
        {
            //Await for response, made this way to accept exit command from both parties
            DSharpPlus.Interactivity.InteractivityResult<DiscordMessage> response = await ctx.Client.GetInteractivity().WaitForMessageAsync(msg => msg.Channel == ctx.Channel && msg.Author.Id == ctx.Member.Id, TimeSpan.FromSeconds(1));
            var session = Bot.GameSessions.GetSession(sessionId);
            for (int time = 0; time < 300 && response.TimedOut; time++)
            {
                //Waits 5 minutes and 1 second
                response = await ctx.Client.GetInteractivity().WaitForMessageAsync(msg => msg.Channel == ctx.Channel && msg.Author.Id == ctx.Member.Id, TimeSpan.FromSeconds(1));
                session = Bot.GameSessions.GetSession(sessionId);
                //Cancel move if an opponent conceeds
                if (session.GameStatus == Status.Completed || session.GameStatus == Status.Exited)
                    return session;
            }
            //Response handler
            session = Bot.GameSessions.GetSession(sessionId);
            if (response.TimedOut)
            {
                session.GameStoppedDisplay("Session timed out.");
                session.SetStatus(Status.Exited);
                return session;
            }
            else
            {
                await response.Result.DeleteAsync();
                if (response.Result.Content.ToLower().Contains("exit"))
                {
                    session.GameStoppedDisplay($"{ctx.User.Username} conceeded");
                    session.SetStatus(Status.Exited);
                    return session;
                }
                else if (session.CurrentTurn.Id == ctx.User.Id)
                {
                    var placed = session.MakeMove(response.Result.Content);
                    if (!placed)
                    {
                        session.GameDisplay($"{ctx.User.Username} : {message.Content} is not a valid move");
                        return session;
                    }
                    else
                    {
                        if (session.Winner == null)
                        {
                            session.GameDisplay("No problems");
                        }
                        else
                        {
                            session.EndGameDisplay(session.Winner.Id == ctx.User.Id);
                        }
                        return session;
                    }
                }
                return session;
            }
        }
    }
}
