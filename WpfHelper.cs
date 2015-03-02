using System.Windows.Media;

namespace VideoJam
{
    public static class WpfHelper
    {
        public static ImageSource ImageSourceFromFullLocator(string locator)
        {
            return
                new ImageSourceConverter().ConvertFromString(
                    locator) as
                    ImageSource;
        }

        public static ImageSource ImageSourceFromRelLocator(string relativeLocator)
        {
            return
                new ImageSourceConverter().ConvertFromString(
                    "pack://application:,,,/VideoJam;component/" + relativeLocator) as
                    ImageSource;
        }
    }
}