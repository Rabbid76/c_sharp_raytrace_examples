using System;
using System.Collections.Generic;
using System.Text;

namespace rt_1_in_one_week.Mathematics
{
    public interface ICamera
    {
        Ray Get(double u, double v);
    }
}
