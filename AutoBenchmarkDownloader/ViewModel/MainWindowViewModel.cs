using AutoBenchmarkDownloader.MVVM;
using System.ComponentModel;

namespace AutoBenchmarkDownloader.ViewModel
{
    internal class MainWindowViewModel
    {
        public DxDiagInfoViewModel DxDiagInfoViewModel { get; }
        public SystemUsageInfoViewModel SystemUsageInfoView { get; }

        public MainWindowViewModel  ()
        {
            DxDiagInfoViewModel = new DxDiagInfoViewModel ();
            SystemUsageInfoView = new SystemUsageInfoViewModel ();
        }
    }
}
