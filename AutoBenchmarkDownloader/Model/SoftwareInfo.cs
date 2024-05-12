using AutoBenchmarkDownloader.MVVM;

namespace AutoBenchmarkDownloader.Model
{
    internal class SoftwareInfo : ViewModelBase
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Address { get; set; }


        private string _iconPath = null!;

        public string IconPath
        {
            get => _iconPath ?? "pack://application:,,,/Resources/SoftwareIcons/default.png";
            set
            {
                _iconPath = value;
                OnPropertyChanged();
            }
        }



        private bool _download;

        public bool Download
        {
            get => _download;
            set
            {
                _download = value;
                OnPropertyChanged();
            }
        }

    }
}
