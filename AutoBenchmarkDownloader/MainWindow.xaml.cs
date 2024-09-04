using AutoBenchmarkDownloader.ViewModel;
using System.Windows;
using System.Windows.Input;
using AutoBenchmarkDownloader.View.Pages;
using MicaWPF.Controls;

namespace AutoBenchmarkDownloader
{
    public partial class MainWindow : MicaWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = SystemMonitorViewModel.Instance;

            Wpf.Ui.Appearance.WindowBackgroundManager.ApplyDarkThemeToWindow(this);
            Loaded += MainWindow_Loaded;
        }

        public void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RootNavigation.Navigate(typeof(HomePage));
        }

        public void NavigateToPage(string targetPage)
        {
            if (targetPage == "DownloadPage") { RootNavigation.Navigate(typeof(DownloadPage)); }
            else if (targetPage == "SystemMonitorInfoPage") { RootNavigation.Navigate(typeof(SystemMonitorInfoPage)); }
            else if (targetPage == "AboutUsPage") { RootNavigation.Navigate(typeof(AboutUsPage)); }
            else if (targetPage == "SettingsPage") { RootNavigation.Navigate(typeof(SettingsPage)); }
        }

        private void MainWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is not UIElement element) return;
            Keyboard.ClearFocus();
            FocusManager.SetFocusedElement(element, null);
        }
    }
}