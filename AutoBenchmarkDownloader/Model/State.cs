using System.Collections.ObjectModel;
using AutoBenchmarkDownloader.MVVM;

namespace AutoBenchmarkDownloader.Model
{
    internal class State : ViewModelBase
    {
        private ObservableCollection<SoftwareInfo> softwareInfos;

        public ObservableCollection<SoftwareInfo> SoftwareInfos
        {
            get { return softwareInfos; }
            set
            {
                softwareInfos = value;
                OnPropertyChanged();
            }
        }


        private string outputPath;

        public string OutputPath
        {
            get { return outputPath; }
            set
            {
                outputPath = value;
                OnPropertyChanged();
            }
        }


    }
}
