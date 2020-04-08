using System;
using rt_2_the_next_week.raytrace.Interfaces;
using rt_2_the_next_week.raytrace.Mathematics;

namespace rt_2_the_next_week.raytrace.Textures
{
    public class CheckerTexture
        : ITexture
    {
        private ITexture _even;
        private ITexture _odd;

        public ITexture Even { get => _even; }
        public ITexture Odd { get => _odd; }

        public CheckerTexture(ITexture even, ITexture odd)
        {
            _even = even;
            _odd = odd;
        }

        static public CheckerTexture Create(double r0, double g0, double b0, double r1, double g1, double b1) => 
            new CheckerTexture(ConstantTexture.Create(r0, g0, b0), ConstantTexture.Create(r1, g1, b1));

        public Vec3 Value(double u, double v, Vec3 p)
        {
            double sines = Math.Sin(10 * p.X) * Math.Sin(10 * p.Y) * Math.Sin(10 * p.Z);
            return sines < 0 ? Odd.Value(0, 0, p) : Even.Value(0, 0, p);
        }
    }
}
