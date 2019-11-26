using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using rt_1_in_one_week.ViewModel;

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

            int x = 0, y = 0;
            System.Drawing.Color c = System.Drawing.Color.FromArgb(255, 0, 0);

            bool run = true;
            while (run)
            {
                viewModel.SetBitmapPixel(100+x, 100+y, c);
                x++;
                viewModel.Progress = ((double)y * 100.0 + (double)x) / 10000.0;
                if (x == 100)
                {
                    y++;
                    x = 0;
                }
                run = y < 100;
                Thread.Sleep(1);
            }
        }
    }
}
