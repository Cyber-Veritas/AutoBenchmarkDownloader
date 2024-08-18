using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoBenchmarkDownloader.View.Pages
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
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
