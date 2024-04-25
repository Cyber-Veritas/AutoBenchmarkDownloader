using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBenchmarkDownloader.Model
{
    public class RamModule
    {
        public required int id { get; set; }
        public required string DeviceLocator { get; set; }
        public required string Manufacturer { get; set; }
        public required string Code { get; set; }
        public required string Speed { get; set; }
        public required string Size { get; set; }
    }
    public class DxDiagInfo
    {
        public required string CpuModel {get; set;}
        public required List<RamModule> RamModules { get; set; }
        public required string RamModulesInfo { get; set; }
        public required string TotalRam {  get; set; }
        public required string Motherboard { get; set; }
        public required string Bios { get; set; }
        public required string Os { get; set; }
        public required string Gpu { get; set; }
        public required string DirectX { get; set; }

        public DxDiagInfo()
        {
            RamModules = new List<RamModule>();
        }
    }
}
