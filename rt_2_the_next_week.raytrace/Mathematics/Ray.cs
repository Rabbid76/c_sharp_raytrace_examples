namespace rt_2_the_next_week.raytrace.Mathematics
{
    public class Ray
    {
        private Vec3 _origin;
        private Vec3 _direction;
        private double _time;

        public Vec3 Origin { get => _origin; }
        public Vec3 Direction { get => _direction; }
        public double Time { get => _time; }

        public Ray(Vec3 origin, Vec3 direction, double time)
        {
            _origin = origin;
            _direction = direction;
            _time = time;
        }

        public Vec3 PointAt(double t) => _origin + _direction * t; 
    }
}
