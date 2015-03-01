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

        public FileDownloadWindow(string downloadUrl, string savePath)
        {
            DownloadUrl = downloadUrl;
            SavePath = savePath;

            InitializeComponent();
        }

        public void Start()
        {
               
        }

        private void Download()
        {
            var tempPath = Path.GetTempFileName();
            var webClient = WebHelper.GetWebClient(DownloadUrl);

            webClient.DownloadProgressChanged += webClient_DownloadProgressChanged;
        }

        private void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
