using AutoBenchmarkDownloader.Model;
using AutoBenchmarkDownloader.MVVM;
using LibreHardwareMonitor.Hardware;
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
            // get advanced data
            List<RamModule> ramModules = RamInfo();
            string RamModuleInfo = ListToStringConverter(ramModules);
            List<CpuAdvanced> cpuAdvanceds = CpuAdvancedInfo();
            List<MotherboardAdvanced> motherboardAdvanceds = MotherboardAdvancedInfo();
            List<GpuAdvanced> gpuAdvanceds = GpuAdvancedInfo();
            List<SystemAdvanced> systemAdvanceds = SystemAdvancedInfo();

            HardwareInfo hardwareInfo = new HardwareInfo()
            {
                CpuAdvanceds = cpuAdvanceds,
                RamModules = ramModules,
                RamModulesInfo = RamModuleInfo,
                TotalRam = TotalRam,
                MotherboardAdvanceds = motherboardAdvanceds,
                GpuAdvanceds = gpuAdvanceds,
                SystemAdvanceds = systemAdvanceds
            };

            hardwareInfos.Add(hardwareInfo);
        }

        private List<CpuAdvanced> CpuAdvancedInfo()
        {
            List<CpuAdvanced> cpuAdvanceds = new List<CpuAdvanced>();

            try
            {
                var infoToGet = new List<string>
                {
                    "Name", "Description", "NumberOfCores", "NumberOfLogicalProcessors",
                    "CurrentClockSpeed"
                };

                var hardwareInfo = GetHardwareInfo("Win32_Processor", infoToGet, "CPU");
                // define cpu advanced information
                CpuAdvanced cpuAdvanced = new CpuAdvanced()
                {
                    Model = hardwareInfo["Name"],
                    CodeName = hardwareInfo["Description"],
                    Litography = "Unknown",
                    Cores = hardwareInfo["NumberOfCores"],
                    Threads = hardwareInfo["NumberOfLogicalProcessors"],
                    Frequency = hardwareInfo["CurrentClockSpeed"],
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
                var baseBoardInfoToGet = new List<string> { "Product", "Manufacturer", "SerialNumber" };
                var biosInfoToGet = new List<string> { "Name", "ReleaseDate" };

                var baseBoardInfo = GetHardwareInfo("Win32_BaseBoard", baseBoardInfoToGet, "MOBO");
                var biosInfo = GetHardwareInfo("Win32_BIOS", biosInfoToGet, "BIOS");

                MotherboardAdvanced motherboardAdvanced = new MotherboardAdvanced()
                {
                    Model = baseBoardInfo["Product"],
                    Bios = biosInfo["Name"],
                    BiosDate = ConvertDate(biosInfo["ReleaseDate"]),
                    Manufacturer = baseBoardInfo["Manufacturer"],
                    Chipset = "Unknown", 
                    SerialNumber = MotherboardSerialNumber(baseBoardInfo["SerialNumber"])
                };

                motherboardAdvanceds.Add(motherboardAdvanced);
            }

            catch (Exception e) 
            {
                Console.WriteLine("unable to find Motherboard info" + e.Message);
            }
            return motherboardAdvanceds;
        }

        private string MotherboardSerialNumber(string serialNumber)
        {
            if (serialNumber == "Default string") { serialNumber = "N/A"; }
            return serialNumber;
        }

        private List<GpuAdvanced> GpuAdvancedInfo()
        {
            List<GpuAdvanced> gpuAdvanceds = new List<GpuAdvanced>();

            try
            {
                var infoToGet = new List<string>
                {
                    "Caption", "AdapterCompatibility", "DriverVersion", "DriverDate",
                    "CurrentHorizontalResolution", "CurrentVerticalResolution", "CurrentRefreshRate"
                };

                var hardwareInfo = GetHardwareInfo("Win32_VideoController", infoToGet, "GPU");

                string gpuResolution = hardwareInfo["CurrentHorizontalResolution"] + "x" + hardwareInfo["CurrentVerticalResolution"];

                GpuAdvanced gpuAdvanced = new GpuAdvanced()
                {
                    Model = hardwareInfo["Caption"],
                    Manufacturer = hardwareInfo["AdapterCompatibility"],
                    VRAM = GpuVRAM(),
                    DriverVer = hardwareInfo["DriverVersion"],
                    DriverDate = ConvertDate(hardwareInfo["DriverDate"]),
                    Resolution = gpuResolution,
                    RefreshRate = hardwareInfo["CurrentRefreshRate"] + "Hz"
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

        private List<SystemAdvanced> SystemAdvancedInfo()
        {
            List<SystemAdvanced> systemAdvanceds = new List<SystemAdvanced>();
            try
            {
                var osInfoToGet = new List<string> { "Caption", "Version" };
                var computerSystemInfoToGet = new List<string> { "SystemType", "Name" };

                var osInfo = GetHardwareInfo("Win32_OperatingSystem", osInfoToGet, "OS");
                var computerSystemInfo = GetHardwareInfo("Win32_ComputerSystem", computerSystemInfoToGet, "Bit");

                SystemAdvanced systemAdvanced = new SystemAdvanced()
                {
                    Os = $"{osInfo["Caption"]} ver.{osInfo["Version"]}",
                    DiskLists = SystemDiskList(),
                    Bit = computerSystemInfo["SystemType"],
                    ComputerName = computerSystemInfo["Name"]
                };

                systemAdvanceds.Add(systemAdvanced);
            }

            catch (Exception e)
            {
                Console.WriteLine("unable to find System info" + e.Message);
            }

            return systemAdvanceds;
        }

        static string SystemDiskList()
        {
            string diskList = "\n";
            int id = 0;
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_DiskDrive");
                foreach (ManagementObject item in searcher.Get())
                {
                    diskList += id++ + ": ";
                    diskList += item["Model"].ToString() + " ";
                    diskList += BytesToGB((ulong)item["size"]) + " GB";
                    diskList += "\n";
                }    
            }

            catch (Exception e)
            {
                diskList = "cannot get disk info";
            }

            return diskList;
        }

        static string GpuVRAM()
        {
            string gpuVRAM = "";
            Computer computer = new Computer
            {
                IsGpuEnabled = true
            };

            computer.Open();

            foreach (var hardware in computer.Hardware)
            {
                if (hardware.HardwareType == HardwareType.GpuAmd || hardware.HardwareType == HardwareType.GpuNvidia || hardware.HardwareType == HardwareType.GpuIntel)
                {
                    hardware.Update();

                    foreach (var sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.SmallData && sensor.Name.Contains("GPU Memory Total"))
                        {
                            gpuVRAM = sensor.Value.GetValueOrDefault().ToString();
                            break;
                        }
                    }
                    break;
                }
            }

            computer.Close();

            return gpuVRAM;
        }

        static string BytesToGB(ulong bytes)
        {
            return Math.Round(((bytes / Math.Pow(1024, 3))), 1).ToString();
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
