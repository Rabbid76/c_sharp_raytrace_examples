using rt_2_the_next_week.raytrace.Interfaces;
using rt_2_the_next_week.raytrace.Mathematics;

namespace rt_2_the_next_week.raytrace.Hitables.Instancing
{
    class Translate
        : IHitable
    {
        Vec3 _offset;
        IHitable _hitable;

        public Translate(IHitable hitable, Vec3 displacment)
        {
            _offset = displacment;
            _hitable = hitable;
        }

        public bool BoundingBox(double t0, double t1, out AABB box)
        {
            if (_hitable.BoundingBox(t0, t1, out box))
            {
                box = new AABB(box.Min + _offset, box.Max + _offset);
                return true; 
            }
            return false;
        }

        public bool Hit(Ray r, double t_min, double t_max, out HitRecord rec)
        {
            var ray_moved = new Ray(r.Origin - _offset, r.Direction, r.Time);
            if (_hitable.Hit(ray_moved, t_min, t_max, out rec))
            {
                rec.Displace(_offset);
                return true;
            }
            return false;
        }
    }
}
