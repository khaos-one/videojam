using System;
using System.IO;
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
            var webClient = WebHelper.GetWebClient(DownloadUrl);

            webClient.DownloadProgressChanged += webClient_DownloadProgressChanged;
            webClient.DownloadFileCompleted += webClient_DownloadFileCompleted;
            webClient.DownloadFileAsync(new Uri(DownloadUrl, UriKind.Absolute), tempPath, tempPath);
        }

        private void webClient_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Error = e.Error;
                DialogResult = false;
            }
            else if (!e.Cancelled)
            {
                StatusTextBlock.Text = "Moving file...";

                var tempPath = e.UserState as string;
                File.Move(tempPath, SavePath);

                StatusTextBlock.Text = "Completed!";
                DialogResult = true;
            }
            else
            {
                DialogResult = false;
            }
        }

        private void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            StatusTextBlock.Text = string.Format("{0} / {1} bytes ({2}%)", e.BytesReceived, e.TotalBytesToReceive, e.ProgressPercentage);
            ProgressBar.Value = e.ProgressPercentage;
        }
    }
}
