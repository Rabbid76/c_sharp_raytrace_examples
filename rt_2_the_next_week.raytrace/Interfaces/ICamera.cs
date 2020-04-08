using rt_2_the_next_week.raytrace.Mathematics;

namespace rt_2_the_next_week.raytrace.Interfaces
{
    public interface ICamera
    {
        Ray Get(double u, double v);
    }
}
