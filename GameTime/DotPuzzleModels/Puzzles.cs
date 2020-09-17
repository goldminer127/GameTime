using System;
using System.Collections.Generic;
using System.Text;

namespace GameTime.DotPuzzleModels
{
    public class Puzzles
    {
        public string Puzzle { get; set; }
        public TimeSpan TimeLimit { get; set; }
        public int Splitter { get; set; }
        public DotDifficulty DotDifficulty { get; set; }
    }
}
