using AutoBenchmarkDownloader.ViewModel;
using System.Windows.Controls;

namespace AutoBenchmarkDownloader.View.Pages
{
    public partial class SystemMonitorInfoPage : Page
    {
        public SystemMonitorInfoPage()
        {
            InitializeComponent();
            DataContext = SystemMonitorViewModel.Instance;
        }
    }
}
