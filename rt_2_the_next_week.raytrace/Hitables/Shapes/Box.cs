using rt_2_the_next_week.raytrace.Interfaces;
using rt_2_the_next_week.raytrace.Mathematics;
using rt_2_the_next_week.raytrace.Hitables.Instancing;
using rt_2_the_next_week.raytrace.Hitables.Collections;

namespace rt_2_the_next_week.raytrace.Hitables.Shapes
{
    class Box
        : IHitable
    {
        Vec3 _pmin;
        Vec3 _pmax;
        IHitable _hitable;
        IMaterial _material;

        public Box(Vec3 pmin, Vec3 pmax, IMaterial material)
        {
            _pmin = pmin;
            _pmax = pmax;
            _material = material;

            IHitable[] hitables = {
                new XYRect(_pmin.X, _pmax.X, _pmin.Y, _pmax.Y, _pmax.Z, _material),
                new FlipNormals(new XYRect(_pmin.X, _pmax.X, _pmin.Y, _pmax.Y, _pmin.Z, _material)),
                new XZRect(_pmin.X, _pmax.X, _pmin.Z, _pmax.Z, _pmax.Y, _material),
                new FlipNormals(new XZRect(_pmin.X, _pmax.X, _pmin.Z, _pmax.Z, _pmin.Y, _material)),
                new YZRect(_pmin.Y, _pmax.Y, _pmin.Z, _pmax.Z, _pmax.X, _material),
                new FlipNormals(new YZRect(_pmin.Y, _pmax.Y, _pmin.Z, _pmax.Z, _pmin.X, _material))
            };
            _hitable = new HitableList(hitables);
        }

        public bool BoundingBox(double t0, double t1, out AABB box)
        {
            box = new AABB(_pmin, _pmax);
            return true;
        }

        public bool Hit(Ray r, double t_min, double t_max, out HitRecord rec)
        {
            return _hitable.Hit(r, t_min, t_max, out rec);
        }
    }
}
