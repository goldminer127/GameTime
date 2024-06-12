using System.Collections.Generic;
using GameTime.Models.Items;

namespace GameTime.Models
{
    public class Player
    {
        public string Name { get; set; }
        public ulong DiscordId { get; set; }
        public string Avatar { get; set; }
        public List<Item> Inventory { get; set; }
    }
}
