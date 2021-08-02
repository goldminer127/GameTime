using System;
using GameTime.Models;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using GameTime.Extensions;
using GameTime.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameTime.Connect4Models;
using DSharpPlus.Interactivity;

namespace GameTime.Connect4Models
{
    public class Connect4Session
    {
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public Player SessionWinner { get; set; }
        public DiscordMessage P1Display { get; set; }
        public DiscordMessage P2Display { get; set; }
        public DiscordChannel P1Channel { get; set; }
        public DiscordChannel P2Channel { get; set; }
        public Connect4Board SessionBoard { get; set; } = new Connect4Board();
        public string SessionKey { get; set; }
        public byte PlayerTurn { get; set; } = 1;
        public bool SessionEnd { get; set; } = false;
        public Connect4Session()
        {
            for (int keyLength = 4; keyLength != 0; keyLength--)
            {
                SessionKey += new Random().Next(0, 10);
            }
        }
    }
}
