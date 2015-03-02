using System.Net;

namespace VideoJam
{
    public static class WebHelper
    {
        public static WebRequest GetWebRequest(string downloadUrl)
        {
            var request = WebRequest.Create(downloadUrl);
            request.Headers.Add("User-Agent", "Khaos-VideoJam/1.0");

            return request;
        }

        public static WebClient GetWebClient(string downloadUrl)
        {
            var client = new WebClient();
            client.Headers.Add("User-Agent", "Khaos-VideoJam/1.0");

            return client;
        }
    }
}