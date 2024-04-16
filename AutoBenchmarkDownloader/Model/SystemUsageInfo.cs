﻿
namespace AutoBenchmarkDownloader.Model
{
    public class SystemUsageInfo
    {
        public required int cpuUsage { get; set; }
        public  required int cpuTemp { get; set; }
        public required int ramUsage { get; set; }
        public required double gpuUsage { get; set; }
        public required int gpuTemp { get; set; }
    }
}
