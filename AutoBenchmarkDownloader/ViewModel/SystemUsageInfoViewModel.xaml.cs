using AutoBenchmarkDownloader.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Threading;

namespace AutoBenchmarkDownloader.ViewModel
{
    public partial class SystemUsageInfoViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<SystemUsageInfo> Infos { get; set; }  

        private SystemUsageInfo selectedInfo;

        public SystemUsageInfo SelectedInfo
        {
            get { return selectedInfo; }
            set { selectedInfo = value; OnPropertyChanged(); }
        }

        private DispatcherTimer timer;
        protected PerformanceCounter cpuCounter;
        protected PerformanceCounter gpuCounter;
        protected PerformanceCounter ramCounter;
        

        public SystemUsageInfoViewModel()
        {
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            gpuCounter = new PerformanceCounter("GPU Engine", "Utilization Percentage");
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");

            Infos = new ObservableCollection<SystemUsageInfo>();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            SelectedInfo = new SystemUsageInfo
            {
                cpuUsage = cpuCounter+"", 
                cpuTemp = "cpuTemp", 
                ramUsage = "Free MB: "+ramCounter.NextValue(), 
                gpuUsage = gpuCounter+"%", 
                gpuTemp = "gpuTemp" 
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
