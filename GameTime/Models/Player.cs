using LiteDB;
using System;
using System.Collections.Generic;
using DSharpPlus.Entities;

namespace GameTime.Models
{
    public class Player
    {
        //General Info
        [BsonId]
        public long ID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int Health { get; set; } = 100;
        public decimal Balance { get; set; } = 0.00m;
        public List<Item> Inventory { get; set; } = new List<Item>();
        public List<string> Metals { get; set; } = new List<string>();
        public DateTime TimeNow { get; set; }
        public int Protection { get; set; } = 0;
        public string ProtectionName { get; set; } = "None";
        public Attachment ActiveAttachment { get; set; } = null;
        public Utility ActiveUtility { get; set; } = null;
        public bool IsDonor { get; set; } = false;
        public int Kills { get; set; } = 0;
        public int Deaths { get; set; } = 0;
        public DateTime Joined { get; set; } = DateTime.Now;
        public bool IsBanned { get; set; } = false;
        public bool GameCooldownIgnore { get; set; } = false;
        public bool TradeBan { get; set; } = false;
        public int Warnings { get; set; }
        public List<long> GuildsOptedIn { get; set; } = new List<long>();
        public bool InMinigame { get; set; } = false;
        public string NameOfMinigame { get; set; } = "";
        public string PlayerColor { get; set; } = DiscordColor.Blurple.ToString();
        public List<Mail> Mail { get; set; } = new List<Mail>(); //new
        public bool AcceptMail { get; set; } = true;

        //Weapon Cooldown
        public DateTime CooldownStartTime { get; set; } = DateTime.Now;
        public DateTime CooldownEndTime { get; set; }
        public TimeSpan CooldownLength { get; set; } = TimeSpan.FromSeconds(0);

        //Heal Cooldown
        public DateTime HealCooldownStart { get; set; } = DateTime.Now;
        public TimeSpan HealCooldownLength { get; set; } = TimeSpan.FromSeconds(0);
        public DateTime HealCooldownEnd { get; set; }

        //Utility Status
        public int RadarUse { get; set; } = 0;

        //Hour Cooldown
        public TimeSpan HourCooldown { get; set; } = TimeSpan.FromHours(0);
        public DateTime CooldownHourEndTime { get; set; }
        public DateTime CooldownHourStartTime { get; set; } = DateTime.Now;

        //Monthly Cooldowon
        public TimeSpan MonthlyCooldown { get; set; } = TimeSpan.FromDays(0);
        public DateTime MonthlyCooldownEnd { get; set; }
        public DateTime MonthlyCooldownStart { get; set; } = DateTime.Now;

        //Weekly Cooldown
        public TimeSpan WeeklyCooldown { get; set; } = TimeSpan.FromDays(0);
        public DateTime WeeklyCooldownEnd { get; set; }
        public DateTime WeeklyCooldownStart { get; set; } = DateTime.Now;

        //Daily Cooldown
        public TimeSpan DailyCooldown { get; set; } = TimeSpan.FromHours(0);
        public DateTime DailyCooldownEnd { get; set; }
        public DateTime DailyCooldownStart { get; set; } = DateTime.Now;

        //Scrambler Cooldown
        public TimeSpan ScramblerCooldown { get; set; }
        public DateTime ScramblerStart { get; set; }
        public DateTime ScramblerEnd { get; set; }

        //Connectx Cooldown
        public TimeSpan DotPuzzleCooldown { get; set; }
        public DateTime DotPuzzleStart { get; set; }
        public DateTime DotPuzzleEnd { get; set; }

        //Optin and out cooldown
        public TimeSpan OptCooldown { get; set; } = TimeSpan.FromHours(0);
        public DateTime OptCooldownStart { get; set; }
        public DateTime OptCooldownEnd { get; set; }

        //Authority Status
        public bool AuthroizedAdmin { get; set; } = false;
        public bool AuthroizedMod { get; set; } = false;

        //Minesweeper Cooldowns
        public TimeSpan MinesweeperCooldown { get; set; }
        public DateTime MinesweeperStart { get; set; }
        public DateTime MinesweeperEnd { get; set; }
        
        //Minigame Stats (New)
        public int TotalMinesweeperPlayed { get; set; }
        public int TotalMinesweeperWon { get; set; }
        public int TotalConnectxPlayed { get; set; }
        public int TotalConnectxWon { get; set; }
        public int TotalScramblerPlayed { get; set; }
        public int TotalScramblerWon { get; set; }
        public int TotalCasinoCreditsWon { get; set; }
        public int TotalCasinoCreditsLost { get; set; }
        
        //Casino
        public long Credits { get; set; }
        public Player()
        {
            Inventory.Add(Crate.StarterItem);
        }
    }
}