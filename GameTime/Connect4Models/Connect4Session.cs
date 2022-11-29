using System;
using System.Collections.Generic;
using DSharpPlus.Entities;
using DSharpPlus.CommandsNext;
using GameTime.MultiplayerSessionModels;
using GameTime.Databases;

namespace GameTime.Connect4Models
{
    public class Connect4Session : GameSession
    {
        public override List<CommandContext> Players { get; protected set; } = new List<CommandContext>();
        public override DiscordUser CurrentTurn { get; protected set; }
        public override DiscordUser Winner { get; protected set; }
        public override DiscordEmbed Display { get; protected set; }
        public Connect4Board Board { get; protected set; } = new Connect4Board();
        public override Status GameStatus { get; protected set; } = Status.Open;
        public override string SessionId { get; protected set; }
        public override bool IsPrivate { get; protected set; }
        public override int PlayerLimit { get; protected set; }
        protected override int PlayerNextIndex { get; set; }
        public Connect4Session(bool isPrivateSession)
        {
            IsPrivate = isPrivateSession;
            SessionId = Bot.GameSessions.GenerateGameId("cn4");
        }
        public Connect4Session(bool isPrivateSession, short playerLimit)
        {
            IsPrivate = isPrivateSession;
            PlayerLimit = playerLimit;
            SessionId = Bot.GameSessions.GenerateGameId("cn4");
        }
        public override bool Join(CommandContext player)
        {
            if(Players.Count < PlayerLimit)
            {
                Players.Add(player);
                Bot.GameSessions.AddPlayerInSession(player.User.Id);
                if (Players.Count == PlayerLimit)
                    Start();
                return true;
            }
            return false;
        }
        public override void Start()
        {
            CurrentTurn = Players[0].User;
            GameStatus = Status.Close;
        }
        public override GameSession SetStatus(Status status)
        {
            GameStatus = status;
            return this;
        }
        public override bool MakeMove(string args)
        {
            try
            {
                return PlaceChip(Int32.Parse(args) - 1);
            }
            catch
            {
                return false;
            }
        }
        //Also checks for win every move
        public bool PlaceChip(int col)
        {
            var placed = Board.PlaceChip(PlayerNextIndex, col, out bool win);
            if(!placed)
            {
                return placed;
            }
            else if(win)
            {
                Winner = Players[PlayerNextIndex].User;
                GameStatus = Status.Completed;
            }
            UpdateTurn();
            return placed;
        }
        private void UpdateTurn()
        {
            PlayerNextIndex++;
            if (PlayerNextIndex == Players.Count)
            {
                PlayerNextIndex = 0;
            }
            CurrentTurn = Players[PlayerNextIndex].User;
        }

        public override bool PlayersInSameChannel(ulong channelId)
        {
            var totalPlayers = 0;
            for(int i = 0; i < Players.Count; i++)
            {
                if(channelId == Players[i].Channel.Id)
                    totalPlayers++;
            }
            return totalPlayers > 1;
        }

        //Display handlers
        public override DiscordEmbed GameDisplay(string status)
        {
            var players = "";
            for (int i = 0; i < Players.Count; i++)
            {
                players += (CurrentTurn.Id == Players[i].User.Id) ? $"**__{Players[i].User.Username}__**" : $"{Players[i].User.Username}";
                if (i + 1 < Players.Count)
                    players += " v ";
            }
            Display = new DiscordEmbedBuilder()
            {
                Title = $"Connect4 {players}",
                Description = "Enter which column you want to place your chip. The first person to get 4 chips in a row horizontally or diagonally wins. Type exit to exit anytime.",
                Color = DiscordColor.Blurple
            }.AddField("Status", status).AddField("Board", Board.ToString()).Build();
            return Display;
        }
        public override DiscordEmbed EndGameDisplay(bool winner)
        {
            var players = "";
            for (int i = 0; i < Players.Count; i++)
            {
                players += $"{Players[i].User.Username}";
                if (i + 1 < Players.Count)
                    players += " v ";
            }
            Display = new DiscordEmbedBuilder()
            {
                Title = $"Connect4 {Winner.Username} Won",
                Description = players,
                Color = winner ? DiscordColor.Green : DiscordColor.Red
            }.AddField("Board", Board.ToString()).Build();
            return Display;
        }
        public override DiscordEmbed GameStoppedDisplay(string reason)
        {
            var players = "";
            for (int i = 0; i < Players.Count; i++)
            {
                players += $"{Players[i].User.Username}";
                if (i + 1 < Players.Count)
                    players += " v ";
            }
            Display = new DiscordEmbedBuilder()
            {
                Title = $"Connect4 {players}",
                Description = reason,
                Color = DiscordColor.Orange
            }.AddField("Board", Board.ToString()).Build();
            return Display;
        }
    }
}
