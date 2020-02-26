using System.Collections.Generic;

namespace rt_2_the_next_week.Process
{
    interface IIterateBuffer 
    {
        public IEnumerator<(int, int)> GetEnumerator();
    }
}
