using System;

namespace rt_1_in_one_week.Process
{
    class IterateBufferExp2
        : IIterateBuffer
    {
        private int _x = -1;
        private int _y = -1;
        private int _cx = 0;
        private int _cy = 0;
        private int _tile_size = 0;
        private int _tx = -1;
        private int _ty = -1;
        private int[] _ix;
        private int[] _iy;

        public int X { get => _x; }
        public int Y { get => _y; }
        public int SizeX { get => _cx; }
        public int EizeY { get => _cy; }

        public IterateBufferExp2(int cx, int cy)
        {
            _cx = cx;
            _cy = cy;
            Reset();
        }

        public bool Next(out int x, out int y)
        {
            (x, y) = (-1, -1);
            if (_tile_size < 0) return false;

            if (_cx < 0 || _cy < 0)
            {
                (x, y) = (_x, _y) = (0, 0);
                return true;
            }

            while (_tile_size > 0)
            {
                if (_tx <= 0 && _ty <= 0)
                {
                    int[] no = new int[2] { (_cx - 1) / _tile_size + 1, (_cy - 1) / _tile_size + 1 };
                    _ix = new int[no[0]];
                    _iy = new int[no[1]];

                    int mx = (no[0] - 1) / 2;
                    for (int i = 0; i < no[0]; ++i)
                        _ix[i] = (i % 2 == 0) ? (mx - i / 2) : (mx + 1 + i / 2);
                    int my = no[1] / 2;
                    for (int j = 0; j < no[1]; ++j)
                        _iy[j] = j % 2 == 0 ? my + j / 2 : my - 1 - j / 2;

                    (_tx, _ty) = (0, 0);
                }

                (int ix, int iy) = (_ix[_tx], _iy[_ty]);
                bool yield_index = _tile_size >= 128 || (ix % 2 != 0) || (iy % 2 != 0);
                if (yield_index)
                    (_x, _y) = (ix * _tile_size, iy * _tile_size);

                _tx++;
                if (_tx >= _ix.Length)
                {
                    _tx = 0;
                    _ty++;
                }
                if (_ty >= _iy.Length)
                {
                    _ty = 0;
                    _tile_size >>= 1;
                }

                if (yield_index)
                {
                    (x, y) = (_x, _y);
                    return true;
                }
            }

            return false;
        }

        public void Reset()
        {
            (_x, _y, _tx, _ty) = (-1, -1, -1, -1);
            int max_s = Math.Max(_cx, _cy);
            int p2 = 0;
            while (max_s > 0)
                (p2, max_s) = (p2 + 1, max_s >> 1);
            _tile_size = 1 << p2 - 1;
        }
    }
}
