using rt_1_in_one_week.raytrace.Mathematics;

namespace rt_1_in_one_week.raytrace.Scenes
{
    public class DefocusBlurScene
        : IScene
    {
        public Camera Camera { get; }
        public IHitableList World { get; }

        public DefocusBlurScene(double aspect)
        {
            IHitable[] hitables = {
                            new Sphere(Vec3.Create(0, -100.5, -1), 100, new Lambertian(Vec3.Create(0.8, 0.8, 0.0))),
                            new Sphere(Vec3.Create(0, 0, -1), 0.5, new Lambertian(Vec3.Create(0.1, 0.2, 0.5))),
                            new Sphere(Vec3.Create(1, 0, -1), 0.5, new Metal(Vec3.Create(0.8, 0.6, 0.2), 0.3)),
                            new Sphere(Vec3.Create(-1, 0, -1), 0.5, new Dielectric(1.5)),
                            new Sphere(Vec3.Create(-1, 0, -1), -0.45, new Dielectric(1.5))
                        };
            World = new HitableList(hitables);
            var lookFrom = Vec3.Create(3, 3, 2);
            var lookAt = Vec3.Create(0, 0, -1);
            var dist_to_focus = (lookFrom - lookAt).Length;
            double aderpture = 2;
            Camera = Camera.CreateLookAt(lookFrom, lookAt, Vec3.Create(0, 1, 0), 25, aspect, aderpture, dist_to_focus);
        }
    }
}
