using rt_2_the_next_week.raytrace.Interfaces;
using rt_2_the_next_week.raytrace.Mathematics;
using rt_2_the_next_week.raytrace.Textures;
using rt_2_the_next_week.raytrace.Materials;
using rt_2_the_next_week.raytrace.Hitables.Collections;
using rt_2_the_next_week.raytrace.Hitables.Shapes;

namespace rt_2_the_next_week.raytrace.Scenes
{
    public class SimpleLightScene
        : IScene
    {
        public Camera Camera { get; }
        public IHitable World { get; }

        public SimpleLightScene(double aspect)
        {
            var perlin_texture = NoiseTexture.Create(4, NoiseTexture.Type.SIN_Z);
            IHitable[] hitables = {
                            new Sphere(Vec3.Create(0, -1000, 0), 1000, new Lambertian(perlin_texture)),
                            new Sphere(Vec3.Create(0, 2, 0), 2, new Lambertian(perlin_texture)),
                            new Sphere(Vec3.Create(0, 7, 0), 2, new DiffuseLight(ConstantTexture.Create(4, 4, 4))),
                            new XYRect(3, 5, 1, 3, -2, new DiffuseLight(ConstantTexture.Create(4, 4, 4)))
                        };
            World = new BVHNode(hitables, 0, 1);
            var lookFrom = Vec3.Create(25, 4, 5);
            var lookAt = Vec3.Create(0, 2, 0);
            double dist_to_focus = 10;
            double aderpture = 0;
            Camera = Camera.CreateLookAt(lookFrom, lookAt, Vec3.Create(0, 1, 0), 20, aspect, aderpture, dist_to_focus);
        }

        public Vec3 Sky(Ray r) => Vec3.Create(0);
    }
}