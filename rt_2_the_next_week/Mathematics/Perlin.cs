using System;

namespace rt_2_the_next_week.Mathematics
{
    public class Perlin
        : INoise
    {
        static Random _sampler = new Random();

        private Vec3[] _rand;
        private int[] _perm_x;
        private int[] _perm_y;
        private int[] _perm_z;

        public Perlin()
        {
            _rand = generate();
            _perm_x = generate_perm();
            _perm_y = generate_perm();
            _perm_z = generate_perm();
        }

        public double Noise(Vec3 p)
        {
            double u = p.X - Math.Floor(p.X);
            double v = p.Y - Math.Floor(p.Y);
            double w = p.Z - Math.Floor(p.Z);
            int i = (int)Math.Floor(p.X);
            int j = (int)Math.Floor(p.Y);
            int k = (int)Math.Floor(p.Z);
            Vec3[,,] c = new Vec3[2, 2, 2];
            for (int di = 0; di < 2; ++di)
            {
                for (int dj = 0; dj < 2; ++dj)
                {
                    for (int dk = 0; dk < 2; ++dk)
                        c[di,dj,dk] = _rand[_perm_x[(i+di) & 255] ^ _perm_y[(j+dj) & 255] ^ _perm_z[(k+dk) & 255]];
                }
            }
            return trilinear_interp(c, u, v, w);
        }

        public double Turb(Vec3 p, int depth = 7)
        {
            double accum = 0;
            Vec3 temp_p = p.Clone();
            double weight = 1.0;
            for (int i = 0; i < depth; ++i)
            {
                accum += weight * Noise(temp_p);
                weight *= 0.5;
                temp_p *= 2;
            }
            return Math.Abs(accum); 
        }

        static private Vec3[] generate()
        {
            Vec3[] rand = new Vec3[256];
            for (int i = 0; i < 256; ++i)
                rand[i] = (Vec3.Create(_sampler.NextDouble(), _sampler.NextDouble(), _sampler.NextDouble()) * 2 - 1).Normalized();
            return rand;
        }

        static private void permute(int[] p)
        {
            for (int i = p.Length-1; i > 0; --i)
            {
                int target = (int)(_sampler.NextDouble() * (i + 1));
                (p[i], p[target]) = (p[target], p[i]); 
            }
        }

        static private int[] generate_perm()
        {
            int[] perm = new int[256];
            for (int i = 0; i < 256; ++i)
                perm[i] = i;
            permute(perm);
            return perm;
        }

        static private double trilinear_interp(Vec3[,,] c, double u, double v, double w)
        {
            double uu = u * u * (3 - 2 * u);
            double vv = v * v * (3 - 2 * v);
            double ww = w * w * (3 - 2 * w);
            double accum = 0;
            for (int i = 0; i < 2; ++i)
            {
                for (int j = 0; j < 2; ++j)
                {
                    for (int k = 0; k < 2; ++k)
                    {
                        var weight_v = Vec3.Create(u-i, v-j, w-k);
                        accum +=
                            ((i * uu) + (1 - i) * (1 - uu)) *
                            ((j * vv) + (1 - j) * (1 - vv)) *
                            ((k * ww) + (1 - k) * (1 - ww)) *
                            c[i, j, k].Dot(weight_v);
                    }
                }
            }
            return accum;
        }
    }
}
