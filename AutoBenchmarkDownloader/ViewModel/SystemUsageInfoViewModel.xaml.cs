using AutoBenchmarkDownloader.Model;
using AutoBenchmarkDownloader.MVVM;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using System.Management;
using System.Windows.Controls.Primitives;



namespace AutoBenchmarkDownloader.ViewModel
{
    public partial class SystemUsageInfoViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<SystemUsageInfo> Infos { get; set; }  

        public SystemUsageInfo selectedInfo;
        private DispatcherTimer timer;
        protected PerformanceCounter cpuCounter;
        //protected PerformanceCounter gpuCounter;
        protected PerformanceCounter ramCounter;
        private static double maxRam;
        private int availableRamPerc; 

        public SystemUsageInfoViewModel()
        {
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            //gpuCounter = new PerformanceCounter("GPU Engine", "Utilization Percentage");
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");

            Infos = new ObservableCollection<SystemUsageInfo>();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            LoadMaxRAM();
        }
        public SystemUsageInfo SelectedInfo
        {
            get { return selectedInfo; }
            set { selectedInfo = value; OnPropertyChanged(); }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // calculate used RAM in percentage
            availableRamPerc = CalculatePercentageRamUsage(maxRam, (double)ramCounter.NextValue());

            // change info values
            SelectedInfo = new SystemUsageInfo
            {
                cpuUsage = (int)cpuCounter.NextValue(),
                cpuTemp = -1,
                ramUsage = availableRamPerc,
                gpuUsage = -1, 
                gpuTemp = -1 
            };
        }

        private void LoadMaxRAM()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem");
            foreach (ManagementObject obj in searcher.Get())
            {
                double maxMemory = Double.Parse(obj["TotalVisibleMemorySize"].ToString());

                maxRam = maxMemory;
            }
        }

        private int CalculatePercentageRamUsage(double maxRAM, double freeRAM)
        {
            maxRAM = maxRAM / 1024;
            int percentage = 100 - Convert.ToInt32((freeRAM/maxRAM)*100);
            return percentage;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
