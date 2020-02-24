namespace rt_1_in_one_week.Mathematics
{
    public  class Lambertian
        : IMaterial
    {
        private Vec3 _albedo;

        public Lambertian(Vec3 albedo)
        {
            _albedo = albedo;
        }

        public bool Scatter(Ray r_in, HitRecord rec, out Vec3 attenuation, out Ray scattered)
        {
            Vec3 target = rec.P + rec.Normal + Sample.RandomInUnitSphere();
            scattered = new Ray(rec.P, target - rec.P);
            attenuation = _albedo;
            return true;
        }
    }
}
