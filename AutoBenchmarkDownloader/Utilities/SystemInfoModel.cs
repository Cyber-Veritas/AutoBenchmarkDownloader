using AutoBenchmarkDownloader.Model;
using AutoBenchmarkDownloader.MVVM;
using LibreHardwareMonitor.Hardware;
using System;
using System.Collections.ObjectModel;
using System.Management;
namespace AutoBenchmarkDownloader.Utilities
{
    class SystemInfoModel : SystemHardwareInfo
    {
        public ObservableCollection<DxDiagInfo> dxDiagInfos { get; set; }
        public string TotalRam;

        public SystemInfoModel()
        {
            dxDiagInfos = new ObservableCollection<DxDiagInfo>();
            string TotalRam = "";
            SetInfo();
        }

        private void SetInfo()
        {
            List<RamModule> ramModules = RamInfo();

            // Win32_PerfFormattedData_GPUPerformanceCounters_GPUAdapterMemory - gpu memory info

            string CpuModel = GetHardwareInfo("Win32_Processor", "Name", "CPU");
            string RamModuleInfo = ListToStringConverter(ramModules);
            string Motherboard = GetHardwareInfo("Win32_BaseBoard", "Product", "MOBO");
            string Bios = "BIOS: " + GetHardwareInfo("Win32_BIOS", "Name", "BIOS");
            string Os = GetHardwareInfo("Win32_OperatingSystem", "Caption", "OS") + " ver." + GetHardwareInfo("Win32_OperatingSystem", "Version", "VERSION");
            string Gpu = GetHardwareInfo("Win32_VideoController", "VideoProcessor", "GPU");
            string GpuDriverDate = "Driver Date: " + ConvertDateDriver(GetHardwareInfo("Win32_VideoController", "DriverDate", "GPU"));
            string GpuDriverVer = "Driver Version: " + GetHardwareInfo("Win32_VideoController", "DriverVersion", "GPU");
            string DirectX = GetHardwareInfo("Win32_DirectXVersion", "Caption", "DX");

            DxDiagInfo dxDiagInfo = new DxDiagInfo()
            {
                CpuModel = CpuModel,
                RamModules = ramModules,
                RamModulesInfo = RamModuleInfo,
                TotalRam = TotalRam,
                Motherboard = Motherboard,
                Bios = Bios,
                Os = Os,
                Gpu = Gpu,
                GpuDriverVer = GpuDriverVer,
                GpuDriverDate = GpuDriverDate,
                DirectX = DirectX,
            };

            dxDiagInfos.Add(dxDiagInfo);
        }

        private List<RamModule> RamInfo()
        {
            List<RamModule> ramModulesList = new List<RamModule>();

            try
            {
                ulong totalRamCounter = 0;
                int id = 0;
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PhysicalMemory");

                foreach (ManagementObject item in searcher.Get())
                {
                    ulong ramCapacity = (ulong)item["Capacity"];

                    // define ram modules
                    RamModule ramModule = new RamModule()
                    {
                        id = id,
                        DeviceLocator = "[" + (string)item["DeviceLocator"] + "]",
                        Manufacturer = (string)item["Manufacturer"],
                        Code = (string)item["PartNumber"],
                        Speed = item["Speed"].ToString() + "MHz",
                        Size = BytesToGB(ramCapacity) + "GB"
                    };

                    ramModulesList.Add(ramModule);
                    totalRamCounter += ramCapacity;
                    id++;
                }
                TotalRam = BytesToGB(totalRamCounter) + "GB";
            }

            catch (Exception e)
            {
                Console.WriteLine("unable to find RAM info" + e.Message);
            }

            return ramModulesList;
        }

        static string BytesToGB(ulong bytes)
        {
            return ((bytes / Math.Pow(1024, 3))).ToString();
        }

        static string ListToStringConverter(List<RamModule> ramModules)
        {
            string ramModulesString = "";

            for (int i = 0; i < ramModules.Count; i++)
            {
                ramModulesString += ramModules[i].id + ":";
                ramModulesString += " " + ramModules[i].DeviceLocator;
                ramModulesString += " " + ramModules[i].Size;
                ramModulesString += " " + ramModules[i].Code;

                if (i < ramModules.Count - 1)
                {
                    ramModulesString += "\n\n";
                }
            }
            return ramModulesString;
        }

        static string ConvertDateDriver(string dateDriver)
        {
            if (dateDriver.Contains("ERROR") == true)
            {
                return "";
            }
            else
            {
                string year = dateDriver.Substring(0, Math.Min(4, dateDriver.Length));
                string day = dateDriver.Substring(4, Math.Min(2, dateDriver.Length));
                string month = dateDriver.Substring(6, Math.Min(2, dateDriver.Length));
                return day + "." + month + "." + year;
            }
        }
    }
}
