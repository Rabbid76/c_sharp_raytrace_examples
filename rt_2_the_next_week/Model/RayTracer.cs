using System;
using rt_2_the_next_week.Mathematics;
using rt_2_the_next_week.Scenes;
using ray_tracing_modules.Color;
using ray_tracing_modules.RayTraycer.Interfaces;

namespace rt_2_the_next_week
{
    public class RayTracer
        : IRayTracer
    {
        private IScene scene;

        public RayTracer(IScene scene)
        {
            this.scene = scene;
        }

        public RGBColor RaytraceColor(double u, double v)
        {
            var colorVector = RaytraceColor(scene.Camera.Get(u, v), scene.World, 0);
            return new RGBColor(colorVector.X, colorVector.Y, colorVector.Z);
        }

        private static Vec3 RaytraceColor(Ray r, IHitable hitable, int depth)
        {
            HitRecord rec;
            if (hitable.Hit(r, 0.001, Double.MaxValue, out rec))
            {
                Ray scattered;
                Vec3 attenuation;
                if (depth < 50 && rec.Material.Scatter(r, rec, out attenuation, out scattered))
                {
                    return attenuation * RaytraceColor(scattered, hitable, depth + 1);
                }
                return Vec3.Create(0.0);
            }
            return CreateSky(r);
        }

        private static Vec3 CreateSky(Ray r)
        {
            var unit_direction = Vec3.Normalize(r.Direction);
            var t = unit_direction.Y * 0.5 + 0.5;
            var v = new Vec3(1.0) * (1.0 - t) + new Vec3(0.5, 0.7, 1.0) * t;
            return v;
        }
    }
}
