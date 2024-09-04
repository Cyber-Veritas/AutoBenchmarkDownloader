using System.Windows;
using System.Windows.Controls;

namespace AutoBenchmarkDownloader.View.Pages
{
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void NavigateToPage(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string targetPage = button.Tag.ToString();

            var mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow?.NavigateToPage(targetPage);
        }
    }
}
