using System;
using System.Collections.Generic;
using rt_1_in_one_week.raytrace.Mathematics;

namespace rt_1_in_one_week.raytrace.Scenes
{
    public class CoverScene
        : IScene
    {
        public Camera Camera { get; }
        public IHitableList World { get; }
        
        public CoverScene(double aspect)
        {
            var sampler = new Random();
            List<IHitable> hitables = new List<IHitable>();
            hitables.Add(new Sphere(Vec3.Create(0, -1000, 0), 1000, new Lambertian(Vec3.Create(0.5, 0.5, 0.5))));
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
                                new Lambertian(Vec3.Create(sampler.NextDouble() * sampler.NextDouble(), sampler.NextDouble() * sampler.NextDouble(), sampler.NextDouble() * sampler.NextDouble()))));
                        else if (choos_mat < 0.9) // metal
                            hitables.Add(new Sphere(center, 0.2,
                                new Metal(Vec3.Create(0.5 * (1 + sampler.NextDouble()), 0.5 * (1 + sampler.NextDouble()), 0.5 * (1 + sampler.NextDouble())), 0.5 * sampler.NextDouble())));
                        else // glass
                            hitables.Add(new Sphere(center, 0.2, new Dielectric(1.5)));
                    }
                }
            }
            hitables.Add(new Sphere(Vec3.Create(0, 1, 0), 1, new Dielectric(1.5)));
            hitables.Add(new Sphere(Vec3.Create(-4, 1, 0), 1, new Lambertian(Vec3.Create(0.4, 0.2, 0.1))));
            hitables.Add(new Sphere(Vec3.Create(4, 1, 0), 1, new Metal(Vec3.Create(0.7, 0.6, 0.5), 0)));
            World = new HitableList(hitables.ToArray());
            var lookFrom = Vec3.Create(12, 2, 3);
            var lookAt = Vec3.Create(0, 0, 0);
            double dist_to_focus = 10;
            double aderpture = 0.1;
            Camera = Mathematics.Camera.CreateLookAt(lookFrom, lookAt, Vec3.Create(0, 1, 0), 20, aspect, aderpture, dist_to_focus);
        }
    }
}
