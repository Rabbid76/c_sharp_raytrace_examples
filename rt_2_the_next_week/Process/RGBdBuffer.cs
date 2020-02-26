using rt_2_the_next_week.Mathematics;

namespace rt_2_the_next_week.Process
{
    public class RGBdBuffer
    {
        public int _cx;
        public int _cy;
        public double[] _buffer;

        public RGBdBuffer(int cx, int cy)
        {
            _cx = cx;
            _cy = cy;
            _buffer = new double[_cy * _cx * 3];
        }

        public RGBdBuffer Set(int x, int y, Vec3 fragment, int sample_no)
        {
            int i = y * _cx * 3 + x * 3;
            if (sample_no <= 0)
            {
                _buffer[i] = fragment.X;
                _buffer[i+1] = fragment.Y;
                _buffer[i+2] = fragment.X;
            }
            else
            {
                _buffer[i] = (_buffer[i] * sample_no + fragment.X) / (sample_no+1);
                _buffer[i + 1] = (_buffer[i+1] * sample_no + fragment.Y) / (sample_no + 1);
                _buffer[i + 2] = (_buffer[i+2] * sample_no + fragment.Z) / (sample_no + 1);
            }
            return this;
        }

        public Vec3 Get(int x, int y)
        {
            int i = y * _cx * 3 + x * 3;
            return Vec3.Create(_buffer[i], _buffer[i + 1], _buffer[i + 2]);
        }
    }
}
