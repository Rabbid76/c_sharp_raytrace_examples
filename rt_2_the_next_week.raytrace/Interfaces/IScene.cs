using rt_2_the_next_week.raytrace.Mathematics;

namespace rt_2_the_next_week.raytrace.Interfaces
{
    public interface IScene
    {
        Camera Camera { get; }
        IHitable World { get; }
        Vec3 Sky(Ray r);
    }
}

