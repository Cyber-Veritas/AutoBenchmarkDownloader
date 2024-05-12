using AutoBenchmarkDownloader.ViewModel;
using System.Windows.Controls;

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
