namespace rt_2_the_next_week.Mathematics
{
    public class ConstantTexture
        : ITexture
    {
        private Vec3 _color;

        public Vec3 Color { get => _color; }

        public ConstantTexture(Vec3 color)
        {
            _color = color;
        }

        static public ConstantTexture Create(double r, double g, double b) => new ConstantTexture(Vec3.Create(r, g, b)); 

        public Vec3 Value(double u, double v, Vec3 p) => _color;
    }
}
