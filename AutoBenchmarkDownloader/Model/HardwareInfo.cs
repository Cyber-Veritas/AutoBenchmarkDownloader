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

    public class CpuAdvanced
    {
        public required string Model { get; set; }
        public required string CodeName { get; set; }
        public required string Litography { get; set; }
        public required string Cores { get; set; }
        public required string Threads { get; set; }
        public required string Frequency { get; set; }
        public required string TDP { get; set; }
        public required string Platform { get; set; }

    }

    public class MotherboardAdvanced
    {
        public required string Model { get; set; }
        public required string Bios { get; set; }
        public required string BiosDate { get; set; }
        public required string Manufacturer { get; set; }
        public required string Chipset { get; set; }
        public required string SerialNumber { get; set; }
    }

    public class GpuAdvanced
    {
        public required string Model { get; set; }
        public required string Manufacturer { get; set; }
        public required string VRAM { get; set; }
        public required string DriverVer { get; set; }
        public required string DriverDate { get; set; }
        public required string ModelCode { get; set; }
    }

    public class HardwareInfo
    {
        public required string CpuModel { get; set; }
        public required string RamModulesInfo { get; set; }
        public required string TotalRam { get; set; }
        public required string Motherboard { get; set; }
        public required string Bios { get; set; }
        public required string Os { get; set; }
        public required string Gpu { get; set; }
        public required string GpuDriverVer { get; set; }
        public required string GpuDriverDate { get; set; }
        public required string DirectX { get; set; }
        public required List<CpuAdvanced> CpuAdvanceds { get; set; }
        public required List<RamModule> RamModules { get; set; }
        public required List<MotherboardAdvanced> MotherboardAdvanceds { get; set; }
        public required List<GpuAdvanced> GpuAdvanceds { get; set; }

        public HardwareInfo()
        {
            CpuAdvanceds = new List<CpuAdvanced>();
            RamModules = new List<RamModule>();
            MotherboardAdvanceds = new List<MotherboardAdvanced>();
            GpuAdvanceds = new List<GpuAdvanced>();
        }
    }
}
