namespace GameTime.Models
{
    public class Material
    {
        public MaterialType Type { get; private set; }
        public Material()
        {

        }
        public Material(MaterialType type)
        {
            Type = type;
        }
    }
}
