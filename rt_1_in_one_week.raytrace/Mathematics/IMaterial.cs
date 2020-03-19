namespace rt_1_in_one_week.raytrace.Mathematics
{
    public interface IMaterial
    {
        bool Scatter(Ray r_in, HitRecord rec, out Vec3 attenuation, out Ray scattered);
    }
}
