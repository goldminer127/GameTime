using System;
using System.Collections.Generic;
using System.Text;

namespace GameTime.MinesweeperModels
{
    public class Tile
    {
        public char Symbol { get; private set; } = ' ';
        public bool IsHidden { get; private set; } = true;
        public bool IsMine { get; private set; } = false;
        public bool IsFlagged { get; private set; } = false;
        public Tile()
        {

        }
        public Tile(bool isMine)
        {
            IsMine = isMine;
        }
        public Tile SetSymbol(char symbol)
        {
            Symbol = symbol;
            return this;
        }
        public Tile SetRevealed()
        {
            if(!IsFlagged)
            {
                IsHidden = false;
            }
            return this;
        }
        public Tile SetFlagged()
        {
            if(IsHidden)
            {
                IsFlagged = true;
            }
            return this;
        }
        //Sets Symbole to Mine
        public Tile SetMine()
        {
            IsMine = true;

            return this;
        }
    }
}
