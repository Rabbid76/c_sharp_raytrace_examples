using System;
using System.Collections.Generic;
using rt_1_in_one_week.raytrace.Mathematics;

namespace rt_1_in_one_week.raytrace.Scenes
{
    public class TestScene
        : IScene
    {
        public Camera Camera { get; }
        public IHitableList World { get; }

        public TestScene(double aspect)
        {
            double R = Math.Cos(Math.PI / 4);
            IHitable[] hitables = {
                            new Sphere(Vec3.Create(-R, 0, -1), R, new Lambertian(Vec3.Create(1, 0, 0))),
                            new Sphere(Vec3.Create(R, 0, -1), R, new Lambertian(Vec3.Create(0, 0, 1)))
                        };
            World = new HitableList(hitables);
            Camera = Camera.CreateByVerticalFiled(90, aspect);
        }
    }
}
