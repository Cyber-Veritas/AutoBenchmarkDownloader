using AutoBenchmarkDownloader.Model;
using AutoBenchmarkDownloader.MVVM;
using System.Collections.ObjectModel;
using System.Management;
using System.Security.Policy;

namespace AutoBenchmarkDownloader.ViewModel
{
    internal class DxDiagInfoViewModel : SystemHardwareInfo
    {
        public ObservableCollection<DxDiagInfo> dxDiagInfos { get; set; }
        public string TotalRam;

        public DxDiagInfoViewModel() 
        {
            dxDiagInfos = new ObservableCollection<DxDiagInfo>();
            string TotalRam = "";
            SetInfo();
        }

        private void SetInfo()
        {
            string CpuModel = GetHardwareInfo("Win32_Processor", "Name", "CPU");
            string Motherboard = GetHardwareInfo("Win32_BaseBoard", "Product", "MOBO");
            string Bios = "BIOS: " + GetHardwareInfo("Win32_BIOS", "Name", "BIOS");
            string Os = GetHardwareInfo("Win32_OperatingSystem", "Caption", "OS") + " " + GetHardwareInfo("Win32_OperatingSystem", "Version", "VERSION");
            string Gpu = GetHardwareInfo("Win32_VideoController", "Name", "GPU");
            string DirectX = GetHardwareInfo("Win32_DirectXVersion", "Caption", "DX");

            List<RamModule> ramModules = RamInfo();
            

            DxDiagInfo dxDiagInfo = new DxDiagInfo()
            {
                CpuModel = CpuModel,
                RamModules = ramModules,
                TotalRam = TotalRam,
                Motherboard = Motherboard,
                Bios = Bios,
                Os = Os,
                Gpu = Gpu,
                DirectX = DirectX
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
                        DeviceLocator = "["+(string)item["DeviceLocator"]+ "]",
                        Manufacturer = (string)item["Manufacturer"],
                        Code = (string)item["PartNumber"],
                        Speed = item["Speed"].ToString(),
                        Size = BytesToGB(ramCapacity) + "GB"
                    };

                    ramModulesList.Add(ramModule);
                    totalRamCounter += ramCapacity;
                    id ++;
                }
                TotalRam = BytesToGB(totalRamCounter)+"GB";
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
    }
}
