using AutoBenchmarkDownloader.Model;
using AutoBenchmarkDownloader.MVVM;
using System.Collections.ObjectModel;
using System.Management;
namespace AutoBenchmarkDownloader.Utilities
{
    class SystemInfoModel : SystemHardwareInfo
    {
        public ObservableCollection<HardwareInfo> hardwareInfos { get; set; }
        public string TotalRam;

        public SystemInfoModel()
        {
            hardwareInfos = new ObservableCollection<HardwareInfo>();
            string TotalRam = "";
            SetInfo();
        }

        private void SetInfo()
        {
            List<RamModule> ramModules = RamInfo();
            List<CpuAdvanced> cpuAdvanceds = CpuAdvancedInfo();
            List<MotherboardAdvanced> motherboardAdvanceds = MotherboardAdvancedInfo();
            List<GpuAdvanced> gpuAdvanceds = GpuAdvancedInfo();

            // Win32_PerfFormattedData_GPUPerformanceCounters_GPUAdapterMemory - gpu memory info

            string CpuModel = GetHardwareInfo("Win32_Processor", "Name", "CPU");
            string RamModuleInfo = ListToStringConverter(ramModules);
            string Motherboard = GetHardwareInfo("Win32_BaseBoard", "Product", "MOBO");
            string Bios = "BIOS: " + GetHardwareInfo("Win32_BIOS", "Name", "BIOS");
            string Os = GetHardwareInfo("Win32_OperatingSystem", "Caption", "OS") + " ver." + GetHardwareInfo("Win32_OperatingSystem", "Version", "VERSION");
            string Gpu = GetHardwareInfo("Win32_VideoController", "Caption", "GPU");
            string GpuDriverDate = "Driver Date: " + ConvertDate(GetHardwareInfo("Win32_VideoController", "DriverDate", "GPU"));
            string GpuDriverVer = "Driver Version: " + GetHardwareInfo("Win32_VideoController", "DriverVersion", "GPU");
            string DirectX = GetHardwareInfo("Win32_DirectXVersion", "Caption", "DX");

            HardwareInfo hardwareInfo = new HardwareInfo()
            {
                CpuModel = CpuModel,
                RamModulesInfo = RamModuleInfo,
                TotalRam = TotalRam,
                Motherboard = Motherboard,
                Bios = Bios,
                Os = Os,
                Gpu = Gpu,
                GpuDriverVer = GpuDriverVer,
                GpuDriverDate = GpuDriverDate,
                DirectX = DirectX,

                CpuAdvanceds = cpuAdvanceds,
                RamModules = ramModules,
                MotherboardAdvanceds = motherboardAdvanceds,
                GpuAdvanceds = gpuAdvanceds
            };

            hardwareInfos.Add(hardwareInfo);
        }

        private List<CpuAdvanced> CpuAdvancedInfo()
        {
            List<CpuAdvanced> cpuAdvanceds = new List<CpuAdvanced>();

            try
            {
                // define cpu advanced information
                CpuAdvanced cpuAdvanced = new CpuAdvanced()
                {
                    Model = GetHardwareInfo("Win32_Processor", "Name", "CPU_Model"),
                    CodeName = GetHardwareInfo("Win32_Processor", "Description", "CPU_CodeName"),
                    Litography = "Unknown",
                    Cores = GetHardwareInfo("Win32_Processor", "NumberOfCores", "CPU_Cores"),
                    Threads = GetHardwareInfo("Win32_Processor", "NumberOfLogicalProcessors", "CPU_Threads"),
                    Frequency = GetHardwareInfo("Win32_Processor", "CurrentClockSpeed", "CPU_Frequency"),
                    TDP = "Unknown",
                    Platform = "Unknown"
                };

                cpuAdvanceds.Add(cpuAdvanced);
            }
            catch (Exception e)
            {
                Console.WriteLine("unable to find CPU info" + e.Message);
            }

            return cpuAdvanceds;
        }

        private List<MotherboardAdvanced> MotherboardAdvancedInfo() 
        {
            List<MotherboardAdvanced> motherboardAdvanceds = new List<MotherboardAdvanced>();
            try
            {
                // define motherboard advanced information
                MotherboardAdvanced motherboardAdvanced = new MotherboardAdvanced()
                {
                    Model = GetHardwareInfo("Win32_BaseBoard", "Model", "MOBO_Model"),
                    Bios = GetHardwareInfo("Win32_BIOS", "Name", "BIOS"),
                    BiosDate = ConvertDate(GetHardwareInfo("Win32_BIOS", "ReleaseDate", "BIOS_Date")),
                    Manufacturer = GetHardwareInfo("Win32_BaseBoard", "Manufacturer", "MOBO_Manufacturer"),
                    Chipset = "Unknown",
                    SerialNumber = GetHardwareInfo("Win32_BaseBoard", "SerialNumber", "MOBO_SerialNumber")
                };

                motherboardAdvanceds.Add(motherboardAdvanced);
            }

            catch (Exception e) 
            {
                Console.WriteLine("unable to find Motherboard info" + e.Message);
            }
            return motherboardAdvanceds;
        }

        private List<GpuAdvanced> GpuAdvancedInfo()
        {
            List<GpuAdvanced> gpuAdvanceds = new List<GpuAdvanced>();
            try
            {
                // define gpu advanced information
                GpuAdvanced gpuAdvanced = new GpuAdvanced()
                {
                    Model = GetHardwareInfo("Win32_VideoController", "Caption", "GPU_Model"),
                    Manufacturer = GetHardwareInfo("Win32_VideoController", "AdapterCompatibility", "GPU_Manufacturer"),
                    VRAM = "",
                    DriverVer = GetHardwareInfo("Win32_VideoController", "DriverVersion", "GPU"),
                    DriverDate = ConvertDate(GetHardwareInfo("Win32_VideoController", "DriverDate", "GPU")),
                    ModelCode = ""
                };

                gpuAdvanceds.Add(gpuAdvanced);
            }

            catch (Exception e)
            {
                Console.WriteLine("unable to find Gpu info" + e.Message);
            }
            return gpuAdvanceds;
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
                        Speed = item["Speed"].ToString() + " MHz",
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
                ramModulesString += "- " + ramModules[i].Manufacturer;

                if (i < ramModules.Count - 1)
                {
                    ramModulesString += "\n\n";
                }
            }
            return ramModulesString;
        }

        static string ConvertDate(string componentDate)
        {
            if (componentDate.Contains("ERROR") == true)
            {
                return "N/A";
            }
            else
            {
                string year = componentDate.Substring(0, Math.Min(4, componentDate.Length));
                string day = componentDate.Substring(4, Math.Min(2, componentDate.Length));
                string month = componentDate.Substring(6, Math.Min(2, componentDate.Length));
                return day + "." + month + "." + year;
            }
        }
    }
}
