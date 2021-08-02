using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using GameTime.Extensions;
using GameTime.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameTime.Connect4Models;

namespace GameTime.Commands
{
    public class Connect4Commands : BaseCommandModule
    {
        private Dictionary<string, Connect4Session> PublicSessions = new Dictionary<string, Connect4Session>();
        private Dictionary<string, Connect4Session> PrivateSessions = new Dictionary<string, Connect4Session>();
        [Command("connect4-join"), Aliases("cn4-join", "cn4j")]
        [Description("Matchmake for connect4")]
        public async Task Connect4Matchmake(CommandContext ctx, [RemainingText] string roomcode = null)
        {
            var user = Bot.PlayerDatabase.GetPlayerByID(Convert.ToInt64(ctx.User.Id));
            if (GeneralFunctions.ValidatePlayer(ctx, user, true))
            {
                user.InMinigame = true;
                if (roomcode == null)
                {
                    var sessionFound = false;
                    foreach (var session in PublicSessions)
                    {
                        if (session.Value.Player2 == null)
                        {
                            session.Value.Player2 = user;
                            session.Value.P2Channel = ctx.Channel;
                            sessionFound = true;
                            await PlaySession(ctx, session.Value, 2);
                        }
                    }
                    if (sessionFound == false)
                    {
                        await CreatePublicSession(ctx, user);
                    }
                }
            }
            else { }
        }
        public async Task CreatePublicSession(CommandContext ctx, Player user)
        {
            var newSession = new Connect4Session();
            newSession.Player1 = user;
            newSession.P1Channel = ctx.Channel;
            PublicSessions.Add(newSession.SessionKey, newSession);
            await PlaySession(ctx, newSession, 1);
        }
        public async Task PlaySession(CommandContext ctx, Connect4Session session, byte player)
        {
            var embed = new DiscordEmbedBuilder();
            var gameEnd = false;
            embed.Title = "Matching...";
            await ctx.Channel.SendMessageAsync(embed: embed);
            while (session.Player2 == null)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                session = PublicSessions[session.SessionKey];
                if(session.Player2 != null)
                {
                    break;
                }
            }
            embed.Title = $"Connect4\n{session.Player1.Name}[1] vs {session.Player2.Name}[2]";
            embed.Description = session.SessionBoard.ConstructDisplay(session.SessionBoard.Board);
            embed.AddField("Current Turn:", $"{session.Player1.Name}");
            embed.WithFooter(session.SessionKey);
            switch (player)
            {
                case 1:
                    session.P1Display = await session.P1Channel.SendMessageAsync(embed: embed);
                    break;
                case 2:
                    session.P2Display = await session.P2Channel.SendMessageAsync(embed: embed);
                    break;
            }
            while (gameEnd == false && session.SessionEnd == false)
            {
                while (session.PlayerTurn != player)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                    try
                    {
                        session = PublicSessions[session.SessionKey];
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
                var interactivity = ctx.Client.GetInteractivity();
                DSharpPlus.Interactivity.InteractivityResult<DiscordMessage> response;
                switch (session.PlayerTurn)
                {
                    case 1:
                        response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && Convert.ToInt64(x.Author.Id) == session.Player1.ID && (Convert.ToInt32(x.Content) > 0 && Convert.ToInt32(x.Content) < 8), TimeSpan.FromSeconds(60));
                        if (response.TimedOut)
                        {
                            gameEnd = true;
                        }
                        else
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
                            }
                            catch
                            {
                            }
                        }
                        break;
                    case 2:
                        response = await interactivity.WaitForMessageAsync(x => x.Channel == ctx.Channel && Convert.ToInt64(x.Author.Id) == session.Player2.ID && (Convert.ToInt32(x.Content) > 0 && Convert.ToInt32(x.Content) < 8), TimeSpan.FromSeconds(60));
                        if (response.TimedOut)
                        {
                            gameEnd = true;
                        }
                        else
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
                            }
                            catch
                            { }
                        }
                        break;
                }
                if (session.PlayerTurn == 1)
                {
                    session.PlayerTurn++;
                    embed.AddField("Current Turn:", $"{session.Player2.Name}");
                }
                else
                {
                    session.PlayerTurn--;
                    embed.AddField("Current Turn:", $"{session.Player1.Name}");
                }
                embed.Description = session.SessionBoard.ConstructDisplay(session.SessionBoard.Board);
                var builtEmbed = embed.Build();
                switch (player)
                {
                    case 1:
                        session.P1Display = await session.P1Display.ModifyAsync(embed: builtEmbed);
                        session.P2Display = await session.P2Display.ModifyAsync(embed: builtEmbed);
                        break;
                    case 2:
                        session.P2Display = await session.P2Display.ModifyAsync(embed: builtEmbed);
                        session.P1Display = await session.P1Display.ModifyAsync(embed: builtEmbed);
                        break;
                }
            }
            PublicSessions.Remove(session.SessionKey);
            await ctx.Channel.SendMessageAsync($"Test Done, {((session.PlayerTurn == 1) ? session.SessionWinner.Name : session.SessionWinner.Name)}");
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
            for (int r = -1; r < 2; r += 2)
            {
                var connectedDirect1 = totalConnected;
                var connectedDirect2 = totalConnected;
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
