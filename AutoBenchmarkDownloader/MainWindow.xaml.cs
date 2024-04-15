using AutoBenchmarkDownloader.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace AutoBenchmarkDownloader
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();
            SoftwareInfos =
            [
                new SoftwareInfo { Name="Software A", Address="https://example.com/", Download=true },
                new SoftwareInfo { Name="Software B", Address="https://example.com/", Download=false }
            ];
        }

        private ObservableCollection<SoftwareInfo> _softwareInfos;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<SoftwareInfo> SoftwareInfos
        {
            get { return _softwareInfos; }
            set 
            { 
                _softwareInfos = value;
                OnPropertyChanged("SoftwareInfos");
                
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}