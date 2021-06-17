using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus.Entities;

namespace GameTime.Models
{
    public class Metals
    {
        public List<DiscordEmoji> MetalsList { get; set; }
        public Metals()
        {
            MetalsList = new List<DiscordEmoji>
            {
                //DiscordEmoji.FromUnicode(Bot.Client, "<:veteran:768672136583184434>"),
                //DiscordEmoji.FromUnicode(Bot.Client, "<:swatter:768266681809109013>"),
            };
        }
        public DiscordEmoji GetMetal(string name)
        {
            foreach(var emoji in MetalsList)
            {
                if(emoji.Name.Contains(name))
                {
                    return emoji;
                }
            }
            return null;
        }
    }
}
