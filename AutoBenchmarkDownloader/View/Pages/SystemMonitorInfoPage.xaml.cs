using AutoBenchmarkDownloader.ViewModel;
using System.Windows.Controls;

namespace AutoBenchmarkDownloader.View.Pages
{
    public partial class SystemMonitorInfoPage : Page
    {
        public SystemMonitorInfoPage()
        {
            InitializeComponent();
            SystemMonitorInfoCombinedViewModel vm = new SystemMonitorInfoCombinedViewModel();
            DataContext = vm;
        }
    }
}
