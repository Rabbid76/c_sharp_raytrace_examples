namespace rt_1_in_one_week.raytrace.Mathematics
{
    public class Ray
    {
        private Vec3 _origin;
        private Vec3 _direction;

        public Vec3 Origin { get => _origin; }
        public Vec3 Direction { get => _direction; }

        public Ray(Vec3 origin, Vec3 direction)
        {
            _origin = origin;
            _direction = direction;
        }

        public Vec3 PointAt(double t) => _origin + _direction * t; 
    }
}
