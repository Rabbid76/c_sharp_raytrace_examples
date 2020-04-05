
namespace rt_2_the_next_week.Mathematics
{
    class DiffuseLight
        : IMaterial
    {
        ITexture _emit;

        public DiffuseLight(ITexture emit)
        {
            _emit = emit;
        }

        public bool Scatter(Ray r_in, HitRecord rec, out Vec3 attenuation, out Ray scattered)
        {
            attenuation = Vec3.Create(0);
            scattered = null;
            return false;
        }

        public Vec3 Emitted(double u, double v, Vec3 p)
        {
            return _emit.Value(u, v, p);
        }
    }
}
