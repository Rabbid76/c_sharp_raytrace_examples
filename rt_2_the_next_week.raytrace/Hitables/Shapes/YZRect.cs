using rt_2_the_next_week.raytrace.Interfaces;
using rt_2_the_next_week.raytrace.Mathematics;

namespace rt_2_the_next_week.raytrace.Hitables.Shapes
{
    class YZRect
        : IHitable
    {
        double _y0;
        double _z0;
        double _y1;
        double _z1;
        double _k;
        IMaterial _material;

        public YZRect(double y0, double y1, double z0, double z1, double k, IMaterial material)
        {
            _y0 = y0;
            _z0 = z0;
            _y1 = y1;
            _z1 = z1;
            _k = k;
            _material = material;
        }

        public bool BoundingBox(double t0, double t1, out AABB box)
        {
            box = new AABB(Vec3.Create(_k - 0.0001, _y0, _z0), Vec3.Create(_k + 0.0001, _y1, _z1));
            return true;
        }

        public bool Hit(Ray r, double t_min, double t_max, out HitRecord rec)
        {
            rec = null;
            var t = (_k - r.Origin.X) / r.Direction.X;
            if (t < t_min || t > t_max)
                return false;

            var y = r.Origin.Y + t * r.Direction.Y;
            var z = r.Origin.Z + t * r.Direction.Z;
            if (y < _y0 || y > _y1 || z < _z0 || z > _z1)
                return false;

            var u = (y - _y0) / (_y1 - _y0);
            var v = (z - _z0) / (_z1 - _z0);
            var p = r.PointAt(t);
            rec = new HitRecord(t, u, v, p, Vec3.Create(1, 0, 0), _material);
            return true;
        }
    }
}