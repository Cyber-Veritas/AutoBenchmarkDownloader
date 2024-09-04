namespace AutoBenchmarkDownloader.ViewModel
{
    internal class SettingsViewModel
    {
        public SystemMonitorViewModel SystemMonitorViewModel { get; }
        public SoftwareInfoViewModel SoftwareInfoViewModel { get; }

        public SettingsViewModel()
        {
            SystemMonitorViewModel = SystemMonitorViewModel.Instance;
            SoftwareInfoViewModel = SoftwareInfoViewModel.Instance;
        }
    }
}
