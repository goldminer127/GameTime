using GameTime.DotPuzzleModels;
using GameTime.ScramblerModels;
using System;

namespace GameTime.Models
{
    public class Crate : Item
    {

        public Crate()
        {
        }

        public static Item StarterItem
        {
            get
            {
                return new Utility() { ID = 101, Name = "Flare", Rarity = Rarity.Unique, Value = 35743.88m, Uses = 1, Special = Special.SupplyCall };
            }
        }

        private static Item GetRandomCommonItem() //Contains: common, uncommon, rare, epic
        {
            var rNum = new Random().Next(1, 301);
            return rNum switch
            {
                1 => new Weapon() { ID = 92, Name = "TAC-50", Subname = "Tac", Rarity = Rarity.Epic, Value = 44830.46m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B50BMG, HighestDamage = 90, Cooldown = TimeSpan.FromMinutes(240.875) },
                2 => new Weapon() { ID = 34, Name = "Staff", Rarity = Rarity.Common, Value = 428.9m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 4, Cooldown = TimeSpan.FromMinutes(7), IsPurchaseable = true },
                3 => new Weapon() { ID = 15, Name = "Bat", Rarity = Rarity.Common, Value = 323.43m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 3, Cooldown = TimeSpan.FromMinutes(6.25), IsPurchaseable = true },
                4 => new Healing() { ID = 14, Name = "Alcohol Wipe", Subname = "Alcohol", Rarity = Rarity.Common, Value = 205.34m, MaxHealingRate = 2, Cooldown = TimeSpan.FromMinutes(2.3), IsPurchaseable = true },
                5 => new Weapon() { ID = 71, Name = "G36C", Subname = "G36", Rarity = Rarity.Uncommon, Value = 4156.87m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B556mm, HighestDamage = 40, LowestDamage = 10, Cooldown = TimeSpan.FromMinutes(64.925) },
                6 => new Ammo() { ID = 75, Name = "Ball Round", Subname = "Ball", Rarity = Rarity.Uncommon, Value = 3614.95m, Type = Ammunition.BallRound },
                7 => new Ammo() { ID = 22, Name = "12 Gauge", Subname = "12", Rarity = Rarity.Common, Value = 1036.71m, Type = Ammunition.B12Gauge, },
                8 => new Weapon() { ID = 18, Name = "Razor", Rarity = Rarity.Common, Value = 534.37m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 5, Cooldown = TimeSpan.FromMinutes(7.75), IsPurchaseable = true },
                9 => new Healing() { ID = 78, Name = "Blood Bag", Subname = "Blood", Rarity = Rarity.Epic, Value = 14784m, MaxHealingRate = 90, LowestHealingRate = 70, Cooldown = TimeSpan.FromMinutes(92) },
                10 => new Weapon() { ID = 87, Name = "Howa 89", Subname = "Howa", Rarity = Rarity.Rare, Value = 18134.37m, WeaponType = WeaponType.Firearm, FireRate = 4, Ammo = Ammunition.B556mm, HighestDamage = 16, LowestDamage = 8, Cooldown = TimeSpan.FromMinutes(94.25) },
                11 => new Weapon() { ID = 23, Name = "Paint Gun", Subname = "PaintGun", Rarity = Rarity.Common, Value = 275.39m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.Paintball, HighestDamage = 2, LowestDamage = 0, Cooldown = TimeSpan.FromMinutes(1.73) },
                12 => new Money() { ID = 19, Name = "Wallet", Rarity = Rarity.Common, Value = 200, HighestAmount = 500m, LowestAmount = 100m, IsSellable = false },
                13 => new Ammo() { ID = 103, Name = "7.92mm", Subname = "792", Rarity = Rarity.Rare, Value = 7933.59m, Type = Ammunition.B792 },
                14 => new Weapon() { ID = 34, Name = "Staff", Rarity = Rarity.Common, Value = 428.9m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 4, Cooldown = TimeSpan.FromMinutes(7), IsPurchaseable = true },
                15 => new Weapon() { ID = 89, Name = "Bayonet", Subname = "Bay", Rarity = Rarity.Rare, Value = 4825.62m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 30, LowestDamage = 25, Cooldown = TimeSpan.FromMinutes(60.5) },
                16 => new Attachment() { ID = 28, Name = "Mag Grip", Subname = "Grip", Rarity = Rarity.Common, Value = 937.44m, IsActive = false, Ability = Addon.ReduceCooldown, Intensity = 2 },
                17 => new Ammo() { ID = 26, Name = "Plastic Pellet", Subname = "Pellet", Rarity = Rarity.Common, Value = 196.44m, Type = Ammunition.PlasticPellet },
                18 => new Ammo() { ID = 25, Name = "Paintball", Subname = "Paint", Rarity = Rarity.Common, Value = 231.84m, Type = Ammunition.Paintball },
                19 => new Weapon() { ID = 24, Name = "Sawed-off", Subname = "Sawedoff", Rarity = Rarity.Common, Value = 2250m, WeaponType = WeaponType.Firearm, FireRate = 12, HighestDamage = 2, LowestDamage = 0, Ammo = Ammunition.B12Gauge, Cooldown = TimeSpan.FromMinutes(43.37), IsPurchaseable = true },
                20 => new Weapon() { ID = 20, Name = "Remington", Rarity = Rarity.Rare, Value = 12421.87m, WeaponType = WeaponType.Firearm, FireRate = 6, HighestDamage = 15, LowestDamage = 1, Ammo = Ammunition.B12Gauge, Cooldown = TimeSpan.FromMinutes(151.5) },
                21 => new Weapon() { ID = 21, Name = "Stevens 555", Subname = "Stevens", Rarity = Rarity.Common, Value = 3304.68m, WeaponType = WeaponType.Firearm, FireRate = 12, HighestDamage = 3, LowestDamage = 0, Ammo = Ammunition.B12Gauge, Cooldown = TimeSpan.FromMinutes(56.03), IsPurchaseable = true },
                22 => new Weapon() { ID = 50, Name = "Firecracker", Subname = "Fire", Rarity = Rarity.Common, Value = 300.24m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 5, LowestDamage = 0, Cooldown = TimeSpan.FromMinutes(9.5), IsPurchaseable = true },
                23 => new Weapon() { ID = 27, Name = "Pool Cue", Subname = "Pool", Rarity = Rarity.Common, Value = 323.43m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 3, Cooldown = TimeSpan.FromMinutes(6.25), IsPurchaseable = true },
                24 => new Ammo() { ID = 26, Name = "Plastic Pellet", Subname = "Pellet", Rarity = Rarity.Common, Value = 196.44m, Type = Ammunition.PlasticPellet },
                25 => new Weapon() { ID = 34, Name = "Staff", Rarity = Rarity.Common, Value = 428.9m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 4, Cooldown = TimeSpan.FromMinutes(7), IsPurchaseable = true },
                26 => new Weapon() { ID = 34, Name = "Staff", Rarity = Rarity.Common, Value = 428.9m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 4, Cooldown = TimeSpan.FromMinutes(7), IsPurchaseable = true },
                27 => new Weapon() { ID = 27, Name = "Pool Cue", Subname = "Pool", Rarity = Rarity.Common, Value = 323.43m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 3, Cooldown = TimeSpan.FromMinutes(6.25), IsPurchaseable = true },
                28 => new Attachment() { ID = 28, Name = "Mag Grip", Subname = "Grip", Rarity = Rarity.Common, Value = 937.44m, IsActive = false, Ability = Addon.ReduceCooldown, Intensity = 2 },
                29 => new Weapon() { ID = 34, Name = "Staff", Rarity = Rarity.Common, Value = 428.9m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 4, Cooldown = TimeSpan.FromMinutes(7), IsPurchaseable = true },
                30 => new Attachment() { ID = 29, Name = "Vertical Grip", Subname = "Vertical", Rarity = Rarity.Common, Value = 1072.85m, IsActive = false, Ability = Addon.HighestDamageIncrease, Intensity = 1 },
                31 => new Ammo() { ID = 75, Name = "Ball Round", Subname = "Ball", Rarity = Rarity.Uncommon, Value = 3614.95m, Type = Ammunition.BallRound },
                32 => new Ammo() { ID = 81, Name = "7.62mm", Subname = "762", Rarity = Rarity.Rare, Value = 8921.87m, Type = Ammunition.B762mm },
                33 => new Weapon() { ID = 50, Name = "Firecracker", Subname = "Fire", Rarity = Rarity.Common, Value = 300.24m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 5, LowestDamage = 0, Cooldown = TimeSpan.FromMinutes(9.5), IsPurchaseable = true },
                34 => new Weapon() { ID = 34, Name = "Staff", Rarity = Rarity.Common, Value = 428.9m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 4, Cooldown = TimeSpan.FromMinutes(7), IsPurchaseable = true },
                35 => new Attachment() { ID = 29, Name = "Vertical Grip", Subname = "Vertical", Rarity = Rarity.Common, Value = 1072.85m, IsActive = false, Ability = Addon.HighestDamageIncrease, Intensity = 1 },
                36 => new Weapon() { ID = 27, Name = "Pool Cue", Subname = "Pool", Rarity = Rarity.Common, Value = 323.43m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 3, Cooldown = TimeSpan.FromMinutes(6.25), IsPurchaseable = true },
                37 => new Weapon() { ID = 51, Name = "P90", Subname = "90", Rarity = Rarity.Uncommon, Value = 4687.5m, WeaponType = WeaponType.Firearm, FireRate = 2, Ammo = Ammunition.B9mm, HighestDamage = 12, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(43.18), IsPurchaseable = true },
                38 => new Weapon() { ID = 52, Name = "Tomahawk", Subname = "Toma", Rarity = Rarity.Uncommon, Value = 1836.96m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, Cooldown = TimeSpan.FromMinutes(32.75), IsPurchaseable = true },
                39 => new Attachment() { ID = 28, Name = "Mag Grip", Subname = "Grip", Rarity = Rarity.Common, Value = 937.44m, IsActive = false, Ability = Addon.ReduceCooldown, Intensity = 2 },
                40 => new Money() { ID = 19, Name = "Wallet", Rarity = Rarity.Common, Value = 200, HighestAmount = 500m, LowestAmount = 100m, IsSellable = false },
                41 => new Attachment() { ID = 28, Name = "Mag Grip", Subname = "Grip", Rarity = Rarity.Common, Value = 937.44m, IsActive = false, Ability = Addon.ReduceCooldown, Intensity = 2 },
                42 => new Weapon() { ID = 77, Name = "M16A4", Subname = "M16", Rarity = Rarity.Rare, Value = 8850m, WeaponType = WeaponType.Firearm, FireRate = 3, Ammo = Ammunition.B556mm, HighestDamage = 14, LowestDamage = 7, Cooldown = TimeSpan.FromMinutes(95.156) },
                43 => new Healing() { ID = 16, Name = "Bandaid", Rarity = Rarity.Common, Value = 157.5m, MaxHealingRate = 1, Cooldown = TimeSpan.FromMinutes(1.15), IsPurchaseable = true },
                44 => new Weapon() { ID = 70, Name = "Broadsword", Subname = "Sword", Rarity = Rarity.Uncommon, Value = 1836.56m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, Cooldown = TimeSpan.FromMinutes(32.75), IsPurchaseable = true },
                45 => new Ammo() { ID = 104, Name = "B56mm", Subname = "56", Rarity = Rarity.Rare, Value = 9640.62m, Type = Ammunition.B56mm },
                46 => new Ammo() { ID = 26, Name = "Plastic Pellet", Subname = "Pellet", Rarity = Rarity.Common, Value = 196.44m, Type = Ammunition.PlasticPellet },
                47 => new Weapon() { ID = 56, Name = "Thompson", Subname = "Tom", Rarity = Rarity.Uncommon, Value = 5965.62m, WeaponType = WeaponType.Firearm, FireRate = 2, Ammo = Ammunition.B45APC, HighestDamage = 17, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(53.95), IsPurchaseable = true },
                48 => new Ammo() { ID = 25, Name = "Paintball", Subname = "Paint", Rarity = Rarity.Common, Value = 231.84m, Type = Ammunition.Paintball },
                49 => new Weapon() { ID = 18, Name = "Razor", Rarity = Rarity.Common, Value = 534.37m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 5, Cooldown = TimeSpan.FromMinutes(7.75), IsPurchaseable = true },
                50 => new Weapon() { ID = 50, Name = "Firecracker", Subname = "Fire", Rarity = Rarity.Common, Value = 300.24m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 5, LowestDamage = 0, Cooldown = TimeSpan.FromMinutes(9.5), IsPurchaseable = true },
                51 => new Weapon() { ID = 62, Name = "Javelin", Subname = "Jave", Rarity = Rarity.Uncommon, Value = 1713.12m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, LowestDamage = 10, Cooldown = TimeSpan.FromMinutes(33.55), IsPurchaseable = true },
                52 => new Weapon() { ID = 23, Name = "Paint Gun", Subname = "PaintGun", Rarity = Rarity.Common, Value = 275.39m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.Paintball, HighestDamage = 2, LowestDamage = 0, Cooldown = TimeSpan.FromMinutes(1.73) },
                53 => new Healing() { ID = 16, Name = "Bandaid", Rarity = Rarity.Common, Value = 157.5m, MaxHealingRate = 1, Cooldown = TimeSpan.FromMinutes(1.15), IsPurchaseable = true },
                54 => new Healing() { ID = 16, Name = "Bandaid", Rarity = Rarity.Common, Value = 157.5m, MaxHealingRate = 1, Cooldown = TimeSpan.FromMinutes(1.15), IsPurchaseable = true },
                55 => new Weapon() { ID = 60, Name = "Five-Seven", Subname = "FiveSeven", Rarity = Rarity.Uncommon, Value = 5139.06m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B57, HighestDamage = 20, LowestDamage = 15, Cooldown = TimeSpan.FromMinutes(40.257), IsPurchaseable = true },
                56 => new Ammo() { ID = 7, Name = "8mm", Subname = "8", Rarity = Rarity.Common, Value = 778.90m, Type = Ammunition.B8MM, },
                57 => new Weapon() { ID = 18, Name = "Razor", Rarity = Rarity.Common, Value = 534.37m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 5, Cooldown = TimeSpan.FromMinutes(7.75), IsPurchaseable = true },
                58 => new Weapon() { ID = 27, Name = "Pool Cue", Subname = "Pool", Rarity = Rarity.Common, Value = 323.43m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 3, Cooldown = TimeSpan.FromMinutes(6.25), IsPurchaseable = true },
                59 => new Healing() { ID = 16, Name = "Bandaid", Rarity = Rarity.Common, Value = 157.5m, MaxHealingRate = 1, Cooldown = TimeSpan.FromMinutes(1.15), IsPurchaseable = true },
                60 => new Attachment() { ID = 64, Name = "Mag Strap", Subname = "Strap", Rarity = Rarity.Uncommon, Value = 2251.79m, IsActive = false, Ability = Addon.ReduceCooldown, Intensity = 10 },
                61 => new Attachment() { ID = 29, Name = "Vertical Grip", Subname = "Vertical", Rarity = Rarity.Common, Value = 1072.85m, IsActive = false, Ability = Addon.HighestDamageIncrease, Intensity = 1 },
                62 => new Healing() { ID = 16, Name = "Bandaid", Rarity = Rarity.Common, Value = 157.5m, MaxHealingRate = 1, Cooldown = TimeSpan.FromMinutes(1.15), IsPurchaseable = true },
                63 => new Weapon() { ID = 27, Name = "Pool Cue", Subname = "Pool", Rarity = Rarity.Common, Value = 323.43m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 3, Cooldown = TimeSpan.FromMinutes(6.25), IsPurchaseable = true },
                64 => new Weapon() { ID = 15, Name = "Bat", Rarity = Rarity.Common, Value = 323.43m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 3, Cooldown = TimeSpan.FromMinutes(6.25), IsPurchaseable = true },
                65 => new Attachment() { ID = 65, Name = "Weapon Stand", Subname = "Stand", Rarity = Rarity.Uncommon, Value = 2646.87m, IsActive = false, Ability = Addon.HighestDamageIncrease, Intensity = 3 },
                66 => new Weapon() { ID = 15, Name = "Bat", Rarity = Rarity.Common, Value = 323.43m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 3, Cooldown = TimeSpan.FromMinutes(6.25), IsPurchaseable = true },
                67 => new Attachment() { ID = 28, Name = "Mag Grip", Subname = "Grip", Rarity = Rarity.Common, Value = 937.44m, IsActive = false, Ability = Addon.ReduceCooldown, Intensity = 2 },
                68 => new Weapon() { ID = 3, Name = "Cruid Knife", Subname = "Cruid", Rarity = Rarity.Common, Value = 645.31m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 10, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(13.25), IsPurchaseable = true },
                69 => new Armor() { ID = 69, Name = "Mail Armor", Subname = "Mail", Rarity = Rarity.Uncommon, Value = 11037.52m, IsActive = false, ProtectionRate = 20 },
                70 => new Ammo() { ID = 7, Name = "8mm", Subname = "8", Rarity = Rarity.Common, Value = 778.90m, Type = Ammunition.B8MM, },
                71 => new Money() { ID = 19, Name = "Wallet", Rarity = Rarity.Common, Value = 200, HighestAmount = 500m, LowestAmount = 100m, IsSellable = false },
                72 => new Armor() { ID = 9, Name = "Wooden Shield", Subname = "Wooden", Rarity = Rarity.Common, Value = 8662.5m, IsActive = false, ProtectionRate = 15, IsPurchaseable = true },
                73 => new Weapon() { ID = 21, Name = "Stevens 555", Subname = "Stevens", Rarity = Rarity.Common, Value = 3304.68m, WeaponType = WeaponType.Firearm, FireRate = 12, HighestDamage = 3, LowestDamage = 0, Ammo = Ammunition.B12Gauge, Cooldown = TimeSpan.FromMinutes(56.03), IsPurchaseable = true },
                74 => new Weapon() { ID = 23, Name = "Paint Gun", Subname = "PaintGun", Rarity = Rarity.Common, Value = 275.39m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.Paintball, HighestDamage = 2, LowestDamage = 0, Cooldown = TimeSpan.FromMinutes(1.73) },
                75 => new Armor() { ID = 9, Name = "Wooden Shield", Subname = "Wooden", Rarity = Rarity.Common, Value = 8662.5m, IsActive = false, ProtectionRate = 15, IsPurchaseable = true },
                76 => new Money() { ID = 19, Name = "Wallet", Rarity = Rarity.Common, Value = 200, HighestAmount = 500m, LowestAmount = 100m, IsSellable = false },
                77 => new Weapon() { ID = 3, Name = "Cruid Knife", Subname = "Cruid", Rarity = Rarity.Common, Value = 645.31m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 10, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(13.25), IsPurchaseable = true },
                78 => new Ammo() { ID = 4, Name = "9mm", Subname = "9", Rarity = Rarity.Common, Value = 1036.71m, Type = Ammunition.B9mm, },
                79 => new Ammo() { ID = 22, Name = "12 Gauge", Subname = "12", Rarity = Rarity.Common, Value = 1036.71m, Type = Ammunition.B12Gauge, },
                80 => new Weapon() { ID = 17, Name = "P08", Rarity = Rarity.Common, Value = 2121.09m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B9mm, HighestDamage = 14, LowestDamage = 10, Cooldown = TimeSpan.FromMinutes(18.35), IsPurchaseable = true },
                81 => new Healing() { ID = 16, Name = "Bandaid", Rarity = Rarity.Common, Value = 157.5m, MaxHealingRate = 1, Cooldown = TimeSpan.FromMinutes(1.15), IsPurchaseable = true },
                82 => new Ammo() { ID = 4, Name = "9mm", Subname = "9", Rarity = Rarity.Common, Value = 1036.71m, Type = Ammunition.B9mm, },
                83 => new Attachment() { ID = 88, Name = "4x Scope", Subname = "4x", Rarity = Rarity.Rare, Value = 9471.99m, IsActive = false, Ability = Addon.HighestDamageIncrease, Intensity = 10 },
                84 => new Attachment() { ID = 29, Name = "Vertical Grip", Subname = "Vertical", Rarity = Rarity.Common, Value = 1072.85m, IsActive = false, Ability = Addon.HighestDamageIncrease, Intensity = 1 },
                85 => new Weapon() { ID = 15, Name = "Bat", Rarity = Rarity.Common, Value = 323.43m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 3, Cooldown = TimeSpan.FromMinutes(6.25), IsPurchaseable = true },
                86 => new Attachment() { ID = 28, Name = "Mag Grip", Subname = "Grip", Rarity = Rarity.Common, Value = 937.44m, IsActive = false, Ability = Addon.ReduceCooldown, Intensity = 2 },
                87 => new Weapon() { ID = 15, Name = "Bat", Rarity = Rarity.Common, Value = 323.43m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 3, Cooldown = TimeSpan.FromMinutes(6.25), IsPurchaseable = true },
                88 => new Weapon() { ID = 2, Name = "Bow", Rarity = Rarity.Common, Value = 1593.75m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.Arrow, HighestDamage = 12, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(14.9), IsPurchaseable = true },
                89 => new Weapon() { ID = 8, Name = "Wooden Spear", Subname = "Spear", Rarity = Rarity.Common, Value = 850.75m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 11, LowestDamage = 4, Cooldown = TimeSpan.FromMinutes(13.15), IsPurchaseable = true },
                90 => new Weapon() { ID = 90, Name = "M1 Carbine", Subname = "Carbine", Rarity = Rarity.Rare, Value = 11506.25m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B30Carbine, HighestDamage = 40, LowestDamage = 30, Cooldown = TimeSpan.FromMinutes(83.468) },
                91 => new Weapon() { ID = 52, Name = "Tomahawk", Subname = "Toma", Rarity = Rarity.Uncommon, Value = 1836.96m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, Cooldown = TimeSpan.FromMinutes(32.75), IsPurchaseable = true },
                92 => new Weapon() { ID = 12, Name = "Pellet Gun", Subname = "PelletGun", Rarity = Rarity.Common, Value = 187.5m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.PlasticPellet, HighestDamage = 2, LowestDamage = 0, Cooldown = TimeSpan.FromMinutes(5), IsPurchaseable = true },
                93 => new Ammo() { ID = 4, Name = "9mm", Subname = "9", Rarity = Rarity.Common, Value = 1036.71m, Type = Ammunition.B9mm, },
                94 => new Attachment() { ID = 28, Name = "Mag Grip", Subname = "Grip", Rarity = Rarity.Common, Value = 937.44m, IsActive = false, Ability = Addon.ReduceCooldown, Intensity = 2 },
                95 => new Weapon() { ID = 21, Name = "Stevens 555", Subname = "Stevens", Rarity = Rarity.Common, Value = 3304.68m, WeaponType = WeaponType.Firearm, FireRate = 12, HighestDamage = 3, LowestDamage = 0, Ammo = Ammunition.B12Gauge, Cooldown = TimeSpan.FromMinutes(56.03), IsPurchaseable = true },
                96 => new Ammo() { ID = 25, Name = "Paintball", Subname = "Paint", Rarity = Rarity.Common, Value = 231.84m, Type = Ammunition.Paintball },
                97 => new Weapon() { ID = 17, Name = "P08", Rarity = Rarity.Common, Value = 2121.09m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B9mm, HighestDamage = 14, LowestDamage = 10, Cooldown = TimeSpan.FromMinutes(18.35), IsPurchaseable = true },
                98 => new Money() { ID = 111, Name = "Money Bag", Subname = "Bag", Rarity = Rarity.Rare, Value = 6250m, HighestAmount = 10000m, LowestAmount = 2500m, IsSellable = false },
                99 => new Weapon() { ID = 23, Name = "Paint Gun", Subname = "PaintGun", Rarity = Rarity.Common, Value = 275.39m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.Paintball, HighestDamage = 2, LowestDamage = 0, Cooldown = TimeSpan.FromMinutes(1.73) },
                100 => new Armor() { ID = 13, Name = "Plastic Shield", Subname = "Plastic", Value = 5775.62m, IsActive = false, ProtectionRate = 10, Rarity = Rarity.Common, IsPurchaseable = true },
                101 => new Weapon() { ID = 3, Name = "Cruid Knife", Subname = "Cruid", Rarity = Rarity.Common, Value = 645.31m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 10, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(13.25), IsPurchaseable = true },
                102 => new Weapon() { ID = 2, Name = "Bow", Rarity = Rarity.Common, Value = 1593.75m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.Arrow, HighestDamage = 12, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(14.9), IsPurchaseable = true },
                103 => new Weapon() { ID = 1, Name = "C96", Subname = "96", Rarity = Rarity.Common, Value = 1769.53m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B8MM, HighestDamage = 15, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(16.62), IsPurchaseable = true },
                104 => new Healing() { ID = 54, Name = "First Aid", Subname = "Aid", Rarity = Rarity.Uncommon, Value = 3864.22m, MaxHealingRate = 35, LowestHealingRate = 25, Cooldown = TimeSpan.FromMinutes(34.5), IsPurchaseable = true },
                105 => new Healing() { ID = 54, Name = "First Aid", Subname = "Aid", Rarity = Rarity.Uncommon, Value = 3864.22m, MaxHealingRate = 35, LowestHealingRate = 25, Cooldown = TimeSpan.FromMinutes(34.5), IsPurchaseable = true },
                106 => new Money() { ID = 19, Name = "Wallet", Rarity = Rarity.Common, Value = 200, HighestAmount = 500m, LowestAmount = 100m, IsSellable = false },
                107 => new Healing() { ID = 16, Name = "Bandaid", Rarity = Rarity.Common, Value = 157.5m, MaxHealingRate = 1, Cooldown = TimeSpan.FromMinutes(1.15), IsPurchaseable = true },
                108 => new Ammo() { ID = 4, Name = "9mm", Subname = "9", Rarity = Rarity.Common, Value = 1036.71m, Type = Ammunition.B9mm, },
                109 => new Weapon() { ID = 89, Name = "Bayonet", Subname = "Bay", Rarity = Rarity.Rare, Value = 4825.62m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 30, LowestDamage = 25, Cooldown = TimeSpan.FromMinutes(60.5) },
                110 => new Healing() { ID = 16, Name = "Bandaid", Rarity = Rarity.Common, Value = 157.5m, MaxHealingRate = 1, Cooldown = TimeSpan.FromMinutes(1.15), IsPurchaseable = true },
                111 => new Weapon() { ID = 3, Name = "Cruid Knife", Subname = "Cruid", Rarity = Rarity.Common, Value = 645.31m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 10, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(13.25), IsPurchaseable = true },
                112 => new Ammo() { ID = 61, Name = "5.7mm", Subname = "57", Rarity = Rarity.Uncommon, Value = 2347.65m, Type = Ammunition.B57, },
                113 => new Ammo() { ID = 22, Name = "12 Gauge", Subname = "12", Rarity = Rarity.Common, Value = 1036.71m, Type = Ammunition.B12Gauge, },
                114 => new Ammo() { ID = 75, Name = "Ball Round", Subname = "Ball", Rarity = Rarity.Uncommon, Value = 3614.95m, Type = Ammunition.BallRound },
                115 => new Weapon() { ID = 77, Name = "M16A4", Subname = "M16", Rarity = Rarity.Rare, Value = 8850m, WeaponType = WeaponType.Firearm, FireRate = 3, Ammo = Ammunition.B556mm, HighestDamage = 14, LowestDamage = 7, Cooldown = TimeSpan.FromMinutes(95.156) },
                116 => new Weapon() { ID = 8, Name = "Wooden Spear", Subname = "Spear", Rarity = Rarity.Common, Value = 850.75m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 11, LowestDamage = 4, Cooldown = TimeSpan.FromMinutes(13.15), IsPurchaseable = true },
                117 => new Healing() { ID = 16, Name = "Bandaid", Rarity = Rarity.Common, Value = 157.5m, MaxHealingRate = 1, Cooldown = TimeSpan.FromMinutes(1.15), IsPurchaseable = true },
                118 => new Weapon() { ID = 52, Name = "Tomahawk", Subname = "Toma", Rarity = Rarity.Uncommon, Value = 1836.96m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, Cooldown = TimeSpan.FromMinutes(32.75), IsPurchaseable = true },
                119 => new Weapon() { ID = 18, Name = "Razor", Rarity = Rarity.Common, Value = 534.37m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 5, Cooldown = TimeSpan.FromMinutes(7.75), IsPurchaseable = true },
                120 => new Weapon() { ID = 3, Name = "Cruid Knife", Subname = "Cruid", Rarity = Rarity.Common, Value = 645.31m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 10, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(13.25), IsPurchaseable = true },
                121 => new Healing() { ID = 85, Name = "Morphine", Subname = "Morph", Rarity = Rarity.Rare, Value = 8024.21m, MaxHealingRate = 65, LowestHealingRate = 50, Cooldown = TimeSpan.FromMinutes(69) },
                122 => new Weapon() { ID = 2, Name = "Bow", Rarity = Rarity.Common, Value = 1593.75m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.Arrow, HighestDamage = 12, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(14.9), IsPurchaseable = true },
                123 => new Healing() { ID = 16, Name = "Bandaid", Rarity = Rarity.Common, Value = 157.5m, MaxHealingRate = 1, Cooldown = TimeSpan.FromMinutes(1.15), IsPurchaseable = true },
                124 => new Money() { ID = 73, Name = "Stash", Subname = "Stash", Rarity = Rarity.Uncommon, Value = 3000m, HighestAmount = 5000m, LowestAmount = 1000m, IsSellable = false },
                125 => new Ammo() { ID = 11, Name = "Arrow", Rarity = Rarity.Common, Value = 944.28m, Type = Ammunition.Arrow, IsPurchaseable = true },
                126 => new Weapon() { ID = 3, Name = "Cruid Knife", Subname = "Cruid", Rarity = Rarity.Common, Value = 645.31m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 10, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(13.25), IsPurchaseable = true },
                127 => new Weapon() { ID = 51, Name = "P90", Subname = "90", Rarity = Rarity.Uncommon, Value = 4687.5m, WeaponType = WeaponType.Firearm, FireRate = 2, Ammo = Ammunition.B9mm, HighestDamage = 12, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(43.18), IsPurchaseable = true },
                128 => new Armor() { ID = 9, Name = "Wooden Shield", Subname = "Wooden", Rarity = Rarity.Common, Value = 8662.5m, IsActive = false, ProtectionRate = 15, IsPurchaseable = true },
                129 => new Weapon() { ID = 2, Name = "Bow", Rarity = Rarity.Common, Value = 1593.75m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.Arrow, HighestDamage = 12, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(14.9), IsPurchaseable = true },
                130 => new Weapon() { ID = 1, Name = "C96", Subname = "96", Rarity = Rarity.Common, Value = 1769.53m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B8MM, HighestDamage = 15, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(16.62), IsPurchaseable = true },
                131 => new Weapon() { ID = 91, Name = "M249", Subname = "249", Rarity = Rarity.Epic, Value = 38800.78m, WeaponType = WeaponType.Firearm, FireRate = 5, Ammo = Ammunition.B556mm, HighestDamage = 20, LowestDamage = 15, Cooldown = TimeSpan.FromMinutes(298.75) },
                132 => new Money() { ID = 19, Name = "Wallet", Rarity = Rarity.Common, Value = 200, HighestAmount = 500m, LowestAmount = 100m, IsSellable = false },
                133 => new Healing() { ID = 14, Name = "Alcohol Wipe", Subname = "Alcohol", Rarity = Rarity.Common, Value = 205.34m, MaxHealingRate = 2, Cooldown = TimeSpan.FromMinutes(2.3), IsPurchaseable = true },
                134 => new Weapon() { ID = 17, Name = "P08", Rarity = Rarity.Common, Value = 2121.09m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B9mm, HighestDamage = 14, LowestDamage = 10, Cooldown = TimeSpan.FromMinutes(18.35), IsPurchaseable = true },
                135 => new Weapon() { ID = 1, Name = "C96", Subname = "96", Rarity = Rarity.Common, Value = 1769.53m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B8MM, HighestDamage = 15, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(16.62), IsPurchaseable = true },
                136 => new Healing() { ID = 16, Name = "Bandaid", Rarity = Rarity.Common, Value = 157.5m, MaxHealingRate = 1, Cooldown = TimeSpan.FromMinutes(1.15), IsPurchaseable = true },
                137 => new Weapon() { ID = 8, Name = "Wooden Spear", Subname = "Spear", Rarity = Rarity.Common, Value = 850.75m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 11, LowestDamage = 4, Cooldown = TimeSpan.FromMinutes(13.15), IsPurchaseable = true },
                138 => new Ammo() { ID = 4, Name = "9mm", Subname = "9", Rarity = Rarity.Common, Value = 1036.71m, Type = Ammunition.B9mm, },
                139 => new Weapon() { ID = 67, Name = "PPSH", Subname = "PPSH", Rarity = Rarity.Uncommon, Value = 7565.62m, WeaponType = WeaponType.Firearm, FireRate = 3, Ammo = Ammunition.B762mm, HighestDamage = 20, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(109.387), IsPurchaseable = true },
                140 => new Weapon() { ID = 52, Name = "Tomahawk", Subname = "Toma", Rarity = Rarity.Uncommon, Value = 1836.96m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, Cooldown = TimeSpan.FromMinutes(32.75), IsPurchaseable = true },
                141 => new Healing() { ID = 14, Name = "Alcohol Wipe", Subname = "Alcohol", Rarity = Rarity.Common, Value = 205.34m, MaxHealingRate = 2, Cooldown = TimeSpan.FromMinutes(2.3), IsPurchaseable = true },
                142 => new Weapon() { ID = 8, Name = "Wooden Spear", Subname = "Spear", Rarity = Rarity.Common, Value = 850.75m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 11, LowestDamage = 4, Cooldown = TimeSpan.FromMinutes(13.15), IsPurchaseable = true },
                143 => new Weapon() { ID = 12, Name = "Pellet Gun", Subname = "PelletGun", Rarity = Rarity.Common, Value = 187.5m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.PlasticPellet, HighestDamage = 2, LowestDamage = 0, Cooldown = TimeSpan.FromMinutes(5), IsPurchaseable = true },
                144 => new Healing() { ID = 14, Name = "Alcohol Wipe", Subname = "Alcohol", Rarity = Rarity.Common, Value = 205.34m, MaxHealingRate = 2, Cooldown = TimeSpan.FromMinutes(2.3), IsPurchaseable = true },
                145 => new Weapon() { ID = 89, Name = "Bayonet", Subname = "Bay", Rarity = Rarity.Rare, Value = 4825.62m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 30, LowestDamage = 25, Cooldown = TimeSpan.FromMinutes(60.5) },
                146 => new Money() { ID = 73, Name = "Stash", Subname = "Stash", Rarity = Rarity.Uncommon, Value = 3000m, HighestAmount = 5000m, LowestAmount = 1000m, IsSellable = false },
                147 => new Weapon() { ID = 6, Name = "Stone", Rarity = Rarity.Common, Value = 112.5m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 1, LowestDamage = 0, Cooldown = TimeSpan.FromMinutes(1), IsPurchaseable = true },
                148 => new Money() { ID = 66, Name = "Lock Box", Subname = "Box", Rarity = Rarity.Uncommon, Value = 1000m, HighestAmount = 1500m, LowestAmount = 500, IsSellable = false },
                149 => new Weapon() { ID = 62, Name = "Javelin", Subname = "Jave", Rarity = Rarity.Uncommon, Value = 1713.12m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, LowestDamage = 10, Cooldown = TimeSpan.FromMinutes(33.55), IsPurchaseable = true },
                150 => new Ammo() { ID = 22, Name = "12 Gauge", Subname = "12", Rarity = Rarity.Common, Value = 1036.71m, Type = Ammunition.B12Gauge, },
                151 => new Weapon() { ID = 24, Name = "Sawed-off", Subname = "Sawedoff", Rarity = Rarity.Common, Value = 2250m, WeaponType = WeaponType.Firearm, FireRate = 12, HighestDamage = 2, LowestDamage = 0, Ammo = Ammunition.B12Gauge, Cooldown = TimeSpan.FromMinutes(43.37), IsPurchaseable = true },
                152 => new Healing() { ID = 14, Name = "Alcohol Wipe", Subname = "Alcohol", Rarity = Rarity.Common, Value = 205.34m, MaxHealingRate = 2, Cooldown = TimeSpan.FromMinutes(2.3), IsPurchaseable = true },
                153 => new Armor() { ID = 79, Name = "Kevlar Set", Subname = "Kevlar", Rarity = Rarity.Rare, Value = 27562.5m, IsActive = false, ProtectionRate = 75 },
                154 => new Weapon() { ID = 62, Name = "Javelin", Subname = "Jave", Rarity = Rarity.Uncommon, Value = 1713.12m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, LowestDamage = 10, Cooldown = TimeSpan.FromMinutes(33.55), IsPurchaseable = true },
                155 => new Weapon() { ID = 15, Name = "Bat", Rarity = Rarity.Common, Value = 323.43m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 3, Cooldown = TimeSpan.FromMinutes(6.25), IsPurchaseable = true },
                156 => new Attachment() { ID = 29, Name = "Vertical Grip", Subname = "Vertical", Rarity = Rarity.Common, Value = 1072.85m, IsActive = false, Ability = Addon.HighestDamageIncrease, Intensity = 1 },
                157 => new Ammo() { ID = 59, Name = ".30 Carbine", Subname = ".30", Rarity = Rarity.Uncommon, Value = 3695.31m, Type = Ammunition.B30Carbine },
                158 => new Weapon() { ID = 27, Name = "Pool Cue", Subname = "Pool", Rarity = Rarity.Common, Value = 323.43m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 3, Cooldown = TimeSpan.FromMinutes(6.25), IsPurchaseable = true },
                159 => new Weapon() { ID = 10, Name = "Dart", Rarity = Rarity.Common, Value = 120.25m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 1, Cooldown = TimeSpan.FromMinutes(1.5) },
                160 => new Money() { ID = 111, Name = "Money Bag", Subname = "Bag", Rarity = Rarity.Rare, Value = 6250m, HighestAmount = 10000m, LowestAmount = 2500m, IsSellable = false },
                161 => new Ammo() { ID = 25, Name = "Paintball", Subname = "Paint", Rarity = Rarity.Common, Value = 231.84m, Type = Ammunition.Paintball },
                162 => new Weapon() { ID = 2, Name = "Bow", Rarity = Rarity.Common, Value = 1593.75m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.Arrow, HighestDamage = 12, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(14.9), IsPurchaseable = true },
                163 => new Weapon() { ID = 17, Name = "P08", Rarity = Rarity.Common, Value = 2121.09m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B9mm, HighestDamage = 14, LowestDamage = 10, Cooldown = TimeSpan.FromMinutes(18.35), IsPurchaseable = true },
                164 => new Ammo() { ID = 22, Name = "12 Gauge", Subname = "12", Rarity = Rarity.Common, Value = 1036.71m, Type = Ammunition.B12Gauge, },
                165 => new Weapon() { ID = 10, Name = "Dart", Rarity = Rarity.Common, Value = 120.25m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 1, Cooldown = TimeSpan.FromMinutes(1.5) },
                166 => new Weapon() { ID = 24, Name = "Sawed-off", Subname = "Sawedoff", Rarity = Rarity.Common, Value = 2250m, WeaponType = WeaponType.Firearm, FireRate = 12, HighestDamage = 2, LowestDamage = 0, Ammo = Ammunition.B12Gauge, Cooldown = TimeSpan.FromMinutes(43.37), IsPurchaseable = true },
                167 => new Healing() { ID = 14, Name = "Alcohol Wipe", Subname = "Alcohol", Rarity = Rarity.Common, Value = 205.34m, MaxHealingRate = 2, Cooldown = TimeSpan.FromMinutes(2.3), IsPurchaseable = true },
                168 => new Armor() { ID = 58, Name = "Tactical Vest", Subname = "Tactical", Rarity = Rarity.Uncommon, Value = 13075.83m, IsActive = false, ProtectionRate = 40 },
                169 => new Healing() { ID = 14, Name = "Alcohol Wipe", Subname = "Alcohol", Rarity = Rarity.Common, Value = 205.34m, MaxHealingRate = 2, Cooldown = TimeSpan.FromMinutes(2.3), IsPurchaseable = true },
                170 => new Weapon() { ID = 5, Name = "Glock", Rarity = Rarity.Common, Value = 2096.87m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B9mm, HighestDamage = 15, LowestDamage = 10, Cooldown = TimeSpan.FromMinutes(19.2), IsPurchaseable = true },
                171 => new Attachment() { ID = 64, Name = "Mag Strap", Subname = "Strap", Rarity = Rarity.Uncommon, Value = 2251.79m, IsActive = false, Ability = Addon.ReduceCooldown, Intensity = 10 },
                172 => new Weapon() { ID = 84, Name = "MG-42", Subname = "MG42", Rarity = Rarity.Rare, Value = 17237.5m, WeaponType = WeaponType.Firearm, FireRate = 8, Ammo = Ammunition.B792, HighestDamage = 8, LowestDamage = 4, Cooldown = TimeSpan.FromMinutes(167.5) },
                173 => new Weapon() { ID = 1, Name = "C96", Subname = "96", Rarity = Rarity.Common, Value = 1769.53m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B8MM, HighestDamage = 15, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(16.62), IsPurchaseable = true },
                174 => new Weapon() { ID = 8, Name = "Wooden Spear", Subname = "Spear", Rarity = Rarity.Common, Value = 850.75m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 11, LowestDamage = 4, Cooldown = TimeSpan.FromMinutes(13.15), IsPurchaseable = true },
                175 => new Weapon() { ID = 62, Name = "Javelin", Subname = "Jave", Rarity = Rarity.Uncommon, Value = 1713.12m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, LowestDamage = 10, Cooldown = TimeSpan.FromMinutes(33.55), IsPurchaseable = true },
                176 => new Weapon() { ID = 55, Name = "P1911", Subname = "19", Rarity = Rarity.Uncommon, Value = 4869.53m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B45APC, HighestDamage = 18, Cooldown = TimeSpan.FromMinutes(23.525), IsPurchaseable = true },
                177 => new Armor() { ID = 13, Name = "Plastic Shield", Subname = "Plastic", Value = 5775.62m, IsActive = false, ProtectionRate = 10, Rarity = Rarity.Common, IsPurchaseable = true },
                178 => new Weapon() { ID = 12, Name = "Pellet Gun", Subname = "PelletGun", Rarity = Rarity.Common, Value = 187.5m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.PlasticPellet, HighestDamage = 2, LowestDamage = 0, Cooldown = TimeSpan.FromMinutes(5), IsPurchaseable = true },
                179 => new Ammo() { ID = 11, Name = "Arrow", Rarity = Rarity.Common, Value = 944.28m, Type = Ammunition.Arrow, IsPurchaseable = true },
                180 => new Weapon() { ID = 2, Name = "Bow", Rarity = Rarity.Common, Value = 1593.75m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.Arrow, HighestDamage = 12, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(14.9), IsPurchaseable = true },
                181 => new Weapon() { ID = 3, Name = "Cruid Knife", Subname = "Cruid", Rarity = Rarity.Common, Value = 645.31m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 10, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(13.25), IsPurchaseable = true },
                182 => new Weapon() { ID = 80, Name = "Combat Knife", Subname = "Combat", Rarity = Rarity.Rare, Value = 5510m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 35, LowestDamage = 30, Cooldown = TimeSpan.FromMinutes(73.875), IsPurchaseable = true },
                183 => new Ammo() { ID = 4, Name = "9mm", Subname = "9", Rarity = Rarity.Common, Value = 1036.71m, Type = Ammunition.B9mm, },
                184 => new Weapon() { ID = 6, Name = "Stone", Rarity = Rarity.Common, Value = 112.5m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 1, LowestDamage = 0, Cooldown = TimeSpan.FromMinutes(1), IsPurchaseable = true },
                185 => new Healing() { ID = 14, Name = "Alcohol Wipe", Subname = "Alcohol", Rarity = Rarity.Common, Value = 205.34m, MaxHealingRate = 2, Cooldown = TimeSpan.FromMinutes(2.3), IsPurchaseable = true },
                186 => new Weapon() { ID = 3, Name = "Cruid Knife", Subname = "Cruid", Rarity = Rarity.Common, Value = 645.31m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 10, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(13.25), IsPurchaseable = true },
                187 => new Weapon() { ID = 76, Name = "AK-47", Subname = "AK", Rarity = Rarity.Rare, Value = 16428.12m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B762mm, HighestDamage = 65, LowestDamage = 35, Cooldown = TimeSpan.FromMinutes(125.031) },
                188 => new Weapon() { ID = 1, Name = "C96", Subname = "96", Rarity = Rarity.Common, Value = 1769.53m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B8MM, HighestDamage = 15, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(16.62), IsPurchaseable = true },
                189 => new Weapon() { ID = 5, Name = "Glock", Rarity = Rarity.Common, Value = 2096.87m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B9mm, HighestDamage = 15, LowestDamage = 10, Cooldown = TimeSpan.FromMinutes(19.2), IsPurchaseable = true },
                190 => new Weapon() { ID = 74, Name = "Musket", Subname = "Musk", Rarity = Rarity.Uncommon, Value = 4803.9m, WeaponType = WeaponType.Firearm, HighestDamage = 60, LowestDamage = 1, FireRate = 1, Ammo = Ammunition.BallRound, Cooldown = TimeSpan.FromMinutes(121.85) },
                191 => new Weapon() { ID = 72, Name = "MK 12", Subname = "MK", Rarity = Rarity.Uncommon, Value = 6103.9m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B556mm, HighestDamage = 30, Cooldown = TimeSpan.FromMinutes(89.592) },
                192 => new Armor() { ID = 13, Name = "Plastic Shield", Subname = "Plastic", Value = 5775.62m, IsActive = false, ProtectionRate = 10, Rarity = Rarity.Common, IsPurchaseable = true },
                193 => new Weapon() { ID = 8, Name = "Wooden Spear", Subname = "Spear", Rarity = Rarity.Common, Value = 850.75m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 11, LowestDamage = 4, Cooldown = TimeSpan.FromMinutes(13.15), IsPurchaseable = true },
                194 => new Weapon() { ID = 6, Name = "Stone", Rarity = Rarity.Common, Value = 112.5m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 1, LowestDamage = 0, Cooldown = TimeSpan.FromMinutes(1), IsPurchaseable = true },
                195 => new Weapon() { ID = 56, Name = "Thompson", Subname = "Tom", Rarity = Rarity.Uncommon, Value = 5965.62m, WeaponType = WeaponType.Firearm, FireRate = 2, Ammo = Ammunition.B45APC, HighestDamage = 17, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(53.95), IsPurchaseable = true },
                196 => new Weapon() { ID = 24, Name = "Sawed-off", Subname = "Sawedoff", Rarity = Rarity.Common, Value = 2250m, WeaponType = WeaponType.Firearm, FireRate = 12, HighestDamage = 2, LowestDamage = 0, Ammo = Ammunition.B12Gauge, Cooldown = TimeSpan.FromMinutes(43.37), IsPurchaseable = true },
                197 => new Ammo() { ID = 53, Name = "5.56mm", Subname = "556", Rarity = Rarity.Uncommon, Value = 4038.28m, Type = Ammunition.B556mm, },
                198 => new Weapon() { ID = 12, Name = "Pellet Gun", Subname = "PelletGun", Rarity = Rarity.Common, Value = 187.5m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.PlasticPellet, HighestDamage = 2, LowestDamage = 0, Cooldown = TimeSpan.FromMinutes(5), IsPurchaseable = true },
                199 => new Ammo() { ID = 59, Name = ".30 Carbine", Subname = ".30", Rarity = Rarity.Uncommon, Value = 3695.31m, Type = Ammunition.B30Carbine },
                200 => new Money() { ID = 66, Name = "Lock Box", Subname = "Box", Rarity = Rarity.Uncommon, Value = 1000m, HighestAmount = 1500m, LowestAmount = 500, IsSellable = false },
                201 => new Weapon() { ID = 55, Name = "P1911", Subname = "19", Rarity = Rarity.Uncommon, Value = 4869.53m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B45APC, HighestDamage = 18, Cooldown = TimeSpan.FromMinutes(23.525), IsPurchaseable = true },
                202 => new Weapon() { ID = 27, Name = "Pool Cue", Subname = "Pool", Rarity = Rarity.Common, Value = 323.43m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 3, Cooldown = TimeSpan.FromMinutes(6.25), IsPurchaseable = true },
                203 => new Weapon() { ID = 6, Name = "Stone", Rarity = Rarity.Common, Value = 112.5m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 1, LowestDamage = 0, Cooldown = TimeSpan.FromMinutes(1), IsPurchaseable = true },
                204 => new Weapon() { ID = 27, Name = "Pool Cue", Subname = "Pool", Rarity = Rarity.Common, Value = 323.43m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 3, Cooldown = TimeSpan.FromMinutes(6.25), IsPurchaseable = true },
                205 => new Weapon() { ID = 80, Name = "Combat Knife", Subname = "Combat", Rarity = Rarity.Rare, Value = 5510m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 35, LowestDamage = 30, Cooldown = TimeSpan.FromMinutes(73.875), IsPurchaseable = true },
                206 => new Weapon() { ID = 2, Name = "Bow", Rarity = Rarity.Common, Value = 1593.75m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.Arrow, HighestDamage = 12, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(14.9), IsPurchaseable = true },
                207 => new Weapon() { ID = 68, Name = "Cross Bow", Subname = "Cross", Rarity = Rarity.Uncommon, Value = 4330.46m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.Arrow, HighestDamage = 16, Cooldown = TimeSpan.FromMinutes(38.36), IsPurchaseable = true },
                208 => new Weapon() { ID = 68, Name = "Cross Bow", Subname = "Cross", Rarity = Rarity.Uncommon, Value = 4330.46m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.Arrow, HighestDamage = 16, Cooldown = TimeSpan.FromMinutes(38.36), IsPurchaseable = true },
                209 => new Weapon() { ID = 8, Name = "Wooden Spear", Subname = "Spear", Rarity = Rarity.Common, Value = 850.75m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 11, LowestDamage = 4, Cooldown = TimeSpan.FromMinutes(13.15), IsPurchaseable = true },
                210 => new Ammo() { ID = 11, Name = "Arrow", Rarity = Rarity.Common, Value = 944.28m, Type = Ammunition.Arrow, IsPurchaseable = true },
                211 => new Ammo() { ID = 61, Name = "5.7mm", Subname = "57", Rarity = Rarity.Uncommon, Value = 2347.65m, Type = Ammunition.B57, },
                212 => new Weapon() { ID = 18, Name = "Razor", Rarity = Rarity.Common, Value = 534.37m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 5, Cooldown = TimeSpan.FromMinutes(7.75), IsPurchaseable = true },
                213 => new Weapon() { ID = 6, Name = "Stone", Rarity = Rarity.Common, Value = 112.5m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 1, LowestDamage = 0, Cooldown = TimeSpan.FromMinutes(1), IsPurchaseable = true },
                214 => new Weapon() { ID = 5, Name = "Glock", Rarity = Rarity.Common, Value = 2096.87m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B9mm, HighestDamage = 15, LowestDamage = 10, Cooldown = TimeSpan.FromMinutes(19.2), IsPurchaseable = true },
                215 => new Weapon() { ID = 50, Name = "Firecracker", Subname = "Fire", Rarity = Rarity.Common, Value = 300.24m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 5, LowestDamage = 0, Cooldown = TimeSpan.FromMinutes(9.5), IsPurchaseable = true },
                216 => new Weapon() { ID = 10, Name = "Dart", Rarity = Rarity.Common, Value = 120.25m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 1, Cooldown = TimeSpan.FromMinutes(1.5) },
                217 => new Weapon() { ID = 15, Name = "Bat", Rarity = Rarity.Common, Value = 323.43m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 3, Cooldown = TimeSpan.FromMinutes(6.25), IsPurchaseable = true },
                218 => new Money() { ID = 66, Name = "Lock Box", Subname = "Box", Rarity = Rarity.Uncommon, Value = 1000m, HighestAmount = 1500m, LowestAmount = 500, IsSellable = false },
                219 => new Ammo() { ID = 7, Name = "8mm", Subname = "8", Rarity = Rarity.Common, Value = 778.90m, Type = Ammunition.B8MM, },
                220 => new Weapon() { ID = 52, Name = "Tomahawk", Subname = "Toma", Rarity = Rarity.Uncommon, Value = 1836.96m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, Cooldown = TimeSpan.FromMinutes(32.75), IsPurchaseable = true },
                221 => new Weapon() { ID = 15, Name = "Bat", Rarity = Rarity.Common, Value = 323.43m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 3, Cooldown = TimeSpan.FromMinutes(6.25), IsPurchaseable = true },
                222 => new Weapon() { ID = 34, Name = "Staff", Rarity = Rarity.Common, Value = 428.9m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 4, Cooldown = TimeSpan.FromMinutes(7), IsPurchaseable = true },
                223 => new Weapon() { ID = 50, Name = "Firecracker", Subname = "Fire", Rarity = Rarity.Common, Value = 300.24m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 5, LowestDamage = 0, Cooldown = TimeSpan.FromMinutes(9.5), IsPurchaseable = true },
                224 => new Weapon() { ID = 8, Name = "Wooden Spear", Subname = "Spear", Rarity = Rarity.Common, Value = 850.75m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 11, LowestDamage = 4, Cooldown = TimeSpan.FromMinutes(13.15), IsPurchaseable = true },
                225 => new Weapon() { ID = 82, Name = "M1 Garand", Subname = "Garand", Rarity = Rarity.Rare, Value = 15771.87m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B3006, HighestDamage = 50, LowestDamage = 40, Cooldown = TimeSpan.FromMinutes(120.031) },
                226 => new Weapon() { ID = 34, Name = "Staff", Rarity = Rarity.Common, Value = 428.9m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 4, Cooldown = TimeSpan.FromMinutes(7), IsPurchaseable = true },
                227 => new Weapon() { ID = 18, Name = "Razor", Rarity = Rarity.Common, Value = 534.37m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 5, Cooldown = TimeSpan.FromMinutes(7.75), IsPurchaseable = true },
                228 => new Ammo() { ID = 105, Name = "30.06 Springfield", Subname = "30.06", Rarity = Rarity.Uncommon, Value = 4739.57m, Type = Ammunition.B3006 },
                229 => new Weapon() { ID = 27, Name = "Pool Cue", Subname = "Pool", Rarity = Rarity.Common, Value = 323.43m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 3, Cooldown = TimeSpan.FromMinutes(6.25), IsPurchaseable = true },
                230 => new Weapon() { ID = 5, Name = "Glock", Rarity = Rarity.Common, Value = 2096.87m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B9mm, HighestDamage = 15, LowestDamage = 10, Cooldown = TimeSpan.FromMinutes(19.2), IsPurchaseable = true },
                231 => new Ammo() { ID = 61, Name = "5.7mm", Subname = "57", Rarity = Rarity.Uncommon, Value = 2347.65m, Type = Ammunition.B57, },
                232 => new Weapon() { ID = 15, Name = "Bat", Rarity = Rarity.Common, Value = 323.43m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 3, Cooldown = TimeSpan.FromMinutes(6.25), IsPurchaseable = true },
                233 => new Weapon() { ID = 3, Name = "Cruid Knife", Subname = "Cruid", Rarity = Rarity.Common, Value = 645.31m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 10, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(13.25), IsPurchaseable = true },
                234 => new Weapon() { ID = 34, Name = "Staff", Rarity = Rarity.Common, Value = 428.9m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 4, Cooldown = TimeSpan.FromMinutes(7), IsPurchaseable = true },
                235 => new Weapon() { ID = 74, Name = "Musket", Subname = "Musk", Rarity = Rarity.Uncommon, Value = 4803.9m, WeaponType = WeaponType.Firearm, HighestDamage = 60, LowestDamage = 1, FireRate = 1, Ammo = Ammunition.BallRound, Cooldown = TimeSpan.FromMinutes(121.85) },
                236 => new Weapon() { ID = 51, Name = "P90", Subname = "90", Rarity = Rarity.Uncommon, Value = 4687.5m, WeaponType = WeaponType.Firearm, FireRate = 2, Ammo = Ammunition.B9mm, HighestDamage = 12, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(43.18), IsPurchaseable = true },
                237 => new Weapon() { ID = 18, Name = "Razor", Rarity = Rarity.Common, Value = 534.37m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 5, Cooldown = TimeSpan.FromMinutes(7.75), IsPurchaseable = true },
                238 => new Weapon() { ID = 27, Name = "Pool Cue", Subname = "Pool", Rarity = Rarity.Common, Value = 323.43m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 3, Cooldown = TimeSpan.FromMinutes(6.25), IsPurchaseable = true },
                239 => new Weapon() { ID = 10, Name = "Dart", Rarity = Rarity.Common, Value = 120.25m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 1, Cooldown = TimeSpan.FromMinutes(1.5) },
                240 => new Weapon() { ID = 50, Name = "Firecracker", Subname = "Fire", Rarity = Rarity.Common, Value = 300.24m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 5, LowestDamage = 0, Cooldown = TimeSpan.FromMinutes(9.5), IsPurchaseable = true },
                241 => new Weapon() { ID = 6, Name = "Stone", Rarity = Rarity.Common, Value = 112.5m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 1, LowestDamage = 0, Cooldown = TimeSpan.FromMinutes(1), IsPurchaseable = true },
                242 => new Ammo() { ID = 53, Name = "5.56mm", Subname = "556", Rarity = Rarity.Uncommon, Value = 4038.28m, Type = Ammunition.B556mm, },
                243 => new Weapon() { ID = 27, Name = "Pool Cue", Subname = "Pool", Rarity = Rarity.Common, Value = 323.43m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 3, Cooldown = TimeSpan.FromMinutes(6.25), IsPurchaseable = true },
                244 => new Ammo() { ID = 11, Name = "Arrow", Rarity = Rarity.Common, Value = 944.28m, Type = Ammunition.Arrow, IsPurchaseable = true },
                245 => new Weapon() { ID = 3, Name = "Cruid Knife", Subname = "Cruid", Rarity = Rarity.Common, Value = 645.31m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 10, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(13.25), IsPurchaseable = true },
                246 => new Weapon() { ID = 80, Name = "Combat Knife", Subname = "Combat", Rarity = Rarity.Rare, Value = 5510m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 35, LowestDamage = 30, Cooldown = TimeSpan.FromMinutes(73.875), IsPurchaseable = true },
                247 => new Armor() { ID = 13, Name = "Plastic Shield", Subname = "Plastic", Value = 5775.62m, IsActive = false, ProtectionRate = 10, Rarity = Rarity.Common, IsPurchaseable = true },
                248 => new Weapon() { ID = 34, Name = "Staff", Rarity = Rarity.Common, Value = 428.9m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 4, Cooldown = TimeSpan.FromMinutes(7), IsPurchaseable = true },
                249 => new Weapon() { ID = 8, Name = "Wooden Spear", Subname = "Spear", Rarity = Rarity.Common, Value = 850.75m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 11, LowestDamage = 4, Cooldown = TimeSpan.FromMinutes(13.15), IsPurchaseable = true },
                250 => new Attachment() { ID = 57, Name = "Laser Sight", Subname = "Laser", Rarity = Rarity.Uncommon, Value = 4672.32m, IsActive = false, Ability = Addon.HighestDamageIncrease, Intensity = 5 },
                251 => new Ammo() { ID = 26, Name = "Plastic Pellet", Subname = "Pellet", Rarity = Rarity.Common, Value = 196.44m, Type = Ammunition.PlasticPellet },
                252 => new Weapon() { ID = 18, Name = "Razor", Rarity = Rarity.Common, Value = 534.37m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 5, Cooldown = TimeSpan.FromMinutes(7.75), IsPurchaseable = true },
                253 => new Weapon() { ID = 70, Name = "Broadsword", Subname = "Sword", Rarity = Rarity.Uncommon, Value = 1836.56m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, Cooldown = TimeSpan.FromMinutes(32.75), IsPurchaseable = true },
                254 => new Ammo() { ID = 7, Name = "8mm", Subname = "8", Rarity = Rarity.Common, Value = 778.90m, Type = Ammunition.B8MM, },
                255 => new Attachment() { ID = 86, Name = "Dual Mags", Subname = "Dual", Rarity = Rarity.Rare, Value = 6739.10m, IsActive = false, Ability = Addon.ReduceCooldown, Intensity = 25 },
                256 => new Weapon() { ID = 8, Name = "Wooden Spear", Subname = "Spear", Rarity = Rarity.Common, Value = 850.75m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 11, LowestDamage = 4, Cooldown = TimeSpan.FromMinutes(13.15), IsPurchaseable = true },
                257 => new Ammo() { ID = 26, Name = "Plastic Pellet", Subname = "Pellet", Rarity = Rarity.Common, Value = 196.44m, Type = Ammunition.PlasticPellet },
                258 => new Weapon() { ID = 70, Name = "Broadsword", Subname = "Sword", Rarity = Rarity.Uncommon, Value = 1836.56m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, Cooldown = TimeSpan.FromMinutes(32.75), IsPurchaseable = true },
                259 => new Weapon() { ID = 5, Name = "Glock", Rarity = Rarity.Common, Value = 2096.87m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B9mm, HighestDamage = 15, LowestDamage = 10, Cooldown = TimeSpan.FromMinutes(19.2), IsPurchaseable = true },
                260 => new Weapon() { ID = 71, Name = "G36C", Subname = "G36", Rarity = Rarity.Uncommon, Value = 4156.87m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B556mm, HighestDamage = 40, LowestDamage = 10, Cooldown = TimeSpan.FromMinutes(64.925) },
                261 => new Attachment() { ID = 57, Name = "Laser Sight", Subname = "Laser", Rarity = Rarity.Uncommon, Value = 4672.32m, IsActive = false, Ability = Addon.HighestDamageIncrease, Intensity = 5 },
                262 => new Weapon() { ID = 10, Name = "Dart", Rarity = Rarity.Common, Value = 120.25m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 1, Cooldown = TimeSpan.FromMinutes(1.5) },
                263 => new Weapon() { ID = 15, Name = "Bat", Rarity = Rarity.Common, Value = 323.43m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 3, Cooldown = TimeSpan.FromMinutes(6.25), IsPurchaseable = true },
                264 => new Ammo() { ID = 75, Name = "Ball Round", Subname = "Ball", Rarity = Rarity.Uncommon, Value = 3614.95m, Type = Ammunition.BallRound },
                265 => new Healing() { ID = 63, Name = "Bandage", Subname = "Bandage", Rarity = Rarity.Uncommon, Value = 1832m, MaxHealingRate = 20, LowestHealingRate = 10, Cooldown = TimeSpan.FromMinutes(17.25), IsPurchaseable = true },
                266 => new Weapon() { ID = 83, Name = "Kar-98", Subname = "Kar", Rarity = Rarity.Rare, Value = 18068.75m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B762mm, HighestDamage = 55, Cooldown = TimeSpan.FromMinutes(186.593) },
                267 => new Weapon() { ID = 18, Name = "Razor", Rarity = Rarity.Common, Value = 534.37m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 5, Cooldown = TimeSpan.FromMinutes(7.75), IsPurchaseable = true },
                268 => new Weapon() { ID = 15, Name = "Bat", Rarity = Rarity.Common, Value = 323.43m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 3, Cooldown = TimeSpan.FromMinutes(6.25), IsPurchaseable = true },
                269 => new Ammo() { ID = 11, Name = "Arrow", Rarity = Rarity.Common, Value = 944.28m, Type = Ammunition.Arrow, IsPurchaseable = true },
                270 => new Healing() { ID = 63, Name = "Bandage", Subname = "Bandage", Rarity = Rarity.Uncommon, Value = 1832m, MaxHealingRate = 20, LowestHealingRate = 10, Cooldown = TimeSpan.FromMinutes(17.25), IsPurchaseable = true },
                271 => new Weapon() { ID = 55, Name = "P1911", Subname = "19", Rarity = Rarity.Uncommon, Value = 4869.53m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B45APC, HighestDamage = 18, Cooldown = TimeSpan.FromMinutes(23.525), IsPurchaseable = true },
                272 => new Weapon() { ID = 70, Name = "Broadsword", Subname = "Sword", Rarity = Rarity.Uncommon, Value = 1836.56m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, Cooldown = TimeSpan.FromMinutes(32.75), IsPurchaseable = true },
                273 => new Ammo() { ID = 25, Name = "Paintball", Subname = "Paint", Rarity = Rarity.Common, Value = 231.84m, Type = Ammunition.Paintball },
                274 => new Weapon() { ID = 60, Name = "Five-Seven", Subname = "FiveSeven", Rarity = Rarity.Uncommon, Value = 5139.06m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B57, HighestDamage = 20, LowestDamage = 15, Cooldown = TimeSpan.FromMinutes(40.257), IsPurchaseable = true },
                275 => new Ammo() { ID = 75, Name = "Ball Round", Subname = "Ball", Rarity = Rarity.Uncommon, Value = 3614.95m, Type = Ammunition.BallRound },
                276 => new Weapon() { ID = 74, Name = "Musket", Subname = "Musk", Rarity = Rarity.Uncommon, Value = 4803.9m, WeaponType = WeaponType.Firearm, HighestDamage = 60, LowestDamage = 1, FireRate = 1, Ammo = Ammunition.BallRound, Cooldown = TimeSpan.FromMinutes(121.85) },
                277 => new Ammo() { ID = 53, Name = "5.56mm", Subname = "556", Rarity = Rarity.Uncommon, Value = 4038.28m, Type = Ammunition.B556mm, },
                278 => new Weapon() { ID = 3, Name = "Cruid Knife", Subname = "Cruid", Rarity = Rarity.Common, Value = 645.31m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 10, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(13.25), IsPurchaseable = true },
                279 => new Armor() { ID = 13, Name = "Plastic Shield", Subname = "Plastic", Value = 5775.62m, IsActive = false, ProtectionRate = 10, Rarity = Rarity.Common, IsPurchaseable = true },
                280 => new Weapon() { ID = 70, Name = "Broadsword", Subname = "Sword", Rarity = Rarity.Uncommon, Value = 1836.56m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, Cooldown = TimeSpan.FromMinutes(32.75), IsPurchaseable = true },
                281 => new Weapon() { ID = 15, Name = "Bat", Rarity = Rarity.Common, Value = 323.43m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 3, Cooldown = TimeSpan.FromMinutes(6.25), IsPurchaseable = true },
                282 => new Ammo() { ID = 81, Name = "7.62mm", Subname = "762", Rarity = Rarity.Rare, Value = 8921.87m, Type = Ammunition.B762mm },
                283 => new Weapon() { ID = 27, Name = "Pool Cue", Subname = "Pool", Rarity = Rarity.Common, Value = 323.43m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 3, Cooldown = TimeSpan.FromMinutes(6.25), IsPurchaseable = true },
                284 => new Weapon() { ID = 56, Name = "Thompson", Subname = "Tom", Rarity = Rarity.Uncommon, Value = 5965.62m, WeaponType = WeaponType.Firearm, FireRate = 2, Ammo = Ammunition.B45APC, HighestDamage = 17, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(53.95), IsPurchaseable = true },
                285 => new Weapon() { ID = 18, Name = "Razor", Rarity = Rarity.Common, Value = 534.37m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 5, Cooldown = TimeSpan.FromMinutes(7.75), IsPurchaseable = true },
                286 => new Healing() { ID = 63, Name = "Bandage", Subname = "Bandage", Rarity = Rarity.Uncommon, Value = 1832m, MaxHealingRate = 20, LowestHealingRate = 10, Cooldown = TimeSpan.FromMinutes(17.25), IsPurchaseable = true },
                287 => new Ammo() { ID = 11, Name = "Arrow", Rarity = Rarity.Common, Value = 944.28m, Type = Ammunition.Arrow, IsPurchaseable = true },
                288 => new Weapon() { ID = 15, Name = "Bat", Rarity = Rarity.Common, Value = 323.43m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 3, Cooldown = TimeSpan.FromMinutes(6.25), IsPurchaseable = true },
                289 => new Ammo() { ID = 105, Name = "30.06 Springfield", Subname = "30.06", Rarity = Rarity.Uncommon, Value = 4739.57m, Type = Ammunition.B3006 },
                290 => new Weapon() { ID = 6, Name = "Stone", Rarity = Rarity.Common, Value = 112.5m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 1, LowestDamage = 0, Cooldown = TimeSpan.FromMinutes(1), IsPurchaseable = true },
                291 => new Weapon() { ID = 55, Name = "P1911", Subname = "19", Rarity = Rarity.Uncommon, Value = 4869.53m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B45APC, HighestDamage = 18, Cooldown = TimeSpan.FromMinutes(23.525), IsPurchaseable = true },
                292 => new Armor() { ID = 13, Name = "Plastic Shield", Subname = "Plastic", Value = 5775.62m, IsActive = false, ProtectionRate = 10, Rarity = Rarity.Common, IsPurchaseable = true },
                293 => new Ammo() { ID = 75, Name = "Ball Round", Subname = "Ball", Rarity = Rarity.Uncommon, Value = 3614.95m, Type = Ammunition.BallRound },
                294 => new Ammo() { ID = 94, Name = ".50 BMG", Subname = "BMG", Rarity = Rarity.Epic, Value = 12582.19m, Type = Ammunition.B50BMG },
                295 => new Ammo() { ID = 11, Name = "Arrow", Rarity = Rarity.Common, Value = 944.28m, Type = Ammunition.Arrow, IsPurchaseable = true },
                296 => new Weapon() { ID = 6, Name = "Stone", Rarity = Rarity.Common, Value = 112.5m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 1, LowestDamage = 0, Cooldown = TimeSpan.FromMinutes(1), IsPurchaseable = true },
                297 => new Weapon() { ID = 95, Name = "SG 550", Subname = "SG", Rarity = Rarity.Epic, Value = 24389.06m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B56mm, HighestDamage = 70, LowestDamage = 55, Cooldown = TimeSpan.FromMinutes(171.025) },
                298 => new Weapon() { ID = 3, Name = "Cruid Knife", Subname = "Cruid", Rarity = Rarity.Common, Value = 645.31m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 10, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(13.25), IsPurchaseable = true },
                299 => new Money() { ID = 112, Name = "Vault", Subname = "Vlt", Rarity = Rarity.Epic, Value = 37500m, HighestAmount = 50000m, LowestAmount = 25000m, IsSellable = false },
                _ => new Armor() { ID = 93, Name = "Ballistic Shield", Subname = "Ballistic", Rarity = Rarity.Epic, Value = 83312.5m, IsActive = false, ProtectionRate = 100 },
            };
        }

