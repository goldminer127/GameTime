namespace GameTime.Connect4Models
{
    public class Chip
    {
        public int PlayerNum { get; private set; }
        public string ChipColor { get; private set; }
        //Largest number should be 6
        public Chip(int playerNum)
        {
            PlayerNum = playerNum;
            SetColor();
        }
        private void SetColor()
        {
            ChipColor =  PlayerNum switch
            {
                0 => ":red_circle:",
                1 => ":yellow_circle:",
                2 => ":blue_circle:",
                3 => ":orange_circle:",
                4 => ":purple_circle:",
                _ => ":brown_circle:"
            };
        }
    }
}
