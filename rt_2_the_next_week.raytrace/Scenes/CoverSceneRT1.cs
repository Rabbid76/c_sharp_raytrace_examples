using System;
using System.Collections.Generic;
using rt_2_the_next_week.raytrace.Interfaces;
using rt_2_the_next_week.raytrace.Mathematics;
using rt_2_the_next_week.raytrace.Textures;
using rt_2_the_next_week.raytrace.Materials;
using rt_2_the_next_week.raytrace.Hitables.Collections;
using rt_2_the_next_week.raytrace.Hitables.Shapes;

namespace rt_2_the_next_week.raytrace.Scenes
{
    public class CoverSceneRT1
        : IScene
    {
        public Camera Camera { get; }
        public IHitable World { get; }

        public CoverSceneRT1(double aspect)
        {
            var sampler = new Random();
            List<IHitable> hitables = new List<IHitable>();
            hitables.Add(new Sphere(Vec3.Create(0, -1000, 0), 1000, new Lambertian(ConstantTexture.Create(0.5, 0.5, 0.5))));
            for (int a = -11; a < 11; ++a)
            {
                for (int b = -11; b < 11; ++b)
                {
                    double choos_mat = sampler.NextDouble();
                    Vec3 center = Vec3.Create(a + 0.9 * sampler.NextDouble(), 0.2, b + 0.9 * sampler.NextDouble());
                    if ((center - Vec3.Create(4, 0.2, 0)).Length > 0.9)
                    {
                        if (choos_mat < 0.8) // diffuse
                            hitables.Add(new Sphere(center, 0.2,
                                new Lambertian(ConstantTexture.Create(sampler.NextDouble() * sampler.NextDouble(), sampler.NextDouble() * sampler.NextDouble(), sampler.NextDouble() * sampler.NextDouble()))));
                        else if (choos_mat < 0.9) // metal
                            hitables.Add(new Sphere(center, 0.2,
                                new Metal(ConstantTexture.Create(0.5 * (1 + sampler.NextDouble()), 0.5 * (1 + sampler.NextDouble()), 0.5 * (1 + sampler.NextDouble())), 0.5 * sampler.NextDouble())));
                        else // glass
                            hitables.Add(new Sphere(center, 0.2, new Dielectric(1.5)));
                    }
                }
            }
            hitables.Add(new Sphere(Vec3.Create(0, 1, 0), 1, new Dielectric(1.5)));
            hitables.Add(new Sphere(Vec3.Create(-4, 1, 0), 1, new Lambertian(ConstantTexture.Create(0.4, 0.2, 0.1))));
            hitables.Add(new Sphere(Vec3.Create(4, 1, 0), 1, new Metal(ConstantTexture.Create(0.7, 0.6, 0.5), 0)));
            World = new BVHNode(hitables.ToArray(), 0, 0);
            var lookFrom = Vec3.Create(12, 2, 3);
            var lookAt = Vec3.Create(0, 0, 0);
            double dist_to_focus = 10;
            double aderpture = 0.1;
            Camera = Camera.CreateLookAt(lookFrom, lookAt, Vec3.Create(0, 1, 0), 20, aspect, aderpture, dist_to_focus);
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