using AutoBenchmarkDownloader.Utilities;

namespace AutoBenchmarkDownloader.ViewModel
{
    class SystemMonitorViewModel
    {
        private readonly SystemInfoModel _systemInfoModel;
        //private readonly SystemMonitorViewModel _monitorViewModel;

        public SystemMonitorViewModel()
        {
            _systemInfoModel = new SystemInfoModel();
            //_monitorViewModel = new SystemMonitorViewModel(); 
        }

        public SystemInfoModel SystemInfoM => _systemInfoModel;
        //public SystemMonitorViewModel MonitorViewModel => _monitorViewModel;
    }
}
