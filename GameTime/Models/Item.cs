namespace GameTime.Models
{
    public abstract class Item
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Quality Quality { get; set; }
        public ItemType Type { get; set; }
        public Rarity Rarity { get; set; }
        public decimal Value { get; set; }
        public string Image { get; set; }
        public Item()
        {

        }
        public Item SetProperties(string id, string name, Quality quality, ItemType type, Rarity rarity, decimal value, string image)
        {
            Id = id;
            Name = name;
            Quality = quality;
            Type = type;
            Rarity = rarity;
            Value = value;
            Image = image;
            return this;
        }
        public Item SetProperties(string id, string name, ItemType type, decimal value, string image)
        {
            Id = id;
            Name = name;
            Type = type;
            Value = value;
            Image = image;
            return this;
        }
        //Checks if this item is the same as item. Ignores type or other details other than id
        public bool Equals(Item item)
        {
            return item.Id == this.Id;
        }
    }
}
