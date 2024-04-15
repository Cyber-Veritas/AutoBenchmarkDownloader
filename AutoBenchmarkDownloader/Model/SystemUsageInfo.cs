
namespace AutoBenchmarkDownloader.Model
{
    public class SystemUsageInfo
    {
        public required string cpuUsage { get; set; }
        public  required string cpuTemp { get; set; }
        public required string ramUsage { get; set; }
        public required string gpuUsage { get; set; }
        public required string gpuTemp { get; set; }
    }
}
