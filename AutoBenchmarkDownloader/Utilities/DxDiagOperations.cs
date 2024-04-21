using AutoBenchmarkDownloader.Model;
using System.Collections.ObjectModel;
using System.Management;

namespace AutoBenchmarkDownloader.Utilities
{
    class DxDiagOperations
    {

        public ObservableCollection<DxDiagInfo> _dxdiaginfos;
        private string CpuName;

        public DxDiagOperations(ObservableCollection<DxDiagInfo> dxDiagInfos) { 
            _dxdiaginfos = dxDiagInfos;

            SetInfo();
        }

        private void SetInfo()
        {
            Cpu();
            List<DxDiagInfo> dxDiagInfo = new List<DxDiagInfo>()
            {
                new() {CpuModel = CpuName,
                Ram = "RAM" ,
                Motherboard = "MB" ,
                Bios = "BIOS" ,
                Os = "OS" ,
                Gpu = "GPU" ,
                DirectX = "DirectX" }
            };

            _dxdiaginfos.Clear();
            foreach (var info in dxDiagInfo)
            {
                _dxdiaginfos.Add(info);
            }
        }

        private void Cpu()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
            foreach (ManagementObject item in searcher.Get())
            {
                CpuName = (string)item["Name"];
            }
        }

    }
}