        private static Item GetRandomUncommonItem()
        {
            var rNum = new Random().Next(1, 151); //Contains: uncommon, rare, epic
            return rNum switch
            {
                1 => new Weapon() { ID = 91, Name = "M249", Subname = "249", Rarity = Rarity.Epic, Value = 38800.78m, WeaponType = WeaponType.Firearm, FireRate = 5, Ammo = Ammunition.B556mm, HighestDamage = 20, LowestDamage = 15, Cooldown = TimeSpan.FromMinutes(298.75) },
                2 => new Ammo() { ID = 94, Name = ".50 BMG", Subname = "BMG", Rarity = Rarity.Epic, Value = 12582.19m, Type = Ammunition.B50BMG },
                3 => new Weapon() { ID = 95, Name = "SG 550", Subname = "SG", Rarity = Rarity.Epic, Value = 24389.06m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B56mm, HighestDamage = 70, LowestDamage = 55, Cooldown = TimeSpan.FromMinutes(171.025) },
                4 => new Weapon() { ID = 52, Name = "Tomahawk", Subname = "Toma", Rarity = Rarity.Uncommon, Value = 1836.96m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, Cooldown = TimeSpan.FromMinutes(32.75), IsPurchaseable = true },
                5 => new Weapon() { ID = 55, Name = "P1911", Subname = "19", Rarity = Rarity.Uncommon, Value = 4869.53m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B45APC, HighestDamage = 18, Cooldown = TimeSpan.FromMinutes(23.525), IsPurchaseable = true },
                6 => new Weapon() { ID = 77, Name = "M16A4", Subname = "M16", Rarity = Rarity.Rare, Value = 8850m, WeaponType = WeaponType.Firearm, FireRate = 3, Ammo = Ammunition.B556mm, HighestDamage = 14, LowestDamage = 7, Cooldown = TimeSpan.FromMinutes(95.156) },
                7 => new Weapon() { ID = 52, Name = "Tomahawk", Subname = "Toma", Rarity = Rarity.Uncommon, Value = 1836.96m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, Cooldown = TimeSpan.FromMinutes(32.75), IsPurchaseable = true },
                8 => new Weapon() { ID = 80, Name = "Combat Knife", Subname = "Combat", Rarity = Rarity.Rare, Value = 5510m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 35, LowestDamage = 30, Cooldown = TimeSpan.FromMinutes(73.875), IsPurchaseable = true },
                9 => new Ammo() { ID = 59, Name = ".30 Carbine", Subname = ".30", Rarity = Rarity.Uncommon, Value = 3695.31m, Type = Ammunition.B30Carbine },
                10 => new Weapon() { ID = 77, Name = "M16A4", Subname = "M16", Rarity = Rarity.Rare, Value = 8850m, WeaponType = WeaponType.Firearm, FireRate = 3, Ammo = Ammunition.B556mm, HighestDamage = 14, LowestDamage = 7, Cooldown = TimeSpan.FromMinutes(95.156) },
                11 => new Weapon() { ID = 52, Name = "Tomahawk", Subname = "Toma", Rarity = Rarity.Uncommon, Value = 1836.96m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, Cooldown = TimeSpan.FromMinutes(32.75), IsPurchaseable = true },
                12 => new Weapon() { ID = 62, Name = "Javelin", Subname = "Jave", Rarity = Rarity.Uncommon, Value = 1713.12m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, LowestDamage = 10, Cooldown = TimeSpan.FromMinutes(33.55), IsPurchaseable = true },
                13 => new Ammo() { ID = 53, Name = "5.56mm", Subname = "556", Rarity = Rarity.Uncommon, Value = 4038.28m, Type = Ammunition.B556mm, },
                14 => new Weapon() { ID = 62, Name = "Javelin", Subname = "Jave", Rarity = Rarity.Uncommon, Value = 1713.12m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, LowestDamage = 10, Cooldown = TimeSpan.FromMinutes(33.55), IsPurchaseable = true },
                15 => new Weapon() { ID = 52, Name = "Tomahawk", Subname = "Toma", Rarity = Rarity.Uncommon, Value = 1836.96m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, Cooldown = TimeSpan.FromMinutes(32.75), IsPurchaseable = true },
                16 => new Money() { ID = 66, Name = "Lock Box", Subname = "Box", Rarity = Rarity.Uncommon, Value = 1000m, HighestAmount = 1500m, LowestAmount = 500, IsSellable = false },
                17 => new Weapon() { ID = 67, Name = "PPSH", Subname = "PPSH", Rarity = Rarity.Uncommon, Value = 7565.62m, WeaponType = WeaponType.Firearm, FireRate = 3, Ammo = Ammunition.B762mm, HighestDamage = 20, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(109.387), IsPurchaseable = true },
                18 => new Ammo() { ID = 59, Name = ".30 Carbine", Subname = ".30", Rarity = Rarity.Uncommon, Value = 3695.31m, Type = Ammunition.B30Carbine },
                19 => new Attachment() { ID = 86, Name = "Dual Mags", Subname = "Dual", Rarity = Rarity.Rare, Value = 6739.10m, IsActive = false, Ability = Addon.ReduceCooldown, Intensity = 25 },
                20 => new Attachment() { ID = 57, Name = "Laser Sight", Subname = "Laser", Rarity = Rarity.Uncommon, Value = 4672.32m, IsActive = false, Ability = Addon.HighestDamageIncrease, Intensity = 5 },
                21 => new Weapon() { ID = 56, Name = "Thompson", Subname = "Tom", Rarity = Rarity.Uncommon, Value = 5965.62m, WeaponType = WeaponType.Firearm, FireRate = 2, Ammo = Ammunition.B45APC, HighestDamage = 17, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(53.95), IsPurchaseable = true },
                22 => new Ammo() { ID = 59, Name = ".30 Carbine", Subname = ".30", Rarity = Rarity.Uncommon, Value = 3695.31m, Type = Ammunition.B30Carbine },
                23 => new Ammo() { ID = 61, Name = "5.7mm", Subname = "57", Rarity = Rarity.Uncommon, Value = 2347.65m, Type = Ammunition.B57, },
                24 => new Weapon() { ID = 80, Name = "Combat Knife", Subname = "Combat", Rarity = Rarity.Rare, Value = 5510m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 35, LowestDamage = 30, Cooldown = TimeSpan.FromMinutes(73.875), IsPurchaseable = true },
                25 => new Weapon() { ID = 67, Name = "PPSH", Subname = "PPSH", Rarity = Rarity.Uncommon, Value = 7565.62m, WeaponType = WeaponType.Firearm, FireRate = 3, Ammo = Ammunition.B762mm, HighestDamage = 20, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(109.387), IsPurchaseable = true },
                26 => new Weapon() { ID = 76, Name = "AK-47", Subname = "AK", Rarity = Rarity.Rare, Value = 16428.12m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B762mm, HighestDamage = 65, LowestDamage = 35, Cooldown = TimeSpan.FromMinutes(125.031) },
                27 => new Weapon() { ID = 70, Name = "Broadsword", Subname = "Sword", Rarity = Rarity.Uncommon, Value = 1836.56m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, Cooldown = TimeSpan.FromMinutes(32.75), IsPurchaseable = true },
                28 => new Weapon() { ID = 84, Name = "MG-42", Subname = "MG42", Rarity = Rarity.Rare, Value = 17237.5m, WeaponType = WeaponType.Firearm, FireRate = 8, Ammo = Ammunition.B792, HighestDamage = 8, LowestDamage = 4, Cooldown = TimeSpan.FromMinutes(167.5) },
                29 => new Weapon() { ID = 52, Name = "Tomahawk", Subname = "Toma", Rarity = Rarity.Uncommon, Value = 1836.96m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, Cooldown = TimeSpan.FromMinutes(32.75), IsPurchaseable = true },
                30 => new Weapon() { ID = 80, Name = "Combat Knife", Subname = "Combat", Rarity = Rarity.Rare, Value = 5510m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 35, LowestDamage = 30, Cooldown = TimeSpan.FromMinutes(73.875), IsPurchaseable = true },
                31 => new Weapon() { ID = 76, Name = "AK-47", Subname = "AK", Rarity = Rarity.Rare, Value = 16428.12m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B762mm, HighestDamage = 65, LowestDamage = 35, Cooldown = TimeSpan.FromMinutes(125.031) },
                32 => new Weapon() { ID = 82, Name = "M1 Garand", Subname = "Garand", Rarity = Rarity.Rare, Value = 15771.87m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B3006, HighestDamage = 50, LowestDamage = 40, Cooldown = TimeSpan.FromMinutes(120.031) },
                33 => new Weapon() { ID = 62, Name = "Javelin", Subname = "Jave", Rarity = Rarity.Uncommon, Value = 1713.12m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, LowestDamage = 10, Cooldown = TimeSpan.FromMinutes(33.55), IsPurchaseable = true },
                34 => new Ammo() { ID = 103, Name = "7.92mm", Subname = "792", Rarity = Rarity.Rare, Value = 7933.59m, Type = Ammunition.B792 },
                35 => new Weapon() { ID = 80, Name = "Combat Knife", Subname = "Combat", Rarity = Rarity.Rare, Value = 5510m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 35, LowestDamage = 30, Cooldown = TimeSpan.FromMinutes(73.875), IsPurchaseable = true },
                36 => new Weapon() { ID = 80, Name = "Combat Knife", Subname = "Combat", Rarity = Rarity.Rare, Value = 5510m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 35, LowestDamage = 30, Cooldown = TimeSpan.FromMinutes(73.875), IsPurchaseable = true },
                37 => new Weapon() { ID = 87, Name = "Howa 89", Subname = "Howa", Rarity = Rarity.Rare, Value = 18134.37m, WeaponType = WeaponType.Firearm, FireRate = 4, Ammo = Ammunition.B556mm, HighestDamage = 16, LowestDamage = 8, Cooldown = TimeSpan.FromMinutes(94.25) },
                38 => new Healing() { ID = 54, Name = "First Aid", Subname = "Aid", Rarity = Rarity.Uncommon, Value = 3864.22m, MaxHealingRate = 35, LowestHealingRate = 25, Cooldown = TimeSpan.FromMinutes(34.5), IsPurchaseable = true },
                39 => new Weapon() { ID = 52, Name = "Tomahawk", Subname = "Toma", Rarity = Rarity.Uncommon, Value = 1836.96m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, Cooldown = TimeSpan.FromMinutes(32.75), IsPurchaseable = true },
                40 => new Attachment() { ID = 86, Name = "Dual Mags", Subname = "Dual", Rarity = Rarity.Rare, Value = 6739.10m, IsActive = false, Ability = Addon.ReduceCooldown, Intensity = 25 },
                41 => new Ammo() { ID = 59, Name = ".30 Carbine", Subname = ".30", Rarity = Rarity.Uncommon, Value = 3695.31m, Type = Ammunition.B30Carbine },
                42 => new Weapon() { ID = 77, Name = "M16A4", Subname = "M16", Rarity = Rarity.Rare, Value = 8850m, WeaponType = WeaponType.Firearm, FireRate = 3, Ammo = Ammunition.B556mm, HighestDamage = 14, LowestDamage = 7, Cooldown = TimeSpan.FromMinutes(95.156) },
                43 => new Money() { ID = 66, Name = "Lock Box", Subname = "Box", Rarity = Rarity.Uncommon, Value = 1000m, HighestAmount = 1500m, LowestAmount = 500, IsSellable = false },
                44 => new Weapon() { ID = 68, Name = "Cross Bow", Subname = "Cross", Rarity = Rarity.Uncommon, Value = 4330.46m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.Arrow, HighestDamage = 16, Cooldown = TimeSpan.FromMinutes(38.36), IsPurchaseable = true },
                45 => new Weapon() { ID = 62, Name = "Javelin", Subname = "Jave", Rarity = Rarity.Uncommon, Value = 1713.12m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, LowestDamage = 10, Cooldown = TimeSpan.FromMinutes(33.55), IsPurchaseable = true },
                46 => new Weapon() { ID = 56, Name = "Thompson", Subname = "Tom", Rarity = Rarity.Uncommon, Value = 5965.62m, WeaponType = WeaponType.Firearm, FireRate = 2, Ammo = Ammunition.B45APC, HighestDamage = 17, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(53.95), IsPurchaseable = true },
                47 => new Ammo() { ID = 104, Name = "B56mm", Subname = "56", Rarity = Rarity.Rare, Value = 9640.62m, Type = Ammunition.B56mm },
                48 => new Weapon() { ID = 52, Name = "Tomahawk", Subname = "Toma", Rarity = Rarity.Uncommon, Value = 1836.96m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, Cooldown = TimeSpan.FromMinutes(32.75), IsPurchaseable = true },
                49 => new Healing() { ID = 85, Name = "Morphine", Subname = "Morph", Rarity = Rarity.Rare, Value = 8024.21m, MaxHealingRate = 65, LowestHealingRate = 50, Cooldown = TimeSpan.FromMinutes(69) },
                50 => new Weapon() { ID = 52, Name = "Tomahawk", Subname = "Toma", Rarity = Rarity.Uncommon, Value = 1836.96m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, Cooldown = TimeSpan.FromMinutes(32.75), IsPurchaseable = true },
                51 => new Weapon() { ID = 70, Name = "Broadsword", Subname = "Sword", Rarity = Rarity.Uncommon, Value = 1836.56m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, Cooldown = TimeSpan.FromMinutes(32.75), IsPurchaseable = true },
                52 => new Weapon() { ID = 71, Name = "G36C", Subname = "G36", Rarity = Rarity.Uncommon, Value = 4156.87m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B556mm, HighestDamage = 40, LowestDamage = 10, Cooldown = TimeSpan.FromMinutes(64.925) },
                53 => new Weapon() { ID = 83, Name = "Kar-98", Subname = "Kar", Rarity = Rarity.Rare, Value = 18068.75m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B762mm, HighestDamage = 55, Cooldown = TimeSpan.FromMinutes(186.593) },
                54 => new Weapon() { ID = 72, Name = "MK 12", Subname = "MK", Rarity = Rarity.Uncommon, Value = 6103.9m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B556mm, HighestDamage = 30, Cooldown = TimeSpan.FromMinutes(89.592) },
                55 => new Weapon() { ID = 60, Name = "Five-Seven", Subname = "FiveSeven", Rarity = Rarity.Uncommon, Value = 5139.06m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B57, HighestDamage = 20, LowestDamage = 15, Cooldown = TimeSpan.FromMinutes(40.257), IsPurchaseable = true },
                56 => new Attachment() { ID = 64, Name = "Mag Strap", Subname = "Strap", Rarity = Rarity.Uncommon, Value = 2251.79m, IsActive = false, Ability = Addon.ReduceCooldown, Intensity = 10 },
                57 => new Weapon() { ID = 62, Name = "Javelin", Subname = "Jave", Rarity = Rarity.Uncommon, Value = 1713.12m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, LowestDamage = 10, Cooldown = TimeSpan.FromMinutes(33.55), IsPurchaseable = true },
                58 => new Ammo() { ID = 105, Name = "30.06 Springfield", Subname = "30.06", Rarity = Rarity.Uncommon, Value = 4739.57m, Type = Ammunition.B3006 },
                59 => new Ammo() { ID = 75, Name = "Ball Round", Subname = "Ball", Rarity = Rarity.Uncommon, Value = 3614.95m, Type = Ammunition.BallRound },
                60 => new Healing() { ID = 85, Name = "Morphine", Subname = "Morph", Rarity = Rarity.Rare, Value = 8024.21m, MaxHealingRate = 65, LowestHealingRate = 50, Cooldown = TimeSpan.FromMinutes(69) },
                61 => new Weapon() { ID = 52, Name = "Tomahawk", Subname = "Toma", Rarity = Rarity.Uncommon, Value = 1836.96m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, Cooldown = TimeSpan.FromMinutes(32.75), IsPurchaseable = true },
                62 => new Ammo() { ID = 53, Name = "5.56mm", Subname = "556", Rarity = Rarity.Uncommon, Value = 4038.28m, Type = Ammunition.B556mm, },
                63 => new Healing() { ID = 63, Name = "Bandage", Subname = "Bandage", Rarity = Rarity.Uncommon, Value = 1832m, MaxHealingRate = 20, LowestHealingRate = 10, Cooldown = TimeSpan.FromMinutes(17.25), IsPurchaseable = true },
                64 => new Armor() { ID = 69, Name = "Mail Armor", Subname = "Mail", Rarity = Rarity.Uncommon, Value = 11037.52m, IsActive = false, ProtectionRate = 20 },
                65 => new Weapon() { ID = 52, Name = "Tomahawk", Subname = "Toma", Rarity = Rarity.Uncommon, Value = 1836.96m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, Cooldown = TimeSpan.FromMinutes(32.75), IsPurchaseable = true },
                66 => new Attachment() { ID = 64, Name = "Mag Strap", Subname = "Strap", Rarity = Rarity.Uncommon, Value = 2251.79m, IsActive = false, Ability = Addon.ReduceCooldown, Intensity = 10 },
                67 => new Ammo() { ID = 105, Name = "30.06 Springfield", Subname = "30.06", Rarity = Rarity.Uncommon, Value = 4739.57m, Type = Ammunition.B3006 },
                68 => new Weapon() { ID = 56, Name = "Thompson", Subname = "Tom", Rarity = Rarity.Uncommon, Value = 5965.62m, WeaponType = WeaponType.Firearm, FireRate = 2, Ammo = Ammunition.B45APC, HighestDamage = 17, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(53.95), IsPurchaseable = true },
                69 => new Money() { ID = 66, Name = "Lock Box", Subname = "Box", Rarity = Rarity.Uncommon, Value = 1000m, HighestAmount = 1500m, LowestAmount = 500, IsSellable = false },
                70 => new Weapon() { ID = 89, Name = "Bayonet", Subname = "Bay", Rarity = Rarity.Rare, Value = 4825.62m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 30, LowestDamage = 25, Cooldown = TimeSpan.FromMinutes(60.5) },
                71 => new Weapon() { ID = 70, Name = "Broadsword", Subname = "Sword", Rarity = Rarity.Uncommon, Value = 1836.56m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, Cooldown = TimeSpan.FromMinutes(32.75), IsPurchaseable = true },
                72 => new Healing() { ID = 54, Name = "First Aid", Subname = "Aid", Rarity = Rarity.Uncommon, Value = 3864.22m, MaxHealingRate = 35, LowestHealingRate = 25, Cooldown = TimeSpan.FromMinutes(34.5), IsPurchaseable = true },
                73 => new Weapon() { ID = 90, Name = "M1 Carbine", Subname = "Carbine", Rarity = Rarity.Rare, Value = 11506.25m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B30Carbine, HighestDamage = 40, LowestDamage = 30, Cooldown = TimeSpan.FromMinutes(83.468) },
                74 => new Weapon() { ID = 51, Name = "P90", Subname = "90", Rarity = Rarity.Uncommon, Value = 4687.5m, WeaponType = WeaponType.Firearm, FireRate = 2, Ammo = Ammunition.B9mm, HighestDamage = 12, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(43.18), IsPurchaseable = true },
                75 => new Ammo() { ID = 61, Name = "5.7mm", Subname = "57", Rarity = Rarity.Uncommon, Value = 2347.65m, Type = Ammunition.B57, },
                76 => new Ammo() { ID = 75, Name = "Ball Round", Subname = "Ball", Rarity = Rarity.Uncommon, Value = 3614.95m, Type = Ammunition.BallRound },
                77 => new Money() { ID = 73, Name = "Stash", Subname = "Stash", Rarity = Rarity.Uncommon, Value = 3000m, HighestAmount = 5000m, LowestAmount = 1000m, IsSellable = false },
                78 => new Weapon() { ID = 55, Name = "P1911", Subname = "19", Rarity = Rarity.Uncommon, Value = 4869.53m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B45APC, HighestDamage = 18, Cooldown = TimeSpan.FromMinutes(23.525), IsPurchaseable = true },
                79 => new Weapon() { ID = 89, Name = "Bayonet", Subname = "Bay", Rarity = Rarity.Rare, Value = 4825.62m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 30, LowestDamage = 25, Cooldown = TimeSpan.FromMinutes(60.5) },
                80 => new Healing() { ID = 63, Name = "Bandage", Subname = "Bandage", Rarity = Rarity.Uncommon, Value = 1832m, MaxHealingRate = 20, LowestHealingRate = 10, Cooldown = TimeSpan.FromMinutes(17.25), IsPurchaseable = true },
                81 => new Weapon() { ID = 55, Name = "P1911", Subname = "19", Rarity = Rarity.Uncommon, Value = 4869.53m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B45APC, HighestDamage = 18, Cooldown = TimeSpan.FromMinutes(23.525), IsPurchaseable = true },
                82 => new Weapon() { ID = 60, Name = "Five-Seven", Subname = "FiveSeven", Rarity = Rarity.Uncommon, Value = 5139.06m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B57, HighestDamage = 20, LowestDamage = 15, Cooldown = TimeSpan.FromMinutes(40.257), IsPurchaseable = true },
                83 => new Weapon() { ID = 72, Name = "MK 12", Subname = "MK", Rarity = Rarity.Uncommon, Value = 6103.9m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B556mm, HighestDamage = 30, Cooldown = TimeSpan.FromMinutes(89.592) },
                84 => new Ammo() { ID = 75, Name = "Ball Round", Subname = "Ball", Rarity = Rarity.Uncommon, Value = 3614.95m, Type = Ammunition.BallRound },
                85 => new Weapon() { ID = 62, Name = "Javelin", Subname = "Jave", Rarity = Rarity.Uncommon, Value = 1713.12m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, LowestDamage = 10, Cooldown = TimeSpan.FromMinutes(33.55), IsPurchaseable = true },
                86 => new Weapon() { ID = 70, Name = "Broadsword", Subname = "Sword", Rarity = Rarity.Uncommon, Value = 1836.56m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, Cooldown = TimeSpan.FromMinutes(32.75), IsPurchaseable = true },
                87 => new Armor() { ID = 58, Name = "Tactical Vest", Subname = "Tactical", Rarity = Rarity.Uncommon, Value = 13075.83m, IsActive = false, ProtectionRate = 40 },
                88 => new Weapon() { ID = 55, Name = "P1911", Subname = "19", Rarity = Rarity.Uncommon, Value = 4869.53m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B45APC, HighestDamage = 18, Cooldown = TimeSpan.FromMinutes(23.525), IsPurchaseable = true },
                89 => new Weapon() { ID = 74, Name = "Musket", Subname = "Musk", Rarity = Rarity.Uncommon, Value = 4803.9m, WeaponType = WeaponType.Firearm, HighestDamage = 60, LowestDamage = 1, FireRate = 1, Ammo = Ammunition.BallRound, Cooldown = TimeSpan.FromMinutes(121.85) },
                90 => new Weapon() { ID = 71, Name = "G36C", Subname = "G36", Rarity = Rarity.Uncommon, Value = 4156.87m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B556mm, HighestDamage = 40, LowestDamage = 10, Cooldown = TimeSpan.FromMinutes(64.925) },
                91 => new Weapon() { ID = 70, Name = "Broadsword", Subname = "Sword", Rarity = Rarity.Uncommon, Value = 1836.56m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, Cooldown = TimeSpan.FromMinutes(32.75), IsPurchaseable = true },
                92 => new Money() { ID = 66, Name = "Lock Box", Subname = "Box", Rarity = Rarity.Uncommon, Value = 1000m, HighestAmount = 1500m, LowestAmount = 500, IsSellable = false },
                93 => new Money() { ID = 111, Name = "Money Bag", Subname = "Bag", Rarity = Rarity.Rare, Value = 6250m, HighestAmount = 10000m, LowestAmount = 2500m, IsSellable = false },
                94 => new Weapon() { ID = 68, Name = "Cross Bow", Subname = "Cross", Rarity = Rarity.Uncommon, Value = 4330.46m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.Arrow, HighestDamage = 16, Cooldown = TimeSpan.FromMinutes(38.36), IsPurchaseable = true },
                95 => new Weapon() { ID = 72, Name = "MK 12", Subname = "MK", Rarity = Rarity.Uncommon, Value = 6103.9m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B556mm, HighestDamage = 30, Cooldown = TimeSpan.FromMinutes(89.592) },
                96 => new Weapon() { ID = 68, Name = "Cross Bow", Subname = "Cross", Rarity = Rarity.Uncommon, Value = 4330.46m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.Arrow, HighestDamage = 16, Cooldown = TimeSpan.FromMinutes(38.36), IsPurchaseable = true },
                97 => new Weapon() { ID = 51, Name = "P90", Subname = "90", Rarity = Rarity.Uncommon, Value = 4687.5m, WeaponType = WeaponType.Firearm, FireRate = 2, Ammo = Ammunition.B9mm, HighestDamage = 12, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(43.18), IsPurchaseable = true },
                98 => new Attachment() { ID = 57, Name = "Laser Sight", Subname = "Laser", Rarity = Rarity.Uncommon, Value = 4672.32m, IsActive = false, Ability = Addon.HighestDamageIncrease, Intensity = 5 },
                99 => new Attachment() { ID = 64, Name = "Mag Strap", Subname = "Strap", Rarity = Rarity.Uncommon, Value = 2251.79m, IsActive = false, Ability = Addon.ReduceCooldown, Intensity = 10 },
                100 => new Ammo() { ID = 75, Name = "Ball Round", Subname = "Ball", Rarity = Rarity.Uncommon, Value = 3614.95m, Type = Ammunition.BallRound },
                101 => new Weapon() { ID = 60, Name = "Five-Seven", Subname = "FiveSeven", Rarity = Rarity.Uncommon, Value = 5139.06m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B57, HighestDamage = 20, LowestDamage = 15, Cooldown = TimeSpan.FromMinutes(40.257), IsPurchaseable = true },
                102 => new Attachment() { ID = 65, Name = "Weapon Stand", Subname = "Stand", Rarity = Rarity.Uncommon, Value = 2646.87m, IsActive = false, Ability = Addon.HighestDamageIncrease, Intensity = 3 },
                103 => new Weapon() { ID = 71, Name = "G36C", Subname = "G36", Rarity = Rarity.Uncommon, Value = 4156.87m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B556mm, HighestDamage = 40, LowestDamage = 10, Cooldown = TimeSpan.FromMinutes(64.925) },
                104 => new Weapon() { ID = 62, Name = "Javelin", Subname = "Jave", Rarity = Rarity.Uncommon, Value = 1713.12m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, LowestDamage = 10, Cooldown = TimeSpan.FromMinutes(33.55), IsPurchaseable = true },
                105 => new Healing() { ID = 63, Name = "Bandage", Subname = "Bandage", Rarity = Rarity.Uncommon, Value = 1832m, MaxHealingRate = 20, LowestHealingRate = 10, Cooldown = TimeSpan.FromMinutes(17.25), IsPurchaseable = true },
                106 => new Healing() { ID = 54, Name = "First Aid", Subname = "Aid", Rarity = Rarity.Uncommon, Value = 3864.22m, MaxHealingRate = 35, LowestHealingRate = 25, Cooldown = TimeSpan.FromMinutes(34.5), IsPurchaseable = true },
                107 => new Ammo() { ID = 103, Name = "7.92mm", Subname = "792", Rarity = Rarity.Rare, Value = 7933.59m, Type = Ammunition.B792 },
                108 => new Weapon() { ID = 70, Name = "Broadsword", Subname = "Sword", Rarity = Rarity.Uncommon, Value = 1836.56m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, Cooldown = TimeSpan.FromMinutes(32.75), IsPurchaseable = true },
                109 => new Money() { ID = 73, Name = "Stash", Subname = "Stash", Rarity = Rarity.Uncommon, Value = 3000m, HighestAmount = 5000m, LowestAmount = 1000m, IsSellable = false },
                110 => new Ammo() { ID = 61, Name = "5.7mm", Subname = "57", Rarity = Rarity.Uncommon, Value = 2347.65m, Type = Ammunition.B57, },
                111 => new Attachment() { ID = 64, Name = "Mag Strap", Subname = "Strap", Rarity = Rarity.Uncommon, Value = 2251.79m, IsActive = false, Ability = Addon.ReduceCooldown, Intensity = 10 },
                112 => new Weapon() { ID = 68, Name = "Cross Bow", Subname = "Cross", Rarity = Rarity.Uncommon, Value = 4330.46m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.Arrow, HighestDamage = 16, Cooldown = TimeSpan.FromMinutes(38.36), IsPurchaseable = true },
                113 => new Weapon() { ID = 90, Name = "M1 Carbine", Subname = "Carbine", Rarity = Rarity.Rare, Value = 11506.25m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B30Carbine, HighestDamage = 40, LowestDamage = 30, Cooldown = TimeSpan.FromMinutes(83.468) },
                114 => new Weapon() { ID = 62, Name = "Javelin", Subname = "Jave", Rarity = Rarity.Uncommon, Value = 1713.12m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, LowestDamage = 10, Cooldown = TimeSpan.FromMinutes(33.55), IsPurchaseable = true },
                115 => new Ammo() { ID = 81, Name = "7.62mm", Subname = "762", Rarity = Rarity.Rare, Value = 8921.87m, Type = Ammunition.B762mm },
                116 => new Attachment() { ID = 65, Name = "Weapon Stand", Subname = "Stand", Rarity = Rarity.Uncommon, Value = 2646.87m, IsActive = false, Ability = Addon.HighestDamageIncrease, Intensity = 3 },
                117 => new Weapon() { ID = 60, Name = "Five-Seven", Subname = "FiveSeven", Rarity = Rarity.Uncommon, Value = 5139.06m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B57, HighestDamage = 20, LowestDamage = 15, Cooldown = TimeSpan.FromMinutes(40.257), IsPurchaseable = true },
                118 => new Weapon() { ID = 60, Name = "Five-Seven", Subname = "FiveSeven", Rarity = Rarity.Uncommon, Value = 5139.06m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B57, HighestDamage = 20, LowestDamage = 15, Cooldown = TimeSpan.FromMinutes(40.257), IsPurchaseable = true },
                119 => new Money() { ID = 66, Name = "Lock Box", Subname = "Box", Rarity = Rarity.Uncommon, Value = 1000m, HighestAmount = 1500m, LowestAmount = 500, IsSellable = false },
                120 => new Weapon() { ID = 89, Name = "Bayonet", Subname = "Bay", Rarity = Rarity.Rare, Value = 4825.62m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 30, LowestDamage = 25, Cooldown = TimeSpan.FromMinutes(60.5) },
                121 => new Weapon() { ID = 74, Name = "Musket", Subname = "Musk", Rarity = Rarity.Uncommon, Value = 4803.9m, WeaponType = WeaponType.Firearm, HighestDamage = 60, LowestDamage = 1, FireRate = 1, Ammo = Ammunition.BallRound, Cooldown = TimeSpan.FromMinutes(121.85) },
                122 => new Money() { ID = 111, Name = "Money Bag", Subname = "Bag", Rarity = Rarity.Rare, Value = 6250m, HighestAmount = 10000m, LowestAmount = 2500m, IsSellable = false },
                123 => new Attachment() { ID = 88, Name = "4x Scope", Subname = "4x", Rarity = Rarity.Rare, Value = 9471.99m, IsActive = false, Ability = Addon.HighestDamageIncrease, Intensity = 10 },
                124 => new Weapon() { ID = 20, Name = "Remington", Rarity = Rarity.Rare, Value = 12421.87m, WeaponType = WeaponType.Firearm, FireRate = 6, HighestDamage = 15, LowestDamage = 1, Ammo = Ammunition.B12Gauge, Cooldown = TimeSpan.FromMinutes(151.5) },
                125 => new Weapon() { ID = 70, Name = "Broadsword", Subname = "Sword", Rarity = Rarity.Uncommon, Value = 1836.56m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, Cooldown = TimeSpan.FromMinutes(32.75), IsPurchaseable = true },
                126 => new Ammo() { ID = 53, Name = "5.56mm", Subname = "556", Rarity = Rarity.Uncommon, Value = 4038.28m, Type = Ammunition.B556mm, },
                127 => new Healing() { ID = 78, Name = "Blood Bag", Subname = "Blood", Rarity = Rarity.Epic, Value = 14784m, MaxHealingRate = 90, LowestHealingRate = 70, Cooldown = TimeSpan.FromMinutes(92) },
                128 => new Weapon() { ID = 70, Name = "Broadsword", Subname = "Sword", Rarity = Rarity.Uncommon, Value = 1836.56m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, Cooldown = TimeSpan.FromMinutes(32.75), IsPurchaseable = true },
                129 => new Ammo() { ID = 81, Name = "7.62mm", Subname = "762", Rarity = Rarity.Rare, Value = 8921.87m, Type = Ammunition.B762mm },
                130 => new Weapon() { ID = 62, Name = "Javelin", Subname = "Jave", Rarity = Rarity.Uncommon, Value = 1713.12m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, LowestDamage = 10, Cooldown = TimeSpan.FromMinutes(33.55), IsPurchaseable = true },
                131 => new Ammo() { ID = 53, Name = "5.56mm", Subname = "556", Rarity = Rarity.Uncommon, Value = 4038.28m, Type = Ammunition.B556mm, },
                132 => new Healing() { ID = 54, Name = "First Aid", Subname = "Aid", Rarity = Rarity.Uncommon, Value = 3864.22m, MaxHealingRate = 35, LowestHealingRate = 25, Cooldown = TimeSpan.FromMinutes(34.5), IsPurchaseable = true },
                133 => new Weapon() { ID = 51, Name = "P90", Subname = "90", Rarity = Rarity.Uncommon, Value = 4687.5m, WeaponType = WeaponType.Firearm, FireRate = 2, Ammo = Ammunition.B9mm, HighestDamage = 12, LowestDamage = 5, Cooldown = TimeSpan.FromMinutes(43.18), IsPurchaseable = true },
                134 => new Weapon() { ID = 89, Name = "Bayonet", Subname = "Bay", Rarity = Rarity.Rare, Value = 4825.62m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 30, LowestDamage = 25, Cooldown = TimeSpan.FromMinutes(60.5) },
                135 => new Weapon() { ID = 70, Name = "Broadsword", Subname = "Sword", Rarity = Rarity.Uncommon, Value = 1836.56m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, Cooldown = TimeSpan.FromMinutes(32.75), IsPurchaseable = true },
                136 => new Weapon() { ID = 89, Name = "Bayonet", Subname = "Bay", Rarity = Rarity.Rare, Value = 4825.62m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 30, LowestDamage = 25, Cooldown = TimeSpan.FromMinutes(60.5) },
                137 => new Armor() { ID = 69, Name = "Mail Armor", Subname = "Mail", Rarity = Rarity.Uncommon, Value = 11037.52m, IsActive = false, ProtectionRate = 20 },
                138 => new Healing() { ID = 63, Name = "Bandage", Subname = "Bandage", Rarity = Rarity.Uncommon, Value = 1832m, MaxHealingRate = 20, LowestHealingRate = 10, Cooldown = TimeSpan.FromMinutes(17.25), IsPurchaseable = true },
                139 => new Ammo() { ID = 61, Name = "5.7mm", Subname = "57", Rarity = Rarity.Uncommon, Value = 2347.65m, Type = Ammunition.B57, },
                140 => new Weapon() { ID = 62, Name = "Javelin", Subname = "Jave", Rarity = Rarity.Uncommon, Value = 1713.12m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 15, LowestDamage = 10, Cooldown = TimeSpan.FromMinutes(33.55), IsPurchaseable = true },
                141 => new Money() { ID = 111, Name = "Money Bag", Subname = "Bag", Rarity = Rarity.Rare, Value = 6250m, HighestAmount = 10000m, LowestAmount = 2500m, IsSellable = false },
                142 => new Healing() { ID = 63, Name = "Bandage", Subname = "Bandage", Rarity = Rarity.Uncommon, Value = 1832m, MaxHealingRate = 20, LowestHealingRate = 10, Cooldown = TimeSpan.FromMinutes(17.25), IsPurchaseable = true },
                143 => new Money() { ID = 73, Name = "Stash", Subname = "Stash", Rarity = Rarity.Uncommon, Value = 3000m, HighestAmount = 5000m, LowestAmount = 1000m, IsSellable = false },
                144 => new Weapon() { ID = 20, Name = "Remington", Rarity = Rarity.Rare, Value = 12421.87m, WeaponType = WeaponType.Firearm, FireRate = 6, HighestDamage = 15, LowestDamage = 1, Ammo = Ammunition.B12Gauge, Cooldown = TimeSpan.FromMinutes(151.5) },
                145 => new Armor() { ID = 79, Name = "Kevlar Set", Subname = "Kevlar", Rarity = Rarity.Rare, Value = 27562.5m, IsActive = false, ProtectionRate = 75 },
                146 => new Attachment() { ID = 65, Name = "Weapon Stand", Subname = "Stand", Rarity = Rarity.Uncommon, Value = 2646.87m, IsActive = false, Ability = Addon.HighestDamageIncrease, Intensity = 3 },
                147 => new Ammo() { ID = 61, Name = "5.7mm", Subname = "57", Rarity = Rarity.Uncommon, Value = 2347.65m, Type = Ammunition.B57, },
                148 => new Weapon() { ID = 92, Name = "TAC-50", Subname = "Tac", Rarity = Rarity.Epic, Value = 44830.46m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B50BMG, HighestDamage = 90, Cooldown = TimeSpan.FromMinutes(240.875) },
                149 => new Money() { ID = 112, Name = "Vault", Subname = "Vlt", Rarity = Rarity.Epic, Value = 37500m, HighestAmount = 50000m, LowestAmount = 25000m, IsSellable = false },
                _ => new Armor() { ID = 93, Name = "Ballistic Shield", Subname = "Ballistic", Rarity = Rarity.Epic, Value = 83312.5m, IsActive = false, ProtectionRate = 100 },
            };
        }

