using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace AutoBenchmarkDownloader.View.Pages
{
    public partial class AboutUsPage : Page
    {
        public AboutUsPage()
        {
            InitializeComponent();
        }
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
        }
    }
}
