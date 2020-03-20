using System;
using System.Collections.Generic;

namespace ray_tracing_modules.Process
{
    class IterateBufferExp2
        : IIterateBuffer
    {
        private int _cx = 0;
        private int _cy = 0;

        public int SizeX { get => _cx; }
        public int SizeY { get => _cy; }

        public IterateBufferExp2(int cx, int cy)
        {
            _cx = cx;
            _cy = cy;
        }

        public IEnumerator<(int, int)> GetEnumerator()
        {
            int max_s = Math.Max(_cx, _cy);
            int p2 = 0;
            while (max_s > 0)
                (p2, max_s) = (p2 + 1, max_s >> 1);
            int tile_size = 1 << p2 - 1;
          
            if (tile_size >= 0)
                yield return (0, 0);

            int[] lix = null;
            int[] liy = null;
            (int tx, int ty) = (-1, -1);
            while (tile_size > 0)
            {
                if (tx <= 0 && ty <= 0)
                {
                    int[] no = new int[2] { (_cx - 1) / tile_size + 1, (_cy - 1) / tile_size + 1 };
                    lix = new int[no[0]];
                    liy = new int[no[1]];

                    int mx = (no[0] - 1) / 2;
                    for (int i = 0; i < no[0]; ++i)
                        lix[i] = (i % 2 == 0) ? (mx - i / 2) : (mx + 1 + i / 2);
                    int my = no[1] / 2;
                    for (int j = 0; j < no[1]; ++j)
                        liy[j] = j % 2 == 0 ? my + j / 2 : my - 1 - j / 2;

                    (tx, ty) = (0, 0);
                }

                (int ix, int iy) = (lix[tx], liy[ty]);
                if (tile_size >= 128 || (ix % 2 != 0) || (iy % 2 != 0))
                    yield return (ix * tile_size, iy * tile_size);

                tx++;
                if (tx >= lix.Length)
                {
                    tx = 0;
                    ty++;
                }
                if (ty >= liy.Length)
                {
                    ty = 0;
                    tile_size >>= 1;
                }
            }
        }
    }
}
