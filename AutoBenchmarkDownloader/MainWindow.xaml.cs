using AutoBenchmarkDownloader.ViewModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        }
    }
}