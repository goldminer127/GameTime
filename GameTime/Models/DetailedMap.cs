using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using GameTime.Models.Enums;

namespace GameTime.Models
{
    public class DetailedMap
    {
        public MapTile[,] Map { get; private set; } = new MapTile[500, 100];
        private int[] _size = new int[] { 500, 100}; //50,105 height, width
        private Random _random = new Random();
        public DetailedMap()
        {
            GenerateMap();
        }
        private void GenerateMap()
        {
            GenerateBiome(10, 20, MapTileType.Forest);
            GenerateBiome(8, 14, MapTileType.Rocky);
            GenerateBiome(6, 10, MapTileType.Sandy);
            GenerateHills(20);
            FillEmpty();
            GenerateRivers(6);
            GenerateRoads(1);
        }
        public void RegenerateMap()
        {
            GenerateMap();
        }
        private void GenerateBiome(int amount, int size, MapTileType type)
        {
            for (int i = amount; i > 0; --i)
                ExpandTile(_random.Next(_size[0]), _random.Next(_size[1]), size, type);
        }
        private void GenerateHills(int amount)
        {
            for (int i = amount; i > 0; --i)
                ExpandTile(_random.Next(_size[0]), _random.Next(_size[1]), _random.Next(3,10), MapTileType.Hills);
        }
        //Idk what I am doing so I am doing it
        private void ExpandTile(int currentRow, int currentCol, int expandWeight, MapTileType type)
        {
            var expand = _random.Next(0, expandWeight);
            var overrideTile = _random.Next(0, 2);
            if (Map[currentRow, currentCol] != null && type == MapTileType.Hills && Map[currentRow, currentCol].Biome != MapTileType.Hills)
                Map[currentRow, currentCol] = new MapTile(Map[currentRow, currentCol].Biome switch
                {
                    MapTileType.Forest => MapTileType.ForestHills,
                    MapTileType.Rocky => MapTileType.RockyHills,
                    MapTileType.Sandy => MapTileType.SandyHills,
                    _ => Map[currentRow, currentCol].Biome
                });
            else if (Map[currentRow, currentCol] == null || overrideTile == 1)
                Map[currentRow, currentCol] = new MapTile(type);
            if (expand != 0 && currentRow - 1 >= 0 && (Map[currentRow - 1, currentCol] == null || Map[currentRow - 1, currentCol].Biome != type)) //Expand up
                ExpandTile(currentRow - 1, currentCol, expandWeight - 1, type);
            expand = _random.Next(0, expandWeight);
            if (expand != 0 && currentRow + 1 < _size[0] && (Map[currentRow + 1, currentCol] == null || Map[currentRow + 1, currentCol].Biome != type)) //Expand down
                ExpandTile(currentRow + 1, currentCol, expandWeight - 1, type);
            expand = _random.Next(0, expandWeight);
            if (expand != 0 && currentCol - 1 >= 0 && (Map[currentRow, currentCol - 1] == null || Map[currentRow, currentCol - 1].Biome != type))
                ExpandTile(currentRow, currentCol - 1, expandWeight - 1, type);
            expand = _random.Next(0, expandWeight);
            if (expand != 0 && currentCol + 1 < _size[1] && (Map[currentRow, currentCol + 1] == null || Map[currentRow, currentCol + 1].Biome != type))
                ExpandTile(currentRow, currentCol + 1, expandWeight - 1, type);
        }
        private void FillEmpty()
        {
            for(int row = 0; row < _size[0]; ++row)
            {
                for(int col = 0; col < _size[1]; ++col)
                {
                    if (Map[row, col] == null)
                        Map[row, col] = new MapTile(MapTileType.Plains);
                }
            }
        }
        private void GenerateRivers(int amount)
        {
            for (int riverAmount = amount; riverAmount > 0; --riverAmount)
            {
                var currentRow = _random.Next(_size[0]);
                var currentCol = _random.Next(_size[1]);
                var directionalWeight = _random.Next(0, 4); //0 up, 1 left, 2 down, 3 right. Means river will lean towards selected direction
                Map[currentRow, currentCol] = new MapTile(MapTileType.River);
                while (currentRow >= 0 && currentRow < _size[0] && currentCol >= 0 && currentCol < _size[1])
                {
                    if (Map[currentRow, currentCol] == null || Map[currentRow, currentCol].Biome != MapTileType.River)
                        Map[currentRow, currentCol] = new MapTile(MapTileType.River);
                    var dir = _random.Next(1, 6);
                    if(dir == 1 || (directionalWeight == 0 && dir == 5)) //up
                        currentRow--;
                    else if (dir == 2 || (directionalWeight == 1 && dir == 5)) //left
                        currentCol--;
                    else if (dir == 3 || (directionalWeight == 2 && dir == 5)) //down
                        currentRow++;
                    else if (dir == 4 || (directionalWeight == 3 && dir == 5)) //right
                        currentCol++;
                }
            }
        }
        private void GenerateRoads(int amount)
        {
            for(int roadAmount = amount; roadAmount > 0; --roadAmount)
            {
                var startSide = _random.Next(4);
                var endSide = _random.Next(4);
                while(endSide == startSide)
                    endSide = _random.Next(4);
                var startLocation = new int[] { (startSide == 0) ? 0 : (startSide == 1) ? _size[0] - 1 : _random.Next(_size[0]), (startSide == 2) ? 0 : (startSide == 3) ? _size[1] - 1 : _random.Next(_size[1]) };
                var endLocation = new int[] { (endSide == 0) ? 0 : (endSide == 1) ? _size[0] - 1 : _random.Next(_size[0]), (endSide == 2) ? 0 : (endSide == 3) ? _size[1] - 1 : _random.Next(_size[1]) };
                CreateRoad(startLocation, endLocation);
            }
        }
        private void CreateRoad(int[] startLocation, int[] endLocation)
        {
            var currentRow = startLocation[0];
            var currentCol = startLocation[1];
            //this is not clean, but it is 5AM and I am tired
            var ignoreVert = ((startLocation[1] == 0 || startLocation[1] == _size[1] - 1) && (endLocation[1] == 0 || endLocation[1] == _size[1] - 1)) ?  -1 : (startLocation[0] < endLocation[0]) ? 0 : 2;
            var ignoreHoriz = ((startLocation[0] == 0 || startLocation[0] == _size[0] - 1) && (endLocation[0] == 0 || endLocation[0] == _size[0] - 1)) ? -1 : (startLocation[1] < endLocation[1]) ? 1 : 3;
            while ((currentRow != endLocation[0] && (ignoreVert == 0 || ignoreVert == 2)) || (currentCol != endLocation[1] && (ignoreHoriz == 1 || ignoreHoriz == 3)) || ((currentRow != endLocation[0] || currentCol != endLocation[1]) && (ignoreVert == -1 && ignoreHoriz == -1)))
            {
                Map[endLocation[0], endLocation[1]] = new MapTile(MapTileType.Flag);
                Map[currentRow, currentCol] = new MapTile(GetRelatedType(Map[currentRow, currentCol], "road"));
                var direction = _random.Next(4);
                if ((direction == 0 && ignoreVert != 0 && !((ignoreVert == -1 || ignoreVert == 2) && currentRow <= endLocation[0])) && ValidateVerticalRoadExpansion(currentRow - 1, currentCol, -1)) //up
                    currentRow--;
                else if ((direction == 1 && ignoreHoriz != 1 && !((ignoreHoriz == -1 || ignoreHoriz == 1) && currentCol >= endLocation[1])) && ValidateHorizontalRoadExpansion(currentRow, currentCol - 1, -1)) //left
                    currentCol--;
                else if ((direction == 2 && ignoreVert != 2 && !((ignoreVert == -1 || ignoreVert == 0) && currentRow >= endLocation[0])) && ValidateVerticalRoadExpansion(currentRow + 1, currentCol, 1)) //down
                    currentRow++;
                else if ((direction == 3 && ignoreHoriz != 3 && !((ignoreHoriz == -1 || ignoreHoriz == 3) && currentCol <= endLocation[1])) && ValidateHorizontalRoadExpansion(currentRow, currentCol + 1, 1)) //right
                    currentCol++;
                //Console.WriteLine(ToString());
            }
        }
        private bool ValidateVerticalRoadExpansion(int row, int col, int direction)
        {
            if (row >= 0 && row < _size[0])
            {
                if ((!(col - 1 < -1) || !TestIfIsRoad(Map[row, col - 1])) && (!(col + 1 > _size[1]) || !TestIfIsRoad(Map[row, col + 1])) && ((!(row + direction < -1) || !(row + direction > _size[0])) || !TestIfIsRoad(Map[row + direction, col])) && !TestIfIsRoad(Map[row, col]))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        private bool ValidateHorizontalRoadExpansion(int row, int col, int direction)
        {
            if (col >= 0 && col < _size[1])
            {
                if ((!(row - 1 < -1) || !TestIfIsRoad(Map[row - 1, col])) && (!(row + 1 > _size[0]) || !TestIfIsRoad(Map[row + 1, col])) && ((!(col + direction < -1) || !(col + direction > _size[1])) || !TestIfIsRoad(Map[row, col + direction])) && !TestIfIsRoad(Map[row, col]))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        private bool TestIfIsRoad(MapTile tile)
        {
            return tile.Biome switch
            {
                MapTileType.GrassRoad => true,
                MapTileType.HillsRoad => true,
                MapTileType.RockyRoad => true,
                MapTileType.RockyHillsRoad => true,
                MapTileType.SandyRoad => true,
                MapTileType.SandyHillsRoad => true,
                MapTileType.ForestRoad => true,
                MapTileType.ForestHillsRoad => true,
                _ => false
            };
        }
        private MapTileType GetRelatedType(MapTile tile, string varient)
        {
            if (varient == "road")
                return tile.Biome switch
                {
                    MapTileType.Plains => MapTileType.GrassRoad,
                    MapTileType.Hills => MapTileType.HillsRoad,
                    MapTileType.Rocky => MapTileType.RockyRoad,
                    MapTileType.RockyHills => MapTileType.RockyHillsRoad,
                    MapTileType.Sandy => MapTileType.SandyRoad,
                    MapTileType.SandyHills => MapTileType.SandyHillsRoad,
                    MapTileType.Forest => MapTileType.ForestRoad,
                    MapTileType.ForestHills => MapTileType.ForestHillsRoad,
                    MapTileType.River => MapTileType.Bridge,
                    _ => MapTileType.GrassRoad
                };
            return 0;
        }
        public override string ToString()
        {
            var str = "";
            for (int i = 0; i < Map.GetLength(0); ++i)
            {
                for (int j = 0; j < Map.GetLength(1); ++j)
                {
                    if (Map[i, j].Biome == Models.Enums.MapTileType.Plains)
                        str += "  ";
                    else if (Map[i, j].Biome == Models.Enums.MapTileType.River)
                        str += "W ";
                    else if (Map[i, j].Biome == Models.Enums.MapTileType.Rocky)
                        str += "g ";
                    else if (Map[i, j].Biome == Models.Enums.MapTileType.Forest)
                        str += "f ";
                    else if (Map[i, j].Biome == Models.Enums.MapTileType.Sandy)
                        str += "s ";
                    else if (Map[i, j].Biome == Models.Enums.MapTileType.Hills)
                        str += "H ";
                    else if (Map[i, j].Biome == Models.Enums.MapTileType.ForestHills)
                        str += "F ";
                    else if (Map[i, j].Biome == Models.Enums.MapTileType.RockyHills)
                        str += "G ";
                    else if (Map[i, j].Biome == Models.Enums.MapTileType.SandyHills)
                        str += "S ";
                    else if (Map[i, j].Biome == Models.Enums.MapTileType.Bridge)
                        str += "B ";
                    else if (Map[i, j].Biome == Models.Enums.MapTileType.Flag)
                        str += "! ";
                    else
                        str += "R ";
                }
                str += "\n";
            }
            return str;
        }
    }
}
