using rt_2_the_next_week.raytrace.Mathematics;

namespace rt_2_the_next_week.raytrace.Interfaces
{
    public interface IMaterial
    {
        bool Scatter(Ray r_in, HitRecord rec, out Vec3 attenuation, out Ray scattered);

        Vec3 Emitted(double u, double v, Vec3 p);
    }
}
