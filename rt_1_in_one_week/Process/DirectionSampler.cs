using System;
using rt_1_in_one_week.Mathematics;

namespace rt_1_in_one_week.Process
{
    public class DirectionSampler
        : ISamapler
    {
        private int _samples = 1;
        private int _current_sample = 0;
        private Random _sampler = new Random();

        public DirectionSampler(int smaples)
        {
            _samples = smaples;
        }

        public bool Next(out Vec3 direction)
        {
            Func<double> rval = () => _sampler.NextDouble() * 2.0 - 1.0;
            direction = new Vec3(rval(), rval(), rval());

            if (_current_sample >= _samples)
                return false;
            _samples ++;
            return true;
        }
    }
}
