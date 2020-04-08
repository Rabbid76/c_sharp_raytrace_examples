using rt_2_the_next_week.raytrace.Interfaces;
using rt_2_the_next_week.raytrace.Mathematics;
using rt_2_the_next_week.raytrace.Textures;
using rt_2_the_next_week.raytrace.Materials;
using rt_2_the_next_week.raytrace.Hitables.Collections;
using rt_2_the_next_week.raytrace.Hitables.Shapes;

namespace rt_2_the_next_week.raytrace.Scenes
{
    public class DefocusBlurScene
        : IScene
    {
        public Camera Camera { get; }
        public IHitable World { get; }

        public DefocusBlurScene(double aspect)
        {
            IHitable[] hitables = {
                            new Sphere(Vec3.Create(0, -100.5, -1), 100, new Lambertian(ConstantTexture.Create(0.8, 0.8, 0.0))),
                            new Sphere(Vec3.Create(0, 0, -1), 0.5, new Lambertian(ConstantTexture.Create(0.1, 0.2, 0.5))),
                            new Sphere(Vec3.Create(1, 0, -1), 0.5, new Metal(ConstantTexture.Create(0.8, 0.6, 0.2), 0.3)),
                            new Sphere(Vec3.Create(-1, 0, -1), 0.5, new Dielectric(1.5)),
                            new Sphere(Vec3.Create(-1, 0, -1), -0.45, new Dielectric(1.5))
                        };
            World = new BVHNode(hitables, 0, 1);
            var lookFrom = Vec3.Create(3, 3, 2);
            var lookAt = Vec3.Create(0, 0, -1);
            var dist_to_focus = (lookFrom - lookAt).Length;
            double aderpture = 2;
            Camera = Camera.CreateLookAt(lookFrom, lookAt, Vec3.Create(0, 1, 0), 25, aspect, aderpture, dist_to_focus);
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