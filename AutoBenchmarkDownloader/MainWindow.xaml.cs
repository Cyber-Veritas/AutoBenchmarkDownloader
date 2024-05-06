using AutoBenchmarkDownloader.ViewModel;
using System.Windows;
using System.Windows.Input;

namespace AutoBenchmarkDownloader
{
    public partial class MainWindow : Window
    {
        private SystemUsageInfoViewModel viewModel;
        public MainWindow()
        {
            InitializeComponent();

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