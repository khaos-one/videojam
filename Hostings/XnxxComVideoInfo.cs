using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace VideoJam.Hostings
{
    public sealed class XnxxComVideoInfo
        : IVideoInfo
    {
        private static readonly Regex PageInfoExtractorRegex =
            new Regex(@"flashvars="".+?\&amp;flv_url=(.+?)\&amp;url_bigthumb=(.+?)\&amp;.*?height=(\d+)&amp;width=(\d+)");

        private static readonly Regex TitleExtractorRegex = new Regex(@"""style5""><strong>([\w\s]+)</strong>",
            RegexOptions.IgnoreCase | RegexOptions.Multiline);

        private readonly long _fileLength;
        private readonly int _height;
        private readonly string _thumbnailUrl;

        private readonly string _title;
        private readonly string _videoUrl;
        private readonly int _width;

        public XnxxComVideoInfo(string url)
        {
            // TODO: Error checking.

            var content = new WebClient().DownloadString(url);
            var match = PageInfoExtractorRegex.Match(content);

            _videoUrl = HttpUtility.UrlDecode(match.Groups[1].Value);
            _thumbnailUrl = match.Groups[2].Value;
            _height = int.Parse(match.Groups[3].Value);
            _width = int.Parse(match.Groups[4].Value);
            _title = TitleExtractorRegex.Match(content).Groups[1].Value.Trim('\n', '\r', ' ');

            // Get file length.
            var request = WebRequest.Create(_videoUrl);
            request.Method = "HEAD";
            var response = request.GetResponse();
            _fileLength = response.ContentLength;
        }

        public string Name
        {
            get { return _title; }
        }

        public Uri PreviewImage
        {
            get { return new Uri(_thumbnailUrl, UriKind.RelativeOrAbsolute); }
        }

        public IList<IVideoQuality> Qualities
        {
            get
            {
                return new List<IVideoQuality>(1) {new XnxxComVideoQuality(_videoUrl, _height, _width, _fileLength)};
            }
        }
    }
}