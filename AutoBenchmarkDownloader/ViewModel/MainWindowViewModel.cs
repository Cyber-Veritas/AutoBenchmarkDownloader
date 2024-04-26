namespace AutoBenchmarkDownloader.ViewModel
{
    internal class MainWindowViewModel
    {
        public SoftwareInfoViewModel SoftwareInfoVm { get; } = new();
        public DxDiagInfoViewModel DxDiagInfoVm { get; } = new();
        public SystemUsageInfoViewModel SystemUsageInfoVm { get; } = new();
    }
}
