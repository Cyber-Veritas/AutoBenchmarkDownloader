using AutoBenchmarkDownloader.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        //private EventHandler Timer_Tick;

        public SystemUsageInfoViewModel()
        {
            Infos = new ObservableCollection<SystemUsageInfo>();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // TEMP - random numbers 
            Random random = new Random();
            SelectedInfo = new SystemUsageInfo
            {
                cpuUsage = random.Next(0, 101), 
                cpuTemp = random.Next(30, 80), 
                ramUsage = random.Next(0, 101), 
                gpuUsage = random.Next(0, 101), 
                gpuTemp = random.Next(30, 80) 
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
