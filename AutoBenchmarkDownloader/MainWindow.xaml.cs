using System.IO;
using System.Net.Http;
using System.Windows;
using AutoBenchmarkDownloader.ViewModel;

namespace AutoBenchmarkDownloader
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainWindowViewModel vm = new MainWindowViewModel();
            DataContext = vm;
        }
    }
}