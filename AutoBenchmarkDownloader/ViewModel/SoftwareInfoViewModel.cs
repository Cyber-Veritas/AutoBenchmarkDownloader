using System.IO;
using System.Windows;
using AutoBenchmarkDownloader.Model;
using AutoBenchmarkDownloader.MVVM;
using AutoBenchmarkDownloader.Utilities;
using Microsoft.Win32;

namespace AutoBenchmarkDownloader.ViewModel
{
    internal class SoftwareInfoViewModel : ViewModelBase
    {
        private static SoftwareInfoViewModel _instance;
        public static SoftwareInfoViewModel Instance => _instance ?? (_instance = new SoftwareInfoViewModel());

        public State CurrentState { get; set; }

        private readonly YamlOperations _yamlOperations;

        private bool _isDownloading = false;

        private int _downloadProgress;
        public int DownloadProgress
        {
            get => _downloadProgress;
            set
            {
                _downloadProgress = value;
                OnPropertyChanged();
            }
        }

        private string _downloadButtonText = "Download";
        public string DownloadButtonText
        {
            get => _downloadButtonText;
            set
            {
                _downloadButtonText = value;
                OnPropertyChanged();
            }
        }


        public RelayCommand DownloadCommand => new(
            execute => DownloadSoftwareAsync(),
            canExecute => CurrentState.SoftwareInfos.Any(info => info.Download) && !_isDownloading);
        
        public RelayCommand SaveConfigCommand => new(
            execute => _yamlOperations.SaveConfig());

        public RelayCommand ResetConfigCommand => new(
            execute => ResetConfig());

        public RelayCommand ResetOutputPathCommand => new(
            execute => _yamlOperations.RestoreDefaultOutputPath());

        public RelayCommand ChooseOutputPathCommand => new(
            execute => ChooseOutputPath());

        public RelayCommand ApplyOutputPathCommand => new(
            execute => ApplyOutputPath((string)execute));

        public RelayCommand AddSoftwareCommand => new(
                       execute => _yamlOperations.AddSoftware());

        public RelayCommand RestoreDefaultSoftwareCommand => new(
            execute => _yamlOperations.RestoreDefaultSoftwareInfos());

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
                _yamlOperations.SaveConfig();
            }
        }

        private void ApplyOutputPath(string path)
        {
            if (Directory.Exists(path))
            {
                CurrentState.OutputPath = path;
                _yamlOperations.SaveConfig();
            }
            else
            {
                var result = MessageBox.Show(
                    $"The directory '{path}' does not exist. Do you want to create it?", 
                    "Create Directory", 
                    MessageBoxButton.YesNo, 
                    MessageBoxImage.Question);

                if (result != MessageBoxResult.Yes) return;
                try
                {
                    Directory.CreateDirectory(path);
                    CurrentState.OutputPath = path;
                    _yamlOperations.SaveConfig();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to create directory: {ex.Message}", 
                        "Error", 
                        MessageBoxButton.OK, 
                        MessageBoxImage.Error);
                }
            }
        }

        private void UpdateDownloadProgress(int value)
        {
            DownloadProgress = value;
        }

        private async Task DownloadSoftwareAsync()
        {
            _isDownloading = true;
            DownloadButtonText = "Downloading...";
            DownloadProgress = 0;
            await DownloadOperations.DownloadSelectedSoftware(CurrentState, UpdateDownloadProgress);
            DownloadButtonText = "Complete";
            await Task.Delay(5000);
            DownloadProgress = 0;
            DownloadButtonText = "Download";
            _isDownloading = false;
        }

        private void ResetConfig()
        {
            _yamlOperations.AddDataFromState(_yamlOperations.DeepCopyState(_yamlOperations._defaultState));
            _yamlOperations.SaveConfig();
        }
    }
}
