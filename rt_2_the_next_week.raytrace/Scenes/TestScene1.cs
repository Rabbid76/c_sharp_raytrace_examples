using System;
using rt_2_the_next_week.raytrace.Interfaces;
using rt_2_the_next_week.raytrace.Mathematics;
using rt_2_the_next_week.raytrace.Textures;
using rt_2_the_next_week.raytrace.Materials;
using rt_2_the_next_week.raytrace.Hitables.Collections;
using rt_2_the_next_week.raytrace.Hitables.Shapes;

namespace rt_2_the_next_week.raytrace.Scenes
{
    public class TestScene1
        : IScene
    {
        public Camera Camera { get; }
        public IHitable World { get; }

        public TestScene1(double aspect)
        {
            IHitable[] hitables = {
                            new Sphere(Vec3.Create(0, 0, -1), 0.5, new Lambertian(ConstantTexture.Create(0.5, 0.1, 0.1))),
                            new Sphere(Vec3.Create(0, -100.5, -1), 100.0, new Lambertian(ConstantTexture.Create(0.1, 0.1, 0.1)))
                        };
            World = new BVHNode(hitables, 0, 1);
            Camera = Camera.CreateByVerticalFiled(90, aspect);
            World = new BVHNode(hitables, 0, 1);
            Camera = new Camera(
                Vec3.Create(-2, -1, -1),
                Vec3.Create(4, 0, 0),
                Vec3.Create(0, 2, 0),
                Vec3.Create(0, 0, 0),
                0, 0, 0);
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