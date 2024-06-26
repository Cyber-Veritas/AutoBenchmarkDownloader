using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using MicaWPF.Controls;
using AutoBenchmarkDownloader.Model;
using AutoBenchmarkDownloader.Utilities.Converters;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using AutoBenchmarkDownloader.View.Pages;
using Microsoft.Win32;

namespace AutoBenchmarkDownloader.View.PopUps
{
    public enum DialogResultState
    {
        Ok,
        Cancel,
        Delete
    }

    public partial class NewItemWindow : MicaWindow
    {
        public SoftwareInfo NewSoftware { get; set; }

        public DialogResultState ResultState { get; private set; }

        public NewItemWindow(SoftwareInfo newSoftware)
        {
            Owner = Application.Current.MainWindow;

            InitializeComponent();
            NewSoftware = newSoftware;

            ResultState = DialogResultState.Cancel;

            var converter = new UriToImageSourceConverter();

            Uri uri;

            if (NewSoftware.IconPath.StartsWith("pack://"))
            {
                uri = new Uri(NewSoftware.IconPath, UriKind.Absolute);
            }
            else
            {
                uri = new Uri(NewSoftware.IconPath, UriKind.Relative);
            }

            if (NewSoftware.Name == "")
            {
                BtnDelete.Visibility = Visibility.Collapsed;
            }

            var imageSource = (ImageSource)converter.Convert(uri, typeof(ImageSource), null, CultureInfo.CurrentCulture);
            ImgIcon.ImageSource = imageSource;

            TbTitle.Text = NewSoftware.Name;
            TbDescription.Text = NewSoftware.Description;
            TbAddress.Text = NewSoftware.Address;

            Left = Owner!.Left + Owner.Width / 2 - Width / 2 + 25;
            Top = Owner.Top + Owner.Height / 2 - Height / 2;

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            BtnOk.IsEnabled = !string.IsNullOrWhiteSpace(TbTitle.Text) && !string.IsNullOrWhiteSpace(TbAddress.Text);
        }

        private void BtnCancel_OnClick(object sender, RoutedEventArgs e)
        {
            ResultState = DialogResultState.Cancel;
            Close();
        }

        private void BtnDelete_OnClick(object sender, RoutedEventArgs e)
        {
            ResultState = DialogResultState.Delete;
            Close();
        }

        private void BtnOk_OnClick(object sender, RoutedEventArgs e)
        {
            NewSoftware.Name = TbTitle.Text;
            NewSoftware.Description = TbDescription.Text;
            NewSoftware.Address = TbAddress.Text;

            if (!NewSoftware.IconPath.StartsWith("pack://") && !NewSoftware.IconPath.StartsWith("Icons/"))
            {
                Directory.CreateDirectory("Icons");

                var iconExtension = GetFileExtension(NewSoftware.IconPath);
                var newPath = $"Icons/{NewSoftware.Name}{iconExtension}";

                File.Copy(NewSoftware.IconPath, newPath, true);

                NewSoftware.IconPath = newPath;
            }

            ResultState = DialogResultState.Ok;
            Close();
        }

        private void NewItemWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is not UIElement element) return;
            Keyboard.ClearFocus();
            FocusManager.SetFocusedElement(this, null);
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

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var fileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.png;*.jpeg;*.jpg;*.ico)|*.png;*.jpeg;*.jpg;*.ico",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };

            var success = fileDialog.ShowDialog();
            if (success != true) return;

            NewSoftware.IconPath = fileDialog.FileName;
            var converter = new UriToImageSourceConverter();
            var uri = new Uri(NewSoftware.IconPath);
            var imageSource = (ImageSource)converter.Convert(uri, typeof(ImageSource), null, CultureInfo.CurrentCulture);
            ImgIcon.ImageSource = imageSource;
        }

        private string GetFileExtension(string filePath)
        {
            return Path.GetExtension(filePath);
        }

    }
}
