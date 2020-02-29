namespace rt_2_the_next_week.Mathematics
{
    public  class Lambertian
        : IMaterial
    {
        private ITexture _albedo;

        public Lambertian(ITexture albedo)
        {
            _albedo = albedo != null ? albedo : new ConstantTexture(Vec3.Create(1));
        }

        public bool Scatter(Ray r_in, HitRecord rec, out Vec3 attenuation, out Ray scattered)
        {
            Vec3 target = rec.P + rec.Normal + Sample.RandomInUnitSphere();
            scattered = new Ray(rec.P, target - rec.P, r_in.Time);
            attenuation = _albedo.Value(0, 0, rec.P);
            return true;
        }
    }
}
