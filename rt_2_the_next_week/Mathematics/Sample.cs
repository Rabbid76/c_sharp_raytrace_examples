using System;

namespace rt_2_the_next_week.Mathematics
{
    static public class Sample
    {
        static Random _sampler = new Random();

        /// <summary>
        /// Compute a point inside a unique sphere
        /// </summary>
        /// <returns></returns>
        public static Vec3 RandomInUnitSphere()
        {
            Func<double> rval = () => _sampler.NextDouble() * 2.0 - 1.0;
            Vec3 p;
            do
            {
                p = new Vec3(rval(), rval(), rval());
            }
            while (p.LengthSquare >= 1.0);
            return p;
        }
    }
}
