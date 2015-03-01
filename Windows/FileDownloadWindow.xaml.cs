using System;
using System.IO;
using System.Media;
using System.Net;

namespace VideoJam.Windows
{
    /// <summary>
    /// Логика взаимодействия для FileDownloadWindow.xaml
    /// </summary>
    public sealed partial class FileDownloadWindow
    {
        public string DownloadUrl { get; private set; }

        public string SavePath { get; private set; }

        public Exception Error { get; private set; }

        public bool Completed { get; private set; }

        private WebClient _webClient;

        public FileDownloadWindow(string downloadUrl, string savePath)
        {
            DownloadUrl = downloadUrl;
            SavePath = savePath;

            InitializeComponent();
        }

        public new bool? ShowDialog()
        {
            StartDownload();
            return base.ShowDialog();
        }

        private void StartDownload()
        {
            NameTextBlock.Text = Path.GetFileName(SavePath);

            var tempPath = Path.GetTempFileName();
            _webClient = WebHelper.GetWebClient(DownloadUrl);

            _webClient.DownloadProgressChanged += webClient_DownloadProgressChanged;
            _webClient.DownloadFileCompleted += webClient_DownloadFileCompleted;
            _webClient.DownloadFileAsync(new Uri(DownloadUrl, UriKind.Absolute), tempPath, tempPath);
        }

        private void webClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            var tempPath = e.UserState as string;

            TheButton.Content = "Close";
            Completed = true;

            if (e.Error != null || e.Cancelled)
            {
                Error = e.Error;
                File.Delete(tempPath);

                StatusTextBlock.Text = e.Cancelled ? "Download cancelled" : "Error while downloading the file";
            }
            else
            {
                TheButton.IsEnabled = false;
                StatusTextBlock.Text = "Moving file...";

                File.Move(tempPath, SavePath);

                StatusTextBlock.Text = "Completed!";
                TheButton.IsEnabled = true;
            }
        }

        private void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            StatusTextBlock.Text = string.Format("{0} / {1} bytes ({2}%)", e.BytesReceived, e.TotalBytesToReceive, e.ProgressPercentage);
            ProgressBar.Value = e.ProgressPercentage;
        }

        private void TheButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Completed)
            {
                DialogResult = true;
            }
            else
            {
                _webClient.CancelAsync();
                SystemSounds.Asterisk.Play();
            }
        }
    }
}
