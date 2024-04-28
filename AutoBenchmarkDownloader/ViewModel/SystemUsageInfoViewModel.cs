using AutoBenchmarkDownloader.Model;
using AutoBenchmarkDownloader.MVVM;
using System.Diagnostics;
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
            Task.Run(() =>
            {
                SelectedInfo = new SystemUsageInfo
                {
                    cpuUsage = CpuPercentage(),
                    ramUsage = RamPercentage(),
                    gpuUsage = GpuPercentage()
                };
            });
        }

        // it is only "_Total" info
        private int CpuPercentage()
        {
            try
            {
                int percCpuUsage = -1;
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PerfFormattedData_PerfOS_Processor WHERE Name='_Total'");
                foreach (ManagementObject obj in searcher.Get())
                {
                    percCpuUsage = Convert.ToInt32(obj["PercentProcessorTime"]);
                    break;
                }

                if (percCpuUsage == 0) { percCpuUsage = 1; }

                return percCpuUsage;
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

        // based on https://stackoverflow.com/questions/56830434/c-sharp-get-total-usage-of-gpu-in-percentage
        private int GpuPercentage()
        {
            try
            {
                var category = new PerformanceCounterCategory("GPU Engine");
                var counterNames = category.GetInstanceNames();

                List<PerformanceCounter> gpuCounters = new List<PerformanceCounter>();

                gpuCounters = counterNames
                                    .Where(counterName => counterName.EndsWith("engtype_3D"))
                                    .SelectMany(counterName => category.GetCounters(counterName))
                                    .Where(counter => counter.CounterName.Equals("Utilization Percentage"))
                                    .ToList();

                gpuCounters.ForEach(x => x.NextValue());
                float percGpuUsageFloat = gpuCounters.Sum(x => x.NextValue());
                int percGpuUsageInt = (int)percGpuUsageFloat;

                return percGpuUsageInt;
            }

            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}
