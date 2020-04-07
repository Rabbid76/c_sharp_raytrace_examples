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
using Microsoft.Win32;
using rt_2_the_next_week.View;
using rt_2_the_next_week.Model;
using WpfViewModelModule.Utility;
using WpfViewModelModule.ControlData;

namespace rt_2_the_next_week.ViewModel
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
        private ICommand _saveImageCommad;
        private ICommand _applySettingsCommand;
        private PropertyUpdate<List<SceneEntry>> _scenes;
        private PropertyUpdate<SceneEntry> _currentScene;
        private PropertyUpdate<int> _render_cx;
        private PropertyUpdate<int> _render_cy;
        private PropertyUpdate<int> _samples;
        private PropertyUpdate<double> _update_rate;
        private PropertyUpdate<double> _progress;
        
        public class SceneEntry
        : ComboBoxUpdate
        {
            public SceneEntry() : base(nameof(Scenes)) { }
            public SceneEntry(string text, string number) : base(nameof(Scenes), text, number) { }
        }

        public RayTraceViewModel()
        {
            List<SceneEntry> scenes = new List<SceneEntry>();
            scenes.Add(new SceneEntry("Cover scene RT 2", "11"));
            scenes.Add(new SceneEntry("Volume", "10"));
            scenes.Add(new SceneEntry("Room", "9"));
            scenes.Add(new SceneEntry("Simple Light", "8"));
            scenes.Add(new SceneEntry("Checker Texture", "5"));
            scenes.Add(new SceneEntry("Noise Texture", "6"));
            scenes.Add(new SceneEntry("Globe", "7"));
            scenes.Add(new SceneEntry("Cover scene RT 1", "0"));
            scenes.Add(new SceneEntry("Cover scene RT 1 motion", "4"));
            scenes.Add(new SceneEntry("Materials", "1"));
            scenes.Add(new SceneEntry("Defocus Blur", "2"));
            scenes.Add(new SceneEntry("Test 1", "3"));

            _scenes = new PropertyUpdate<List<SceneEntry>>(scenes, nameof(Scenes), OnPropertyChanged );
            _currentScene = new PropertyUpdate<SceneEntry>(scenes[0], nameof(CurrentScene), OnPropertyChanged );
            _render_cx = new PropertyUpdate<int>(400, nameof(RenderWidth), OnPropertyChanged );
            _render_cy = new PropertyUpdate<int>(200, nameof(RenderHeight), OnPropertyChanged );
            _samples = new PropertyUpdate<int>(100, nameof(RenderSamples), OnPropertyChanged );
            _update_rate = new PropertyUpdate<double>(0.1, nameof(RenderUpdateRate), (string name) => { OnPropertyChanged(name); OnPropertyChanged(nameof(RenderUpdateRateTip)); });
            _progress = new PropertyUpdate<double>(0.0, nameof(Progress), (string name) => { OnPropertyChanged(name); OnPropertyChanged(nameof(ProgressTip)); });
           
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

        public ICommand SaveImageCommand { get => _saveImageCommad; set => _saveImageCommad = value; }
        public ICommand ApplySettingsCommand { get => _applySettingsCommand; set => _applySettingsCommand = value; }
        public List<SceneEntry> Scenes { get => _scenes; set => _scenes.Set(value); } 
        public SceneEntry CurrentScene { get => _currentScene; set => _currentScene.Set(value); }
        public int RenderWidth { get => _render_cx; set => _render_cx.Set(value); }
        public int RenderHeight { get => _render_cy; set => _render_cy.Set(value); }
        public int RenderSamples { get => _samples; set => _samples.Set(value); }
        public double RenderUpdateRate { get => _update_rate; set => _update_rate.Set(value); }
        public string RenderUpdateRateTip { get => "Update rate " + ((int)(RenderUpdateRate * 100.0 + 0.5)).ToString() + "%"; }
        public double Progress { get => _progress; set => _progress.Set(value); }
        public string ProgressTip { get => "Progress " + ((int)(_progress * 100.0 + 0.5)).ToString() + "%"; }

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

            Bitmap bm = new Bitmap(_render_cx, _render_cy);
            RayTraceBitmap = bm;

            _rt_model?.StartRayTrace();
        }

        private void SaveImage(object obj)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Png Image|*.png";
                saveFileDialog.Title = "Save current rendering to image file";
                saveFileDialog.FileName = @"test.png";
                saveFileDialog.InitialDirectory = @"c:\temp";
                bool? ret = saveFileDialog.ShowDialog();

                if (ret.HasValue && ret.Value && saveFileDialog.FileName != "")
                {
                    var fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create);
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(this._rt_image));
                    encoder.Save(fileStream);
                    fileStream.Close();
                }
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
            RenderSamples = _samplesRange.Clamp(RenderSamples);
            RenderUpdateRate = _updateRange.Clamp(RenderUpdateRate);
            RestartRaytrace();
        }
    }
}
