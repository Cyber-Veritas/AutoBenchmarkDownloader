using AutoBenchmarkDownloader.Model;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using AutoBenchmarkDownloader.ViewModel;
using YamlDotNet.Serialization;

namespace AutoBenchmarkDownloader.Utilities
{
    internal class YamlOperations
    {
        public const string DefaultYamlPath = "default.yaml";

        private ObservableCollection<SoftwareInfo> _softwareInfos;

        public YamlOperations(ObservableCollection<SoftwareInfo> softwareInfos)
        {
            _softwareInfos = softwareInfos;
        }

        Dictionary<string, List<SoftwareInfo>> _defaultData = new Dictionary<string, List<SoftwareInfo>>()
        {
            { "SoftwareInfos", new List<SoftwareInfo> {
                new() { Name = "FurMark 2", Address = "https://geeks3d.com/dl/get/748", Download = true },
                new() { Name = "CPU-Z", Address = "https://download.cpuid.com/cpu-z/cpu-z_2.09-en.zip", Download = false }
            }}
        };

        public void LoadConfig()
        {
            var deserializer = new DeserializerBuilder().Build();

            try
            {
                using (var reader = new StreamReader(DefaultYamlPath))
                {
                    var appConfig = deserializer.Deserialize<Dictionary<string, List<SoftwareInfo>>>(reader);
                    AddDataFromDictionary(appConfig);
                }
            }
            catch (Exception ex)
            {
                var result = MessageBox.Show($"Error loading default.yaml: {ex.Message}. Do you want to use default config?", "ERROR!", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                if (result == MessageBoxResult.OK)
                {
                    AddDataFromDictionary(_defaultData);
                    SaveDefaultConfig();
                }
                else
                {
                    Application.Current.Shutdown();
                }
            }
        }

        public void SaveDefaultConfig()
        {
            var serializer = new SerializerBuilder().Build();
            using (var writer = new StreamWriter(DefaultYamlPath))
            {
                serializer.Serialize(writer, _defaultData);
            }
            AddDataFromDictionary(_defaultData);
        }

        public void SaveConfig()
        {
            var dataToSave = new Dictionary<string, List<SoftwareInfo>>()
            {
                { "SoftwareInfos", _softwareInfos.ToList() }
            };

            var serializer = new SerializerBuilder().Build();

            using (var writer = new StreamWriter(DefaultYamlPath))
            {
                serializer.Serialize(writer, dataToSave);
            }
        }

        private void AddDataFromDictionary(Dictionary<string, List<SoftwareInfo>> sourceDictionary)
        {
            _softwareInfos.Clear();
            foreach (var info in sourceDictionary["SoftwareInfos"])
            {
                _softwareInfos.Add(info);
            }
        }
    }
}
