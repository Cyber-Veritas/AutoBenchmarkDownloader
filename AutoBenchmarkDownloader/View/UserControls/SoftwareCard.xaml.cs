using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AutoBenchmarkDownloader.View.UserControls
{
    public partial class SoftwareCard : UserControl
    {


        public Uri SoftwareIcon
        {
            get
            {
                var uriString = (string)GetValue(SoftwareIconProperty);
                return new Uri(uriString);
            }
            set => SetValue(SoftwareIconProperty, value);
        }

        public static readonly DependencyProperty SoftwareIconProperty =
            DependencyProperty.Register(nameof(SoftwareIcon), typeof(Uri), typeof(SoftwareCard),
                new PropertyMetadata(null));



        public string SoftwareTitle
        {
            get => (string)GetValue(SoftwareTitleProperty);
            set => SetValue(SoftwareTitleProperty, value);
        }

        public static readonly DependencyProperty SoftwareTitleProperty =
            DependencyProperty.Register(nameof(SoftwareTitle), typeof(string), typeof(SoftwareCard),
                new PropertyMetadata(""));


        public string SoftwareDescription
        {
            get => (string)GetValue(SoftwareDescriptionProperty);
            set => SetValue(SoftwareDescriptionProperty, value);
        }

        public static readonly DependencyProperty SoftwareDescriptionProperty =
            DependencyProperty.Register(nameof(SoftwareDescription), typeof(string), typeof(SoftwareCard),
                new PropertyMetadata(""));


        public bool SoftwareDownload
        {
            get => (bool)GetValue(SoftwareDownloadProperty);
            set => SetValue(SoftwareDownloadProperty, value);
        }

        public static readonly DependencyProperty SoftwareDownloadProperty =
            DependencyProperty.Register(nameof(SoftwareDownload), typeof(bool), typeof(SoftwareCard),
                new PropertyMetadata(false));




        public SoftwareCard()
        {
            InitializeComponent();
        }

        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is not Border border) return;

            border.Background.Opacity = 0.3;
            SvgEdit.Opacity = 0.7;
        }

        private void UIElement_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is not Border border) return;

            border.Background.Opacity = 1.0;
            SvgEdit.Opacity = 0.0;
        }

    }
}
