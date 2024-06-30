using AutoBenchmarkDownloader.MVVM;
using AutoBenchmarkDownloader.Utilities;

namespace AutoBenchmarkDownloader.ViewModel
{
    class SystemMonitorViewModel : ViewModelBase
    {
        private static SystemMonitorViewModel _instance;
        private readonly SystemInfoModel _systemInfoModel;
        private readonly SystemUsageModel _systemUsageModel;

        private bool _isDataLoaded = false;

        public SystemMonitorViewModel()
        {
            _systemInfoModel = new SystemInfoModel();
            _systemUsageModel = new SystemUsageModel();
            LoadData();
        }

        public static SystemMonitorViewModel Instance => _instance ?? (_instance = new SystemMonitorViewModel());

        public SystemInfoModel SystemInfoM => _systemInfoModel;
        public SystemUsageModel SystemUsageM => _systemUsageModel;

        private void LoadData()
        {
            if (_isDataLoaded) return;

            _systemInfoModel.SetInfo();
            _isDataLoaded = true;
        }
    }
}
