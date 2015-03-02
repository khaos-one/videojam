namespace VideoJam.Hostings
{
    public sealed class XnxxComVideoQuality
        : IVideoQuality
    {
        public XnxxComVideoQuality(string downloadUrl, int height, int width, long fileLength)
        {
            Width = width;
            Height = height;
            DownloadUrl = downloadUrl;
            FileLength = fileLength;
        }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public long FileLength { get; private set; }

        public string FormatName
        {
            get { return "Flash Video (AVC)"; }
        }

        public string FormatExtension
        {
            get { return ".flv"; }
        }

        public string TextRepresentation
        {
            get
            {
                return string.Format(FormatName + " ({0}x{1}) — {2}", Width, Height,
                    StringHelper.BytesToString(FileLength));
            }
        }

        public string DownloadUrl { get; private set; }
    }
}