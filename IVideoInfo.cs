using System;
using System.Collections.Generic;

namespace VideoJam
{
	public interface IVideoInfo
	{
		string Name { get; }

		Uri PreviewImage { get; }

		IList<IVideoQuality> Qualities { get; }
	}
}