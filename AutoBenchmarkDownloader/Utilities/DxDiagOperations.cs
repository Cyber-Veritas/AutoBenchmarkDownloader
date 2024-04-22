using AutoBenchmarkDownloader.Model;
using AutoBenchmarkDownloader.MVVM;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Management;
using System.Windows.Markup;

namespace AutoBenchmarkDownloader.Utilities
{
    class DxDiagOperations : SystemHardwareInfo
    {

        public ObservableCollection<DxDiagInfo> _dxdiaginfos;
        //private string CpuName;

        public DxDiagOperations(ObservableCollection<DxDiagInfo> dxDiagInfos) { 
            _dxdiaginfos = dxDiagInfos;

            SetInfo();
        }

        private void SetInfo()
        {
            List<DxDiagInfo> dxDiagInfo = new List<DxDiagInfo>()
            {
                new() {CpuModel = GetHardwareInfo("Win32_Processor", "Name", "CPU"),
                Ram = RamInfo(),
                Motherboard = GetHardwareInfo("Win32_BaseBoard", "Product", "MOBO"),
                Bios = "BIOS: "+GetHardwareInfo("Win32_BIOS", "Name", "BIOS"),
                Os = GetHardwareInfo("Win32_OperatingSystem", "Caption", "OS")+" "+GetHardwareInfo("Win32_OperatingSystem", "Version", "VERSION"),
                Gpu = GetHardwareInfo("Win32_VideoController", "Name", "GPU"),
                DirectX = GetHardwareInfo("Win32_DirectXVersion", "Caption", "DX") } 
            };

            _dxdiaginfos.Clear();
            foreach (var info in dxDiagInfo)
            {
                _dxdiaginfos.Add(info);
            }
        }

        private static string RamInfo()
        {
            try
            {
                int numberOfModules = 0;

                ManagementObjectSearcher searcherModule = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PhysicalMemoryArray");
                foreach (ManagementObject itemModule in searcherModule.Get())
                {
                    numberOfModules = Convert.ToInt32(itemModule["MemoryDevices"]);
                    break;
                }

                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PhysicalMemory");
                foreach (ManagementObject item in searcher.Get())
                {
                    string sizeInGB_SM = BytesToGB((ulong)item["Capacity"], (ulong)numberOfModules);
                    string sizeInGB_MM = BytesToGB((ulong)item["Capacity"], (ulong)numberOfModules, true);
                    string speed = item["Speed"].ToString() + " MHz";

                    return sizeInGB_MM + " GB " + "("+ numberOfModules + "x" + sizeInGB_SM + ")" + " " + speed;
                }

                return "[RAM info ERROR]";
            }
            catch (Exception e)
            {
                return "[RAM info ERROR]";
            }
        }

        static string BytesToGB(ulong bytes, ulong modules, bool type = false)
        {
            if (type)
            {
                return (modules * (bytes / Math.Pow(1024, 3))).ToString();              
            }
            return ((bytes / Math.Pow(1024, 3))).ToString();
        }
    }
}
