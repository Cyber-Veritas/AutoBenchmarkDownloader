using AutoBenchmarkDownloader.MVVM;
using AutoBenchmarkDownloader.Utilities;
using System.Windows.Input;

namespace AutoBenchmarkDownloader.ViewModel
{
    class SystemMonitorViewModel : ViewModelBase
    {
        private static SystemMonitorViewModel _instance;
        private readonly SystemInfoModel _systemInfoModel;
        private readonly SystemUsageModel _systemUsageModel;
        private bool _isDataLoaded = false;
        private int _refreshInterval;

        public SystemMonitorViewModel()
        {
            _systemInfoModel = new SystemInfoModel();
            _systemUsageModel = new SystemUsageModel();
            LoadData();
            SetIntervalCommand = new RelayCommand(SetInterval);
        }
        public int RefreshInterval
        {
            get => _refreshInterval;
            set
            {
                if (_refreshInterval != value)
                {
                    _refreshInterval = value;
                    _systemUsageModel.SetRefreshInterval(_refreshInterval);
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SetIntervalCommand { get; }

        private void SetInterval(object parameter)
        {
            if (parameter is string intervalStr && int.TryParse(intervalStr, out int interval))
            {
                RefreshInterval = interval;
            }
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
