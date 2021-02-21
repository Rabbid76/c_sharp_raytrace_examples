using System;
using rt_2_the_next_week.raytrace.Interfaces;
using rt_2_the_next_week.raytrace.Mathematics;
using rt_2_the_next_week.raytrace.Textures;
using rt_2_the_next_week.raytrace.Materials;
using rt_2_the_next_week.raytrace.Hitables.Collections;
using rt_2_the_next_week.raytrace.Hitables.Shapes;

namespace rt_2_the_next_week.raytrace.Scenes
{
    public class TestScene2
        : IScene
    {
        public Camera Camera { get; }
        public IHitable World { get; }

        public TestScene2(double aspect)
        {
            double R = Math.Cos(Math.PI / 4);
            IHitable[] hitables = {
                            new Sphere(Vec3.Create(-R, 0, -1), R, new Lambertian(ConstantTexture.Create(1, 0, 0))),
                            new Sphere(Vec3.Create(R, 0, -1), R, new Lambertian(ConstantTexture.Create(0, 0, 1)))
                        };
            World = new BVHNode(hitables, 0, 1);
            Camera = Camera.CreateByVerticalFiled(90, aspect);
        }

        public Vec3 Sky(Ray r)
        {
            var unit_direction = Vec3.Normalize(r.Direction);
            var t = unit_direction.Y * 0.5 + 0.5;
            var v = new Vec3(1.0) * (1.0 - t) + new Vec3(0.5, 0.7, 1.0) * t;
            return v;
        }
    }
}
