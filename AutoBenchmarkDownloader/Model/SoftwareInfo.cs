using AutoBenchmarkDownloader.MVVM;

namespace AutoBenchmarkDownloader.Model
{
    internal class SoftwareInfo : ViewModelBase
    {
        public required string Name { get; set; }
        public required string Address { get; set; }

        private bool download;

        public bool Download
        {
            get { return download; }
            set
            {
                download = value;
                OnPropertyChanged();
            }
        }

    }
}
