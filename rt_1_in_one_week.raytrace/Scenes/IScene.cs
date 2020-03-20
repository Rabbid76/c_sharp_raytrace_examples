using System.Collections.Generic;
using rt_1_in_one_week.raytrace.Mathematics;

namespace rt_1_in_one_week.raytrace.Scenes
{
    public interface IScene
    {
        Camera Camera { get; }
        IHitableList World { get; }
    }
}
