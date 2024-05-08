using System.Windows.Controls;
using AutoBenchmarkDownloader.ViewModel;

namespace AutoBenchmarkDownloader.View.Pages
{
    public partial class DownloadPage : Page
    {
        public DownloadPage()
        {
            InitializeComponent();
            SoftwareInfoViewModel vm = new SoftwareInfoViewModel();
            DataContext = vm;
        }
    }
}
