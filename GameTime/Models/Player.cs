using DSharpPlus.EventArgs;
using LiteDB;
using System;
using System.Collections.Generic;

namespace GameTime.Models
{
    public class Player
    {
        [BsonId]
        public long ID { get; set; }
        public string Name { get; set; }
        public int Health { get; set; } = 100;
        public decimal Balance { get; set; } = 0.00m;
        public List<Item> Inventory { get; set; } = new List<Item>();
        public DateTime CooldownStartTime { get; set; } = DateTime.Now;
        public DateTime TimeNow { get; set; }
        public DateTime CooldownEndTime { get; set; }
        public TimeSpan CooldownLength { get; set; } = TimeSpan.FromSeconds(0);
        public DateTime HealCooldownStart { get; set; } = DateTime.Now;
        public TimeSpan HealCooldownLength { get; set; } = TimeSpan.FromSeconds(0);
        public DateTime HealCooldownEnd { get; set; }
        public int Protection { get; set; } = 0;
        public string ProtectionName { get; set; } = "None";
        public Attachment ActiveAttachment { get; set; } = null;
        public Utility ActiveUtility { get; set; } = null;
        public int RadarUse { get; set; } = 0;
        public TimeSpan HourCooldown { get; set; } = TimeSpan.FromHours(0);
        public DateTime CooldownHourEndTime { get; set; }
        public DateTime CooldownHourStartTime { get; set; } = DateTime.Now;
        public TimeSpan OptCooldown { get; set; } = TimeSpan.FromHours(0);
        public DateTime OptCooldownStart { get; set; }
        public DateTime OptCooldownEnd { get; set; }
        public TimeSpan MonthlyCooldown { get; set; } = TimeSpan.FromDays(0);
        public DateTime MonthlyCooldownEnd { get; set; }
        public DateTime MonthlyCooldownStart { get; set; } = DateTime.Now;
        public TimeSpan WeeklyCooldown { get; set; } = TimeSpan.FromDays(0);
        public DateTime WeeklyCooldownEnd { get; set; }
        public DateTime WeeklyCooldownStart { get; set; } = DateTime.Now;
        public TimeSpan DailyCooldown { get; set; } = TimeSpan.FromHours(0);
        public DateTime DailyCooldownEnd { get; set; }
        public DateTime DailyCooldownStart { get; set; } = DateTime.Now;
        public TimeSpan ScramblerCooldown { get; set; }
        public DateTime ScramblerStart { get; set; }
        public DateTime ScramblerEnd { get; set; }
        public TimeSpan DotPuzzleCooldown { get; set; }
        public DateTime DotPuzzleStart { get; set; }
        public DateTime DotPuzzleEnd { get; set; }
        public bool IsBanned { get; set; } = false;
        public bool GameCooldownIgnore { get; set; } = false;
        public bool TradeBan { get; set; } = false;
        public List<long> GuildsOptedIn { get; set; } = new List<long>();
        public bool AuthroizedAdmin { get; set; } = false;
        public bool AuthroizedMod { get; set; } = false;
        public Player()
        {
            Inventory.Add(Crate.StarterItem);
        }
    }
}