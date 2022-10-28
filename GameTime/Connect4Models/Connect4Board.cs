using System;
namespace GameTime.Connect4Models
{
    public class Connect4Board
    {
        public Connect4Slot[,] Board { get; set; }
        public Connect4Board()
        {
            GenerateBoard(6, 7);
        }
        private void GenerateBoard(int r, int c)
        {
            Board = new Connect4Slot[r, c];
            for(int row = 0; row < r; row++)
            {
                for(int col = 0; col < c; col++)
                {
                    Board[row, col] = new Connect4Slot();
                }
            }
        }
        public bool PlaceChip(int player, int col, out bool win)
        {
            //Check for false placement, happens when attempting to put chips on second line
            for(int row = Board.GetLength(0) - 1; row > -1; row--)
            {
                //Console.WriteLine((Board[row, col].Chip == null) + " TEST");
                if (Board[row,col].Chip == null)
                {
                    Board[row, col].SetChip(new Chip(player));
                    win = CheckForWin(row, col, player);
                    return true;
                }
            }
            win = false;
            return false;
        }
        public bool CheckForWin(int row, int col, int player)
        {
            if(!CheckDiagnols(row, col, player))
            {
                if (!CheckHorizontal(row, col, player))
                {
                    if (!CheckVertical(row, col, player))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        private bool CheckHorizontal(int row, int col, int player)
        {
            var connectedChips = 0;
            for (int c = -1; c < 2; c += 2)
            {
                for (int i = 0; i < 4 && col + c >= 0 && col + c < Board.GetLength(1); i++)
                {
                    if (Board[row, col + (i * c)].Chip != null && Board[row, col + (i * c)].Chip.PlayerNum == player && connectedChips != 4)
                    {
                        connectedChips++;
                        //Console.WriteLine($"{row} {col + (i * c)} {i} * {c} {Board[row, col + (i * c)].Chip.PlayerNum} {connectedChips}");
                    }
                    else
                    {
                        connectedChips = 0;
                        break;
                    }
                }
            }
            return connectedChips >= 4;
        }
        private bool CheckVertical(int row, int col, int player)
        {
            var connectedChips = 0;
            for (int r = -1; r < 2; r += 2)
            {
                for (int i = 0; i < 4 && row + (i * r) >= 0 && row + (i * r) < Board.GetLength(0); i++)
                {
                    if (Board[row + (i * r), col].Chip != null)
                    {
                        if(Board[row + (i * r), col].Chip.PlayerNum == player && connectedChips != 4)
                        {
                            connectedChips++;
                        }
                        else
                        {
                            connectedChips = 0;
                            break;
                        }
                    }
                    else
                    {
                        connectedChips = 0;
                        break;
                    }
                }
            }
            return connectedChips >= 4;
        }
        private bool CheckDiagnols(int row, int col, int player)
        {
            var connectedLeftDiagnol = 0;
            var connectedRightDiagnol = 0;
            for (int r = -1; r < 2; r += 2)
            {
                for (int c = -1; c < 2; c += 2)
                {
                    for (int i = 0; i < 4 && (row + (i * r)) >= 0 && (row + (i * r)) < Board.GetLength(0) && col + (i * c) >= 0 && col + (i * c) < Board.GetLength(1); i++)
                    {
                        if (Board[row + (i * r), col + (i * c)].Chip != null && Board[row + (i * r), col + (i * c)].Chip.PlayerNum == player && connectedLeftDiagnol < 4 && connectedRightDiagnol < 4)
                        {
                            if(r == c)
                            {
                                connectedLeftDiagnol++;
                            }
                            else
                            {
                                connectedRightDiagnol++;
                            }
                            Console.WriteLine($"r {r} c {c} row {row + (i * r)} col {col + (i * c)} Left {connectedLeftDiagnol} Right {connectedRightDiagnol}");
                        }
                        else if(connectedLeftDiagnol >= 4 || connectedRightDiagnol >= 4)
                        {
                            break;
                        }
                        else
                        {
                            if(r == c)
                            {
                                connectedLeftDiagnol--;
                            }
                            else
                            {
                                connectedRightDiagnol--;
                            }
                            break;
                        }
                    }
                }
            }
            Console.WriteLine($"Left {connectedLeftDiagnol} Right {connectedRightDiagnol}");
            return connectedRightDiagnol >= 4 || connectedLeftDiagnol >= 4;
        }
        public override string ToString()
        {
            var result = "===================\n";
            for(int row = 0; row < Board.GetLength(0); row++)
            {
                result += "| ";
                for(int col = 0; col < Board.GetLength(1); col++)
                {
                    if(Board[row, col].Chip == null)
                    {
                        result += $":white_circle: ";
                    }
                    else
                    {
                        result += $"{Board[row, col].Chip.ChipColor} ";
                    }
                }
                result += "|\n";
            }
            result += "===================\n"; //Change on length
            return result + "| :one: :two: :three: :four: :five: :six: :seven: |";
        }
    }
}
