using System;

namespace rt_2_the_next_week.Mathematics
{
    class NoiseTexture
        : ITexture
    {
        public enum Type { DEFAULT, TURB, SIN_Z }

        private INoise _noise;
        private double _scale;
        private Type _type;

        public NoiseTexture(double scale, Type type)
        {
            _noise = new Perlin();
            _scale = scale;
            _type = type;
        }

        public static ITexture Create(double scale, Type type = Type.DEFAULT) => new NoiseTexture(scale, type);

        public Vec3 Value(double u, double v, Vec3 p)
        {
            double noise = 0.5;
            switch (_type)
            {
                default:
                case Type.DEFAULT: noise = _noise.Noise(p * _scale); break;
                case Type.TURB: noise = _noise.Turb(p * _scale); break;
                case Type.SIN_Z: noise = Math.Sin(_scale * p.Z + 10 * _noise.Turb(p * _scale)); break;
            }
            return Vec3.Create(1.0) * (noise * 0.5 + 0.5);
        }
    }
}
