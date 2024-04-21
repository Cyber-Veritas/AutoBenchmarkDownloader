using AutoBenchmarkDownloader.ViewModel;
using System.Text;
﻿using System.IO;
using System.Net.Http;
using System.Windows;
using AutoBenchmarkDownloader.ViewModel;

namespace AutoBenchmarkDownloader
{
    public partial class MainWindow : Window
    {
        private SystemUsageInfoViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();

            // initialize system usage info
            viewModel = new SystemUsageInfoViewModel(); 

            DataContext = viewModel;
            MainWindowViewModel vm = new MainWindowViewModel();
            DataContext = vm;
        }
    }
}