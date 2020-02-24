using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using rt_1_in_one_week.ViewModel;
using rt_1_in_one_week.Mathematics;
using rt_1_in_one_week.Color;
using rt_1_in_one_week.Process;

namespace rt_1_in_one_week.Model
{
    public class RayTraceModel
    {
        private RayTraceViewModel _vm;
        private Thread _rt_thread;
        private bool _render = false;

        public RayTraceModel()
        { }

        public RayTraceViewModel ViewModel 
        {
            set { _vm = value; }
        }

        public void StartRayTrace()
        {
            _render = true;
            _rt_thread = new Thread(RayTraceThread);
            _rt_thread.Start(this);
        }

        public void TerminateRayTrace()
        {
            _render = false;
            _rt_thread?.Join();
        }

        private static void RayTraceThread(Object obj)
        {
            var model = obj as RayTraceModel;
            var viewModel = model._vm;

            // get current settings
            var cx = viewModel.BitmapWidth;
            var cy = viewModel.BitmapHeight;
            var no_samples = viewModel.RenderSamples;
            var update_rate = (double)viewModel.RenderUpdateRate;
            int no_samples_outer = Math.Max(1, (int)(no_samples * update_rate + 0.5));
            int no_samples_inner = (no_samples + no_samples_outer - 1) / no_samples_outer;

            var iterator = new IterateBufferExp2(cx, cy);
            var aspcet = (double)cx / (double)cy;
            var sampler = new Random();

            var cam = new Camera(
                Vec3.Create(-aspcet, -1, -1),
                Vec3.Create(2 * aspcet, 0, 0),
                Vec3.Create(0, 2, 0),
                Vec3.Create(0, 0, 0));

            IHitable[] hitables = {
                new Sphere(Vec3.Create(0, -100.5, -1), 100, new Lambertian(Vec3.Create(0.8, 0.8, 0.0))),
                new Sphere(Vec3.Create(0, 0, -1), 0.5, new Lambertian(Vec3.Create(0.8, 0.3, 0.3))), 
                new Sphere(Vec3.Create(1, 0, -1), 0.5, new Metal(Vec3.Create(0.8, 0.6, 0.2), 0.3)),
                new Sphere(Vec3.Create(-1, 0, -1), 0.5, new Dielectric(1.5)),
                new Sphere(Vec3.Create(-1, 0, -1), -0.45, new Dielectric(1.5))
            };
            var world = new HitableList(hitables);

            Vec3[,] colFiled = new Vec3[cx, cy];
            for (int i = 0; i < cx; ++i)
                for (int j = 0; j < cy; ++j)
                    colFiled[i, j] = Vec3.Create(0.0);

            int x = 0, y = 0;
            int processed = 0;
            for (int outer_i = 0; model._render && (outer_i * no_samples_inner) < no_samples; ++outer_i)
            {
                iterator.Reset();
                while (model._render && iterator.Next(out x, out y))
                {
                    int no_start = no_samples_inner * outer_i;
                    int no_end = Math.Min(no_samples, no_start + no_samples_inner);
                    for (int inner_i = no_start; model._render && inner_i < no_end; ++inner_i)
                    {
                        (double dx, double dy) = ((double)x + sampler.NextDouble(), (double)y + sampler.NextDouble());
                        (double u, double v) = (dx / cx, dy / cy);
                        colFiled[x, y] += ReytraceColor(cam.Get(u, v), world, 0);
                    }

                    var col = colFiled[x, y] / no_end;
                    viewModel.SetBitmapPixel(x, cy - y - 1, ColorFactory.CreateSquare(col));
                    viewModel.Progress = (double)(++processed) / (double)(cx * cy * no_samples_outer);

                    //Thread.Yield();
                    //Thread.SpinWait(1);
                    //Thread.Sleep(1);
                }
            }
        }

        private static Vec3 CreateSky(Ray r)
        {
            var unit_direction = Vec3.Normalize(r.Direction);
            var t = unit_direction.Y * 0.5 + 0.5;
            var v = new Vec3(1.0) * (1.0 - t) + new Vec3(0.5, 0.7, 1.0) * t;
            return v;
        }

        private static Vec3 ReytraceColor(Ray r, IHitable hitable, int depth)
        {
            HitRecord rec;
            if (hitable.Hit(r, 0.001, Double.MaxValue, out rec))
            {
                Ray scattered;
                Vec3 attenuation;
                if (depth < 50 && rec.Material.Scatter(r, rec, out attenuation, out scattered))
                {
                    return attenuation * ReytraceColor(scattered, hitable, depth + 1);
                }
                return Vec3.Create(0.0);
            }
            return CreateSky(r);
        }
    }
}