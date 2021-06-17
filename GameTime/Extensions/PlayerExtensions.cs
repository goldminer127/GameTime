using GameTime.Models;
using System;

namespace GameTime.Extensions
{
    public static class PlayerExtensions
    {
        public static string GetHealthCooldown(this Player user)
        {
            user.TimeNow = DateTime.Now;
            if (user.HealCooldownEnd >= DateTime.Now)
            {
                var count = user.HealCooldownEnd.Subtract(user.TimeNow);
                var seconds = (uint)count.TotalSeconds;
                var minutes = seconds / 60;
                var hours = minutes / 60;
                return $"{(hours % 24):00}:{(minutes % 60):00}:{(seconds % 60):00}";
            }
            else
            {
                return "Ready";
            }
        }
        public static string GetScremblerCooldown(this Player user)
        {
            user.TimeNow = DateTime.Now;
            if (user.ScramblerEnd >= DateTime.Now)
            {
                var count = user.ScramblerEnd.Subtract(user.TimeNow);
                var seconds = (uint)count.TotalSeconds;
                var minutes = seconds / 60;
                var hours = minutes / 60;
                return $"{(hours % 24):00}:{(minutes % 60):00}:{(seconds % 60):00}";
            }
            else
            {
                return "Ready";
            }
        }
        public static string GetMinesweeperCooldown(this Player user)
        {
            user.TimeNow = DateTime.Now;
            if (user.MinesweeperEnd >= DateTime.Now)
            {
                var count = user.MinesweeperEnd.Subtract(user.TimeNow);
                var seconds = (uint)count.TotalSeconds;
                var minutes = seconds / 60;
                var hours = minutes / 60;
                return $"{(hours % 24):00}:{(minutes % 60):00}:{(seconds % 60):00}";
            }
            else
            {
                return "Ready";
            }
        }
        public static string GetDotPuzzleCooldown(this Player user)
        {
            user.TimeNow = DateTime.Now;
            if (user.DotPuzzleEnd >= DateTime.Now && user.GameCooldownIgnore != true)
            {
                var count = user.DotPuzzleEnd.Subtract(user.TimeNow);
                var seconds = (uint)count.TotalSeconds;
                var minutes = seconds / 60;
                var hours = minutes / 60;
                return $"{(hours % 24):00}:{(minutes % 60):00}:{(seconds % 60):00}";
            }
            else if (user.GameCooldownIgnore != true)
            {
                return "Ready";
            }
            else
            {
                return "Ignored";
            }
        }

        public static string GetWeaponCooldown(this Player user)
        {
            user.TimeNow = DateTime.Now;
            if (user.CooldownEndTime >= DateTime.Now)
            {
                var count = user.CooldownEndTime.Subtract(user.TimeNow);
                var seconds = (uint)count.TotalSeconds;
                var minutes = seconds / 60;
                var hours = minutes / 60;
                return $"{(hours % 24):00}:{(minutes % 60):00}:{(seconds % 60):00}";
            }
            else
            {
                return "Ready";
            }
        }

        public static string GetOptCooldown(this Player user)
        {
            user.TimeNow = DateTime.Now;
            if (user.OptCooldownEnd >= DateTime.Now)
            {
                var count = user.OptCooldownEnd.Subtract(user.TimeNow);
                var seconds = (uint)count.TotalSeconds;
                var minutes = seconds / 60;
                var hours = minutes / 60;
                return $"{(hours % 24):00}:{(minutes % 60):00}:{(seconds % 60):00}";
            }
            else
            {
                return "Ready";
            }
        }

        public static string GetHourCooldown(this Player user)
        {
            user.TimeNow = DateTime.Now;
            if (user.CooldownHourEndTime >= DateTime.Now)
            {
                var count = user.CooldownHourEndTime.Subtract(user.TimeNow);
                var seconds = (uint)count.TotalSeconds;
                var minutes = seconds / 60;
                var hours = minutes / 60;
                return $"{(hours % 24):00}:{(minutes % 60):00}:{(seconds % 60):00}";
            }
            else
            {
                return "Ready";
            }
        }
        public static string GetDailyCooldown(this Player user)
        {
            user.TimeNow = DateTime.Now;
            if (user.DailyCooldownEnd >= DateTime.Now)
            {
                var count = user.DailyCooldownEnd.Subtract(user.TimeNow);
                var seconds = (uint)count.TotalSeconds;
                var minutes = seconds / 60;
                var hours = minutes / 60;
                return $"{(hours % 24):00}:{(minutes % 60):00}:{(seconds % 60):00}";
            }
            else
            {
                return "Ready";
            }
        }

        public static string GetMonthCooldown(this Player user)
        {
            user.TimeNow = DateTime.Now;
            var count = user.MonthlyCooldownEnd.Subtract(user.TimeNow);
            if (user.MonthlyCooldownEnd >= DateTime.Now)
            {
                var timer = "";
                if (count.Days <= 0 && count.Seconds > 0)
                {
                    if (count.Hours < 10 && count.Hours > 0)
                    {
                        timer += $"0{count.Hours}:";
                    }
                    else if (count.Hours >= 10)
                    {
                        timer += $"{count.Hours}:";
                    }
                    else if (count.Hours <= 0)
                    {
                        timer += "00:";
                    }
                    if (count.Minutes < 10 && count.Minutes > 0)
                    {
                        timer += $"0{count.Minutes}:";
                    }
                    else if (count.Minutes >= 10)
                    {
                        timer += $"{count.Minutes}:";
                    }
                    else if (count.Minutes <= 0)
                    {
                        timer += "00:";
                    }
                    if (count.Seconds < 10 && count.Seconds > 0)
                    {
                        timer += $"0{count.Seconds}";
                    }
                    else if (count.Seconds >= 10)
                    {
                        timer += $"{count.Seconds}";
                    }
                    else if (count.Seconds <= 0)
                    {
                        timer += "00";
                    }
                    if (timer == "00:00:00")
                    {
                        timer = "Ready";
                    }
                }
                else
                {
                    timer += $"{count.Days} days left";
                }
                return timer;
            }
            else
            {
                return "Ready";
            }
        }
        public static string GetWeeklyCooldown(this Player user)
        {
            user.TimeNow = DateTime.Now;
            var count = user.WeeklyCooldownEnd.Subtract(user.TimeNow);
            if (user.WeeklyCooldownEnd >= DateTime.Now)
            {
                var timer = "";
                if (count.Days <= 0 && count.Seconds > 0)
                {
                    if (count.Hours < 10 && count.Hours > 0)
                    {
                        timer += $"0{count.Hours}:";
                    }
                    else if (count.Hours >= 10)
                    {
                        timer += $"{count.Hours}:";
                    }
                    else if (count.Hours <= 0)
                    {
                        timer += "00:";
                    }
                    if (count.Minutes < 10 && count.Minutes > 0)
                    {
                        timer += $"0{count.Minutes}:";
                    }
                    else if (count.Minutes >= 10)
                    {
                        timer += $"{count.Minutes}:";
                    }
                    else if (count.Minutes <= 0)
                    {
                        timer += "00:";
                    }
                    if (count.Seconds < 10 && count.Seconds > 0)
                    {
                        timer += $"0{count.Seconds}";
                    }
                    else if (count.Seconds >= 10)
                    {
                        timer += $"{count.Seconds}";
                    }
                    else if (count.Seconds <= 0)
                    {
                        timer += "00";
                    }
                    if (timer == "00:00:00")
                    {
                        timer = "Ready";
                    }
                }
                else
                {
                    timer += $"{count.Days} days left";
                }
                return timer;
            }
            else
            {
                return "Ready";
            }
        }
    }
}