
namespace GameTime.Models
{
    public abstract class Item
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Subname { get; set; } = "None";
        public Rarity Rarity { get; set; }
        public decimal Value { get; set; }
        public int Multiple { get; set; } = 1;
        public bool IsPurchaseable { get; set; } = false;
        public bool IsSellable { get; set; } = true;
    }
}