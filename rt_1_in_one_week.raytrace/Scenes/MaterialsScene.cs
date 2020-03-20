
using rt_1_in_one_week.raytrace.Mathematics;

namespace rt_1_in_one_week.raytrace.Scenes
{
    public class MaterialsScene
        : IScene
    {
        public Camera Camera { get; }
        public IHitableList World { get; }

        public MaterialsScene(double aspect)
        {
            IHitable[] hitables = {
                            new Sphere(Vec3.Create(0, -100.5, 0), 100, new Lambertian(Vec3.Create(0.8, 0.8, 0.0))),
                            new Sphere(Vec3.Create(0, 0, 0), 0.5, new Lambertian(Vec3.Create(0.1, 0.2, 0.5))),
                            new Sphere(Vec3.Create(1, 0, 0), 0.5, new Metal(Vec3.Create(0.8, 0.6, 0.2), 0.3)),
                            new Sphere(Vec3.Create(-1, 0, 0), 0.5, new Dielectric(1.5)),
                            new Sphere(Vec3.Create(-1, 0, 0), -0.45, new Dielectric(1.5))
                        };
            World = new HitableList(hitables);
            Camera = Camera.CreateLookAt(Vec3.Create(0.25, 0.5, 2.2), Vec3.Create(0.1, 0.0, 0), Vec3.Create(0, 1, 0), 45, aspect, 0, 1);
        }
    }
}
