using System.Collections.Generic;

namespace rt_1_in_one_week.Process
{
    public class IterateBufferLinear
        : IIterateBuffer
    {
        private int _cx = 0;
        private int _cy = 0;

        public int SizeX { get => _cx; }
        public int EizeY { get => _cy; }

        public IterateBufferLinear(int cx, int cy)
        {
            _cx = cx;
            _cy = cy;
        }

        public IEnumerator<(int, int)> GetEnumerator()
        {
            for (int y = 0; y < _cy; ++y)
            {
                for (int x = 0; x < _cx; ++x)
                    yield return (x, y);
            }
        }
    }
}
