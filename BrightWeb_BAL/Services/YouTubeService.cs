using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BrightWeb_BAL.Services
{
    public class YouTubeService
    {
        private readonly HttpClient _httpClient;

        public YouTubeService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("YouTubeAPI");
        }

        public async Task<byte[]> GetVideoDetails(string videoId)
        {
            // Use the YouTube Data API to get video details
            HttpResponseMessage response = await _httpClient.GetAsync($"videos?key=AIzaSyBdM8aBRj-fws4hqxruQejoWrl8BwfO4JI&part=snippet&id={videoId}");
            if (response.IsSuccessStatusCode)
            {
                var video = await response.Content.ReadAsByteArrayAsync();
                return video;
            }
            return null;
        }
    }
    public class YouTubeVideo
    {
        public string Id { get; set; }
        public YouTubeSnippet Snippet { get; set; }
    }

    public class YouTubeSnippet
    {
        public string Title { get; set; }
        public string Description { get; set; }
        // Add more properties as needed
    }
}
