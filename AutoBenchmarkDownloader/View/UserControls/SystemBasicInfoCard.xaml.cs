using System.Windows;
using System.Windows.Controls;

namespace AutoBenchmarkDownloader.View.UserControls
{
    public partial class SystemBasicInfoCard : UserControl
    {
        // GPU INFO
        public static readonly DependencyProperty GpuInfoProperty =
            DependencyProperty.Register("GpuInfo", typeof(string), typeof(SystemBasicInfoCard), new PropertyMetadata(""));

        public string GpuInfo
        {
            get { return (string)GetValue(GpuInfoProperty); }
            set { SetValue(GpuInfoProperty, value); }
        }

        public SystemBasicInfoCard()
        {
            InitializeComponent();
        }
    }
}
