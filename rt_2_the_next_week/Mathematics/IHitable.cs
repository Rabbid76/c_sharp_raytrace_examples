namespace rt_2_the_next_week.Mathematics
{
    public interface IHitable
    {
        bool BoundingBox(double t0, double t1, out AABB box);
        bool Hit(Ray r, double t_min, double t_max, out HitRecord rec);
    }
}
