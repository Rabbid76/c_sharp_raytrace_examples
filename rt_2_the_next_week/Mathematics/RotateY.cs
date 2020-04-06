using System;

namespace rt_2_the_next_week.Mathematics
{
    class RotateY
        : IHitable
    {
        double _sin_theta;
        double _cos_theta;
        IHitable _hitable;
        bool _has_box;
        AABB _aabb;

        public RotateY(IHitable hitable, double angle)
        {
            _hitable = hitable;
            AABB box;
            _has_box = _hitable.BoundingBox(0, 1, out box);
            var radians = angle * Math.PI / 180;
            _sin_theta = Math.Sin(radians);
            _cos_theta = Math.Cos(radians);
            var rmin = Rotate(box.Min);
            _aabb = new AABB(rmin, rmin);
            _aabb = _aabb | Rotate(box.Max);
            _aabb = _aabb | Rotate(Vec3.Create(box.Min.X, box.Min.Y, box.Max.Z));
            _aabb = _aabb | Rotate(Vec3.Create(box.Max.X, box.Max.Y, box.Min.Z));
        }

        public bool BoundingBox(double t0, double t1, out AABB box)
        {
            box = _aabb;
            return _has_box;
        }

        public bool Hit(Ray r, double t_min, double t_max, out HitRecord rec)
        {
            var origin = RotateInverse(r.Origin);
            var direction = RotateInverse(r.Direction);
            var ray_rotated = new Ray(origin, direction, r.Time);
            if (_hitable.Hit(ray_rotated, t_min, t_max, out rec))
            {
                var p = Rotate(rec.P);
                var n = Rotate(rec.Normal);
                rec = new HitRecord(rec.T, rec.U, rec.V, p, n, rec.Material);
                return true;
            }
            return false;
        }

        private Vec3 Rotate(Vec3 v)
        {
            var x = _cos_theta * v.X + _sin_theta * v.Z;
            var z = -_sin_theta * v.X + _cos_theta * v.Z;
            return Vec3.Create(x, v.Y, z); 
        }

        private Vec3 RotateInverse(Vec3 v)
        {
            var x = _cos_theta * v.X - _sin_theta * v.Z;
            var z = _sin_theta * v.X + _cos_theta * v.Z;
            return Vec3.Create(x, v.Y, z);
        }
    }
}
