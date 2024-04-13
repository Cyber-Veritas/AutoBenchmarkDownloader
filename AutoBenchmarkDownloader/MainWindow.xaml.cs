using AutoBenchmarkDownloader.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
                new() { Name="Software A", Address="https://example.com/", Download=true },
                new() { Name="Software B", Address="https://example.com/", Download=false }
            ];
        }

        private ObservableCollection<SoftwareInfo> softwareInfos;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<SoftwareInfo> SoftwareInfos
        {
            get { return softwareInfos; }
            set 
            { 
                softwareInfos = value; 
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SoftwareInfos"));
            }
        }



    }
}