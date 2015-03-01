using System.Text.RegularExpressions;
using System.Windows.Media;

namespace VideoJam.Hostings
{
    public sealed class XnxxCom
        : SingletonBase<XnxxCom>,
          IVideoHosting
    {
        private static readonly Regex IsHostedRegex = new Regex(@"xnxx.com/video[\d]+/", RegexOptions.Compiled);

        public string Name
        {
            get { return "xnxx.com"; }
        }

        public ImageSource HostingImage
        {
            get { return WpfHelper.ImageSourceFromRelLocator("Resources/Images/VideoXnxxComBig.png"); }
        }

        public bool IsHosted(string videoUrl)
        {
            return IsHostedRegex.IsMatch(videoUrl);
        }

        public IVideoInfo GetVideoInfo(string videoUrl)
        {
            return new XnxxComVideoInfo(videoUrl);
        }
    }
}
