using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;

namespace GameTime.MultiplayerSessionModels
{
    public abstract class GameSession
    {
        public abstract List<CommandContext> Players { get; protected set; }
        public abstract DiscordUser CurrentTurn { get; protected set; }
        public abstract DiscordUser Winner { get; protected set; }
        public abstract DiscordEmbed Display { get; protected set; }
        public abstract Status GameStatus { get; protected set; }
        public abstract string SessionId { get; protected set; }
        public abstract bool IsPrivate { get; protected set; }
        public abstract int PlayerLimit { get; protected set; }
        protected abstract int PlayerNextIndex { get; set; }
        protected abstract void GenerateGameId();
        public abstract void Join(CommandContext playerCtx);
        public abstract void Start();
        public abstract bool MakeMove(string args);
        /*Use this to check when both players are in the same channel. The bot will send 1 display message rather
          than 2 messages in the same channel. */
        public abstract bool PlayersInSameChannel(ulong channelId);
        public abstract GameSession SetStatus(Status status);
        public abstract DiscordEmbed GameDisplay(string status);
        public abstract DiscordEmbed EndGameDisplay(bool winner);
        public abstract DiscordEmbed GameStoppedDisplay(string reason);
    }
}
