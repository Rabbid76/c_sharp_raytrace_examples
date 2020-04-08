using rt_2_the_next_week.raytrace.Interfaces;
using rt_2_the_next_week.raytrace.Mathematics;

namespace rt_2_the_next_week.raytrace.Hitables.Shapes
{
    class XZRect
        : IHitable
    {
        double _x0;
        double _z0;
        double _x1;
        double _z1;
        double _k;
        IMaterial _material;

        public XZRect(double x0, double x1, double z0, double z1, double k, IMaterial material)
        {
            _x0 = x0;
            _z0 = z0;
            _x1 = x1;
            _z1 = z1;
            _k = k;
            _material = material;
        }

        public bool BoundingBox(double t0, double t1, out AABB box)
        {
            box = new AABB(Vec3.Create(_x0, _k - 0.0001, _z0), Vec3.Create(_x1, _k + 0.0001, _z1));
            return true;
        }

        public bool Hit(Ray r, double t_min, double t_max, out HitRecord rec)
        {
            rec = null;
            var t = (_k - r.Origin.Y) / r.Direction.Y;
            if (t < t_min || t > t_max)
                return false;

            var x = r.Origin.X + t * r.Direction.X;
            var z = r.Origin.Z + t * r.Direction.Z;
            if (x < _x0 || x > _x1 || z < _z0 || z > _z1)
                return false;

            var u = (x - _x0) / (_x1 - _x0);
            var v = (z - _z0) / (_z1 - _z0);
            var p = r.PointAt(t);
            rec = new HitRecord(t, u, v, p, Vec3.Create(0, 1, 0), _material);
            return true;
        }
    }
}

