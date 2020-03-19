namespace rt_1_in_one_week.raytrace.Mathematics
{
    public interface IHitable
    {
        bool Hit(Ray r, double t_min, double t_max, out HitRecord rec);
    }
}
