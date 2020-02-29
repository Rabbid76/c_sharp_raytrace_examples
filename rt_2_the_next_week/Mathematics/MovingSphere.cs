using System;

namespace rt_2_the_next_week.Mathematics
{
    class MovingSphere
        : IHitable
    {
        Vec3 _center0;
        Vec3 _center1;
        double _time0;
        double _time1;
        double _radius;
        IMaterial _material;

        Vec3 Center(double time) => _center0 + (_center1 - _center0) * (time - _time0) / (_time1 - _time0);

        public MovingSphere(Vec3 cpt0, Vec3 cpt1, double t0, double t1, double radius, IMaterial material)
        {
            (_center0, _center1) = (cpt0, cpt1);
            (_time0, _time1) = (t0, t1);
            _radius = radius;
            _material = material;
        }

        public bool BoundingBox(double t0, double t1, out AABB box)
        {
            var cpt0 = Center(t0);
            var cpt1 = Center(t1);
            var vrad = Vec3.Create(_radius);
            var boxt0 = new AABB(cpt0 - vrad, cpt0 + vrad);
            var boxt1 = new AABB(cpt1 - vrad, cpt1 + vrad);
            box = boxt0 | boxt1;
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
            var center = Center(r.Time);
            //var oc = r.Origin - center;
            //var a = r.Direction.DotSelf();
            //var b = 2 * oc.Dot(r.Direction);
            //var c = oc.DotSelf() - _radius * _radius;

            (double cptx, double cpty, double cptz) = center.Components;
            (double ox, double oy, double oz) = r.Origin.Components;
            (double dx, double dy, double dz) = r.Direction.Components;
            (double ocx, double ocy, double ocz) = (ox - cptx, oy - cpty, oz - cptz);
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
                    //rec = new HitRecord(temp, p, (p - center) / _radius, _material);
                    (double px, double py, double pz) = (ox + dx * temp, oy + dy * temp, oz + dz * temp);
                    rec = new HitRecord(temp, Vec3.Create(px, py, pz), Vec3.Create(px - cptx, py - cpty, pz - cptz) / _radius, _material);
                    return true;
                }
            }
            rec = null;
            return false;
        }
    }
}
