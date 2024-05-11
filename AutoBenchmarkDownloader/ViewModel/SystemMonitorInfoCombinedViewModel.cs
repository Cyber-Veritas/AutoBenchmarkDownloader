namespace AutoBenchmarkDownloader.ViewModel
{
    internal class SystemMonitorInfoCombinedViewModel
    {
        public DxDiagInfoViewModel DxDiagInfoVm { get; } = new();
        public SystemUsageInfoViewModel SystemUsageInfoVm { get; } = new();
    }
}
