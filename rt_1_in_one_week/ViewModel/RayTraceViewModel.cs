using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using rt_1_in_one_week.View;
using rt_1_in_one_week.Model;
using WpfViewModelModule;

namespace rt_1_in_one_week.ViewModel
{
    public class RayTraceViewModel
        : INotifyPropertyChanged
    {
        private static RangeLimit<int> _bitmapSizeRange = new RangeLimit<int>(8, 4096);
        private static RangeLimit<int> _samplesRange = new RangeLimit<int>(1, 10000);
        private static RangeLimit<double> _updateRange = new RangeLimit<double>(0.01, 1.0);

        public event PropertyChangedEventHandler PropertyChanged;

        private RayTracingView _form;
        private RayTraceModel _rt_model = new RayTraceModel();

        private BitmapSource _rt_image;
        private Bitmap _rt_bitmap;
        private bool _rt_bitmap_valid = false;
        private readonly object _bitmap_lock = new object();
        private double _progress = 0.0;
        private ICommand _saveImageCommad;
        private ICommand _applySettingsCommand;
        private int _render_cx = 400;
        private int _render_cy = 200;
        private int _samples = 100;
        private double _update_rate = 0.1;

        public RayTraceViewModel()
        {
            _saveImageCommad = new RelayCommand(SaveImage, param => true);
            _applySettingsCommand = new RelayCommand(ApplySettings, param => true);
           
            _rt_model.ViewModel = this;

            // TODO $$$ do not start raytracing at startup?
            RestartRaytrace();
        }

        public RayTracingView Form
        {
            get => _form;
            set
            {
                _form = value;
                _form.Closing += CloseWindow;
            }
        }

        private void CloseWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _rt_model?.TerminateRayTrace();
        }

        protected internal void OnPropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }

        private void ChangeProperty<T>(string propertyname, ref T property, T value)
        {
            property = value;
            OnPropertyChanged(propertyname);
        }

        public ICommand SaveImageCommand
        {
            get { return _saveImageCommad; }
            set { _saveImageCommad = value; }
        }

        public ICommand ApplySettingsCommand
        {
            get { return _applySettingsCommand; }
            set { _applySettingsCommand = value; }
        }

        public int RenderWidth
        {
            get => _render_cx;
            set
            {
                _render_cx = value;
                this.OnPropertyChanged("RenderWidth");
            }
        }

        public int RenderHeight
        {
            get => _render_cy;
            set => ChangeProperty("RenderHeight", ref _render_cy, value);
        }

        public int RenderSamples
        {
            get => _samples;
            set => ChangeProperty("RenderSamples", ref _samples, value);
        }

        public double RenderUpdateRate
        {
            get => _update_rate;
            set
            {
                _update_rate = value;
                this.OnPropertyChanged("RenderUpdateRate");
                this.OnPropertyChanged("RenderUpdateRateTip");
            }
        }

        public string RenderUpdateRateTip
        {
            get => "Update rate " + ((int)(RenderUpdateRate * 100.0 + 0.5)).ToString() + "%";
        }

        public double Progress
        {
            get => _progress;
            set
            {
                _progress = value * 100.0;
                OnPropertyChanged("Progress");
                OnPropertyChanged("ProgressTip");
            }
        }

        public string ProgressTip
        {
            get => "Progress " + ((int)(_progress + 0.5)).ToString() + "%";
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
                this._rt_image = value as BitmapSource;
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

        public int BitmapWidth { get => _rt_bitmap.Width; }
        public int BitmapHeight { get => _rt_bitmap.Height; }

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

        private void RestartRaytrace()
        {
            _rt_model?.TerminateRayTrace();

            // TODO $$$ image size
            Bitmap bm = new Bitmap(_render_cx, _render_cy);
            RayTraceBitmap = bm;

            _rt_model?.StartRayTrace();
        }

        private void SaveImage(object obj)
        {
            try
            {
                // TODO $$$ file open dialog

                var fileStream = new FileStream(@"c:\temp\test.png", FileMode.Create);
                BitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(this._rt_image));
                encoder.Save(fileStream);
            }
            catch (Exception)
            {
                // [...]
            }
        }

        private void ApplySettings(object obj)
        {
            RenderWidth = _bitmapSizeRange.Clamp(RenderWidth);
            RenderHeight = _bitmapSizeRange.Clamp(RenderHeight);
            RenderSamples = _samplesRange.Clamp(_samples);
            RenderUpdateRate = _updateRange.Clamp(_update_rate);
            RestartRaytrace();
        }
    }
}
