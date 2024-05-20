using AutoBenchmarkDownloader.MVVM;

namespace AutoBenchmarkDownloader.Model
{
    internal class SoftwareInfo : ViewModelBase
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required string Address { get; set; }

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
