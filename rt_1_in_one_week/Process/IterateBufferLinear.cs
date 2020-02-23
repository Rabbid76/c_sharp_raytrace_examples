namespace rt_1_in_one_week.Process
{
    public class IterateBufferLinear
        : IIterateBuffer
    {
        private int _x = -1;
        private int _y = -1;
        private int _cx = 0;
        private int _cy = 0;

        public int X { get => _x; }
        public int Y { get => _y; }
        public int SizeX { get => _cx; }
        public int EizeY { get => _cy; }

        public IterateBufferLinear(int cx, int cy)
        {
            _cx = cx;
            _cy = cy;
        }

        public bool Next(out int x, out int y)
        {
            (x, y) = (-1, -1);
            if (_x >= _cx-1 && _y >= _cy-1) return false;

            if (_y < 0) _y = 0;
            _x++;
            if (_x >= _cx) { _y++; _x = 0; }

            (x, y) = (_x, _y);
            return true;
        }

        public void Reset() => (_x, _y) = (-1, -1);
    }
}
