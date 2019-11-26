using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using rt_1_in_one_week.View;
using rt_1_in_one_week.Model;

namespace rt_1_in_one_week.ViewModel
{
    public class RayTraceViewModel
        : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private RayTraceModel _rt_model = new RayTraceModel();

        private ImageSource _rt_image;
        private Bitmap _rt_bitmap;
        private bool _rt_bitmap_valid = false;
        private readonly object _bitmap_lock = new object();
        private double _progress = 0.0;

        public RayTraceViewModel()
        {
            // temporary test image
            // TODO $$$ delete
            Bitmap bm = new Bitmap(400, 400);
            System.Drawing.Color c = System.Drawing.Color.FromArgb(0, 0, 255);
            for (int x = 0; x < 100; ++x)
            {
                for (int y = 0; y < 100; ++y)
                {
                    bm.SetPixel(x + 100, y + 100, c);
                }
            }
            RayTraceBitmap = bm;

            _rt_model.ViewModel = this;
            // TODO $$$ move to command
            _rt_model.StartRayTrace();
        }

        protected internal void OnPropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }

        public ImageSource RayTraceImage
        {
            get
            {
                UpdateImageSource();
                return this._rt_image;
            }
            set
            {
                this._rt_image = value;
                this.OnPropertyChanged("RayTraceImage");
            }
        }

        public Bitmap RayTraceBitmap
        {
            set
            {
                lock (_bitmap_lock)
                {
                    this._rt_bitmap = value;
                }
                NotifyImgeSourceChanged();
            }
        }

        public void SetBitmapPixel(int x, int y, System.Drawing.Color c)
        {
            lock (_bitmap_lock)
            {
                if (this._rt_bitmap != null)
                    this._rt_bitmap.SetPixel(x, y, c);
            }
            NotifyImgeSourceChanged();
        }

        private void NotifyImgeSourceChanged()
        {
            this._rt_bitmap_valid = false; 
            this.OnPropertyChanged("RayTraceImage");
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        private void UpdateImageSource()
        {
            if (this._rt_bitmap_valid)
                return;

            IntPtr bm_handle = IntPtr.Zero;
            lock (this._bitmap_lock)
            {
                bm_handle = this._rt_bitmap.GetHbitmap();
            }
            try
            {
                // [WPF - converting Bitmap to ImageSource](https://stackoverflow.com/questions/26260654/wpf-converting-bitmap-to-imagesource)
                this._rt_image = Imaging.CreateBitmapSourceFromHBitmap(bm_handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                this._rt_bitmap_valid = true;
            }
            catch { }
            finally
            {
                DeleteObject(bm_handle);
            }
        }

        public double Progress
        {
            get { return _progress; }
            set
            {
                _progress = value * 100.0;
                OnPropertyChanged("Progress");
            }
        }
    }
}
