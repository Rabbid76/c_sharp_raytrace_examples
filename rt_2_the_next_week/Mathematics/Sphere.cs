using System;
using System.Collections.Generic;
using System.Text;

namespace rt_2_the_next_week.Mathematics
{
    public class Sphere
        : IHitable
    {
        //Vec3 _center;
        double _cptx, _cpty, _cptz;
        double _radius;
        IMaterial _material;

        Vec3 Center { get => Vec3.Create(_cptx, _cpty, _cptz); }
        
        public Sphere(Vec3 center, double radius, IMaterial material)
        {
            (_cptx, _cpty, _cptz) = center.Components;
            _radius = radius;
            _material = material;
        }

        public bool BoundingBox(double t0, double t1, out AABB box)
        {
            box = new AABB(Center - Vec3.Create(_radius), Center + Vec3.Create(_radius));
            return true;
        }

        /// <summary>
        /// Ray - Sphere intersection
        /// 
        /// Sphere:         dot(p-C, p-C) = R*R            `C`: center, `p`: point on the sphere, `R`, radius 
        /// Ray:            p(t) = A + B * t               `A`: origin, `B`: direction        
        /// Intersection:   dot(A +B*t-C, A+B*t-C) = R*R
        /// t*t*dot(B,B) + 2*t*dot(B,A-C) + dot(A-C,A-C) - R*R = 0
        /// </summary>
        public bool Hit(Ray r, double t_min, double t_max, out HitRecord rec)
        {
            //var oc = r.Origin - _center;
            //var a = r.Direction.DotSelf();
            //var b = 2 * oc.Dot(r.Direction);
            //var c = oc.DotSelf() - _radius * _radius;

            (double ox, double oy, double oz) = r.Origin.Components;
            (double dx, double dy, double dz) = r.Direction.Components;
            (double ocx, double ocy, double ocz) = (ox - _cptx, oy - _cpty, oz - _cptz);
            var a = dx * dx + dy * dy + dz * dz;
            var b = 2 * (ocx * dx + ocy * dy + ocz * dz);
            var c = ocx * ocx + ocy * ocy + ocz * ocz - _radius * _radius;

            var discriminant = b * b - 4 * a * c;
            if (discriminant > 0)
            {
                double temp = (-b - Math.Sqrt(discriminant)) / (2 * a);
                bool hit = t_min < temp && temp < t_max;
                if (hit == false)
                {
                    temp = (-b + Math.Sqrt(discriminant)) / (2 * a);
                    hit = t_min < temp && temp < t_max;
                }
                if (hit)
                {
                    //var p = r.PointAt(temp);
                    //rec = new HitRecord(temp, p, (p - _center) / _radius, _material);
                    (double px, double py, double pz) = (ox + dx * temp, oy + dy * temp, oz + dz * temp);
                    rec = new HitRecord(temp, Vec3.Create(px, py, pz), Vec3.Create(px - _cptx, py - _cpty, pz - _cptz) / _radius, _material);
                    return true;
                }
            }
            rec = null;
            return false;
        }
    }
}
