using rt_2_the_next_week.Mathematics;

namespace rt_2_the_next_week.Scenes
{
    public interface IScene
    {
        Camera Camera { get; }
        IHitable World { get; }
    }
}

