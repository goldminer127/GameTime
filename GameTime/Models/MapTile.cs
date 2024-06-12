using GameTime.Models.Enums;
using GameTime.MultiplayerSessionModels;

namespace GameTime.Models
{
    public class MapTile
    {
        public MapTileType Biome { get; private set; }
        public Player PlayerOccupent { get; set; }
        public bool IsOccupied { get; set; }
        public MapTile(MapTileType biome)
        {
            Biome = biome;
        }
    }
}
