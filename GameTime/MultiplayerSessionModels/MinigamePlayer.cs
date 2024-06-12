using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTime.MultiplayerSessionModels
{
    public class MinigamePlayer
    {
        public string Name { get; private set; }
        public ulong Id { get; private set; }
        public ulong Channel { get; private set; }
        public DiscordEmbed Display { get; set; }
        public DiscordMessage PreviousDisplay { get; set; }
        public MinigamePlayer(string name, ulong id, ulong channel)
        {
            Name = name;
            Channel = channel;
            Id = id;
        }
    }
}
