using AutoBenchmarkDownloader.Model;
using AutoBenchmarkDownloader.MVVM;
using AutoBenchmarkDownloader.Utilities;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Intrinsics.Arm;

namespace AutoBenchmarkDownloader.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<SoftwareInfo> SoftwareInfos { get; set; }
        public ObservableCollection<DxDiagInfo> dxDiagInfos { get; set; }

        private readonly YamlOperations _yamlOperations;
        private readonly DownloadOperations _downloadOperations;
        private readonly DxDiagOperations _dxDiagInfo;

        public RelayCommand DownloadCommand => new(
            execute => _downloadOperations.DownloadSelectedSoftware(),
            canExecute => SoftwareInfos.Any(info => info.Download));
        
        public RelayCommand SaveConfigCommand => new(
            execute => _yamlOperations.SaveConfig());

        public MainWindowViewModel()
        {
            SoftwareInfos = new ObservableCollection<SoftwareInfo>();
            dxDiagInfos = new ObservableCollection<DxDiagInfo>();
            _yamlOperations = new YamlOperations(SoftwareInfos);
            _downloadOperations = new DownloadOperations(SoftwareInfos);
            _dxDiagInfo = new DxDiagOperations(dxDiagInfos);

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
