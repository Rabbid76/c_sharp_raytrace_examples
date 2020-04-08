using rt_2_the_next_week.raytrace.Mathematics;

namespace rt_2_the_next_week.raytrace.Interfaces
{
    public interface INoise
    {
        double Noise(Vec3 p);
        double Turb(Vec3 p, int depth = 7);
    }
}
