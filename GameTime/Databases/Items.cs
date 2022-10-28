using System.Collections.Generic;
using System.Text;
using GameTime.Models;

namespace GameTime.Databases
{
    public class ItemDatabase
    {
        public Dictionary<string, Item> Items { get; private set; }
        public ItemDatabase()
        {
            Items = LoadItems();
        }
        private Dictionary<string, Item> LoadItems()
        {
            return new Dictionary<string, Item>
            {
                { "Glock", new Firearm{Id = "0", Name = "Glock", Quality = Quality.Standard, Type = ItemType.Firearm, Value = 120.75m, Image = null}.Build(12, 22, null, null, 0.07, 0.80, "9MM") },
            };
        }
        /*
        private List<Part> MakeParts()
        {
            return new List<Part>
            {
                //new Part()
            };
        }
        */
    }
}