        private static Item GetRandomRareItem() //Contains: rare, epic
        {
            var rNum = new Random().Next(1, 101);
            return rNum switch
            {
                1 => new Weapon() { ID = 95, Name = "SG 550", Subname = "SG", Rarity = Rarity.Epic, Value = 24389.06m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B56mm, HighestDamage = 70, LowestDamage = 55, Cooldown = TimeSpan.FromMinutes(171.025) },
                2 => new Ammo() { ID = 94, Name = ".50 BMG", Subname = "BMG", Rarity = Rarity.Epic, Value = 12582.19m, Type = Ammunition.B50BMG },
                3 => new Healing() { ID = 78, Name = "Blood Bag", Subname = "Blood", Rarity = Rarity.Epic, Value = 14784m, MaxHealingRate = 90, LowestHealingRate = 70, Cooldown = TimeSpan.FromMinutes(92) },
                4 => new Weapon() { ID = 83, Name = "Kar-98", Subname = "Kar", Rarity = Rarity.Rare, Value = 18068.75m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B762mm, HighestDamage = 55, Cooldown = TimeSpan.FromMinutes(186.593) },
                5 => new Weapon() { ID = 80, Name = "Combat Knife", Subname = "Combat", Rarity = Rarity.Rare, Value = 5510m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 35, LowestDamage = 30, Cooldown = TimeSpan.FromMinutes(73.875), IsPurchaseable = true },
                6 => new Ammo() { ID = 81, Name = "7.62mm", Subname = "762", Rarity = Rarity.Rare, Value = 8921.87m, Type = Ammunition.B762mm },
                7 => new Ammo() { ID = 104, Name = "B56mm", Subname = "56", Rarity = Rarity.Rare, Value = 9640.62m, Type = Ammunition.B56mm },
                8 => new Weapon() { ID = 89, Name = "Bayonet", Subname = "Bay", Rarity = Rarity.Rare, Value = 4825.62m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 30, LowestDamage = 25, Cooldown = TimeSpan.FromMinutes(60.5) },
                9 => new Weapon() { ID = 90, Name = "M1 Carbine", Subname = "Carbine", Rarity = Rarity.Rare, Value = 11506.25m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B30Carbine, HighestDamage = 40, LowestDamage = 30, Cooldown = TimeSpan.FromMinutes(83.468) },
                10 => new Weapon() { ID = 90, Name = "M1 Carbine", Subname = "Carbine", Rarity = Rarity.Rare, Value = 11506.25m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B30Carbine, HighestDamage = 40, LowestDamage = 30, Cooldown = TimeSpan.FromMinutes(83.468) },
                11 => new Money() { ID = 111, Name = "Money Bag", Subname = "Bag", Rarity = Rarity.Rare, Value = 6250m, HighestAmount = 10000m, LowestAmount = 2500m, IsSellable = false },
                12 => new Weapon() { ID = 87, Name = "Howa 89", Subname = "Howa", Rarity = Rarity.Rare, Value = 18134.37m, WeaponType = WeaponType.Firearm, FireRate = 4, Ammo = Ammunition.B556mm, HighestDamage = 16, LowestDamage = 8, Cooldown = TimeSpan.FromMinutes(94.25) },
                13 => new Ammo() { ID = 104, Name = "B56mm", Subname = "56", Rarity = Rarity.Rare, Value = 9640.62m, Type = Ammunition.B56mm },
                14 => new Weapon() { ID = 89, Name = "Bayonet", Subname = "Bay", Rarity = Rarity.Rare, Value = 4825.62m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 30, LowestDamage = 25, Cooldown = TimeSpan.FromMinutes(60.5) },
                15 => new Weapon() { ID = 90, Name = "M1 Carbine", Subname = "Carbine", Rarity = Rarity.Rare, Value = 11506.25m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B30Carbine, HighestDamage = 40, LowestDamage = 30, Cooldown = TimeSpan.FromMinutes(83.468) },
                16 => new Ammo() { ID = 81, Name = "7.62mm", Subname = "762", Rarity = Rarity.Rare, Value = 8921.87m, Type = Ammunition.B762mm },
                17 => new Weapon() { ID = 80, Name = "Combat Knife", Subname = "Combat", Rarity = Rarity.Rare, Value = 5510m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 35, LowestDamage = 30, Cooldown = TimeSpan.FromMinutes(73.875), IsPurchaseable = true },
                18 => new Weapon() { ID = 76, Name = "AK-47", Subname = "AK", Rarity = Rarity.Rare, Value = 16428.12m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B762mm, HighestDamage = 65, LowestDamage = 35, Cooldown = TimeSpan.FromMinutes(125.031) },
                19 => new Weapon() { ID = 80, Name = "Combat Knife", Subname = "Combat", Rarity = Rarity.Rare, Value = 5510m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 35, LowestDamage = 30, Cooldown = TimeSpan.FromMinutes(73.875), IsPurchaseable = true },
                20 => new Weapon() { ID = 80, Name = "Combat Knife", Subname = "Combat", Rarity = Rarity.Rare, Value = 5510m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 35, LowestDamage = 30, Cooldown = TimeSpan.FromMinutes(73.875), IsPurchaseable = true },
                21 => new Ammo() { ID = 103, Name = "7.92mm", Subname = "792", Rarity = Rarity.Rare, Value = 7933.59m, Type = Ammunition.B792 },
                22 => new Weapon() { ID = 90, Name = "M1 Carbine", Subname = "Carbine", Rarity = Rarity.Rare, Value = 11506.25m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B30Carbine, HighestDamage = 40, LowestDamage = 30, Cooldown = TimeSpan.FromMinutes(83.468) },
                23 => new Money() { ID = 111, Name = "Money Bag", Subname = "Bag", Rarity = Rarity.Rare, Value = 6250m, HighestAmount = 10000m, LowestAmount = 2500m, IsSellable = false },
                24 => new Weapon() { ID = 90, Name = "M1 Carbine", Subname = "Carbine", Rarity = Rarity.Rare, Value = 11506.25m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B30Carbine, HighestDamage = 40, LowestDamage = 30, Cooldown = TimeSpan.FromMinutes(83.468) },
                25 => new Weapon() { ID = 20, Name = "Remington", Rarity = Rarity.Rare, Value = 12421.87m, WeaponType = WeaponType.Firearm, FireRate = 6, HighestDamage = 15, LowestDamage = 1, Ammo = Ammunition.B12Gauge, Cooldown = TimeSpan.FromMinutes(151.5) },
                26 => new Ammo() { ID = 81, Name = "7.62mm", Subname = "762", Rarity = Rarity.Rare, Value = 8921.87m, Type = Ammunition.B762mm },
                27 => new Weapon() { ID = 89, Name = "Bayonet", Subname = "Bay", Rarity = Rarity.Rare, Value = 4825.62m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 30, LowestDamage = 25, Cooldown = TimeSpan.FromMinutes(60.5) },
                28 => new Weapon() { ID = 90, Name = "M1 Carbine", Subname = "Carbine", Rarity = Rarity.Rare, Value = 11506.25m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B30Carbine, HighestDamage = 40, LowestDamage = 30, Cooldown = TimeSpan.FromMinutes(83.468) },
                29 => new Weapon() { ID = 77, Name = "M16A4", Subname = "M16", Rarity = Rarity.Rare, Value = 8850m, WeaponType = WeaponType.Firearm, FireRate = 3, Ammo = Ammunition.B556mm, HighestDamage = 14, LowestDamage = 7, Cooldown = TimeSpan.FromMinutes(95.156) },
                30 => new Weapon() { ID = 84, Name = "MG-42", Subname = "MG42", Rarity = Rarity.Rare, Value = 17237.5m, WeaponType = WeaponType.Firearm, FireRate = 8, Ammo = Ammunition.B792, HighestDamage = 8, LowestDamage = 4, Cooldown = TimeSpan.FromMinutes(167.5) },
                31 => new Attachment() { ID = 86, Name = "Dual Mags", Subname = "Dual", Rarity = Rarity.Rare, Value = 6739.10m, IsActive = false, Ability = Addon.ReduceCooldown, Intensity = 25 },
                32 => new Weapon() { ID = 89, Name = "Bayonet", Subname = "Bay", Rarity = Rarity.Rare, Value = 4825.62m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 30, LowestDamage = 25, Cooldown = TimeSpan.FromMinutes(60.5) },
                33 => new Weapon() { ID = 82, Name = "M1 Garand", Subname = "Garand", Rarity = Rarity.Rare, Value = 15771.87m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B3006, HighestDamage = 50, LowestDamage = 40, Cooldown = TimeSpan.FromMinutes(120.031) },
                34 => new Weapon() { ID = 76, Name = "AK-47", Subname = "AK", Rarity = Rarity.Rare, Value = 16428.12m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B762mm, HighestDamage = 65, LowestDamage = 35, Cooldown = TimeSpan.FromMinutes(125.031) },
                35 => new Ammo() { ID = 103, Name = "7.92mm", Subname = "792", Rarity = Rarity.Rare, Value = 7933.59m, Type = Ammunition.B792 },
                36 => new Weapon() { ID = 90, Name = "M1 Carbine", Subname = "Carbine", Rarity = Rarity.Rare, Value = 11506.25m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B30Carbine, HighestDamage = 40, LowestDamage = 30, Cooldown = TimeSpan.FromMinutes(83.468) },
                37 => new Healing() { ID = 85, Name = "Morphine", Subname = "Morph", Rarity = Rarity.Rare, Value = 8024.21m, MaxHealingRate = 65, LowestHealingRate = 50, Cooldown = TimeSpan.FromMinutes(69) },
                38 => new Attachment() { ID = 86, Name = "Dual Mags", Subname = "Dual", Rarity = Rarity.Rare, Value = 6739.10m, IsActive = false, Ability = Addon.ReduceCooldown, Intensity = 25 },
                39 => new Money() { ID = 111, Name = "Money Bag", Subname = "Bag", Rarity = Rarity.Rare, Value = 6250m, HighestAmount = 10000m, LowestAmount = 2500m, IsSellable = false },
                40 => new Ammo() { ID = 104, Name = "B56mm", Subname = "56", Rarity = Rarity.Rare, Value = 9640.62m, Type = Ammunition.B56mm },
                41 => new Ammo() { ID = 103, Name = "7.92mm", Subname = "792", Rarity = Rarity.Rare, Value = 7933.59m, Type = Ammunition.B792 },
                42 => new Weapon() { ID = 89, Name = "Bayonet", Subname = "Bay", Rarity = Rarity.Rare, Value = 4825.62m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 30, LowestDamage = 25, Cooldown = TimeSpan.FromMinutes(60.5) },
                43 => new Weapon() { ID = 77, Name = "M16A4", Subname = "M16", Rarity = Rarity.Rare, Value = 8850m, WeaponType = WeaponType.Firearm, FireRate = 3, Ammo = Ammunition.B556mm, HighestDamage = 14, LowestDamage = 7, Cooldown = TimeSpan.FromMinutes(95.156) },
                44 => new Weapon() { ID = 80, Name = "Combat Knife", Subname = "Combat", Rarity = Rarity.Rare, Value = 5510m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 35, LowestDamage = 30, Cooldown = TimeSpan.FromMinutes(73.875), IsPurchaseable = true },
                45 => new Weapon() { ID = 84, Name = "MG-42", Subname = "MG42", Rarity = Rarity.Rare, Value = 17237.5m, WeaponType = WeaponType.Firearm, FireRate = 8, Ammo = Ammunition.B792, HighestDamage = 8, LowestDamage = 4, Cooldown = TimeSpan.FromMinutes(167.5) },
                46 => new Healing() { ID = 85, Name = "Morphine", Subname = "Morph", Rarity = Rarity.Rare, Value = 8024.21m, MaxHealingRate = 65, LowestHealingRate = 50, Cooldown = TimeSpan.FromMinutes(69) },
                47 => new Weapon() { ID = 76, Name = "AK-47", Subname = "AK", Rarity = Rarity.Rare, Value = 16428.12m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B762mm, HighestDamage = 65, LowestDamage = 35, Cooldown = TimeSpan.FromMinutes(125.031) },
                48 => new Weapon() { ID = 89, Name = "Bayonet", Subname = "Bay", Rarity = Rarity.Rare, Value = 4825.62m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 30, LowestDamage = 25, Cooldown = TimeSpan.FromMinutes(60.5) },
                49 => new Money() { ID = 111, Name = "Money Bag", Subname = "Bag", Rarity = Rarity.Rare, Value = 6250m, HighestAmount = 10000m, LowestAmount = 2500m, IsSellable = false },
                50 => new Weapon() { ID = 89, Name = "Bayonet", Subname = "Bay", Rarity = Rarity.Rare, Value = 4825.62m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 30, LowestDamage = 25, Cooldown = TimeSpan.FromMinutes(60.5) },
                51 => new Weapon() { ID = 77, Name = "M16A4", Subname = "M16", Rarity = Rarity.Rare, Value = 8850m, WeaponType = WeaponType.Firearm, FireRate = 3, Ammo = Ammunition.B556mm, HighestDamage = 14, LowestDamage = 7, Cooldown = TimeSpan.FromMinutes(95.156) },
                52 => new Weapon() { ID = 20, Name = "Remington", Rarity = Rarity.Rare, Value = 12421.87m, WeaponType = WeaponType.Firearm, FireRate = 6, HighestDamage = 15, LowestDamage = 1, Ammo = Ammunition.B12Gauge, Cooldown = TimeSpan.FromMinutes(151.5) },
                53 => new Ammo() { ID = 81, Name = "7.62mm", Subname = "762", Rarity = Rarity.Rare, Value = 8921.87m, Type = Ammunition.B762mm },
                54 => new Weapon() { ID = 80, Name = "Combat Knife", Subname = "Combat", Rarity = Rarity.Rare, Value = 5510m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 35, LowestDamage = 30, Cooldown = TimeSpan.FromMinutes(73.875), IsPurchaseable = true },
                55 => new Ammo() { ID = 81, Name = "7.62mm", Subname = "762", Rarity = Rarity.Rare, Value = 8921.87m, Type = Ammunition.B762mm },
                56 => new Money() { ID = 111, Name = "Money Bag", Subname = "Bag", Rarity = Rarity.Rare, Value = 6250m, HighestAmount = 10000m, LowestAmount = 2500m, IsSellable = false },
                57 => new Weapon() { ID = 82, Name = "M1 Garand", Subname = "Garand", Rarity = Rarity.Rare, Value = 15771.87m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B3006, HighestDamage = 50, LowestDamage = 40, Cooldown = TimeSpan.FromMinutes(120.031) },
                58 => new Weapon() { ID = 89, Name = "Bayonet", Subname = "Bay", Rarity = Rarity.Rare, Value = 4825.62m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 30, LowestDamage = 25, Cooldown = TimeSpan.FromMinutes(60.5) },
                59 => new Weapon() { ID = 76, Name = "AK-47", Subname = "AK", Rarity = Rarity.Rare, Value = 16428.12m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B762mm, HighestDamage = 65, LowestDamage = 35, Cooldown = TimeSpan.FromMinutes(125.031) },
                60 => new Money() { ID = 111, Name = "Money Bag", Subname = "Bag", Rarity = Rarity.Rare, Value = 6250m, HighestAmount = 10000m, LowestAmount = 2500m, IsSellable = false },
                61 => new Healing() { ID = 85, Name = "Morphine", Subname = "Morph", Rarity = Rarity.Rare, Value = 8024.21m, MaxHealingRate = 65, LowestHealingRate = 50, Cooldown = TimeSpan.FromMinutes(69) },
                62 => new Ammo() { ID = 103, Name = "7.92mm", Subname = "792", Rarity = Rarity.Rare, Value = 7933.59m, Type = Ammunition.B792 },
                63 => new Weapon() { ID = 77, Name = "M16A4", Subname = "M16", Rarity = Rarity.Rare, Value = 8850m, WeaponType = WeaponType.Firearm, FireRate = 3, Ammo = Ammunition.B556mm, HighestDamage = 14, LowestDamage = 7, Cooldown = TimeSpan.FromMinutes(95.156) },
                64 => new Weapon() { ID = 89, Name = "Bayonet", Subname = "Bay", Rarity = Rarity.Rare, Value = 4825.62m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 30, LowestDamage = 25, Cooldown = TimeSpan.FromMinutes(60.5) },
                65 => new Weapon() { ID = 82, Name = "M1 Garand", Subname = "Garand", Rarity = Rarity.Rare, Value = 15771.87m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B3006, HighestDamage = 50, LowestDamage = 40, Cooldown = TimeSpan.FromMinutes(120.031) },
                66 => new Weapon() { ID = 80, Name = "Combat Knife", Subname = "Combat", Rarity = Rarity.Rare, Value = 5510m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 35, LowestDamage = 30, Cooldown = TimeSpan.FromMinutes(73.875), IsPurchaseable = true },
                67 => new Ammo() { ID = 81, Name = "7.62mm", Subname = "762", Rarity = Rarity.Rare, Value = 8921.87m, Type = Ammunition.B762mm },
                68 => new Weapon() { ID = 77, Name = "M16A4", Subname = "M16", Rarity = Rarity.Rare, Value = 8850m, WeaponType = WeaponType.Firearm, FireRate = 3, Ammo = Ammunition.B556mm, HighestDamage = 14, LowestDamage = 7, Cooldown = TimeSpan.FromMinutes(95.156) },
                69 => new Weapon() { ID = 82, Name = "M1 Garand", Subname = "Garand", Rarity = Rarity.Rare, Value = 15771.87m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B3006, HighestDamage = 50, LowestDamage = 40, Cooldown = TimeSpan.FromMinutes(120.031) },
                70 => new Attachment() { ID = 88, Name = "4x Scope", Subname = "4x", Rarity = Rarity.Rare, Value = 9471.99m, IsActive = false, Ability = Addon.HighestDamageIncrease, Intensity = 10 },
                71 => new Attachment() { ID = 86, Name = "Dual Mags", Subname = "Dual", Rarity = Rarity.Rare, Value = 6739.10m, IsActive = false, Ability = Addon.ReduceCooldown, Intensity = 25 },
                72 => new Weapon() { ID = 83, Name = "Kar-98", Subname = "Kar", Rarity = Rarity.Rare, Value = 18068.75m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B762mm, HighestDamage = 55, Cooldown = TimeSpan.FromMinutes(186.593) },
                73 => new Weapon() { ID = 89, Name = "Bayonet", Subname = "Bay", Rarity = Rarity.Rare, Value = 4825.62m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 30, LowestDamage = 25, Cooldown = TimeSpan.FromMinutes(60.5) },
                74 => new Weapon() { ID = 80, Name = "Combat Knife", Subname = "Combat", Rarity = Rarity.Rare, Value = 5510m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 35, LowestDamage = 30, Cooldown = TimeSpan.FromMinutes(73.875), IsPurchaseable = true },
                75 => new Weapon() { ID = 77, Name = "M16A4", Subname = "M16", Rarity = Rarity.Rare, Value = 8850m, WeaponType = WeaponType.Firearm, FireRate = 3, Ammo = Ammunition.B556mm, HighestDamage = 14, LowestDamage = 7, Cooldown = TimeSpan.FromMinutes(95.156) },
                76 => new Weapon() { ID = 76, Name = "AK-47", Subname = "AK", Rarity = Rarity.Rare, Value = 16428.12m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B762mm, HighestDamage = 65, LowestDamage = 35, Cooldown = TimeSpan.FromMinutes(125.031) },
                77 => new Money() { ID = 111, Name = "Money Bag", Subname = "Bag", Rarity = Rarity.Rare, Value = 6250m, HighestAmount = 10000m, LowestAmount = 2500m, IsSellable = false },
                78 => new Weapon() { ID = 20, Name = "Remington", Rarity = Rarity.Rare, Value = 12421.87m, WeaponType = WeaponType.Firearm, FireRate = 6, HighestDamage = 15, LowestDamage = 1, Ammo = Ammunition.B12Gauge, Cooldown = TimeSpan.FromMinutes(151.5) },
                79 => new Weapon() { ID = 80, Name = "Combat Knife", Subname = "Combat", Rarity = Rarity.Rare, Value = 5510m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 35, LowestDamage = 30, Cooldown = TimeSpan.FromMinutes(73.875), IsPurchaseable = true },
                80 => new Weapon() { ID = 89, Name = "Bayonet", Subname = "Bay", Rarity = Rarity.Rare, Value = 4825.62m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 30, LowestDamage = 25, Cooldown = TimeSpan.FromMinutes(60.5) },
                81 => new Attachment() { ID = 86, Name = "Dual Mags", Subname = "Dual", Rarity = Rarity.Rare, Value = 6739.10m, IsActive = false, Ability = Addon.ReduceCooldown, Intensity = 25 },
                82 => new Healing() { ID = 85, Name = "Morphine", Subname = "Morph", Rarity = Rarity.Rare, Value = 8024.21m, MaxHealingRate = 65, LowestHealingRate = 50, Cooldown = TimeSpan.FromMinutes(69) },
                83 => new Healing() { ID = 85, Name = "Morphine", Subname = "Morph", Rarity = Rarity.Rare, Value = 8024.21m, MaxHealingRate = 65, LowestHealingRate = 50, Cooldown = TimeSpan.FromMinutes(69) },
                84 => new Weapon() { ID = 80, Name = "Combat Knife", Subname = "Combat", Rarity = Rarity.Rare, Value = 5510m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 35, LowestDamage = 30, Cooldown = TimeSpan.FromMinutes(73.875), IsPurchaseable = true },
                85 => new Weapon() { ID = 89, Name = "Bayonet", Subname = "Bay", Rarity = Rarity.Rare, Value = 4825.62m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 30, LowestDamage = 25, Cooldown = TimeSpan.FromMinutes(60.5) },
                86 => new Weapon() { ID = 91, Name = "M249", Subname = "249", Rarity = Rarity.Epic, Value = 38800.78m, WeaponType = WeaponType.Firearm, FireRate = 5, Ammo = Ammunition.B556mm, HighestDamage = 20, LowestDamage = 15, Cooldown = TimeSpan.FromMinutes(298.75) },
                87 => new Armor() { ID = 79, Name = "Kevlar Set", Subname = "Kevlar", Rarity = Rarity.Rare, Value = 27562.5m, IsActive = false, ProtectionRate = 75 },
                88 => new Healing() { ID = 85, Name = "Morphine", Subname = "Morph", Rarity = Rarity.Rare, Value = 8024.21m, MaxHealingRate = 65, LowestHealingRate = 50, Cooldown = TimeSpan.FromMinutes(69) },
                89 => new Weapon() { ID = 80, Name = "Combat Knife", Subname = "Combat", Rarity = Rarity.Rare, Value = 5510m, WeaponType = WeaponType.Melee, FireRate = 1, Ammo = Ammunition.None, HighestDamage = 35, LowestDamage = 30, Cooldown = TimeSpan.FromMinutes(73.875), IsPurchaseable = true },
                90 => new Weapon() { ID = 87, Name = "Howa 89", Subname = "Howa", Rarity = Rarity.Rare, Value = 18134.37m, WeaponType = WeaponType.Firearm, FireRate = 4, Ammo = Ammunition.B556mm, HighestDamage = 16, LowestDamage = 8, Cooldown = TimeSpan.FromMinutes(94.25) },
                91 => new Money() { ID = 111, Name = "Money Bag", Subname = "Bag", Rarity = Rarity.Rare, Value = 6250m, HighestAmount = 10000m, LowestAmount = 2500m, IsSellable = false },
                92 => new Ammo() { ID = 104, Name = "B56mm", Subname = "56", Rarity = Rarity.Rare, Value = 9640.62m, Type = Ammunition.B56mm },
                93 => new Attachment() { ID = 86, Name = "Dual Mags", Subname = "Dual", Rarity = Rarity.Rare, Value = 6739.10m, IsActive = false, Ability = Addon.ReduceCooldown, Intensity = 25 },
                94 => new Ammo() { ID = 81, Name = "7.62mm", Subname = "762", Rarity = Rarity.Rare, Value = 8921.87m, Type = Ammunition.B762mm },
                95 => new Weapon() { ID = 20, Name = "Remington", Rarity = Rarity.Rare, Value = 12421.87m, WeaponType = WeaponType.Firearm, FireRate = 6, HighestDamage = 15, LowestDamage = 1, Ammo = Ammunition.B12Gauge, Cooldown = TimeSpan.FromMinutes(151.5) },
                96 => new Weapon() { ID = 87, Name = "Howa 89", Subname = "Howa", Rarity = Rarity.Rare, Value = 18134.37m, WeaponType = WeaponType.Firearm, FireRate = 4, Ammo = Ammunition.B556mm, HighestDamage = 16, LowestDamage = 8, Cooldown = TimeSpan.FromMinutes(94.25) },
                97 => new Ammo() { ID = 81, Name = "7.62mm", Subname = "762", Rarity = Rarity.Rare, Value = 8921.87m, Type = Ammunition.B762mm },
                98 => new Money() { ID = 112, Name = "Vault", Subname = "Vlt", Rarity = Rarity.Epic, Value = 37500m, HighestAmount = 50000m, LowestAmount = 25000m, IsSellable = false },
                99 => new Weapon() { ID = 92, Name = "TAC-50", Subname = "Tac", Rarity = Rarity.Epic, Value = 44830.46m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B50BMG, HighestDamage = 90, Cooldown = TimeSpan.FromMinutes(240.875) },
                _ => new Armor() { ID = 93, Name = "Ballistic Shield", Subname = "Ballistic", Rarity = Rarity.Epic, Value = 83312.5m, IsActive = false, ProtectionRate = 100 },
            };
        }
        private static Item GetRandomEpicItem() //Contains: epic
        {
            var rNum = new Random().Next(1, 17);
            return rNum switch
            {
                1 => new Weapon() { ID = 91, Name = "M249", Subname = "249", Rarity = Rarity.Epic, Value = 38800.78m, WeaponType = WeaponType.Firearm, FireRate = 5, Ammo = Ammunition.B556mm, HighestDamage = 20, LowestDamage = 15, Cooldown = TimeSpan.FromMinutes(298.75) },
                2 => new Weapon() { ID = 92, Name = "TAC-50", Subname = "Tac", Rarity = Rarity.Epic, Value = 44830.46m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B50BMG, HighestDamage = 90, Cooldown = TimeSpan.FromMinutes(240.875) },
                3 => new Ammo() { ID = 94, Name = ".50 BMG", Subname = "BMG", Rarity = Rarity.Epic, Value = 12582.19m, Type = Ammunition.B50BMG },
                4 => new Weapon() { ID = 91, Name = "M249", Subname = "249", Rarity = Rarity.Epic, Value = 38800.78m, WeaponType = WeaponType.Firearm, FireRate = 5, Ammo = Ammunition.B556mm, HighestDamage = 20, LowestDamage = 15, Cooldown = TimeSpan.FromMinutes(298.75) },
                5 => new Money() { ID = 112, Name = "Vault", Subname = "Vlt", Rarity = Rarity.Epic, Value = 37500m, HighestAmount = 50000m, LowestAmount = 25000m, IsSellable = false },
                6 => new Weapon() { ID = 92, Name = "TAC-50", Subname = "Tac", Rarity = Rarity.Epic, Value = 44830.46m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B50BMG, HighestDamage = 90, Cooldown = TimeSpan.FromMinutes(240.875) },
                7 => new Weapon() { ID = 95, Name = "SG 550", Subname = "SG", Rarity = Rarity.Epic, Value = 24389.06m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B56mm, HighestDamage = 70, LowestDamage = 55, Cooldown = TimeSpan.FromMinutes(171.025) },
                8 => new Money() { ID = 112, Name = "Vault", Subname = "Vlt", Rarity = Rarity.Epic, Value = 37500m, HighestAmount = 50000m, LowestAmount = 25000m, IsSellable = false },
                9 => new Money() { ID = 112, Name = "Vault", Subname = "Vlt", Rarity = Rarity.Epic, Value = 37500m, HighestAmount = 50000m, LowestAmount = 25000m, IsSellable = false },
                10 => new Weapon() { ID = 95, Name = "SG 550", Subname = "SG", Rarity = Rarity.Epic, Value = 24389.06m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B56mm, HighestDamage = 70, LowestDamage = 55, Cooldown = TimeSpan.FromMinutes(171.025) },
                11 => new Money() { ID = 112, Name = "Vault", Subname = "Vlt", Rarity = Rarity.Epic, Value = 37500m, HighestAmount = 50000m, LowestAmount = 25000m, IsSellable = false },
                12 => new Weapon() { ID = 95, Name = "SG 550", Subname = "SG", Rarity = Rarity.Epic, Value = 24389.06m, WeaponType = WeaponType.Firearm, FireRate = 1, Ammo = Ammunition.B56mm, HighestDamage = 70, LowestDamage = 55, Cooldown = TimeSpan.FromMinutes(171.025) },
                13 => new Money() { ID = 112, Name = "Vault", Subname = "Vlt", Rarity = Rarity.Epic, Value = 37500m, HighestAmount = 50000m, LowestAmount = 25000m, IsSellable = false },
                14 => new Money() { ID = 112, Name = "Vault", Subname = "Vlt", Rarity = Rarity.Epic, Value = 37500m, HighestAmount = 50000m, LowestAmount = 25000m, IsSellable = false },
                15 => new Armor() { ID = 93, Name = "Ballistic Shield", Subname = "Ballistic", Rarity = Rarity.Epic, Value = 83312.5m, IsActive = false, ProtectionRate = 100 },
                _ => new Healing() { ID = 78, Name = "Blood Bag", Subname = "Blood", Rarity = Rarity.Epic, Value = 14784m, MaxHealingRate = 90, LowestHealingRate = 70, Cooldown = TimeSpan.FromMinutes(92) },
            };
        }

