namespace rt_2_the_next_week.Mathematics
{
    public abstract class Material
        : IMaterial
    {
        public abstract bool Scatter(Ray r_in, HitRecord rec, out Vec3 attenuation, out Ray scattered);

        public Vec3 Emitted(double u, double v, Vec3 p) => Vec3.Create(0);
    }
}
