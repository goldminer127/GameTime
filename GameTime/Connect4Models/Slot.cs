namespace GameTime.Connect4Models
{
    public class Connect4Slot
    {
        public Chip Chip { get; private set; } = null;
        public Connect4Slot()
        {
            
        }
        public Connect4Slot SetChip(Chip chip)
        {
            Chip = chip;
            return this;
        }
    }
}
