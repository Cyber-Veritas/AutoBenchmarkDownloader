namespace AutoBenchmarkDownloader.ViewModel
{
    internal class MainWindowViewModel
    {
        public DxDiagInfoViewModel DxDiagInfoVm { get; } = new();
        public SystemUsageInfoViewModel SystemUsageInfoVm { get; } = new();
    }
}
