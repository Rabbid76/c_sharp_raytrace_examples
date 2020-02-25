using System.Collections.Generic;

namespace rt_1_in_one_week.Process
{
    interface IIterateBuffer 
    {
        public IEnumerator<(int, int)> GetEnumerator();
    }
}
