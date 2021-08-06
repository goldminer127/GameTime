
namespace GameTime.Connect4Models
{
    public class Connect4Board
    {
        public byte[,] Board { get; set; } = new byte[6, 7];
        public Connect4Board()
        {
            ConstructBoard(Board);
        }
        private void ConstructBoard(byte[,] board)
        {
            for(int row = 0; row < board.GetLength(0); row++)
            {
                for(int col = 0; col < board.GetLength(1); col++)
                {
                    board[row, col] = 0;
                }
            }
        }
        public string ConstructDisplay(byte[,] board)
        {
            var display = "Respond with the column number you want to place your chip.\n===================\n";
            for (int row = 0; row < board.GetLength(0); row++)
            {
                display += "| ";
                for (int col = 0; col < board.GetLength(1); col++)
                {
                    switch(board[row, col])
                    {
                        case 0:
                            display += $":white_circle: ";
                            break;
                        case 1:
                            display += $":yellow_circle: ";
                            break;
                        case 2:
                            display += $":red_circle: ";
                            break;
                    }
                }
                display += "|\n";
            }
            display += "===================\n| :one: :two: :three: :four: :five: :six: :seven: |";
            return display;
        }
    }
}
