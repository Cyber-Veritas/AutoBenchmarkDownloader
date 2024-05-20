using System.Windows;
using System.Windows.Controls;

namespace AutoBenchmarkDownloader.View.UserControls
{
    public partial class SoftwareCard : UserControl
    {
        public string SoftwareTitle
        {
            get => (string)GetValue(SoftwareTitleProperty);
            set => SetValue(SoftwareTitleProperty, value);
        }

        public static readonly DependencyProperty SoftwareTitleProperty =
            DependencyProperty.Register(nameof(SoftwareTitle), typeof(string), typeof(SoftwareCard), new PropertyMetadata(""));


        public string SoftwareDescription
        {
            get => (string)GetValue(SoftwareDescriptionProperty);
            set => SetValue(SoftwareDescriptionProperty, value);
        }

        public static readonly DependencyProperty SoftwareDescriptionProperty =
            DependencyProperty.Register(nameof(SoftwareDescription), typeof(string), typeof(SoftwareCard), new PropertyMetadata(""));


        public bool SoftwareDownload
        {
            get => (bool)GetValue(SoftwareDownloadProperty);
            set => SetValue(SoftwareDownloadProperty, value);
        }

        public static readonly DependencyProperty SoftwareDownloadProperty =
            DependencyProperty.Register(nameof(SoftwareDownload), typeof(bool), typeof(SoftwareCard), new PropertyMetadata(false));




        public SoftwareCard()
        {
            InitializeComponent();
        }

    }
}
