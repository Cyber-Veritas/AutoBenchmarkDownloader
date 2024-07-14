using AutoBenchmarkDownloader.Model;
using AutoBenchmarkDownloader.MVVM;
using System.Diagnostics;
using System.Management;
using System.Windows.Threading;
using LibreHardwareMonitor.Hardware;

namespace AutoBenchmarkDownloader.Utilities
{
    class SystemUsageModel : ViewModelBase
    {
        public SystemUsageInfo selectedInfo;
        private DispatcherTimer timer;
        private Computer computer;

        private int percCpuUsage = -1;
        private int prevCpuUsage = -1;

        public SystemUsageModel()
        {
            computer = new Computer()
            {
                IsCpuEnabled = true
            };  

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        public void SetRefreshInterval(int interval)
        {
            timer.Interval = TimeSpan.FromMilliseconds(interval);
        }

        public SystemUsageInfo SelectedInfo
        {
            get { return selectedInfo; }
            set { selectedInfo = value; OnPropertyChanged(); }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                SelectedInfo = new SystemUsageInfo
                {
                    cpuUsage = CpuUsage(),
                    ramUsage = RamPercentage(),
                    gpuUsage = GpuPercentage()
                };
            });
            prevCpuUsage = percCpuUsage;
        }

        private int CpuUsage()
        {
            try
            {
                computer.Open();
                foreach (IHardware hardwareItem in computer.Hardware)
                {
                    if (hardwareItem.HardwareType == HardwareType.Cpu)
                    {
                        hardwareItem.Update();
                        foreach (ISensor sensor in hardwareItem.Sensors)
                        {
                            if (sensor.SensorType == SensorType.Load && sensor.Name.Contains("Total"))
                            {
                                percCpuUsage = (int)sensor.Value.GetValueOrDefault();
                                break;
                            }
                        }
                        break;
                    }
                }
                // set first value for prevCpuUsage
                if (prevCpuUsage == -1) { prevCpuUsage = percCpuUsage; }

                // protect for value above 100 percent from LibreHardwareMonitor issue
                if (percCpuUsage > 100) { percCpuUsage = prevCpuUsage; }

                return percCpuUsage;
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
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

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
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

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return -1;
            }
        }
    }
}
