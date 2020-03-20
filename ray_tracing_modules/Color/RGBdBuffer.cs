namespace ray_tracing_modules.Color
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

        public RGBdBuffer Set(int x, int y, RGBColor fragment, int sample_no)
        {
            int i = y * _cx * 3 + x * 3;
            if (sample_no <= 0)
            {
                _buffer[i] = fragment.R;
                _buffer[i+1] = fragment.G;
                _buffer[i+2] = fragment.B;
            }
            else
            {
                _buffer[i] = (_buffer[i] * sample_no + fragment.R) / (sample_no+1);
                _buffer[i + 1] = (_buffer[i+1] * sample_no + fragment.G) / (sample_no + 1);
                _buffer[i + 2] = (_buffer[i+2] * sample_no + fragment.B) / (sample_no + 1);
            }
            return this;
        }

        public RGBColor Get(int x, int y)
        {
            int i = y * _cx * 3 + x * 3;
            return new RGBColor(_buffer[i], _buffer[i + 1], _buffer[i + 2]);
        }
    }
}
