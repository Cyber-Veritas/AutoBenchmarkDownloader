using AutoBenchmarkDownloader.Model;
using AutoBenchmarkDownloader.View.PopUps;
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
            new SoftwareInfo { Name = "CPU-Z",  IconPath = "pack://application:,,,/Resources/SoftwareIcons/cpu-z.ico", Description = "System information software", Address = "https://www.geeks3d.com/dl/get/10066", Download = true },
            new SoftwareInfo { Name = "GPU-Z", IconPath = "pack://application:,,,/Resources/SoftwareIcons/gpu-z.ico", Description = "GPU information software", Address = "https://www.geeks3d.com/dl/get/10001", Download = true },
            new SoftwareInfo { Name = "HWiNFO", IconPath = "pack://application:,,,/Resources/SoftwareIcons/hwinfo.ico", Description = "Diagnostic software", Address = "https://www.geeks3d.com/dl/get/10055", Download = true },
            new SoftwareInfo { Name = "HWMonitor", IconPath = "pack://application:,,,/Resources/SoftwareIcons/hwmonitor.ico", Description = "Hardware health monitoring", Address = "https://www.geeks3d.com/dl/get/10026", Download = true },
            new SoftwareInfo { Name = "AIDA64", IconPath = "pack://application:,,,/Resources/SoftwareIcons/aida64.ico", Description = "System information software", Address = "https://download.aida64.com/aida64extreme730.zip", Download = false },
            new SoftwareInfo { Name = "Afterburner", IconPath = "pack://application:,,,/Resources/SoftwareIcons/afterburner.ico", Description = "GPU overclocking", Address = "https://www.geeks3d.com/dl/get/10183", Download = false },
            new SoftwareInfo { Name = "CrystalDiskInfo", IconPath = "pack://application:,,,/Resources/SoftwareIcons/crystaldiskinfo.ico", Description = "Disk health monitoring", Address = "https://www.geeks3d.com/dl/get/10014", Download = false },
            new SoftwareInfo { Name = "CrystalDiskMark", IconPath = "pack://application:,,,/Resources/SoftwareIcons/crystaldiskmark.ico", Description = "Disk benchmark", Address = "https://www.geeks3d.com/dl/get/10015", Download = false },
            new SoftwareInfo { Name = "Valley Benchmark", IconPath = "pack://application:,,,/Resources/SoftwareIcons/valleybench.ico", Description = "Performance and stability test", Address = "https://assets.unigine.com/d/Unigine_Valley-1.0.exe", Download = false },
            new SoftwareInfo { Name = "FurMark 2", IconPath = "pack://application:,,,/Resources/SoftwareIcons/furmark2.ico", Description = "GPU stress test", Address = "https://geeks3d.com/dl/get/748", Download = false },
            new SoftwareInfo { Name = "Cinebench 2024", IconPath = "pack://application:,,,/Resources/SoftwareIcons/cinebench.ico", Description = "CPU benchmark", Address = "https://www.geeks3d.com/dl/get/10211", Download = false },
            new SoftwareInfo { Name = "Prime95", IconPath = "pack://application:,,,/Resources/SoftwareIcons/prime95.ico", Description = "CPU stress test", Address = "https://geeks3d.com/dl/get/10236", Download = false },
            new SoftwareInfo { Name = "Blender Benchmark", IconPath = "pack://application:,,,/Resources/SoftwareIcons/blenderbench.ico", Description = "3D render speed test", Address = "https://download.blender.org/release/BlenderBenchmark2.0/launcher/benchmark-launcher-3.1.0-windows.zip", Download = false },
            //new SoftwareInfo { Name = "Example", Description = "Example software", Address = "https://example.com/", Download = false }
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

    public void AddSoftware()
    {
        var info = new SoftwareInfo()
        {
            Name = "",
            Description = "",
            Address = "",
            Download = true
        };

        var newItemWindow = new NewItemWindow(info);
        newItemWindow.ShowDialog();

        if (newItemWindow.ResultState == DialogResultState.Ok)
        {
            _currentState.SoftwareInfos.Add(info);
            SaveConfig();
        }
    }

    public void EditSoftware(string softwareTitle)
    {
        var originalInfo = _currentState.SoftwareInfos.FirstOrDefault(i => i.Name == softwareTitle);

        var info = originalInfo!.Clone();

        var newItemWindow = new NewItemWindow(info);
        newItemWindow.ShowDialog();

        switch (newItemWindow.ResultState)
        {
            case DialogResultState.Ok:
                originalInfo.Modify(info);
                SaveConfig();
                break;

            case DialogResultState.Delete:
                if (originalInfo.IconPath != "pack://application:,,,/Resources/SoftwareIcons/default.png")
                {
                    File.Delete(originalInfo.IconPath);
                }
                _currentState.SoftwareInfos.Remove(originalInfo);
                SaveConfig();
                break;


        }
    }

}
