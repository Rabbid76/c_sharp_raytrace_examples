using rt_2_the_next_week.raytrace.Mathematics;

namespace rt_2_the_next_week.raytrace.Interfaces
{
    public interface ITexture
    {
        Vec3 Value(double u, double v, Vec3 p);
    }
}
