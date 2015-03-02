using System.Text.RegularExpressions;
using System.Windows.Media;

namespace VideoJam.Hostings
{
    public sealed class YouTubeCom
        : SingletonBase<YouTubeCom>,
          IVideoHosting
    {
        private static readonly Regex IsHostedRegex =
            new Regex(@"^.*((youtu.be\/)|(v\/)|(\/u\/\w\/)|(embed\/)|(watch\?))\??v?=?([^#\&\?]*).*",
                RegexOptions.Compiled);

        public string Name
        {
            get { return "youtube.com"; }
        }

        public ImageSource HostingImage
        {
            get { return WpfHelper.ImageSourceFromRelLocator("Resources/Images/VideoYouTubeBig.png"); }
        }

        public bool IsHosted(string videoUrl)
        {
            return IsHostedRegex.IsMatch(videoUrl);
        }

        public IVideoInfo GetVideoInfo(string videoUrl)
        {
            var match = IsHostedRegex.Match(videoUrl);
            return new YouTubeComVideoInfo(match.Groups[7].Value);
        }
    }
}
