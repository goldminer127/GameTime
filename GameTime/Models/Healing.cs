using System;

namespace GameTime.Models
{
    public class Healing : Item
    {
        public int MaxHealingRate { get; set; }
        public int LowestHealingRate { get; set; } = -1;
        public TimeSpan Cooldown { get; set; }
    }
}