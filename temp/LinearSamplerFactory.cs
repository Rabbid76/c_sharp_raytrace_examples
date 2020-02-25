using System;

namespace rt_1_in_one_week.Process
{
    public class LinearSamplerFactory
        : ISamplerFactory
    {
        int _min_samples = 100;
        int _max_samples = 100;
        int _depth_reduction = 0;
        bool _depth0_sample1 = false;

        public LinearSamplerFactory(int mins, int maxs, int reduction, bool depth0_sample1)
        {
            _min_samples = mins;
            _max_samples = maxs;
            _depth_reduction = reduction;
        }

        public ISamapler Create(int depth, int x, int y)
        {
            int depth_samples =  _depth0_sample1 && depth == 0 ?
                1 : 
                Math.Max(_min_samples, _max_samples - depth * _depth_reduction);
            var sampler = new Sampler(depth_samples);
            return sampler;
        }
    }
}
