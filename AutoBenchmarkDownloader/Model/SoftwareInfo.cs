﻿using AutoBenchmarkDownloader.MVVM;

namespace AutoBenchmarkDownloader.Model
{
    public class SoftwareInfo : ViewModelBase
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

        public SoftwareInfo Clone()
        {
            return new SoftwareInfo
            {
                Name = Name,
                Description = Description,
                Address = Address,
                IconPath = IconPath,
                Download = Download
            };
        }

        public bool Compare(SoftwareInfo other)
        {
            return Name == other.Name && Description == other.Description && Address == other.Address && IconPath == other.IconPath;
        }

        public void Modify(SoftwareInfo other)
        {
            Name = other.Name;
            Description = other.Description;
            Address = other.Address;
            IconPath = other.IconPath;

            OnPropertyChanged(null);
        }
    }
}
