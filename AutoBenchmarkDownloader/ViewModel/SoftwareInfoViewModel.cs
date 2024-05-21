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

        public RelayCommand DownloadCommand => new(
            execute => DownloadOperations.DownloadSelectedSoftware(CurrentState),
            canExecute => CurrentState.SoftwareInfos.Any(info => info.Download));
        
        public RelayCommand SaveConfigCommand => new(
            execute => _yamlOperations.SaveConfig());

        public RelayCommand ResetConfigCommand => new(
            execute => _yamlOperations.AddDataFromState(_yamlOperations.DeepCopyState(_yamlOperations._defaultState)));

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
    }
}
