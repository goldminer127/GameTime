using System;
using System.Collections.Generic;
using System.Text;

namespace GameTime.MinesweeperModels
{
    public class MinesweeperSquare
    {
        public bool IsMine { get; set; }
        public int TotalMines { get; set; }
        public bool Flagged { get; set; }
        public bool IsRevealed { get; set; }
    }
}
