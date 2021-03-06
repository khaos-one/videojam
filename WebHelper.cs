﻿using System.Net;

namespace VideoJam
{
    public static class WebHelper
    {
        public static WebRequest GetWebRequest(string downloadUrl)
        {
            var request = WebRequest.Create(downloadUrl);

            if (request.Headers["User-Agent"] == null)
            {
                request.Headers.Add("User-Agent", "Khaos-VideoJam/1.0");
            }

            return request;
        }

        public static WebClient GetWebClient(string downloadUrl)
        {
            var client = new WebClient();
            if (client.Headers["User-Agent"] == null)
            {
                client.Headers.Add("User-Agent", "Khaos-VideoJam/1.0");
            }

            return client;
        }

        public static long GetContentLength(string url)
        {
            var request = WebRequest.Create(url);
            request.Timeout = 1000;
            request.Method = "HEAD";
            var response = request.GetResponse();

            return response.ContentLength;
        }
    }
}