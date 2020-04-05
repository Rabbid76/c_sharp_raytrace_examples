namespace rt_2_the_next_week.Mathematics
{
    public class Metal
        : IMaterial
    {
        private ITexture _albedo;
        private double _fuzz;

        public Metal(ITexture albedo, double fuzz)
        {
            _albedo = albedo != null ? albedo : new ConstantTexture(Vec3.Create(1));
            _fuzz = fuzz;
        }

        public bool Scatter(Ray r_in, HitRecord rec, out Vec3 attenuation, out Ray scattered)
        {
            Vec3 redflected = Vec3.Reflect(r_in.Direction.Normalized(), rec.Normal);
            scattered = new Ray(rec.P, redflected + Sample.RandomInUnitSphere() * _fuzz, r_in.Time);
            attenuation = _albedo.Value(rec.U, rec.V, rec.P);
            return scattered.Direction.Dot(rec.Normal) > 0.0;
        }
    }
}