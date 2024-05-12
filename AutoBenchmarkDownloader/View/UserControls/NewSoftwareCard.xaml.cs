using System.Windows;
using System.Windows.Controls;
using AutoBenchmarkDownloader.View.PopUps;

namespace AutoBenchmarkDownloader.View.UserControls
{
    public partial class NewSoftwareCard : UserControl
    {
        public NewSoftwareCard()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var newItemWindow = new NewItemWindow();
            newItemWindow.ShowDialog();
        }
    }
}
