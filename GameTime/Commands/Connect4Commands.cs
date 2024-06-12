using System;
using System.Threading.Tasks;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Interactivity.Extensions;
using GameTime.Connect4Models;
using GameTime.MultiplayerSessionModels;
using GameTime.MultiplayerSessionModels.Enums;

namespace GameTime.Commands
{
    public class Connect4Commands : BaseCommandModule
    {
        [Command("connect4p"), Aliases("cn4p")]
        public async Task JoinPrivateConnect4(CommandContext ctx, string id)
        {
            if (Bot.GameSessions.GetSession(id) == null)
            {
                await ctx.Channel.SendMessageAsync($"Private session {id} does not exist");
            }
            else if(Bot.GameSessions.PlayerInSession(ctx.User.Id))
            {
                await ctx.Channel.SendMessageAsync("You are already in a game");
            }
            else
            {
                var session = Bot.GameSessions.SearchOpenPrivateSession(id);
                var joinStatus = session.Join(ctx);
                if (joinStatus == JoinStatus.Success)
                {
                    Bot.GameSessions.UpdateSession(session);
                    await GameSearch(ctx, session, false); //never primary instance
                }
                else
                {
                    await ctx.Channel.SendMessageAsync("Could not join game, session is full.");
                }
            }
        }
        //Creates private session
        [Command("connect4p")]
        public async Task JoinPrivateConnect4(CommandContext ctx)
        {
            if (Bot.GameSessions.PlayerInSession(ctx.User.Id))
            {
                await ctx.Channel.SendMessageAsync("You are already in a game");
            }
            else
            {
                var session = new Connect4Session(true, 2);
                session.Join(ctx);
                await GameSearch(ctx, session, true); //always primary instance
            }
        }
        [Command("connect4"), Aliases("cn4j")]
        public async Task JoinConnect4(CommandContext ctx)
        {
            if (Bot.GameSessions.PlayerInSession(ctx.User.Id))
            {
                await ctx.Channel.SendMessageAsync("You are already in a game");
            }
            else
            {
                var primaryInstance = false; //For removing session from database and single displays
                if (Bot.GameSessions.IsPublicEmpty())
                {
                    var session = new Connect4Session(false, 2);
                    session.Join(ctx);
                    primaryInstance = true;
                    await GameSearch(ctx, session, primaryInstance);
                }
                else
                {
                    var session = Bot.GameSessions.SearchOpenPublicSession("cn4");
                    session.Join(ctx);
                    Bot.GameSessions.UpdateSession(session);
                    await GameSearch(ctx, session, primaryInstance);
                }
            }
        }
        private async Task GameSearch(CommandContext ctx, GameSession session, bool primaryInstance)
        {
            var message = await ctx.Channel.SendMessageAsync((session.IsPrivate && primaryInstance) ? $"Searching for player...\nId: {session.SessionId}\nRespond with \"exit\" to leave queue" : "Searching for game...\nRespond with \"exit\" to leave queue");
            while (session.GameStatus != Status.Close && session.GameStatus != Status.Exited)
            {
                var response = await ctx.Client.GetInteractivity().WaitForMessageAsync(msg => msg.Channel == ctx.Channel && msg.Author.Id == ctx.Member.Id && msg.Content.ToLower() == "exit", TimeSpan.FromSeconds(2));
                if (response.TimedOut)
                    session = Bot.GameSessions.GetSession(session.SessionId);
                else
                {
                    session.Exit(ctx.User.Id);
                    await message.ModifyAsync("Exited queue");
                }
            }
            if (session.GameStatus != Status.Exited)
            {
                await message.DeleteAsync();
                await ActionHandler(ctx, session, primaryInstance);
            }
        }
        private async Task ActionHandler(CommandContext ctx, GameSession session, bool primaryInstance)
        {
            var interactivity = ctx.Client.GetInteractivity();
            while (session.GameStatus != Status.Exited || session.GameStatus == Status.Completed)
            {
                var response = await interactivity.WaitForMessageAsync(msg => msg.Channel == ctx.Channel && msg.Author.Id == ctx.Member.Id, TimeSpan.FromMinutes(1));
                if(response.TimedOut)
                    session.Exit(ctx.User.Id, $"{ctx.User.Username} timed out.");
                else
                {
                    var result = session.MakeMove(response.Result.Content.ToLower(), ctx.User.Id);
                    if (result == MoveStatus.InvalidTurn)
                    {
                        /*
                        var msgBuilder = new DiscordFollowupMessageBuilder(){ 
                                IsEphemeral = true,
                                Content = "Not currently your turn."
                        };
                        */
                        await ctx.Channel.SendMessageAsync("Not currently your turn.");
                    }
                }
            }
        }
    }
}
