using System;
using System.Collections.Generic;
using System.Net;
using System.Web;

namespace VideoJam.Hostings
{
    public sealed class YouTubeComVideoInfo
        : IVideoInfo
    {
        private readonly IList<IVideoQuality> qualities = new List<IVideoQuality>();

        public YouTubeComVideoInfo(string videoId)
        {
            // TODO: Error checking.

            var content =
                new WebClient().DownloadString(
                    string.Format(
                        "http://www.youtube.com/get_video_info?&video_id={0}&el=detailpage&ps=default&eurl=&gl=US&hl=en",
                        videoId));
            var infoValues = HttpUtility.ParseQueryString(content);
            Name = infoValues["title"];
            PreviewImage = new Uri(HttpUtility.UrlDecode(infoValues["thumbnail_url"]), UriKind.Absolute);
            var qualitiesInfo = infoValues["url_encoded_fmt_stream_map"].Split(',');

            foreach (var q in qualitiesInfo)
            {
                try
                {
                    qualities.Add(new YouTubeComVideoQuality(q));
                }
                catch (TimeoutException)
                {
                }
                catch (WebException)
                {
                }
            }
        }

        public string Name { get; private set; }

        public Uri PreviewImage { get; private set; }

        public IList<IVideoQuality> Qualities
        {
            get { return qualities; }
        }
    }
}