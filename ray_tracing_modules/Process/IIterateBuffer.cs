using System.Collections.Generic;

namespace ray_tracing_modules.Process
{
    interface IIterateBuffer 
        : IEnumerable<(int, int)>
    {
    }
}
