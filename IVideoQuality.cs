namespace VideoJam
{
	public interface IVideoQuality
	{
		int Width { get; }
		int Height { get; }
		long FileLength { get; }
		string FormatName { get; }
		string FormatExtension { get; }
		string TextRepresentation { get; }
		string DownloadUrl { get; }
	}
}