using AutoBenchmarkDownloader.MVVM;
using AutoBenchmarkDownloader.Utilities;

namespace AutoBenchmarkDownloader.ViewModel
{
    class SystemMonitorViewModel : ViewModelBase
    {
        private readonly SystemInfoModel _systemInfoModel;
        private readonly SystemUsageModel _systemUsageModel;

        public SystemMonitorViewModel()
        {
            _systemInfoModel = new SystemInfoModel();
            _systemUsageModel = new SystemUsageModel(); 
        }

        public SystemInfoModel SystemInfoM => _systemInfoModel;
        public SystemUsageModel SystemUsageM => _systemUsageModel;
    }
}
