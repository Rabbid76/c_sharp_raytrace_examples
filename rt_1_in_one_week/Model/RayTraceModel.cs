using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using rt_1_in_one_week.ViewModel;
using rt_1_in_one_week.Mathematics;
using rt_1_in_one_week.Color;

namespace rt_1_in_one_week.Model
{
    public class RayTraceModel
    {
        RayTraceViewModel _vm;
        Thread _rt_thread = new Thread(RayTraceThread);

        public RayTraceModel()
        { }

        ~RayTraceModel()
        {
            _rt_thread.Abort();
        }

        public RayTraceViewModel ViewModel 
        {
            set { _vm = value; }
        }

        public void StartRayTrace()
        {
            _rt_thread.Start(_vm);
        }

        private static void RayTraceThread(Object obj)
        {
            RayTraceViewModel viewModel = obj as RayTraceViewModel;

            var cx = viewModel.BitmapWidth;
            var cy = viewModel.BitmapHeight;

            int x = 0, y = 0;
            
            bool run = true;
            while (run)
            {
                var fx = (double)x / cx;
                var fy = (double)y / cy;
                var v = new Vec3(fx, fy, (1.0 - fx) * (1.0 - fy));
                viewModel.SetBitmapPixel(x, y, ColorFactory.Create(v));
                x++;
                viewModel.Progress = ((double)y * cx + (double)x) / (cx*cy);
                if (x == cx)
                {
                    y++;
                    x = 0;
                }
                run = y < cy;
                //Thread.Yield();
                //Thread.SpinWait(1);
                Thread.Sleep(1);
            }
        }
    }
}
