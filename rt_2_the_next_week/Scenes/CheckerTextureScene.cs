using rt_2_the_next_week.Mathematics;

namespace rt_2_the_next_week.Scenes
{
    public class CheckerTextureScene
        : IScene
    {
        public Camera Camera { get; }
        public IHitable World { get; }

        public CheckerTextureScene(double aspect)
        {
            var checker_texture = CheckerTexture.Create(0.2, 0.3, 0.2, 0.9, 0.9, 0.9);
            IHitable[] hitables = {
                            new Sphere(Vec3.Create(0, -10, 0), 10, new Lambertian(checker_texture)),
                            new Sphere(Vec3.Create(0, 10, 0), 10, new Lambertian(checker_texture))
                        };
            World = new BVHNode(hitables, 0, 1);
            var lookFrom = Vec3.Create(13, 2, 3);
            var lookAt = Vec3.Create(0, 0, 0);
            double dist_to_focus = 10;
            double aderpture = 0;
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