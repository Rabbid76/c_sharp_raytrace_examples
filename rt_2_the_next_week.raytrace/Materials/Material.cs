using rt_2_the_next_week.raytrace.Interfaces;
using rt_2_the_next_week.raytrace.Mathematics;

namespace rt_2_the_next_week.raytrace.Materials
{
    public abstract class Material
        : IMaterial
    {
        public abstract bool Scatter(Ray r_in, HitRecord rec, out Vec3 attenuation, out Ray scattered);

        public Vec3 Emitted(double u, double v, Vec3 p) => Vec3.Create(0);
    }
}
