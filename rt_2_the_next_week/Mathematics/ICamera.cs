using System;
using System.Collections.Generic;
using System.Text;

namespace rt_2_the_next_week.Mathematics
{
    public interface ICamera
    {
        Ray Get(double u, double v);
    }
}
