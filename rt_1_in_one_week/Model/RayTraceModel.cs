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

            var cx = viewModel.BitmapWidth;
            var cy = viewModel.BitmapHeight;
            var iterator = new IterateBufferExp2(cx, cy);
            var aspcet = (double)cx / (double)cy;
            var sampler = new Random();

            var cam = new Camera(
                Vec3.Create(-aspcet, -1, -1),
                Vec3.Create(2 * aspcet, 0, 0),
                Vec3.Create(0, 2, 0),
                Vec3.Create(0, 0, 0));

            IHitable[] hitables = {
                new Sphere(Vec3.Create(0, 0, -1), 0.5), 
                new Sphere(Vec3.Create(0, -100.5, -1), 100)
            };
            var world = new HitableList(hitables);

            Vec3[,] colFiled = new Vec3[cx, cy];
            for (int i = 0; i < cx; ++i)
                for (int j = 0; j < cy; ++j)
                    colFiled[i, j] = Vec3.Create(0.0);

            int ns_outer = 10;
            int ns_inner = 10;
            int x = 0, y = 0;
            int processed = 0;
            for (int outer_i = 0; model._render && outer_i < ns_outer; ++outer_i)
            {
                iterator.Reset();
                while (model._render && iterator.Next(out x, out y))
                {
                    for (int inner_i = 0; model._render && inner_i < ns_inner; ++inner_i)
                    {
                        (double dx, double dy) = ((double)x + sampler.NextDouble(), (double)y + sampler.NextDouble());
                        (double u, double v) = (dx / cx, dy / cy);
                        colFiled[x, y] += color(cam.Get(u, v), world);
                    }

                    var col = colFiled[x, y] / ((outer_i + 1) * ns_inner);
                    viewModel.SetBitmapPixel(x, cy - y - 1, ColorFactory.Create(col));
                    viewModel.Progress = (double)(++processed) / (double)(cx * cy * ns_outer);

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

        private static Vec3 color(Ray r, IHitable hitable)
        {
            HitRecord hit_record;
            if (hitable.Hit(r, 0.0, Double.MaxValue, out hit_record))
                return hit_record.Normal * 0.5 + 0.5;
            return CreateSky(r);
        }
    }
}

// Chapter 3: Rays, a simple camera and background
// Chapter 4: Adding a sphere 
// Chapter 5: Surface normals and multiple objects
// Chapter 6: Antialiasing
