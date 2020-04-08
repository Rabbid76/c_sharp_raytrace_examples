using System.Collections.Generic;
using rt_2_the_next_week.raytrace.Interfaces;
using rt_2_the_next_week.raytrace.Mathematics;

namespace rt_2_the_next_week.raytrace.Hitables.Collections
{
    public class HitableList
        : IHitableList
    {
        List<IHitable> _list;

        public HitableList(IHitable[] hitables)
        {
            _list = new List<IHitable>(hitables);
        }

        public bool BoundingBox(double t0, double t1, out AABB box)
        {
            if (_list.Count <= 0)
            {
                box = null;
                return false;
            }

            if (!_list[0].BoundingBox(t0, t1, out box))
                return false;
            
            if (_list.Count == 1)
                return true;
            for (int i = 1; i < _list.Count; ++i)
            {
                AABB tempbox;
                if (!_list[i].BoundingBox(t0, t1, out tempbox))
                    return false;
                box |= tempbox;
            }
            return true;
        }

        public bool Hit(Ray r, double t_min, double t_max, out HitRecord rec)
        {
            rec = null;
            HitRecord temp_rec;
            bool hit_anything = false;
            double closest_so_far = t_max;
            foreach (var hitable in _list)
            { 
                if (hitable.Hit(r, t_min, closest_so_far, out temp_rec))
                {
                    hit_anything = true;
                    closest_so_far = temp_rec.T;
                    rec = temp_rec;
                }
            }
            return hit_anything;
        }
    }
}
