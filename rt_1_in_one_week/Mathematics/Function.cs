using System;

namespace rt_1_in_one_week.Mathematics
{
    public static class Function
    {
        public static double Schlick(double cosine, double ref_idx)
        {
            double r0 = (1 - ref_idx) / (1 + ref_idx);
            r0 = r0 * r0;
            return r0 + (1 - r0) * Math.Pow(1 - cosine, 5);
        }
    }
}