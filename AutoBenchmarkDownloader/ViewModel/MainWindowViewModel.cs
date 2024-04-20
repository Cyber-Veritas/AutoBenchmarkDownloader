using AutoBenchmarkDownloader.Model;
using AutoBenchmarkDownloader.MVVM;
using AutoBenchmarkDownloader.Utilities;
using System.Collections.ObjectModel;
using System.IO;

namespace AutoBenchmarkDownloader.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<SoftwareInfo> SoftwareInfos { get; set; }

        private readonly YamlOperations _yamlOperations;
        private readonly DownloadOperations _downloadOperations;

        public RelayCommand DownloadCommand => new(
            execute => _downloadOperations.DownloadSelectedSoftware(),
            canExecute => SoftwareInfos.Any(info => info.Download));
        
        public RelayCommand SaveConfigCommand => new(
            execute => _yamlOperations.SaveConfig());
        

        public MainWindowViewModel()
        {
            SoftwareInfos = new ObservableCollection<SoftwareInfo>();
            _yamlOperations = new YamlOperations(SoftwareInfos);
            _downloadOperations = new DownloadOperations(SoftwareInfos);

            if (File.Exists(YamlOperations.DefaultYamlPath))
            {
                _yamlOperations.LoadConfig();
            }
            else
            {
                _yamlOperations.SaveDefaultConfig();
            }
        }


        private SoftwareInfo _selectedSoftwareInfo;

        public SoftwareInfo SelectedSoftwareInfo
        {
            get { return _selectedSoftwareInfo; }
            set
            {
                _selectedSoftwareInfo = value;
                OnPropertyChanged();
            }
        }
    }
}
