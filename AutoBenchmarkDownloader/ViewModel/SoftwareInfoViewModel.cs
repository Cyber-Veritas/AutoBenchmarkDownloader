using System.IO;
using AutoBenchmarkDownloader.Model;
using AutoBenchmarkDownloader.MVVM;
using AutoBenchmarkDownloader.Utilities;
using Microsoft.Win32;

namespace AutoBenchmarkDownloader.ViewModel
{
    internal class SoftwareInfoViewModel
    {
        public State CurrentState { get; set; }

        private readonly YamlOperations _yamlOperations;

        private bool _isDownloading = false;

        public RelayCommand DownloadCommand => new(
            execute => DownloadSoftwareAsync(),
            canExecute => CurrentState.SoftwareInfos.Any(info => info.Download) && !_isDownloading);
        
        public RelayCommand SaveConfigCommand => new(
            execute => _yamlOperations.SaveConfig());

        public RelayCommand ResetConfigCommand => new(
            execute => ResetConfig());

        public RelayCommand ChooseOutputPathCommand => new(
            execute => ChooseOutputPath());

        public RelayCommand AddSoftwareCommand => new(
                       execute => _yamlOperations.AddSoftware());

        public RelayCommand EditSoftwareCommand => new(
            execute => _yamlOperations.EditSoftware((string)execute));


        public SoftwareInfoViewModel()
        {
            CurrentState = new State()
            {
                SoftwareInfos = [],
                OutputPath = ""
            };

            _yamlOperations = new YamlOperations(CurrentState);

            if (File.Exists(YamlOperations.DefaultYamlPath))
            {
                _yamlOperations.LoadConfig();
            }
            else
            {
                _yamlOperations.SaveDefaultConfig();
            }
        }

        private void ChooseOutputPath()
        {
            var dialog = new OpenFolderDialog
            {
                InitialDirectory = CurrentState.OutputPath
            };
            var result = dialog.ShowDialog();

            if (result == true)
            {
                CurrentState.OutputPath = dialog.FolderName;
            }
        }

        private async Task DownloadSoftwareAsync()
        {
            _isDownloading = true;
            await DownloadOperations.DownloadSelectedSoftware(CurrentState);
            _isDownloading = false;
        }

        private void ResetConfig()
        {
            _yamlOperations.AddDataFromState(_yamlOperations.DeepCopyState(_yamlOperations._defaultState));
            _yamlOperations.SaveConfig();
        }
    }
}
