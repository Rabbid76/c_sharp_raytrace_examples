using System;
using System.Collections.Generic;
using System.Linq;
using ray_tracing_modules.Color;
using ray_tracing_modules.RayTraycer.Interfaces;

namespace ray_tracing_modules.Process
{
    public class RayTraceProcessEnumertor
        : IEnumerator<RayTraceModel>
    {
        private bool _keepRendering = false;
        private Func<double, double, RGBColor> _background;
        private readonly int _cx;
        private readonly int _cy;
        private readonly(int x, int y)[] _positions;
        private readonly int _no_samples;
        private readonly double _update_rate;
        private readonly int _no_samples_outer;
        private readonly int _no_samples_inner;
        private RayTraceModel _current;
        private Random _sampler;
        private RGBdBuffer _colorBuffer;
        private int _outer_i;
        private int _inner_i;
        private int _processed;

        public RayTraceProcessEnumertor(
           IRayTraceConfigurationModel confguration, Func<double, double, RGBColor> background)
        {
            this._cx = confguration.Width;
            this._cy = confguration.Height;
            this._positions = new IterateBufferExp2(this._cx, this._cy).ToArray();
            this._no_samples = confguration.Samples;
            this._update_rate = confguration.UpdateRate;
            this._no_samples_outer = Math.Max(1, (int)(this._no_samples * this._update_rate + 0.5));
            this._no_samples_inner = (this._no_samples + this._no_samples_outer - 1) / this._no_samples_outer;

            this._background = background;
            this._sampler = new Random();
            Reset();
        }

        void IDisposable.Dispose() { }

        public bool KeepRendering { set => this._keepRendering = value; }

        public void Reset()
        {
            this._keepRendering = true;
            this._colorBuffer = new RGBdBuffer(this._cx, this._cy);
            this._outer_i = 0;
            this._inner_i = 0;
            this._processed = 0;
        }

        object System.Collections.IEnumerator.Current { get => Current; }

        public RayTraceModel Current { get => _current; }

        public bool MoveNext()
        {
            if (_keepRendering == false)
                return false;
            if ((this._outer_i * this._no_samples_inner) >= this._no_samples)
                return false;

            (int x, int y) = this._positions[this._inner_i];
            int no_start = this._no_samples_inner * this._outer_i;
            int no_end = Math.Min(this._no_samples, no_start + this._no_samples_inner);
            for (int sample_i = no_start; this._keepRendering && sample_i < no_end; ++sample_i)
            {
                (double dx, double dy) = ((double)x + this._sampler.NextDouble(), (double)y + this._sampler.NextDouble());
                (double u, double v) = (dx / this._cx, dy / this._cy);
                var sampleColor = this._background(u, v);
                this._colorBuffer.Set(x, y, sampleColor, sample_i);
            }

            var color = this._colorBuffer.Get(x, y);
            var progress_value = (double)(++this._processed) / (double)(this._cx * this._cy * this._no_samples_outer);
            this._current = new RayTraceModel { X = x, Y = this._cy - y - 1, Color = color, Progress = progress_value };

            this._inner_i++;
            if (this._inner_i >= this._positions.Length)
            {
                this._inner_i = 0;
                this._outer_i++;
            }
            return true;
        }
    }
}
