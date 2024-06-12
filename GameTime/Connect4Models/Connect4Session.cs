using System;
using System.Linq;
using System.Collections.Generic;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using GameTime.MultiplayerSessionModels;
using GameTime.Databases;
using GameTime.MultiplayerSessionModels.Enums;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using static System.Collections.Specialized.BitVector32;

namespace GameTime.Connect4Models
{
    public class Connect4Session : GameSession
    {
        protected override int PlayerLimit { get; set; }
        public override Status GameStatus { get; protected set; } = Status.Open;
        public override List<MinigamePlayer> Players { get; protected set; } = new List<MinigamePlayer>();
        public override int CurrentPlayer { get; protected set; }
        public override int Winner { get; protected set; } = -1;
        public Connect4Board Board { get; protected set; } = new Connect4Board();
        public override string SessionId { get; protected set; }
        public override bool IsPrivate { get; protected set; }
        public override DiscordEmbed GameDisplay { get; protected set; }
        public override DiscordMessage PreviousMessage { get; protected set; }
        public override bool SameChannel { get; protected set; } = false;
        public Connect4Session(bool isPrivateSession)
        {
            IsPrivate = isPrivateSession;
            SessionId = Bot.GameSessions.AddSession(this, "cn4");
        }
        public Connect4Session(bool isPrivateSession, short playerLimit)
        {
            IsPrivate = isPrivateSession;
            PlayerLimit = playerLimit;
            SessionId = Bot.GameSessions.AddSession(this, "cn4");
        }
        public override JoinStatus Join(CommandContext player)
        {
            if(Players.Count < PlayerLimit)
            {
                Players.Add(new MinigamePlayer(player.User.Username, player.User.Id, player.Channel.Id));
                Bot.GameSessions.AddPlayerInSession(player.User.Id);
                if (Players.Count == PlayerLimit)
                    Start();
                return JoinStatus.Success;
            }
            return JoinStatus.PlayerLimitReached;
        }
        public override void Start()
        {
            CurrentPlayer = 0;
            GameStatus = Status.Close;
            foreach (var player in Players)
                if (Players.Where(p => p.Channel == player.Channel).Count() > 1)
                    SameChannel = true;
            if (SameChannel)
                ConstructCommonGameDisplay("No problem.");
            else
                ConstructGameDisplay("No problem.");
            SendDisplays();
        }
        public override void Exit(ulong playerId, [Optional] string reason)
        {
            if (GameStatus == Status.Close)
            {
                if (SameChannel)
                    ConstructCommonGameDisplay("No problem.", reason ?? $"{Players.Single(player => player.Id == playerId).Name} exited the game.");
                else
                    ConstructGameDisplay("No problem.", reason ?? $"{Players.Single(player => player.Id == playerId).Name} exited the game.");
                SendDisplays();
            }
            GameStatus = Status.Exited;
            CloseGame();
        }
        protected override void CloseGame()
        {
            for (int i = 0; i < Players.Count; i++)
                Bot.GameSessions.RemovePlayerInSession(Players[i].Id);
            Bot.GameSessions.RemoveSession(this);
        }
        public override MoveStatus MakeMove(string args, ulong playerId)
        {
            if (args != "exit" && Players[CurrentPlayer].Id != playerId)
                return MoveStatus.InvalidTurn;
            else if (args == "exit")
            {
                Exit(playerId);
                return MoveStatus.Success;
            }
            else
            {
                try
                {
                    var result = PlaceChip(Int32.Parse(args) - 1);
                    if (result == MoveStatus.Success)
                        TurnHandler();
                    if (SameChannel)
                        ConstructCommonGameDisplay((result == MoveStatus.InvalidMove) ? $"Attempted move {args} by {Players[CurrentPlayer].Name} was invalid." : "No problem.");
                    else
                        ConstructGameDisplay((result == MoveStatus.InvalidMove) ? $"Attempted move {args} by {Players[CurrentPlayer].Name} was invalid." : "No problem.");
                    SendDisplays();
                    if(Winner != -1)
                        CloseGame();
                    return result;
                }
                catch (Exception e)
                {
                    if (e is ArgumentNullException || e is FormatException || e is OverflowException)
                        return MoveStatus.InvalidArgs;
                    else
                        return MoveStatus.MoveErrored;
                }
            }
        }
        //Also checks for win every move
        private MoveStatus PlaceChip(int col)
        {
            var placed = Board.PlaceChip(CurrentPlayer, col, out bool win);
            if(win)
            {
                Winner = CurrentPlayer;
                GameStatus = Status.Completed;
            }
            return placed;
        }
        protected override void TurnHandler()
        {
            CurrentPlayer = (CurrentPlayer + 1) % Players.Count;
        }
        protected override void ConstructGameDisplay(string status, [Optional]string reason)
        {
            var players = "";
            for (int i = 0; i < Players.Count; i++)
            {
                players += (Players[CurrentPlayer].Id == Players[i].Id && Winner == -1) ? $"**__{Players[i].Name}__**" : $"{Players[i].Name}";
                if (i + 1 < Players.Count)
                    players += " v ";
            }
            for(int i = 0; i < Players.Count; i++)
            {
                Players[i].Display = new DiscordEmbedBuilder()
                {
                    Title = (Winner == -1) ? $"Connect4 {players}" : $"Connect4 {Players[Winner].Name} Won",
                    Description = reason ?? "Enter which column you want to place your chip. The first person to get 4 chips in a row horizontally or diagonally wins. Type exit to exit anytime.",
                    Color = (reason != null) ? DiscordColor.Yellow : (Winner == -1) ? DiscordColor.Blurple : (Winner == i) ? DiscordColor.Green : DiscordColor.Red,
                    Footer = (reason == null) ? null : new DiscordEmbedBuilder.EmbedFooter() { Text = "You will be timed out after 1 minute of not making a move."}
                }.AddField("Status", status).AddField("Board", Board.ToString()).Build();
            }
        }
        protected override void ConstructCommonGameDisplay(string status, [Optional] string reason)
        {
            var players = "";
            for (int i = 0; i < Players.Count; i++)
            {
                players += (Players[CurrentPlayer].Id == Players[i].Id && Winner == -1) ? $"**__{Players[i].Name}__**" : $"{Players[i].Name}";
                if (i + 1 < Players.Count)
                    players += " v ";
            }
            GameDisplay = new DiscordEmbedBuilder()
            {
                Title = (Winner == -1) ? $"Connect4 {players}" : $"Connect4 {Players[Winner].Name} Won",
                Description = reason ?? "Enter which column you want to place your chip. The first person to get 4 chips in a row horizontally or diagonally wins. Type exit to exit anytime.",
                Color = (reason != null) ? DiscordColor.Yellow : (Winner == -1) ? DiscordColor.Blurple : DiscordColor.Green, //Green for has winner
                Footer = (reason == null) ? null : new DiscordEmbedBuilder.EmbedFooter() { Text = "You will be timed out after 1 minute of not making a move." }
            }.AddField("Status", status).AddField("Board", Board.ToString()).Build();
        }
        protected override void SendDisplays()
        {
            if(PreviousMessage != null)
            {
                PreviousMessage.DeleteAsync();
                PreviousMessage = null;
            }
            if (SameChannel)
                PreviousMessage = Bot.Client.GetChannelAsync(Players[CurrentPlayer].Channel).Result.SendMessageAsync(GameDisplay).Result;
            else
                foreach (var player in Players)
                {
                    if (player.PreviousDisplay != null)
                    {
                        player.PreviousDisplay.DeleteAsync();
                        player.PreviousDisplay = null;
                    }
                    player.PreviousDisplay = Bot.Client.GetChannelAsync(player.Channel).Result.SendMessageAsync(player.Display).Result;
                }
        }
    }
}