        private static Item GetRandomUniqueItem() //Contains: unique
        {
            var rNum = new Random().Next(1, 21);
            return rNum switch
            {
                1 => new Weapon() { ID = 96, Name = "Boys AT Rifle", Subname = "ATRifle", Rarity = Rarity.Unique, Value = 75634.98m, WeaponType = WeaponType.HeavyFirearm, FireRate = 1, Ammo = Ammunition.BRG55, HighestDamage = 25, Cooldown = TimeSpan.FromMinutes(83.468), Special = Special.ArmorBust },
                2 => new Attachment() { ID = 97, Name = "Extended Clip", Subname = "Extended", Rarity = Rarity.Unique, Value = 50347.87m, Ability = Addon.IncreaseHitRate, Intensity = 1, IsActive = false },
                3 => new Ammo() { ID = 98, Name = "RG.55", Subname = "rg", Rarity = Rarity.Unique, Type = Ammunition.BRG55, Value = 32812.5m },
                4 => new Utility() { ID = 102, Name = "Radar", Rarity = Rarity.Unique, Value = 69347.12m, Uses = 3, Special = Special.Scan },
                5 => new Attachment() { ID = 97, Name = "Extended Clip", Subname = "Extended", Rarity = Rarity.Unique, Value = 50347.87m, Ability = Addon.IncreaseHitRate, Intensity = 1, IsActive = false },
                6 => new Utility() { ID = 101, Name = "Flare", Rarity = Rarity.Unique, Value = 35743.88m, Uses = 1, Special = Special.SupplyCall },
                8 => new Utility() { ID = 102, Name = "Radar", Rarity = Rarity.Unique, Value = 69347.12m, Uses = 3, Special = Special.Scan },
                9 => new Weapon() { ID = 96, Name = "Boys AT Rifle", Subname = "ATRifle", Rarity = Rarity.Unique, Value = 75634.98m, WeaponType = WeaponType.HeavyFirearm, FireRate = 1, Ammo = Ammunition.BRG55, HighestDamage = 25, Cooldown = TimeSpan.FromMinutes(83.468), Special = Special.ArmorBust },
                10 => new Utility() { ID = 101, Name = "Flare", Rarity = Rarity.Unique, Value = 35743.88m, Uses = 1, Special = Special.SupplyCall },
                11 => new Utility() { ID = 101, Name = "Flare", Rarity = Rarity.Unique, Value = 35743.88m, Uses = 1, Special = Special.SupplyCall },
                12 => new Ammo() { ID = 98, Name = "RG.55", Subname = "rg", Rarity = Rarity.Unique, Type = Ammunition.BRG55, Value = 32812.5m },
                13 => new Utility() { ID = 101, Name = "Flare", Rarity = Rarity.Unique, Value = 35743.88m, Uses = 1, Special = Special.SupplyCall },
                14 => new Weapon() { ID = 96, Name = "Boys AT Rifle", Subname = "ATRifle", Rarity = Rarity.Unique, Value = 75634.98m, WeaponType = WeaponType.HeavyFirearm, FireRate = 1, Ammo = Ammunition.BRG55, HighestDamage = 25, Cooldown = TimeSpan.FromMinutes(83.468), Special = Special.ArmorBust },
                15 => new Utility() { ID = 101, Name = "Flare", Rarity = Rarity.Unique, Value = 35743.88m, Uses = 1, Special = Special.SupplyCall },
                16 => new Utility() { ID = 101, Name = "Flare", Rarity = Rarity.Unique, Value = 35743.88m, Uses = 1, Special = Special.SupplyCall },
                17 => new Utility() { ID = 101, Name = "Flare", Rarity = Rarity.Unique, Value = 35743.88m, Uses = 1, Special = Special.SupplyCall },
                18 => new Ammo() { ID = 100, Name = "GattlingDrum", Subname = "Drum", Rarity = Rarity.Unique, Value = 65625m, Type = Ammunition.GattlingDrum },
                19 => new Weapon() { ID = 99, Name = "Gattling Gun", Subname = "Gattling", Rarity = Rarity.Unique, Value = 133185.93m, WeaponType = WeaponType.Firearm, FireRate = 3, Ammo = Ammunition.GattlingDrum, HighestDamage = 40, LowestDamage = 20, Cooldown = TimeSpan.FromMinutes(887.062), Special = Special.SpreadShot },
                _ => new Utility() { ID = 102, Name = "Radar", Rarity = Rarity.Unique, Value = 69347.12m, Uses = 3, Special = Special.Scan },
            };

        }
        public static Item OpenCrate(Rarity rarity)
        {
            Item obtained = null;
            if (rarity == Rarity.Common)
            {
                obtained = GetRandomCommonItem();
            }
            else if (rarity == Rarity.Uncommon)
            {
                obtained = GetRandomUncommonItem();
            }
            else if (rarity == Rarity.Rare)
            {
                obtained = GetRandomRareItem();
            }
            else if (rarity == Rarity.Epic)
            {
                obtained = GetRandomEpicItem();
            }
            else if (rarity == Rarity.Unique)
            {
                obtained = GetRandomUniqueItem();
            }
            return obtained;
        }

