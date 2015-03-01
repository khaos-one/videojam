using System.Windows.Media;

namespace VideoJam
{
	public interface IVideoHosting
	{
		string Name { get; }

		ImageSource HostingImage { get; }

	    bool IsHosted(string videoUrl);

	    IVideoInfo GetVideoInfo(string videoUrl);
	}
}