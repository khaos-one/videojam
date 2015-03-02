using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using VideoJam.Hostings;

namespace VideoJam.Windows
{
    /// <summary>
    ///     Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow
    {
        private readonly IList<IVideoHosting> _hostings = new[]
        {
            XnxxCom.Instance
        };

        private IVideoHosting _currentHosting;
        private IVideoInfo _currentVideoInfo;

        public MainWindow()
        {
            InitializeComponent();
            VideoUrlTextBox.Focus();
        }

        private object VideoPreviewImageSource
        {
            get
            {
                var image = new BitmapImage();

                try
                {
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    image.UriSource = _currentVideoInfo.PreviewImage;
                    image.EndInit();
                }
                catch
                {
                    return DependencyProperty.UnsetValue;
                }

                return image;
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            var aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

        private void VideoUrlTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _currentHosting = null;
            string videoUrl = ((TextBox) sender).Text;

            foreach (IVideoHosting hosting in _hostings.Where(hosting => hosting.IsHosted(videoUrl)))
            {
                _currentHosting = hosting;
                break;
            }

            if (_currentHosting == null)
            {
                VideoHostingImage.Source = WpfHelper.ImageSourceFromRelLocator("Resources/Images/VideoGenericBig.png");
                GetVideoButton.IsEnabled = false;
            }
            else
            {
                VideoHostingImage.Source = _currentHosting.HostingImage;
                GetVideoButton.IsEnabled = true;
            }
        }

        private void GetVideoButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentHosting == null)
                return;

            _currentVideoInfo = null;
            string videoUrl = VideoUrlTextBox.Text;

            VideoDetailsFrame.Visibility = Visibility.Hidden;
            BusyIndicatorFrame.IsBusy = true;

            Task.Factory.StartNew(() => { _currentVideoInfo = _currentHosting.GetVideoInfo(videoUrl); })
                .ContinueWith(task =>
                {
                    VideoNameTextBlock.Text = _currentVideoInfo.Name;
                    VideoQualitiesComboBox.ItemsSource = _currentVideoInfo.Qualities;
                    VideoQualitiesComboBox.SelectedIndex = 0;
                    VideoPreviewImage.Source = VideoPreviewImageSource as ImageSource;

                    BusyIndicatorFrame.IsBusy = false;
                    VideoDetailsFrame.Visibility = Visibility.Visible;
                }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void DownloadVideoButton_Click(object sender, RoutedEventArgs e)
        {
            if (VideoQualitiesComboBox.Items.Count == 0)
            {
                return;
            }

            var selectedQuality = VideoQualitiesComboBox.SelectedItem as IVideoQuality ??
                                  VideoQualitiesComboBox.Items[0] as IVideoQuality;

            var saveFileDialog = new SaveFileDialog
            {
                FileName = _currentVideoInfo.Name,
                DefaultExt = selectedQuality.FormatExtension,
                Filter = selectedQuality.FormatName + "|*" + selectedQuality.FormatExtension
            };
            var result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                var downloadWindow = new FileDownloadWindow(selectedQuality.DownloadUrl, saveFileDialog.FileName);
                var downloadResult = downloadWindow.ShowDialog();
            }
        }
    }
}