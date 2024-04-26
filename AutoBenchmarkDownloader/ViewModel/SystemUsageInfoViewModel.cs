using AutoBenchmarkDownloader.Model;
using AutoBenchmarkDownloader.MVVM;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Management;
using System.Windows.Threading;

namespace AutoBenchmarkDownloader.ViewModel
{
    internal class SystemUsageInfoViewModel : ViewModelBase
    {
        public SystemUsageInfo selectedInfo;
        private DispatcherTimer timer;

        public SystemUsageInfoViewModel() 
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        public SystemUsageInfo SelectedInfo { 
            get { return selectedInfo; }
            set { selectedInfo = value; OnPropertyChanged(); }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            SelectedInfo = new SystemUsageInfo
            {
                cpuUsage = CpuPercentage(),
                cpuTemp = -1,
                ramUsage = RamPercentage(),
                gpuUsage = GpuPercentage(),
                gpuTemp = -1,
            };
        }
        private int CpuPercentage()
        {
            try
            {
                return 1;
            }

            catch (Exception ex)
            {
                return -1;
            }
        }

        private int RamPercentage()
        {
            try
            {
                double maxMemory = 0;
                double freeMemory = 0;
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem");
                foreach (ManagementObject obj in searcher.Get())
                {
                    maxMemory = Double.Parse(obj["TotalVisibleMemorySize"].ToString()) / 1024;
                    freeMemory = Double.Parse(obj["FreePhysicalMemory"].ToString()) / 1024;
                    break;
                }
                int percRamUsage = 100 - Convert.ToInt32((freeMemory / maxMemory) * 100);

                return percRamUsage;
            }

            catch(Exception e) 
            { 
                return -1;
            }
        }

        private int GpuPercentage()
        {
            try
            {
                double gpuDecode = 0;
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PerfFormattedData_GPUPerformanceCounters_GPUEngine");
                ManagementObjectCollection collection = searcher.Get();

                foreach (ManagementObject obj in collection)
                {
                    gpuDecode += (double)obj["UtilizationPercentage"];
                }
                return Convert.ToInt32(gpuDecode);
            }

            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}
