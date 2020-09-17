using System;
using System.Collections.Generic;
using System.Text;

namespace GameTime.DotPuzzleModels
{
    public class PuzzleList
    {
        public List<Puzzles> AllPuzzles { get; set; }
        public PuzzleList()
        {
            AllPuzzles = new List<Puzzles>();
            List<string> puzzle = new List<string>();
            AllPuzzles.Add(new Puzzles() { Puzzle = " X- X- X- +- X- X- X- X- X- X- X- X- X- X- X- X-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 4, DotDifficulty = DotDifficulty.Extreme });
            AllPuzzles.Add(new Puzzles() { Puzzle = " X- O- O- O- O- X- O- O- O- +- O- O- O- O- O- O- O- O- O- X- O- O- X- O- O- O- O- O- O- O- O- O- O- O- X- O- O- O- O- X- O- O- O- O- X- O- O- O- O-", TimeLimit = TimeSpan.FromSeconds(40), Splitter = 7, DotDifficulty = DotDifficulty.Hard });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- O- O- O- O- O- O- X- O- X- O- O- O- X- O- O- +- X- O- O- O- O- O- O- O- O- O- X- O- O- O- X- O- O- O- X- O- X- O- O- O- O- O- O- O- O- O-", TimeLimit = TimeSpan.FromSeconds(40), Splitter = 7, DotDifficulty = DotDifficulty.Hard });
            AllPuzzles.Add(new Puzzles() { Puzzle = " +- O- O- O- O- O- O- X- O- O- O- O- X- O- O- O-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 4, DotDifficulty = DotDifficulty.Easy });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- X- O- O- O- O- O- X- O- O- +- O- O- O-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 4, DotDifficulty = DotDifficulty.Easy });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- O- O- O- X- O- +- O- O- O- O- O- X- O-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 4, DotDifficulty = DotDifficulty.Easy });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- O- O- O- O- X- O- O- O- O- O- O- O- O- X- O- X- O- O- O- O- O- O- O- O- +- O- O- O- O- X- O- O- O-", TimeLimit = TimeSpan.FromSeconds(35), Splitter = 6, DotDifficulty = DotDifficulty.Medium });
            AllPuzzles.Add(new Puzzles() { Puzzle = " X- O- O- O- O- O- O- O- O- O- O- O- O- X- O- O- O- O- O- O- O- O- O- X- +- O- O- O- O- O- O- O- O- O- X- O-", TimeLimit = TimeSpan.FromSeconds(35), Splitter = 6, DotDifficulty = DotDifficulty.Medium });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- X- O- X- O- O- O- O- O- O- X- O- X- +- O- O- O- O- O- O- X- O- O-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 5, DotDifficulty = DotDifficulty.Hard });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- O- X- O- O- X- O- O- X- O- O- O- O- O- X- O- O- O- O- O- O- X- O- O- O- O- O- O- O- O- O- O- O- O- O- X- O- X- O- O- O- X- O- O- O- O- O- O- O- X- O- O- O- O- +- X- O- O- O- O- O- X- O- O- O- O- O- X- O- O- X- X- O- O- O- O- O- O- X-", TimeLimit = TimeSpan.FromSeconds(50), Splitter = 9, DotDifficulty = DotDifficulty.Extreme });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- X- X- X- O- O- O- O- O- X- O- O- O- X- +- O- O- O- X- O- O- O- O- O- O- O- O- X- O- X- X- X- O- O- O- O- X- O- O- O- X- O- O- O- O- O- X- X- X- O- O- O- O- O- O- O- O- O- O- O- O- O- O- O- O- O- O- O- O- O- O- O- O- O- O- O- O- O- O-", TimeLimit = TimeSpan.FromSeconds(25), Splitter = 9, DotDifficulty = DotDifficulty.Extreme });
            AllPuzzles.Add(new Puzzles() { Puzzle = " +- O- O- O- O- O- O- O- O- O- O- X- O- X- O- O- O- O- O- O- O- O- O- X- O- O- O- X- O- O- O- O- O- O- O- O-", TimeLimit = TimeSpan.FromSeconds(35), Splitter = 6, DotDifficulty = DotDifficulty.Medium });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- X- O- O- O- O- +- O- O- O- O- O- O- O- X- O- O- X- O- O- O- O- O- O- O- X- O- O- O- O- O- O- O- O-", TimeLimit = TimeSpan.FromSeconds(35), Splitter = 6, DotDifficulty = DotDifficulty.Medium });
            AllPuzzles.Add(new Puzzles() { Puzzle = " X- O- O- O- +- O- O- O- O- O- O- O- O- O- O- O- O- O- O- O- X- O- O- O- X-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 5, DotDifficulty = DotDifficulty.Medium });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- O- X- X- O- O- O- O- O- O- O- O- O- O- O- +- O- O- O- O- O- O- X-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 5, DotDifficulty = DotDifficulty.Medium });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- X- O- +- O- O- O- O- O- O- O- O- O- O- O- X-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 4, DotDifficulty = DotDifficulty.Easy });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- X- O- O- O- O- O- O- O- +- O- O- O- O- O- X-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 4, DotDifficulty = DotDifficulty.Easy });
            AllPuzzles.Add(new Puzzles() { Puzzle = " X- O- O- O- O- O- O- O- O- X- O- O- O- O- +- O- O- O- O- X- O- O- O- O- O-", TimeLimit = TimeSpan.FromSeconds(35), Splitter = 5, DotDifficulty = DotDifficulty.Easy });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- X- O- O- O- O- O- O- O- X- O- O- O- O- O- O- O- +- O- O- O- O- O- X-", TimeLimit = TimeSpan.FromSeconds(35), Splitter = 5, DotDifficulty = DotDifficulty.Easy });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- X- O- O- O- O- O- O- O- O- O- O- O- O- O- O- O- +- O- O- O- X- O- O- O-", TimeLimit = TimeSpan.FromSeconds(35), Splitter = 5, DotDifficulty = DotDifficulty.Easy });
            AllPuzzles.Add(new Puzzles() { Puzzle = " X- O- O- O- O- O- O- O- O- +- O- O- O- O- O- X-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 4, DotDifficulty = DotDifficulty.Easy });
            AllPuzzles.Add(new Puzzles() { Puzzle = " X- O- O- O- O- O- O- O- O- X- X- O- O- O- O- O- O- O- O- +- O- O- O- O- X-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 5, DotDifficulty = DotDifficulty.Medium });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- +- O- O- X- O- O- O- X- O- O- O- O- O- O- X- O- X- O- O- O- O- O-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 5, DotDifficulty = DotDifficulty.Medium });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- X- O- O- O- +- O- O- O- O- O- O- X- O- O- X- O- O- O- O- O- X- O- O-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 5, DotDifficulty = DotDifficulty.Medium });
            AllPuzzles.Add(new Puzzles() { Puzzle = " X- O- O- X- O- O- O- O- O- O- O- O- O- O- O- O- X- O- O- O- X- O- O- +- O-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 5, DotDifficulty = DotDifficulty.Medium });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- O- O- X- O- O- O- O- O- O- O- O- X- O- +- O- O- O- X- O- O- X- O-", TimeLimit = TimeSpan.FromSeconds(25), Splitter = 5, DotDifficulty = DotDifficulty.Hard });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- X- O- X- O- O- O- O- O- O- +- O- O- O- O- O- O- O- O- O- O- X- O-", TimeLimit = TimeSpan.FromSeconds(25), Splitter = 5, DotDifficulty = DotDifficulty.Hard });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- X- O- O- O- O- O- O- X- O- X- O- O- O- O- O- +- O- O- X- O- X- O- O- O-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 5, DotDifficulty = DotDifficulty.Hard });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- O- X- O- O- X- O- O- O- O- O- X- O- O- O- O- O- O- X- O- +- O- O-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 5, DotDifficulty = DotDifficulty.Medium });
            AllPuzzles.Add(new Puzzles() { Puzzle = " X- O- O- O- X- O- O- O- O- O- O- O- X- O- O- O- O- O- O- O- +- O- O- O- X-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 5, DotDifficulty = DotDifficulty.Medium });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- X- O- O- O- X- O- O- O- O- O- O- X- O- O- O- O- O- O- O- X- O- O- +-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 5, DotDifficulty = DotDifficulty.Medium });
            AllPuzzles.Add(new Puzzles() { Puzzle = " X- O- O- O- O- O- O- +- O- X- O- O- O- O- O- O-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 4, DotDifficulty = DotDifficulty.Easy });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- O- O- O- O- O- O- O- O- O- O- X- +- X-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 4, DotDifficulty = DotDifficulty.Easy });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- +- O- O- O- O- X- O- O- O- O- O- X- O- O- O-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 4, DotDifficulty = DotDifficulty.Easy });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- O- O- O- O- X- O- O- +- O- O- X- O- O-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 4, DotDifficulty = DotDifficulty.Easy });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- X- O- O- O- X- O- O- O- O- O- O- O- O- O- O- O- O- O- O- O- X- O- X- O- X- O- X- X- O- X- O- X- O- X- O- O- O- +- O- O- O- O- O- O- O- O- O- O-", TimeLimit = TimeSpan.FromSeconds(80), Splitter = 7, DotDifficulty = DotDifficulty.Extreme });
            AllPuzzles.Add(new Puzzles() { Puzzle = " X- O- O- O- O- O- X- O- O- O- X- O- O- O- O- O- O- O- O- O- O- X- O- +- X- O- O- X- O- O- O- O- O- O- O- O- O- O- X- O- O- O- X- O- O- O- O- O- X-", TimeLimit = TimeSpan.FromSeconds(80), Splitter = 7, DotDifficulty = DotDifficulty.Extreme });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- O- X- O- O- X- O- O- O- O- O- O- O- O- O- X- O- O- O- +- O- O- X- O- O- O- O- X- O- O- O- X- O- O-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 6, DotDifficulty = DotDifficulty.Hard });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- O- O- O- O- X- O- X- O- O- O- O- O- O- O- O- O- O- O- X- O- O- O- X- O- +- O- X- O- O- O- X- O- O-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 6, DotDifficulty = DotDifficulty.Hard });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- O- O- O- O- X- O- O- X- O- O- X- O- O- O- O- X- O- O- O- +- O- O- O- X- O- O- O- O- X- O- O- O- O-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 6, DotDifficulty = DotDifficulty.Hard });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- O- O- X- O- O- O- X- O- O- O- O- O- O- O- O- O- O- O- +- O- X- X- X- O- O- O- O- O- O- O- O- X- O-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 6, DotDifficulty = DotDifficulty.Hard });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- X- O- O- O- X- O- O- O- O- O- O- O- O- X- O- O- O- O- +- O- O- O- O- X- O- O- O- O- O- O- O- O- O-", TimeLimit = TimeSpan.FromSeconds(35), Splitter = 6, DotDifficulty = DotDifficulty.Medium });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- O- O- O- O- O- O- O- O- X- O- X- O- O- O- O- O- O- +- O- O- O- O- O- O- X- O- O- O- O- X- O- O- O-", TimeLimit = TimeSpan.FromSeconds(35), Splitter = 6, DotDifficulty = DotDifficulty.Medium });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- O- O- O- O- +- O- O- X- O- O- O- O- O- O- O- O- X- O- O- O- X- O- O- O- O- O- O- O- O- O- X- O- O-", TimeLimit = TimeSpan.FromSeconds(35), Splitter = 6, DotDifficulty = DotDifficulty.Medium });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- O- O- O- O- O- X- O- O- O- O- O- O- O- O- +- O- X- O- X- O- O- O- O- O- O- O- O- O- O- X- O- O- O-", TimeLimit = TimeSpan.FromSeconds(35), Splitter = 6, DotDifficulty = DotDifficulty.Medium });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- O- O- +- O- X- X- O- O- O- O- O- O- O-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 4, DotDifficulty = DotDifficulty.Easy });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- X- O- O- X- O- O- O- O- O- O- +- O- O- O- O-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 4, DotDifficulty = DotDifficulty.Easy });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- +- X- X- O- O- O- O- O- O- O- O- O- O- O-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 4, DotDifficulty = DotDifficulty.Easy });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- O- O- O- X- O- O- X- O- +- O- O- O- O-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 4, DotDifficulty = DotDifficulty.Easy });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- X- O- O- O- +- O- O- O- O- O- O- X- O- O-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 4, DotDifficulty = DotDifficulty.Easy });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- X- O- O- O- O- O- +- O- O- O- O- O- O- X-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 4, DotDifficulty = DotDifficulty.Easy });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- +- X- O- O- O- O- O- O- O- O- X- O- O-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 4, DotDifficulty = DotDifficulty.Easy });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- O- O- +- O- O- O- O- X- O- O- O- X- O-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 4, DotDifficulty = DotDifficulty.Easy });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- O- O- O- O- O- O- O- O- O- O- O- O- O- O- O- O- O- O- O- X- O- X- O- O- X- O- O- X- +- X- O- O- X-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 6, DotDifficulty = DotDifficulty.Hard });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- X- O- O- O- O- O- O- O- O- O- X- O- O- O- X- O- O- O- +- O- O- X- O- O- O- O- O- O- O- X- O- X- O-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 6, DotDifficulty = DotDifficulty.Hard });
            AllPuzzles.Add(new Puzzles() { Puzzle = " X- O- O- O- O- +- O- O- X- O- O- O- O- O- O- X- O- O- O- X- O- O- O- O- O- O- X- O- X- O- O- O- O- O- O- O-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 6, DotDifficulty = DotDifficulty.Hard });
            AllPuzzles.Add(new Puzzles() { Puzzle = " X- O- O- O- O- O- O- O- O- X- X- O- O- O- O- O- O- +- O- O- O- O- O- X- O-", TimeLimit = TimeSpan.FromSeconds(25), Splitter = 5, DotDifficulty = DotDifficulty.Hard });
            AllPuzzles.Add(new Puzzles() { Puzzle = " X- O- O- O- O- O- O- O- X- O- O- O- O- O- O- O- O- O- O- X- X- O- O- +- O-", TimeLimit = TimeSpan.FromSeconds(25), Splitter = 5, DotDifficulty = DotDifficulty.Hard });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- O- X- O- O- X- O- O- X- O- O- +- O- O- O- O- O- O- X- O- X- O- O-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 5, DotDifficulty = DotDifficulty.Hard });
            AllPuzzles.Add(new Puzzles() { Puzzle = " X- O- O- X- O- O- O- O- O- O- O- O- X- O- X- O- O- O- O- O- O- X- O- O- +-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 5, DotDifficulty = DotDifficulty.Hard });
            AllPuzzles.Add(new Puzzles() { Puzzle = " X- O- +- O- X- O- O- O- O- O- O- O- X- O- O- X- O- O- O- O- O- O- O- X- O-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 5, DotDifficulty = DotDifficulty.Hard });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- X- O- O- O- O- O- O- O- O- O- O- O- O- +- O- X- O- X- O- O- X- O- O- O- O- O- O- O- X- O- X- O- O- O- O-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 6, DotDifficulty = DotDifficulty.Hard });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- O- O- O- O- X- O- O- X- O- O- O- X- O- O- O- O- O- O- O- O- X- X- O- O- O- +- O- O- O- O- X- O- O-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 6, DotDifficulty = DotDifficulty.Hard });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- X- O- O- O- X- O- O- O- O- O- O- X- O- O- O- O- O- X- O- O- O- +- O- O- X- O- O- O- O- O- O- O- O- O- X-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 6, DotDifficulty = DotDifficulty.Hard });
            AllPuzzles.Add(new Puzzles() { Puzzle = " X- O- O- O- O- X- O- +- O-", TimeLimit = TimeSpan.FromSeconds(25), Splitter = 3, DotDifficulty = DotDifficulty.Easy });
            AllPuzzles.Add(new Puzzles() { Puzzle = " X- O- O- O- O- O- O- O- +-", TimeLimit = TimeSpan.FromSeconds(25), Splitter = 3, DotDifficulty = DotDifficulty.Easy });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- O- O- +- X- O- O-", TimeLimit = TimeSpan.FromSeconds(25), Splitter = 3, DotDifficulty = DotDifficulty.Easy });
            AllPuzzles.Add(new Puzzles() { Puzzle = " X- O- O- O- O- O- O- O- O- O- O- O- +- O- O- X- O- O- O- O- O- O- O- O- X-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 5, DotDifficulty = DotDifficulty.Medium });
            AllPuzzles.Add(new Puzzles() { Puzzle = " +- O- O- O- O- O- O- O- O- X- X- O- O- O- O- O- X- O- O- O- X- O- O- O- O-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 5, DotDifficulty = DotDifficulty.Medium });
            AllPuzzles.Add(new Puzzles() { Puzzle = " X- O- O- O- O- O- O- O- O- O- +- O- O- O- O- X- ", TimeLimit = TimeSpan.FromSeconds(15), Splitter = 4, DotDifficulty = DotDifficulty.Medium });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- X- O- +- O- X- O- O- ", TimeLimit = TimeSpan.FromSeconds(10), Splitter = 3, DotDifficulty = DotDifficulty.Medium });
            AllPuzzles.Add(new Puzzles() { Puzzle = " X- O- O- O- +- O- O- O- X- ", TimeLimit = TimeSpan.FromSeconds(10), Splitter = 3, DotDifficulty = DotDifficulty.Medium });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- O- O- O- O- X- O- O- O- O- O- O- O- O- O- O- X- O- O- O- O- O- O- O- O- O- +- O- O- O- O- O- O- X-", TimeLimit = TimeSpan.FromSeconds(35), Splitter = 6, DotDifficulty = DotDifficulty.Medium });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- O- O- O- O- O- O- O- O- X- O- +- X- O- O- O- O- O- O- O- O- X- O- X- O- O- O- O- O- O- O- O- O- O-", TimeLimit = TimeSpan.FromSeconds(35), Splitter = 6, DotDifficulty = DotDifficulty.Medium });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- O- O- O- O- X- O- O- O- O- O- O- O- +- X- O- O- X- O- O- O- O- O- O- O- O- O- O- O- O- O- X- O- O-", TimeLimit = TimeSpan.FromSeconds(35), Splitter = 6, DotDifficulty = DotDifficulty.Medium });
            AllPuzzles.Add(new Puzzles() { Puzzle = " X- O- O- X- O- O- O- O- O- O- O- O- O- O- +- O- O- X- O- O- X- O- O- O- O-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 5, DotDifficulty = DotDifficulty.Medium });
            AllPuzzles.Add(new Puzzles() { Puzzle = " X- O- X- O- O- O- O- O- O- O- O- +- O- O- O- O- O- O- O- O- X- O- X- O- O-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 5, DotDifficulty = DotDifficulty.Medium });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- O- O- O- O- O- O- X- X- O- O- O- O- +- X- O- O- O- O- O- O- X- O-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 5, DotDifficulty = DotDifficulty.Medium });
            AllPuzzles.Add(new Puzzles() { Puzzle = " X- O- O- O- O- O- O- O- O- O- O- O- O- O- O- O- X- O- O- X- O- O- O- O- O- O- O- O- O- O- +- O- O- X- O- O-", TimeLimit = TimeSpan.FromSeconds(35), Splitter = 6, DotDifficulty = DotDifficulty.Medium });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- X- O- O- O- O- O- O- O- O- O- O- O- O- O- X- O- O- X- O- O- O- O- O- O- O- +- O- O- O- O- O- O- O- O- X-", TimeLimit = TimeSpan.FromSeconds(35), Splitter = 6, DotDifficulty = DotDifficulty.Medium });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- O- O- O- O- X- O- O- O- O- O- O- O- X- O- O- O- O- +- O- O- O- O- O- O- O- X- O- O- X- O- O- O- O-", TimeLimit = TimeSpan.FromSeconds(35), Splitter = 6, DotDifficulty = DotDifficulty.Medium });
            AllPuzzles.Add(new Puzzles() { Puzzle = " X- O- O- X- O- O- O- O- O- O- X- O- O- +- O- O- O- O- O- O- O- O- O- O- O- O- X- O- O- O- X- O- O- O- O- X-", TimeLimit = TimeSpan.FromSeconds(30), Splitter = 6, DotDifficulty = DotDifficulty.Hard });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- X- O- O- O- O- X- O- O- O- X- O- O- O- X- O- O- O- O- O- O- O- O- O- +- O- O- X- O- O- O- O- O- O- O- O- X- O- O- O- X- O- O- O- O- O- O- O- O-", TimeLimit = TimeSpan.FromSeconds(40), Splitter = 7, DotDifficulty = DotDifficulty.Hard });
            AllPuzzles.Add(new Puzzles() { Puzzle = " X- O- O- O- X- O- O- O- O- O- O- O- X- O- O- X- O- O- O- O- O- O- O- O- O- O- O- X- O- O- X- O- O- O- O- O- O- O- O- +- O- O- O- X- O- O- O- O- O-", TimeLimit = TimeSpan.FromSeconds(40), Splitter = 7, DotDifficulty = DotDifficulty.Hard });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- X- O- O- O- O- O- O- O- O- X- O- O- X- O- X- O- O- O- O- O- O- X- O- X- O- O- O- +- O- O- O- O- O- O- O- O- O- X- O- O- O- O- O- O- O- O-", TimeLimit = TimeSpan.FromSeconds(40), Splitter = 7, DotDifficulty = DotDifficulty.Hard });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- O- O- X- O- O- O- O- X- O- O- O- +- O- O- O- O- O- O- O- O- O- O- X- O- O- O- O- O- O- O- O- X- O- O- O- X- O- O- O- X- O- O- O- O- X- O- O- O-", TimeLimit = TimeSpan.FromSeconds(40), Splitter = 7, DotDifficulty = DotDifficulty.Hard });
            AllPuzzles.Add(new Puzzles() { Puzzle = " X- O- X- O- X- O- X- O- O- O- O- O- O- O- O- O- O- O- O- O- O- +- X- O- X- O- X- O- O- O- O- O- O- O- O- O- O- O- O- O- O- O- O- X- O- X- O- X- O-", TimeLimit = TimeSpan.FromSeconds(80), Splitter = 7, DotDifficulty = DotDifficulty.Extreme });
            AllPuzzles.Add(new Puzzles() { Puzzle = " O- X- O- O- O- X- O- O- O- O- O- O- O- O- O- O- X- O- X- O- X- O- +- O- O- O- O- O- O- O- O- O- O- O- O- O- O- O- X- O- O- O- O- O- X- X- O- O- O- O- O- X- O- O- O- O- O- X- O- O- O- X- O- O- X- O- O- O- X- O- O- O- O- O- O- X- O- O- O- X- O-", TimeLimit = TimeSpan.FromSeconds(50), Splitter = 9, DotDifficulty = DotDifficulty.Extreme });
        }
        public List<string> ConstructPuzzle(string rawPuzzle)
        {
            List<string> puzzle = new List<string>();
            var splitPuzzle = rawPuzzle.Split("-");
            foreach (var s in splitPuzzle)
            {
                puzzle.Add(s);
            }
            return puzzle;
        }
    }
}
