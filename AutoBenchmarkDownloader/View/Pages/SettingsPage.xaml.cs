using AutoBenchmarkDownloader.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace AutoBenchmarkDownloader.View.Pages
{
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
            DataContext = SystemMonitorViewModel.Instance;
        }

        private void SetIntervalButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is SystemMonitorViewModel viewModel)
            {
                viewModel.SetIntervalCommand.Execute(IntervalTextBox.Text);
            }
        }
    }
}
