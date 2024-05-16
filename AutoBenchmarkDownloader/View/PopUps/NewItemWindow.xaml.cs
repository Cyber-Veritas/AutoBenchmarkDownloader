using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using MicaWPF.Controls;
using AutoBenchmarkDownloader.Model;
using AutoBenchmarkDownloader.Utilities.Converters;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using AutoBenchmarkDownloader.View.Pages;

namespace AutoBenchmarkDownloader.View.PopUps
{
    public partial class NewItemWindow : MicaWindow
    {
        public SoftwareInfo NewSoftware { get; set; }

        public NewItemWindow(SoftwareInfo newSoftware)
        {
            Owner = Application.Current.MainWindow;

            InitializeComponent();
            NewSoftware = newSoftware;

            var converter = new UriToImageSourceConverter();
            var uri = new Uri(NewSoftware.IconPath);
            var imageSource = (ImageSource)converter.Convert(uri, typeof(ImageSource), null, CultureInfo.CurrentCulture);
            ImgIcon.ImageSource = imageSource;

            Left = Owner!.Left + Owner.Width / 2 - Width / 2 + 25;
            Top = Owner.Top + Owner.Height / 2 - Height / 2;

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            BtnOk.IsEnabled = !string.IsNullOrWhiteSpace(TbTitle.Text) && !string.IsNullOrWhiteSpace(TbAddress.Text);
        }

        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void BtnOk_OnClick(object sender, RoutedEventArgs e)
        {
            NewSoftware.Name = TbTitle.Text;
            NewSoftware.Description = TbDescription.Text;
            NewSoftware.Address = TbAddress.Text;

            DialogResult = true;
            Close();
        }

        private void NewItemWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is not UIElement element) return;
            Keyboard.ClearFocus();
            FocusManager.SetFocusedElement(this, null);
        }

    }
}
