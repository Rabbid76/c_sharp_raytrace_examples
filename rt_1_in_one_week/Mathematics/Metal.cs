namespace rt_1_in_one_week.Mathematics
{
    public class Metal
        : IMaterial
    {
        private Vec3 _albedo;
        private double _fuzz;

        public Metal(Vec3 albedo, double fuzz)
        {
            _albedo = albedo;
            _fuzz = fuzz;
        }

        public bool Scatter(Ray r_in, HitRecord rec, out Vec3 attenuation, out Ray scattered)
        {
            Vec3 redflected = Vec3.Reflect(r_in.Direction.Normalized(), rec.Normal);
            scattered = new Ray(rec.P, redflected + Sample.RandomInUnitSphere() * _fuzz);
            attenuation = _albedo;
            return scattered.Direction.Dot(rec.Normal) > 0.0;
        }
    }
}