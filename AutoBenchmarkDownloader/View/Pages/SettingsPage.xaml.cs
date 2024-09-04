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
            DataContext = new SettingsViewModel();

            PathTextBox.Text = (DataContext as SettingsViewModel)?.SoftwareInfoViewModel.CurrentState.OutputPath;
        }

        private void ResetPathButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is SettingsViewModel viewModel)
            {
                viewModel.SoftwareInfoViewModel.ResetOutputPathCommand.Execute(null);
                PathTextBox.Text = (DataContext as SettingsViewModel)?.SoftwareInfoViewModel.CurrentState.OutputPath;
            }
        }

        private void ChoosePathButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is SettingsViewModel viewModel)
            {
                viewModel.SoftwareInfoViewModel.ChooseOutputPathCommand.Execute(null);
                PathTextBox.Text = (DataContext as SettingsViewModel)?.SoftwareInfoViewModel.CurrentState.OutputPath;
            }
        }

        private void ApplyPathButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is SettingsViewModel viewModel)
            {
                viewModel.SoftwareInfoViewModel.ApplyOutputPathCommand.Execute(PathTextBox.Text);
                PathTextBox.Text = (DataContext as SettingsViewModel)?.SoftwareInfoViewModel.CurrentState.OutputPath;
            }
        }


        private void ResetSoftwareButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is SettingsViewModel viewModel)
            {
                viewModel.SoftwareInfoViewModel.RestoreDefaultSoftwareCommand.Execute(null);
                PathTextBox.Text = (DataContext as SettingsViewModel)?.SoftwareInfoViewModel.CurrentState.OutputPath;
            }
        }


        private void SetIntervalButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is SettingsViewModel viewModel)
            {
                viewModel.SystemMonitorViewModel.SetIntervalCommand.Execute(IntervalTextBox.Text);
            }
        }
    }
}
