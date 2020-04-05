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

        private Vec3 RaytraceColor(Ray r, IHitable hitable, int depth)
        {
            HitRecord rec;
            if (hitable.Hit(r, 0.001, Double.MaxValue, out rec))
            {
                Ray scattered;
                Vec3 attenuation;
                var emitted = rec.Material.Emitted(rec.U, rec.V, rec.P);
                if (depth < 50 && rec.Material.Scatter(r, rec, out attenuation, out scattered))
                {
                    return emitted + attenuation * RaytraceColor(scattered, hitable, depth + 1);
                }
                return emitted;
            }
            return scene.Sky(r);
        }
    }
}
