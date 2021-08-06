using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Interactivity;
using GameTime.Extensions;
using GameTime.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameTime.Connect4Models;

namespace GameTime.Commands
{
    public partial class MultiplayerCommands : BaseCommandModule
    {
        private Dictionary<string, Connect4Session> PublicSessions = new Dictionary<string, Connect4Session>();
        private Dictionary<string, Connect4Session> PrivateSessions = new Dictionary<string, Connect4Session>();
        [Command("connect4-join"), Aliases("cn4-join", "cn4j")]
        [Description("Matchmake for connect4")]
        public async Task Connect4Matchmake(CommandContext ctx, [RemainingText] string sessioncode = null)
        {
            var user = Bot.PlayerDatabase.GetPlayerByID(Convert.ToInt64(ctx.User.Id));
            if (GeneralFunctions.ValidatePlayer(ctx, user, true))
            {
                user.InMinigame = true;
                Bot.PlayerDatabase.UpdatePlayer(user);
                if (sessioncode == null)
                {
                    var sessionFound = false;
                    foreach (var session in PublicSessions)
                    {
                        if (session.Value.Player2 == null)
                        {
                            session.Value.Player2 = user;
                            session.Value.P2Channel = ctx.Channel;
                            sessionFound = true;
                            await PlayConnect4Session(ctx, session.Value, 2);
                        }
                    }
                    if (sessionFound == false)
                    {
                        await CreatePublicSession(ctx, user);
                    }
                }
                else
                {
                    var session = PrivateSessions[sessioncode];
                    session.Player2 = user;
                    session.P2Channel = ctx.Channel;
                    try
                    {
                        await PlayConnect4Session(ctx, session, 2);
                    }
                    catch
                    {
                        await ctx.Channel.SendMessageAsync("Connect4 session does not exist.");
                    }
                }
                user.InMinigame = false;
                Bot.PlayerDatabase.UpdatePlayer(user);
            }
            else { }
        }
        [Command("connect4-create"), Aliases("cn4-c", "cn4c")]
        [Description("Create a private connect4 session")]
        public async Task CreateConnect4Session(CommandContext ctx)
        {
            var user = Bot.PlayerDatabase.GetPlayerByID(Convert.ToInt64(ctx.User.Id));
            if (GeneralFunctions.ValidatePlayer(ctx, user, true))
            {
                user.InMinigame = true;
                Bot.PlayerDatabase.UpdatePlayer(user);
                await CreatePrivateSession(ctx, user);
                user.InMinigame = false;
                Bot.PlayerDatabase.UpdatePlayer(user);
            }
        }
        public async Task CreatePrivateSession(CommandContext ctx, Player user)
        {
            var newSession = new Connect4Session(false);
            newSession.Player1 = user;
            newSession.P1Channel = ctx.Channel;
            PrivateSessions.Add(newSession.SessionKey, newSession);
            await PlayConnect4Session(ctx, newSession, 1);
        }
        public async Task CreatePublicSession(CommandContext ctx, Player user)
        {
            var newSession = new Connect4Session(true);
            newSession.Player1 = user;
            newSession.P1Channel = ctx.Channel;
            PublicSessions.Add(newSession.SessionKey, newSession);
            await PlayConnect4Session(ctx, newSession, 1);
        }
        public async Task PlayConnect4Session(CommandContext ctx, Connect4Session session, byte player)
        {
            var embed = new DiscordEmbedBuilder();
            var gameEnd = false;
            if (player == 1)
            {
                switch (session.IsPublic)
                {
                    case true:
                        embed.Title = " Connect4";
                        embed.Description = "Matching...";
                        embed.WithFooter("Respond with exit to exit matchmaking");
                        break;
                    //For private sessions
                    case false:
                        embed.Title = $"Connect4\nwaiting for opponent...";
                        embed.Description = "Respond with exit to exit matchmaking";
                        embed.WithFooter($"Use the following command to join:\ng/cn4j {session.SessionKey}");
                        break;
                }
            }
            else
            {
                switch (session.IsPublic)
                {
                    case true:
                        embed.Title = " Connect4";
                        embed.Description = "Matching...";
                        embed.WithFooter("Respond with exit to exit matchmaking");
                        break;
                    //For private sessions
                    case false:
                        embed.Title = $"Connecting to private match";
                        break;
                }
            }
            await ctx.Channel.SendMessageAsync(embed: embed);
            var interactivity = ctx.Client.GetInteractivity();
            DSharpPlus.Interactivity.InteractivityResult<DiscordMessage> response;
            while (session.Player2 == null)
            {
                response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && x.Author.Id == ctx.Member.Id && x.Content.ToLower().Contains("exit"), TimeSpan.FromSeconds(1.5));
                if(session.IsPublic == true)
                    session = PublicSessions[session.SessionKey];
                else
                    session = PrivateSessions[session.SessionKey];
                if (session.Player2 != null)
                {
                    break;
                }
                else if (response.TimedOut)
                {
                }
                else if (response.Result.Content.ToLower().Contains("exit"))
                {
                    embed = new DiscordEmbedBuilder();
                    embed.Title = "Matching exited";
                    await ctx.Channel.SendMessageAsync(embed: embed);
                    gameEnd = true;
                    break;
                }
            }
            if (gameEnd == false)
            {
                embed.AddField("Current Turn:", $"{session.Player1.Name}");
                switch (session.IsPublic)
                {
                    case true:
                        embed.Title = $"Connect4\n{session.Player1.Name}[:yellow_circle:] vs {session.Player2.Name}[:red_circle:]";
                        embed.Description = session.SessionBoard.ConstructDisplay(session.SessionBoard.Board);
                        embed.WithFooter(" ");
                        break;
                    //For private sessions
                    case false:
                        embed.Title = $"Connect4\n{session.Player1.Name}[:yellow_circle:] vs {session.Player2.Name}[:red_circle:]";
                        embed.Description = session.SessionBoard.ConstructDisplay(session.SessionBoard.Board);
                        embed.WithFooter(session.SessionKey);
                        break;
                }
                switch (player)
                {
                    case 1:
                        session.P1Display = await session.P1Channel.SendMessageAsync(embed: embed);
                        break;
                    case 2:
                        if (session.P1Channel != session.P2Channel)
                            session.P2Display = await session.P2Channel.SendMessageAsync(embed: embed);
                        break;
                }
            }
            while (gameEnd == false && session.SessionEnd == false)
            {
                while (session.PlayerTurn != player)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1.5));
                    try
                    {
                        if (session.IsPublic)
                            session = PublicSessions[session.SessionKey];
                        else
                            session = PrivateSessions[session.SessionKey];
                    }
                    catch
                    {
                        gameEnd = true;
                        break;
                    }
                }
                if (gameEnd == true)
                    break;
                embed.ClearFields();
                embed.WithFooter("Respond with exit to exit session.");
                switch (session.PlayerTurn)
                {
                    case 1:
                        response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && Convert.ToInt64(x.Author.Id) == session.Player1.ID, TimeSpan.FromSeconds(60));
                        try
                        {
                            if (response.TimedOut)
                            {
                                if (session.PlayerTurn == 1)
                                    embed.AddField("Session Timed Out", $"{session.Player1.Name} did not make a move in time");
                                else if (session.PlayerTurn == 2)
                                    embed.AddField("Session Timed Out", $"{session.Player2.Name} did not make a move in time");
                                gameEnd = true;
                            }
                            else if (response.Result.Content.ToLower() == "exit")
                            {
                                if (session.PlayerTurn == 1)
                                    embed.AddField("Session Stopped", $"{session.Player1.Name} exited");
                                else if (session.PlayerTurn == 2)
                                    embed.AddField("Session Stopped", $"{session.Player2.Name} exited");
                                gameEnd = true;
                            }
                            else if ((Convert.ToInt32(response.Result.Content) > 0 && Convert.ToInt32(response.Result.Content) < 8))
                            {
                                try
                                {
                                    var col = Convert.ToInt32(response.Result.Content) - 1;
                                    byte row;
                                    for (row = 5; row >= 0; row--)
                                    {
                                        if (session.SessionBoard.Board[row, col] == 0)
                                        {
                                            session.SessionBoard.Board[row, col] = 1;
                                            break;
                                        }
                                    }
                                    if (CheckForWin(session.SessionBoard, row, col) && session.SessionEnd != true)
                                    {
                                        session.SessionWinner = session.Player1;
                                        session.SessionEnd = true;
                                        embed.AddField("Winner", $"{session.SessionWinner.Name}");
                                        embed.Color = new DiscordColor(session.SessionWinner.PlayerColor);
                                        gameEnd = true;
                                    }
                                    else if (session.SessionEnd == true)
                                        gameEnd = true;
                                    if (session.PlayerTurn == 1 && gameEnd == false)
                                        session.PlayerTurn++;
                                    else if (session.PlayerTurn == 2 && gameEnd == false)
                                        session.PlayerTurn--;
                                }
                                catch
                                {
                                }
                            }
                        }
                        catch { }
                        await response.Result.DeleteAsync();
                        break;
                    case 2:
                        response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && Convert.ToInt64(x.Author.Id) == session.Player2.ID, TimeSpan.FromSeconds(60));
                        try
                        {
                            if (response.TimedOut)
                            {
                                gameEnd = true;
                            }
                            else if (response.Result.Content.ToLower() == "exit")
                            {
                                if (session.PlayerTurn == 1)
                                    embed.AddField("Session Stopped", $"{session.Player2.Name} exited");
                                else if (session.PlayerTurn == 2)
                                    embed.AddField("Session Stopped", $"{session.Player1.Name} exited");
                                gameEnd = true;
                            }
                            else if ((Convert.ToInt32(response.Result.Content) > 0 && Convert.ToInt32(response.Result.Content) < 8))
                            {
                                try
                                {
                                    var col = Convert.ToInt32(response.Result.Content) - 1;
                                    byte row;
                                    for (row = 5; row >= 0; row--)
                                    {
                                        if (session.SessionBoard.Board[row, col] == 0)
                                        {
                                            session.SessionBoard.Board[row, col] = 2;
                                            break;
                                        }
                                    }
                                    if (CheckForWin(session.SessionBoard, row, col) && session.SessionEnd != true)
                                    {
                                        session.SessionWinner = session.Player2;
                                        session.SessionEnd = true;
                                        embed.AddField("Winner", $"{session.SessionWinner.Name}");
                                        embed.Color = new DiscordColor(session.SessionWinner.PlayerColor);
                                        gameEnd = true;
                                    }
                                    else if (session.SessionEnd == true)
                                        gameEnd = true;
                                    if (session.PlayerTurn == 1 && gameEnd == false)
                                        session.PlayerTurn++;
                                    else if (session.PlayerTurn == 2 && gameEnd == false)
                                        session.PlayerTurn--;
                                }
                                catch
                                { }
                            }
                        }
                        catch { }
                        await response.Result.DeleteAsync();
                        break;
                }
                if (session.PlayerTurn == 2 && gameEnd == false)
                    embed.AddField("Current Turn:", $"{session.Player2.Name}");
                else if (session.PlayerTurn == 1 && gameEnd == false)
                    embed.AddField("Current Turn:", $"{session.Player1.Name}");
                embed.Description = session.SessionBoard.ConstructDisplay(session.SessionBoard.Board);
                var builtEmbed = embed.Build();
                session.P1Display = await session.P1Display.ModifyAsync(embed: builtEmbed);
                if (session.P1Channel != session.P2Channel)
                    session.P2Display = await session.P2Display.ModifyAsync(embed: builtEmbed);
            }
            switch (session.IsPublic)
            {
                case true:
                    PublicSessions.Remove(session.SessionKey);
                    break;
                //For private sessions
                case false:
                    PrivateSessions.Remove(session.SessionKey);
                    break;
            }
        }
        //Takes most recent inputs of row and col
        //Made at like 2 AM so ingore horrible code
        private bool CheckForWin(Connect4Board board, byte row, int col)
        {
            var tile = board.Board[row, col];
            if ((CheckDiagnols(board, row, col, tile, 1) == 4) || (CheckCross(board, row, col, tile, 1) == 4))
            {
                return true;
            }
            return false;
        }
        private byte CheckDiagnols(Connect4Board board, byte row, int col, byte playerTurn, byte totalConnected)
        {
            var connectedDirect1 = totalConnected;
            var connectedDirect2 = totalConnected;
            for (int r = -1; r < 2; r += 2)
            {
                for (int c = -1; c < 2; c += 2)
                {
                    try
                    {
                        for (int l = 1; l < 4; l++)
                        {
                            var indexR = row + (r * l);
                            var indexC = col + (c * l);
                            if (board.Board[indexR, indexC] == playerTurn)
                            {
                                if ((r == -1 && c == -1) || (r == 1 && c == 1))
                                    connectedDirect1++;
                                else
                                    connectedDirect2++;
                            }
                        }
                    }
                    catch { }
                }
                if (connectedDirect1 == 4 || connectedDirect2 == 4)
                    return 4;
            }
            return totalConnected;
        }
        private byte CheckCross(Connect4Board board, byte row, int col, byte playerTurn, byte totalConnected)
        {
            var connected = totalConnected;
            for (int r = -1; r < 2; r += 2)
            {
                try
                {
                    for (int l = 1; l < 4; l++)
                    {
                        var index = row + (r * l);
                        if (board.Board[index, col] == playerTurn)
                        {
                            connected++;
                        }
                        else
                            break;
                    }
                }
                catch { }
                if (connected == 4)
                    return 4;
            }
            connected = totalConnected;
            for (int c = -1; c < 2; c += 2)
            {
                try
                {
                    for (int l = 1; l < 4; l++)
                    {

                        var index = col + (c * l);
                        if (board.Board[row, index] == playerTurn)
                        {
                            connected++;
                        }
                        else
                            break;
                    }
                }
                catch { }
                if (connected == 4)
                    return 4;
            }
            return totalConnected;
        }
    }
}
