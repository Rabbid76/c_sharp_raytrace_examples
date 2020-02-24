using System;
using System.Collections.Generic;
using System.Text;

namespace rt_1_in_one_week.Mathematics
{
    public class Sphere
        : IHitable
    {
        Vec3 _center;
        double _radius;
        IMaterial _material;

        public Sphere(Vec3 center, double radius, IMaterial material)
        {
            _center = center;
            _radius = radius;
            _material = material;
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
            var oc = r.Origin - _center;
            var a = r.Direction.DotSelf();
            var b = 2.0 * oc.Dot(r.Direction);
            var c = oc.DotSelf() - _radius * _radius;
            var discriminant = b * b - 4 * a * c;
            if (discriminant > 0)
            {
                double temp = (-b - Math.Sqrt(discriminant)) / (2 * a);
                if (t_min < temp && temp < t_max)
                {
                    var p = r.PointAt(temp);
                    rec = new HitRecord(temp, p, (p - _center) / _radius, _material);
                    return true;
                }
                temp = (-b + Math.Sqrt(discriminant)) / (2 * a);
                if (t_min < temp && temp < t_max)
                {
                    var p = r.PointAt(temp);
                    rec = new HitRecord(temp, p, (p - _center) / _radius, _material);
                    return true;
                }
            }
            rec = null;
            return false;
        }
    }
}
