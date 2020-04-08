using rt_2_the_next_week.raytrace.Interfaces;
using rt_2_the_next_week.raytrace.Mathematics;
using rt_2_the_next_week.raytrace.Textures;

namespace rt_2_the_next_week.raytrace.Materials
{
    public  class Lambertian
        : Material
    {
        private ITexture _albedo;

        public Lambertian(ITexture albedo)
        {
            _albedo = albedo != null ? albedo : new ConstantTexture(Vec3.Create(1));
        }

        public override bool Scatter(Ray r_in, HitRecord rec, out Vec3 attenuation, out Ray scattered)
        {
            Vec3 target = rec.P + rec.Normal + Sample.RandomInUnitSphere();
            scattered = new Ray(rec.P, target - rec.P, r_in.Time);
            attenuation = _albedo.Value(rec.U, rec.V, rec.P);
            return true;
        }
    }
}
