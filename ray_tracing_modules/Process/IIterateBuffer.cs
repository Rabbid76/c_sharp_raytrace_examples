using System.Collections.Generic;

namespace ray_tracing_modules.Process
{
    interface IIterateBuffer 
    {
        IEnumerator<(int, int)> GetEnumerator();
    }
}
