using AutoBenchmarkDownloader.Model;
using System.IO;
using System.Windows;
using YamlDotNet.Serialization;

namespace AutoBenchmarkDownloader.Utilities;


internal class YamlOperations
{
    public const string DefaultYamlPath = "default.yaml";

    private readonly State _currentState;

    public YamlOperations(State currentState)
    {
        _currentState = currentState;
    }

    public readonly State _defaultState = new()
    {
        SoftwareInfos =
        [
            new SoftwareInfo { Name = "FurMark 2", Address = "https://geeks3d.com/dl/get/748", Download = true },
            new SoftwareInfo { Name = "CPU-Z", Address = "https://download.cpuid.com/cpu-z/cpu-z_2.09-en.zip", Download = false }
        ],
        OutputPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
    };


    public void LoadConfig()
    {
        var deserializer = new DeserializerBuilder().Build();

        try
        {
            using (var reader = new StreamReader(DefaultYamlPath))
            {
                var savedState = deserializer.Deserialize<State>(reader);
                AddDataFromState(savedState);
            }
        }
        catch (Exception ex)
        {
            var result = MessageBox.Show($"Error loading default.yaml: {ex.Message}. Do you want to use default config?", "ERROR!", MessageBoxButton.OKCancel, MessageBoxImage.Error);
            if (result == MessageBoxResult.OK)
            {
                AddDataFromState(DeepCopyState(_defaultState));
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
            serializer.Serialize(writer, _defaultState);
        }
        AddDataFromState(DeepCopyState(_defaultState));
    }

    public void SaveConfig()
    {
        var dataToSave = new State()
            {
                SoftwareInfos = _currentState.SoftwareInfos,
                OutputPath = _currentState.OutputPath
            };

        var serializer = new SerializerBuilder().Build();

        using (var writer = new StreamWriter(DefaultYamlPath))
        {
            serializer.Serialize(writer, dataToSave);
        }
    }

    public State DeepCopyState(State source)
    {
        var serializer = new SerializerBuilder().Build();
        var deserializer = new DeserializerBuilder().Build();

        var serialized = serializer.Serialize(source);
        return deserializer.Deserialize<State>(serialized);
    }

    public void AddDataFromState(State sourceState)
    {
        foreach (var info in sourceState.SoftwareInfos)
        {
            var existingInfo = _currentState.SoftwareInfos.FirstOrDefault(i => i.Name == info.Name);
            if (existingInfo != null && existingInfo.Download != info.Download) 
            {
                existingInfo.Download = info.Download;
            }
            else if (existingInfo == null)
            {
                _currentState.SoftwareInfos.Add(info);
            }
        }

        _currentState.OutputPath = sourceState.OutputPath;

    }
}
