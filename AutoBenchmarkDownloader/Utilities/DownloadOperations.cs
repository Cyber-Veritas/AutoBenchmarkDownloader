using AutoBenchmarkDownloader.Model;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Windows;

namespace AutoBenchmarkDownloader.Utilities
{
    internal class DownloadOperations
    {
        private ObservableCollection<SoftwareInfo> _softwareInfos;

        public DownloadOperations(ObservableCollection<SoftwareInfo> softwareInfos)
        {
            _softwareInfos = softwareInfos;
        }

        public async void DownloadSelectedSoftware()
        {
            var itemsToDownload = _softwareInfos.Where(info => info.Download);

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
