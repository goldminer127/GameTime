using System;
using System.Collections.Generic;

namespace GameTime.MinesweeperModels
{
    class MinesweeperBoard
    {
        public MinesweeperSquare[,] Board { get; set; } = new MinesweeperSquare[10, 10];
        public MineDifficulty Difficulty { get; }
        public MinesweeperBoard(MineDifficulty difficulty)
        {
            Difficulty = difficulty;
            for (int row = 0; row < Board.GetLength(0); row++)
            {
                for (int col = 0; col < Board.GetLength(1); col++)
                {
                    Board[row, col] = new MinesweeperSquare();
                }
            }
            var n = (int)Difficulty;
            int totalMines = n switch
            {
                0 => 10,
                1 => 20,
                _ => 40,
            };
            ConstructBoard(totalMines);
        }
        private void ConstructBoard(int totalMines)
        {
            var positions = new List<int>();
            for(int i = 0; i < totalMines; i++)
            {
                var pos = new Random().Next(0, 100);
                while (positions.Contains(pos))
                {
                    pos = new Random().Next(0, 100);
                }
                positions.Add(pos);
            }
            foreach(int n in positions)
            {
                int x = n / 10;
                int y = (n % 10);
                Board[x, y].IsMine = true;
                Board[x, y].TotalMines = -1;
            }
            
            //Add adjacent mines
            for(int row = 0; row < Board.GetLength(0); row++)
            {
                for(int col = 0; col < Board.GetLength(1); col++)
                {
                    if(Board[row, col].IsMine == false)
                    {
                        try
                        {
                            if(Board[row - 1, col - 1].IsMine)
                            {
                                Board[row, col].TotalMines++;
                            }
                        }
                        catch{}
                        try
                        {
                            if (Board[row - 1, col].IsMine)
                            {
                                Board[row, col].TotalMines++;
                            }
                        }
                        catch { }
                        try
                        {
                            if (Board[row - 1, col + 1].IsMine)
                            {
                                Board[row, col].TotalMines++;
                            }
                        }
                        catch { }
                        try
                        {
                            if (Board[row , col - 1].IsMine)
                            {
                                Board[row, col].TotalMines++;
                            }
                        }
                        catch { }
                        try
                        {
                            if (Board[row, col + 1].IsMine)
                            {
                                Board[row, col].TotalMines++;
                            }
                        }
                        catch { }
                        try
                        {
                            if (Board[row + 1, col - 1].IsMine)
                            {
                                Board[row, col].TotalMines++;
                            }
                        }
                        catch { }
                        try
                        {
                            if (Board[row + 1, col].IsMine)
                            {
                                Board[row, col].TotalMines++;
                            }
                        }
                        catch { }
                        try
                        {
                            if (Board[row + 1, col + 1].IsMine)
                            {
                                Board[row, col].TotalMines++;
                            }
                        }
                        catch { }
                    }
                }
            }
        }
    }
}
