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
        private int cpuTemperature = -1;

        public SystemUsageModel()
        {
            computer = new Computer()
            {
                IsCpuEnabled = true,
                IsGpuEnabled = true,
                IsMemoryEnabled = true
            };
            computer.Open();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
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
                var (cpuPercentage, cpuTemp) = CpuUsage();
                SelectedInfo = new SystemUsageInfo
                {
                    cpuUsage = cpuPercentage,
                    cpuTemperature = cpuTemp,
                    ramUsage = RamPercentage(),
                    gpuUsage = GpuPercentage()
                };
            });
            prevCpuUsage = percCpuUsage;
        }

        private (int, int) CpuUsage()
        {
            try
            {
                foreach (var hardwareItem in computer.Hardware)
                {
                    if (hardwareItem.HardwareType == HardwareType.Cpu)
                    {
                        hardwareItem.Update();
                        foreach (var sensor in hardwareItem.Sensors)
                        {
                            if (sensor.SensorType == SensorType.Load && sensor.Name.Contains("Total"))
                            {
                                percCpuUsage = (int)sensor.Value.GetValueOrDefault();
                            }
                            if (sensor.SensorType == SensorType.Temperature && sensor.Name.Contains("Core"))
                            {
                                cpuTemperature = (int)sensor.Value.GetValueOrDefault(); // need to fix
                            }                         
                        }
                        break;
                    }
                }
                // set first value for prevCpuUsage
                if (prevCpuUsage == -1) { prevCpuUsage = percCpuUsage; }

                // protect for value above 100 percent from LibreHardwareMonitor issue
                if (percCpuUsage > 100) { percCpuUsage = prevCpuUsage; }

                return (percCpuUsage, cpuTemperature);
            }

            catch (Exception e)
            {
                return (-1, -1);
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
