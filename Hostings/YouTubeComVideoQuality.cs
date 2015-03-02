using System;
using System.Collections.Specialized;
using System.Web;

namespace VideoJam.Hostings
{
    public sealed class YouTubeComVideoQuality
        : IVideoQuality
    {
        public YouTubeComVideoQuality(string encodedVideoMap)
        {
            NameValueCollection data = HttpUtility.ParseQueryString(encodedVideoMap);
            string server = Uri.UnescapeDataString(data["fallback_host"]);
            DownloadUrl = Uri.UnescapeDataString(data["url"]) + "&fallback_host=" + server;
            FileLength = WebHelper.GetContentLength(DownloadUrl);
            var itagInfo = new YouTubeComITagInfo(Uri.UnescapeDataString(data["itag"]));
            Width = (int) itagInfo.VideoDimensions.Width;
            Height = (int) itagInfo.VideoDimensions.Height;
            FormatExtension = itagInfo.VideoExtentions;
            FormatName = "YouTube Video";
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public long FileLength { get; private set; }
        public string FormatName { get; private set; }
        public string FormatExtension { get; private set; }

        public string TextRepresentation
        {
            get
            {
                return String.Format
                    ("{0} ({1}x{2}) — {3}", FormatExtension.ToUpper(), Width, Height,
                        StringHelper.BytesToString(FileLength));
            }
        }

        public string DownloadUrl { get; private set; }
    }
}