        public static Crate GetRandomCrate()
        {
            var crate = new Crate();
            var rNum = new Random().Next(1, 100);
            crate.Rarity = rNum switch
            {
                var x when x >= 1 && x <= 3 => Rarity.Epic,
                var x when x >= 4 && x <= 10  => Rarity.Rare,
                var x when x >= 11 && x <= 25 => Rarity.Uncommon,
                var x when x >= 26 && x <= 74 => Rarity.Common,
                var x when x >= 75 && x <= 89 => Rarity.Uncommon,
                var x when x >= 90 && x <= 96 => Rarity.Rare,
                var x when x >= 97 && x <= 100 => Rarity.Epic,
            };
            if (crate.Rarity == Rarity.Common)
            {
                crate.ID = 106;
                crate.Name = "Common Crate";
                crate.Subname = "Common";
                crate.Value = 3368.53m;
                crate.IsPurchaseable = true;
                crate.IsSellable = false;
            }
            else if(crate.Rarity == Rarity.Uncommon)
            {
                crate.ID = 107;
                crate.Name = "Uncommon Crate";
                crate.Subname = "Uncommon";
                crate.Value = 6435.59m;
                crate.IsPurchaseable = true;
                crate.IsSellable = false;
            }
            else if (crate.Rarity == Rarity.Rare)
            {
                crate.ID = 108;
                crate.Name = "Rare Crate";
                crate.Subname = "Rare";
                crate.Value = 11937.58m;
                crate.IsPurchaseable = true;
                crate.IsSellable = false;
            }
            else if (crate.Rarity == Rarity.Epic)
            {
                crate.ID = 109;
                crate.Name = "Epic Crate";
                crate.Subname = "Epic";
                crate.Value = 35812.73m;
                crate.IsSellable = false;
            }
            return crate;
        }
        public static Crate GetMonthly()
        {
            return new Crate() { Rarity = Rarity.Unique, ID = 110, Name = "Unique Crate", Subname = "Unique", Value = 107438.20m, IsSellable = false };
        }
        public static Crate GetDaily()
        {
            var crate = new Crate();
            var rNum = new Random().Next(1, 100);
            crate.Rarity = rNum switch
            {
                var x when x >= 1 && x <= 10 => Rarity.Rare,
                var x when x >= 11 && x <= 89 => Rarity.Uncommon,
                var x when x >= 90 && x == 100 => Rarity.Rare,
                _ => Rarity.Common
            };
            if (crate.Rarity == Rarity.Rare)
            {
                crate.ID = 108;
                crate.Name = "Rare Crate";
                crate.Subname = "Rare";
                crate.Value = 11937.58m;
                crate.IsPurchaseable = true;
                crate.IsSellable = false;
            }
            else if (crate.Rarity == Rarity.Uncommon)
            {
                crate.ID = 107;
                crate.Name = "Uncommon Crate";
                crate.Subname = "Uncommon";
                crate.Value = 6435.59m;
                crate.IsPurchaseable = true;
                crate.IsSellable = false;
            }
            return crate;
        }
        public static Crate GetWeekly()
        {
            var crate = new Crate();
            var rNum = new Random().Next(1, 24);
            crate.Rarity = rNum switch
            {
                var x when x >= 1 && x <= 21 => Rarity.Rare,
                var x when x == 22 || x == 23 => Rarity.Epic,
                _ => Rarity.Common
            };
            if (crate.Rarity == Rarity.Rare)
            {
                crate.ID = 108;
                crate.Name = "Rare Crate";
                crate.Subname = "Rare";
                crate.Value = 11937.58m;
                crate.IsPurchaseable = true;
                crate.IsSellable = false;
            }
            else if (crate.Rarity == Rarity.Epic)
            {
                crate.ID = 109;
                crate.Name = "Epic Crate";
                crate.Subname = "Epic";
                crate.Value = 35812.73m;
                crate.IsSellable = false;
            }
            return crate;
        }
        public static Crate GetScrambleReward(Words word)
        {
            var crate = new Crate();
            if (word.Difficulty == Difficulty.Easy)
            {
                crate.Rarity = Rarity.Common;
                crate.ID = 106;
                crate.Name = "Common Crate";
                crate.Subname = "Common";
                crate.Value = 3368.53m;
                crate.IsPurchaseable = true;
                crate.IsSellable = false;
            }
            else if (word.Difficulty == Difficulty.Moderate)
            {
                crate.Rarity = Rarity.Common;
                crate.ID = 106;
                crate.Name = "Common Crate";
                crate.Subname = "Common";
                crate.Value = 3368.53m;
                crate.IsPurchaseable = true;
                crate.IsSellable = false;
            }
            else if (word.Difficulty == Difficulty.Hard)
            {
                crate.Rarity = Rarity.Uncommon;
                crate.ID = 107;
                crate.Name = "Uncommon Crate";
                crate.Subname = "Uncommon";
                crate.Value = 6435.59m;
                crate.IsPurchaseable = true;
                crate.IsSellable = false;
            }
            else if (word.Difficulty == Difficulty.Extreme)
            {
                crate.Rarity = Rarity.Rare;
                crate.ID = 108;
                crate.Name = "Rare Crate";
                crate.Subname = "Rare";
                crate.Value = 11937.58m;
                crate.IsPurchaseable = true;
                crate.IsSellable = false;
            }
            else if (word.Difficulty == Difficulty.Insane)
            {
                crate.Rarity = Rarity.Unique;
                crate.ID = 110;
                crate.Name = "Unique Crate";
                crate.Subname = "Unique";
                crate.Value = 107438.20m;
                crate.IsPurchaseable = false;
                crate.IsSellable = false;
            }
            else if (word.Difficulty == Difficulty.Impossible)
            {
                crate.Rarity = Rarity.Unique;
                crate.ID = 110;
                crate.Name = "Unique Crate";
                crate.Subname = "Unique";
                crate.Value = 107438.20m;
                crate.IsPurchaseable = false;
                crate.IsSellable = false;
            }
            return crate;
        }
        public static Crate GetDotPuzzleReward(Puzzles puzzle)
        {
            var crate = new Crate();
            if (puzzle.DotDifficulty == DotDifficulty.Easy)
            {
                crate.Rarity = Rarity.Common;
                crate.ID = 106;
                crate.Name = "Common Crate";
                crate.Subname = "Common";
                crate.Value = 3368.53m;
                crate.IsPurchaseable = true;
                crate.IsSellable = false;
            }
            else if (puzzle.DotDifficulty == DotDifficulty.Medium)
            {
                crate.Rarity = Rarity.Uncommon;
                crate.ID = 107;
                crate.Name = "Uncommon Crate";
                crate.Subname = "Uncommon";
                crate.Value = 6435.59m;
                crate.IsPurchaseable = true;
                crate.IsSellable = false;
            }
            else if (puzzle.DotDifficulty == DotDifficulty.Hard)
            {
                crate.Rarity = Rarity.Uncommon;
                crate.ID = 107;
                crate.Name = "Uncommon Crate";
                crate.Subname = "Uncommon";
                crate.Value = 6435.59m;
                crate.IsPurchaseable = true;
                crate.IsSellable = false;
            }
            else if (puzzle.DotDifficulty == DotDifficulty.Extreme)
            {
                crate.Rarity = Rarity.Rare;
                crate.ID = 108;
                crate.Name = "Rare Crate";
                crate.Subname = "Rare";
                crate.Value = 11937.58m;
                crate.IsPurchaseable = true;
                crate.IsSellable = false;
            }
            return crate;
        }
    }
}