namespace rt_2_the_next_week.Mathematics
{
    public interface IMaterial
    {
        bool Scatter(Ray r_in, HitRecord rec, out Vec3 attenuation, out Ray scattered);

        Vec3 Emitted(double u, double v, Vec3 p);
    }
}
