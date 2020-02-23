using System.Collections.Generic;

namespace rt_1_in_one_week.Mathematics
{
    public class HitableList
        : IHitableList
    {
        List<IHitable> _list;

        public HitableList(IHitable[] hitables)
        {
            _list = new List<IHitable>(hitables);
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
