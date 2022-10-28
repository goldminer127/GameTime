using System.Collections.Generic;
using System;

namespace GameTime.MinesweeperModels
{
    public class MinesweeperBoard
    {
        public Tile[,] Board { get; private set; }
        public Difficulty Difficulty { get; private set; }
        public int TotalFlags { get; private set; }
        public int Status { get; private set; } = 0; //0 == ongoing, 1 == loose, 2 == win
        private int TotalMines { get; set; }
        private int FlaggedMines { get; set; }
        //Does not include label row and column
        public MinesweeperBoard(int length, int height, Difficulty difficulty)
        {
            Difficulty = difficulty;
            TotalMines = (int)difficulty;
            GenerateBoard(length, height);
        }
        public MinesweeperBoard(int length, int height, int totalMines)
        {
            Difficulty = Difficulty.Random;
            TotalMines = totalMines;
            TotalFlags = (int)(totalMines * 1.5);
            GenerateBoard(length, height);
        }
        private void GenerateBoard(int length, int height)
        {
            Board = new Tile[length,height];
            for (int row = 0; row < length; row++)
            {
                for(int col = 0; col < height; col++)
                {
                    Board[row, col] = new Tile();
                }
            }
            PlaceMines(length, height);
        }
        private void PlaceMines(int length, int height)
        {
            Random random = new Random();
            for(int i = 0; i < TotalMines; i++)
            {
                var row = random.Next(0, length);
                var col = random.Next(0, height);
                if (Board[row, col].IsMine)
                {
                    i--;
                }
                else
                {
                    Board[row, col].SetMine().SetSymbol('@');
                    CalculateMines(row, col);
                }
            }
        }
        private void CalculateMines(int row, int col)
        {
            for(int r = -1; r < 2; r++)
            {
                for(int c = -1; c < 2 && (row + r) >= 0 && (row + r) < Board.GetLength(0); c++)
                {
                    if((col + c) >= 0 && (col + c) < Board.GetLength(1))
                    {
                        if(Board[row + r, col + c].Symbol == ' ' && !Board[row + r, col + c].IsMine)
                        {
                            Board[row + r, col + c].SetSymbol('1');
                        }
                        else if(!Board[row + r, col + c].IsMine)
                        {
                            var newSymbol = Board[row + r, col + c].Symbol + 1;
                            Board[row + r, col + c].SetSymbol((char)newSymbol);
                        }
                    }
                }
            }
        }
        public MinesweeperBoard FlagTile(char letter, char num)
        {
            int x = letter - 65;
            int y = num - 48;
            var tile = Board[x, y];
            if (tile.IsMine && !tile.IsFlagged)
            {
                Board[x, y].SetFlagged();
                FlaggedMines++;
                if(FlaggedMines == TotalMines)
                {
                    Status = 2;
                }
            }
            else
            {
                Board[x, y].SetFlagged();
            }
            return this;
        }
        public MinesweeperBoard RevealTile(char letter, char num)
        {
            int x = letter - 65;
            int y = num - 48;
            if (x >= 0 && x < Board.GetLength(0) && y >= 0 && y < Board.GetLength(1))
            {
                if(Board[x, y].IsFlagged)
                {
                    return this;
                }
                else if(Board[x, y].Symbol == ' ')
                {
                    RevealEmpty(x, y);
                }
                else
                {
                    if(Board[x, y].IsMine)
                    {
                        Status = 1;
                    }
                    Board[x, y].SetRevealed();
                }
            }
            return this;
        }
        //Does not work in cases there is a hanging edge. Must fix
        private void RevealEmpty(int row, int col)
        {
            Board[row, col].SetRevealed();
            for (int r = -1; r < 2; r++)
            {
                for (int c = -1; c < 2 && (row + r) >= 0 && (row + r) < Board.GetLength(0); c++)
                {
                    if ((col + c) >= 0 && (col + c) < Board.GetLength(1))
                    {
                        var tile = Board[row + r, col + c];
                        if (tile.Symbol == ' ')
                        {
                            if(tile.IsHidden)
                            {
                                RevealEmpty(row + r, col + c);
                            }
                        }
                        else
                        {
                            Board[row + r, col + c].SetRevealed();
                        }
                    }
                }
            }
        }
        public override string ToString()
        {
            string result = "``` ";
            //Generate number cords
            for(int i = 0; i < Board.GetLength(1); i++)
            {
                result += $" {i}";
            }
            result += "\n";
            for(int row = 0; row < Board.GetLength(0); row++)
            {
                //Generate letter cords
                result += $"{char.ConvertFromUtf32(65 + row)}";
                for (int col = 0; col < Board.GetLength(1); col++)
                {
                    if (Board[row, col].IsFlagged)
                    {
                        result += " !";
                    }
                    else if (Board[row, col].IsHidden)
                    {
                        result += " #";
                    }
                    else
                    {
                        result += $" {Board[row, col].Symbol}";
                    }
                }
                result += "\n";
            }
            return result += "```";
        }
    }
}
