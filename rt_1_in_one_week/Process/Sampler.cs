using System;
using rt_1_in_one_week.Mathematics;

namespace rt_1_in_one_week.Process
{
    public class Sampler
        : ISamapler
    {
        private int _samples = 1;
        private int _current_sample = 0;
        private Random _sampler = new Random();

        public Sampler(int smaples)
        {
            _samples = smaples;
        }

        public bool Next()
        {
            if (_current_sample >= _samples)
                return false;
            _samples ++;
            return true;
        }
    }
}
