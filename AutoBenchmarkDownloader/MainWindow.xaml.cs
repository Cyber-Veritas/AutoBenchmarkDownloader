using AutoBenchmarkDownloader.ViewModel;
using System.Windows;
using System.Windows.Input;
using MicaWPF.Controls;

namespace AutoBenchmarkDownloader
{
    public partial class MainWindow : MicaWindow
    {
        private SystemUsageInfoViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();

            Wpf.Ui.Appearance.WindowBackgroundManager.ApplyDarkThemeToWindow(this);

            MainWindowViewModel vm = new MainWindowViewModel();
            DataContext = vm;
        }

        private void MainWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is not UIElement element) return;
            Keyboard.ClearFocus();
            FocusManager.SetFocusedElement(element, null);
        }
    }
}