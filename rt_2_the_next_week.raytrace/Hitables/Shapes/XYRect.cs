using rt_2_the_next_week.raytrace.Interfaces;
using rt_2_the_next_week.raytrace.Mathematics;

namespace rt_2_the_next_week.raytrace.Hitables.Shapes
{
    class XYRect
        : IHitable
    {
        double _x0;
        double _y0;
        double _x1;
        double _y1;
        double _k;
        IMaterial _material;

        public XYRect(double x0, double x1, double y0, double y1, double k, IMaterial material)
        {
            _x0 = x0;
            _y0 = y0;
            _x1 = x1;
            _y1 = y1;
            _k = k;
            _material = material;
        }

        public bool BoundingBox(double t0, double t1, out AABB box)
        {
            box = new AABB(Vec3.Create(_x0, _y0, _k-0.0001), Vec3.Create(_x1, _y1, _k + 0.0001));
            return true;
        }

        public bool Hit(Ray r, double t_min, double t_max, out HitRecord rec)
        {
            rec = null;
            var t = (_k - r.Origin.Z) / r.Direction.Z;
            if (t < t_min || t > t_max)
                return false;

            var x = r.Origin.X + t * r.Direction.X;
            var y = r.Origin.Y + t * r.Direction.Y;
            if (x < _x0 || x > _x1 || y < _y0 || y > _y1)
                return false;

            var u = (x - _x0) / (_x1 - _x0);
            var v = (y - _y0) / (_y1 - _y0);
            var p = r.PointAt(t);
            rec = new HitRecord(t, u, v, p, Vec3.Create(0, 0, 1), _material);
            return true;
        }
    }
}
