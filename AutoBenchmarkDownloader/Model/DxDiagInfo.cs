using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBenchmarkDownloader.Model
{
    public class DxDiagInfo
    {
        public required string CpuModel {get; set;}
        public required string Ram { get; set; }
        public required string Motherboard { get; set; }
        public required string Bios { get; set; }
        public required string Os { get; set; }
        public required string Gpu { get; set; }
        public required string DirectX { get; set; }
    }
}
