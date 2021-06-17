using System;

namespace GameTime.DotPuzzleModels
{
    public class Puzzles
    {
        public string Puzzle { get; set; }
        public TimeSpan TimeLimit { get; set; }
        public int Splitter { get; set; }
        public int Moves { get; set; }
        public DotDifficulty DotDifficulty { get; set; }
    }
}
