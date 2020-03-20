using System;
using rt_1_in_one_week.raytrace.Mathematics;
using rt_1_in_one_week.raytrace.Scenes;
using ray_tracing_modules.Color;
using ray_tracing_modules.RayTraycer.Interfaces;

namespace rt_1_in_one_week.raytrace.RayTracer
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

        private Vec3 RaytraceColor(Ray r, IHitable hitable, int depth)
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
        private Vec3 CreateSky(Ray r)
        {
            var unit_direction = Vec3.Normalize(r.Direction);
            var t = unit_direction.Y * 0.5 + 0.5;
            var v = new Vec3(1.0) * (1.0 - t) + new Vec3(0.5, 0.7, 1.0) * t;
            return v;
        }
    }
}
