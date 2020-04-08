using rt_2_the_next_week.raytrace.Interfaces;
using rt_2_the_next_week.raytrace.Mathematics;

namespace rt_2_the_next_week.raytrace.Hitables.Instancing
{
    class FlipNormals
        : IHitable
    {
        IHitable _source;

        public FlipNormals(IHitable source)
        {
            _source = source;
        }

        public bool BoundingBox(double t0, double t1, out AABB box)
        {
            return _source.BoundingBox(t0, t1, out box);
        }

        public bool Hit(Ray r, double t_min, double t_max, out HitRecord rec)
        {
            if (_source.Hit(r, t_min, t_max, out rec))
            {
                rec.InvertNormal();
                return true;
            }
            rec = null;
            return false;
        }
    }
}
