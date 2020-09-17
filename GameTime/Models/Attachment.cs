namespace GameTime.Models
{
    public class Attachment : Item
    {
        public Addon Ability { get; set; }
        public int Intensity { get; set; }
        public bool IsActive { get; set; } = false;
    }
}
