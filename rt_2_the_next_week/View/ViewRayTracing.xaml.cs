using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using rt_2_the_next_week.ViewModel;

namespace rt_2_the_next_week.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class RayTracingView : Window
    {
        public RayTracingView()
        {
            InitializeComponent();
            var vm = this.DataContext as RayTraceViewModel;
            vm.Form = this;
        }
    }
}
