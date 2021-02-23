using System;
using System.Collections.Generic;
using rt_2_the_next_week.raytrace.Interfaces;
using rt_2_the_next_week.raytrace.Mathematics;

namespace rt_2_the_next_week.raytrace.Hitables.Collections
{
    /// <summary>
    /// Bonding Volume Hierarchy Node
    /// </summary>
    public class BVHNode
        : IHitable
    {
        static Random _sampler = new Random();

        IHitable _left;
        IHitable _right;
        AABB _box;

        public BVHNode(IHitable[] hitables, double time0, double time1)
        {
            if (hitables.Length == 1)
            {
                _left = hitables[0];
                _right = null;
                _left.BoundingBox(time0, time1, out _box);
                return;
            }
            
            int axis = _sampler.Next(0, 3);
            List<IHitable> list = new List<IHitable>(hitables);
            list.Sort((IHitable a, IHitable b) =>
            {
                AABB sort_box_left, sort_box_right;
                if (!a.BoundingBox(time0, time1, out sort_box_left) || !b.BoundingBox(time0, time1, out sort_box_right))
                {
                    throw new InvalidOperationException("error computing AABB");
                }
                return sort_box_left.Min[axis] < sort_box_right.Min[axis] ? -1 : 1;
            });

            int half = list.Count - list.Count / 2;
            _left = half == 1 ? list[0] : new BVHNode(list.GetRange(0, half).ToArray(), time0, time1);
            _right = half == list.Count-1 ? list[list.Count-1] : new BVHNode(list.GetRange(half, list.Count-half).ToArray(), time0, time1);
           
            AABB box_left, box_right;
            if (!_left.BoundingBox(time0, time1, out box_left) || !_right.BoundingBox(time0, time1, out box_right))
            {
                throw new InvalidOperationException("error computing AABB");
            }
            _box = box_left | box_right;
        }



        public bool BoundingBox(double t0, double t1, out AABB box)
        {
            box = _box;
            return true;
        }

        public bool Hit(Ray r, double t_min, double t_max, out HitRecord rec)
        {
            rec = null;
            if (!_box.Hit(r, t_min, t_max))
                return false;

            HitRecord left_rec, right_rec;
            bool hit_left = _left.Hit(r, t_min, t_max, out left_rec);
            if (_right == null)
            {
                rec = left_rec;
                return hit_left;
            }

            bool hit_right = _right.Hit(r, t_min, t_max, out right_rec);
            if (hit_left && hit_right)
            {
                rec = left_rec.T < right_rec.T ? left_rec : right_rec;
                return true;
            }
            else if (hit_left)
            {
                rec = left_rec;
                return true;
            }
            else if (hit_right)
            {
                rec = right_rec;
                return true;
            }
            return false;
        }
    }
}
