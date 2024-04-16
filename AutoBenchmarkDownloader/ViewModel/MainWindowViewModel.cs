using System.Collections.ObjectModel;
using System.IO;
using AutoBenchmarkDownloader.Model;
using AutoBenchmarkDownloader.MVVM;
using System.Net.Http;
using System.Windows;

namespace AutoBenchmarkDownloader.ViewModel
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<SoftwareInfo> SoftwareInfos { get; set; }

        public RelayCommand DownloadCommand => new RelayCommand(execute => DownloadSelectedSoftware(), canExecute => SoftwareInfos.Any(info => info.Download));
        
        public MainWindowViewModel()
        {
            SoftwareInfos =
            [
                new SoftwareInfo { Name = "FurMark 2", Address = "https://geeks3d.com/dl/get/748", Download = true },
                new SoftwareInfo { Name = "CPU-Z", Address = "https://download.cpuid.com/cpu-z/cpu-z_2.09-en.zip", Download = false }
            ];
        }

        private SoftwareInfo selectedSoftwareInfo;

        public SoftwareInfo SelectedSoftwareInfo
        {
            get { return selectedSoftwareInfo; }
            set
            {
                selectedSoftwareInfo = value;
                OnPropertyChanged();
            }
        }

        private async void DownloadSelectedSoftware()
        {
            var itemsToDownload = SoftwareInfos.Where(info => info.Download);

            foreach (var item in itemsToDownload)
            {
                var url = item.Address;

                try
                {
                    using (var client = new HttpClient())
                    {
                        var response = await client.GetAsync(url);
                        response.EnsureSuccessStatusCode();

                        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), item.Name + ".zip");

                        using (var fileStream = File.Create(filePath))
                        {
                            await response.Content.CopyToAsync(fileStream);
                        }

                        MessageBox.Show("File downloaded successfully!"); 
                    }
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show($"Download failed: {ex.Message}");  
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}"); 
                }
            }

        }
    }
}
