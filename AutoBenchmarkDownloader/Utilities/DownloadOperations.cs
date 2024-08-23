using AutoBenchmarkDownloader.Model;
using System.IO;
using System.Net.Http;
using System.Windows;

namespace AutoBenchmarkDownloader.Utilities
{
    internal static class DownloadOperations
    {

        public static async Task DownloadSelectedSoftware(State currentState, Action<int> updateDownloadProgress)
        {
            if (!Path.Exists(currentState.OutputPath))
            {
                var result = MessageBox.Show("Defined path does not exist. Do you want to create it?", "Warning",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    Directory.CreateDirectory(currentState.OutputPath);
                }
                else
                {
                    return;
                }
            }

            var itemsToDownload = currentState.SoftwareInfos.Where(info => info.Download);
            Directory.CreateDirectory(Path.Combine(currentState.OutputPath, "Benchmark"));

            var totalItems = itemsToDownload.Count();
            var completedItems = 0;

            foreach (var item in itemsToDownload)
            {
                var url = item.Address;

                try
                {
                    using (var client = new HttpClient())
                    {
                        var response = await client.GetAsync(url);
                        response.EnsureSuccessStatusCode();

                        var originalFileName = response.RequestMessage?.RequestUri?.Segments.Last();

                        var fileName = originalFileName ?? item.Name + ".zip";

                        var filePath = Path.Combine(currentState.OutputPath, "Benchmark", fileName);

                        using (var fileStream = File.Create(filePath))
                        {
                            await response.Content.CopyToAsync(fileStream);
                        }

                        //MessageBox.Show("File downloaded successfully!"); 
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

                completedItems++;
                var progressValue = (completedItems * 100) / totalItems;
                updateDownloadProgress(progressValue);
            }
        }
    }
}
