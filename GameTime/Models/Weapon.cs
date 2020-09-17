using System;

namespace GameTime.Models
{
    public class Weapon : Item
    {
        public int HighestDamage { get; set; }
        public int LowestDamage { get; set; } = -1;
        public int FireRate { get; set; }
        public WeaponType WeaponType { get; set; }
        public Ammunition Ammo { get; set; } = Ammunition.None;
        public TimeSpan Cooldown { get; set; }
        public Special Special { get; set; } = Special.None;

    }
}