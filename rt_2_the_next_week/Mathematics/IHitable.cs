namespace rt_2_the_next_week.Mathematics
{
    public interface IHitable
    {
        bool Hit(Ray r, double t_min, double t_max, out HitRecord rec);
    }
}
