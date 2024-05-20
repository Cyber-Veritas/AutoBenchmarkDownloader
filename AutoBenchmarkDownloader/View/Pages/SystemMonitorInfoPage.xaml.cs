using AutoBenchmarkDownloader.ViewModel;
using System.Windows.Controls;

namespace AutoBenchmarkDownloader.View.Pages
{
    public partial class SystemMonitorInfoPage : Page
    {
        public SystemMonitorInfoPage()
        {
            InitializeComponent();
            SystemMonitorViewModel vm = new SystemMonitorViewModel();
            DataContext = vm;
        }
    }
}
