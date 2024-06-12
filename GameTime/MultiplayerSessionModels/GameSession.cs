using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using GameTime.MultiplayerSessionModels.Enums;
using System.Runtime.InteropServices;

namespace GameTime.MultiplayerSessionModels
{
    public abstract class GameSession
    {
        /// <summary>
        /// Amount of players allowed to join.
        /// </summary>
        protected abstract int PlayerLimit { get; set; }
        /// <summary>
        /// Session status.
        /// </summary>
        public abstract Status GameStatus { get; protected set; }
        /// <summary>
        /// All players within a specific game session.
        /// </summary>
        public abstract List<MinigamePlayer> Players { get; protected set; }
        /// <summary>
        /// Current player index.
        /// </summary>
        public abstract int CurrentPlayer { get; protected set; }
        /// <summary>
        /// Index of winner.
        /// </summary>
        public abstract int Winner { get; protected set; }
        /// <summary>
        /// Game session Id.
        /// </summary>
        public abstract string SessionId { get; protected set; }
        /// <summary>
        /// Joinable game only via code.
        /// </summary>
        public abstract bool IsPrivate { get; protected set; }
        /// <summary>
        /// Common embed display universal for both players.
        /// </summary>
        public abstract DiscordEmbed GameDisplay { get; protected set; }
        /// <summary>
        /// Stores last sent common display. Used for removing duplicate displays.
        /// </summary>
        public abstract DiscordMessage PreviousMessage { get; protected set; }
        /// <summary>
        /// Use this to check when ANY players are in the same channel. Each game will handle this scenario differently.
        /// </summary>
        public abstract bool SameChannel { get; protected set; }
        /// <summary>
        /// Adds player to game session.
        /// </summary>
        /// <param name="playerCtx">Context of player joining.</param>
        /// <returns>Returns status code of player joining.</returns>
        public abstract JoinStatus Join(CommandContext playerCtx);
        /// <summary>
        /// Initializing remaining variables to start a running game.
        /// </summary>
        public abstract void Start();
        /// <summary>
        /// Used by a player to exit the game. If used in 2 player games, the game will end.
        /// </summary>
        /// <param name="playerId">Id of the player exiting.</param>
        /// <param name="reason">Optional reason to exit.</param>
        public abstract void Exit(ulong playerId, [Optional] string reason);
        /// <summary>
        /// Closes the game upon exit or completion
        /// </summary>
        protected abstract void CloseGame();
        /// <summary>
        /// Allows player to make a single move for a game.
        /// </summary>
        /// <param name="args">args for move.</param>
        /// <returns>Returns status of move.</returns>
        public abstract MoveStatus MakeMove(string args, ulong playerId);
        /// <summary>
        /// Handles rotating turns.
        /// </summary>
        protected abstract void TurnHandler();
        /// <summary>
        /// Constructs displays for each individual players.
        /// </summary>
        /// <param name="status">Status regarding the stage of the game. Addresses invalid moves or any issues regarding a move.</param>
        /// <param name="reason">Tags the reason for a game to have stopped.</param>
        protected abstract void ConstructGameDisplay(string status, [Optional]string reason);
        /// <summary>
        /// Constructs display for the game session, shares common view for all players.
        /// </summary>
        /// <param name="status">Status regarding the stage of the game. Addresses invalid moves or any issues regarding a move.</param>
        /// <param name="reason">Tags the reason for a game to have stopped.</param>
        protected abstract void ConstructCommonGameDisplay(string status, [Optional]string reason);
        protected abstract void SendDisplays();
    }
}
