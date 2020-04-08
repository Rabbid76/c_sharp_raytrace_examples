
namespace rt_2_the_next_week.raytrace.Mathematics
{
    /// <summary>
    /// [Axis aligned bounding box](https://en.wikipedia.org/wiki/Bounding_volume)
    /// </summary>
    public class AABB
    {
        Vec3 _min;
        Vec3 _max;

        public Vec3 Min { get => _min; }
        public Vec3 Max { get => _max; }

        public AABB(Vec3 a, Vec3 b)
        {
            _min = Vec3.Create(ffmin(a.X, b.X), ffmin(a.Y, b.Y), ffmin(a.Z, b.Z));
            _max = Vec3.Create(ffmax(a.X, b.X), ffmax(a.Y, b.Y), ffmax(a.Z, b.Z));
        }

        static double ffmin(double a, double b) { return a < b ? a : b; }
        static double ffmax(double a, double b) { return a > b ? a : b; }

        public static AABB operator |(AABB a, AABB b)
        {
            Vec3 small = Vec3.Create(ffmin(a.Min.X, b.Min.X), ffmin(a.Min.Y, b.Min.Y), ffmin(a.Min.Z, b.Min.Z));
            Vec3 big = Vec3.Create(ffmax(a.Max.X, b.Max.X), ffmax(a.Max.Y, b.Max.Y), ffmax(a.Max.Z, b.Max.Z));
            return new AABB(small, big);
        }

        public static AABB operator |(AABB a, Vec3 b)
        {
            Vec3 small = Vec3.Create(ffmin(a.Min.X, b.X), ffmin(a.Min.Y, b.Y), ffmin(a.Min.Z, b.Z));
            Vec3 big = Vec3.Create(ffmax(a.Max.X, b.X), ffmax(a.Max.Y, b.Y), ffmax(a.Max.Z, b.Z));
            return new AABB(small, big);
        }

        public bool Hit(Ray r, double t_min, double t_max)
        {
            for (int a = 0; a < 3; ++ a)
            {
                //double t0 = ffmin((_min[a] - r.Origin[a]) / r.Direction[a], (_max[a] - r.Origin[a]) / r.Direction[a]);
                //double t1 = ffmax((_min[a] - r.Origin[a]) / r.Direction[a], (_max[a] - r.Origin[a]) / r.Direction[a]);
                //double tmin = ffmax(t0, t_min);
                //double tmax = ffmin(t1, t_max);
             
                double invD = 1 / r.Direction[a];
                double t0 = (_min[a] - r.Origin[a]) * invD;
                double t1 = (_max[a] - r.Origin[a]) * invD;
                if (invD < 0)
                    (t0, t1) = (t1, t0);
                double tmin = t0 > t_min ? t0 : t_min;
                double tmax = t1 < t_max ? t1 : t_max;
             
                if (tmax <= tmin)
                    return false;
            }
            return true;
        }
    }
}
