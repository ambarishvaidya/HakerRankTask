using HackerRankClient.HttpImplementation;
using HackerRankClient.Model;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HackerRankClient
{
    public class HackerRankWebClientImplementation : IHackerRankWebClient
    {
        private IHttpHackerRank _httpClient;
        public HackerRankWebClientImplementation()
        {
            _httpClient = new HttpHackerRankImpl("https://hacker-news.firebaseio.com/", "v0");
        }

        internal HackerRankWebClientImplementation(IHttpHackerRank httpClient)
        {
            _httpClient = httpClient;
        }

        
        public async Task<IEnumerable<int>> GetAllStoryIdsAsync()
        {
            try
            {
                var response = await _httpClient.GetAllStoryIdsAsync();
                return response;

            }catch(Exception oex)
            {
                return Array.Empty<int>();
            }            
        }

        public async Task<Story> GetStoryAsync(int storyId)
        {
            try
            {
                var response = await _httpClient.GetStoryAsync(storyId);
                return response;

            }
            catch (Exception oex)
            {
                return new Story();
            }
        }

        public async Task<IEnumerable<Story>> GetTopStoriesAsync(int count)
        {
            try
            {
                var response = await _httpClient.GetTopStoriesAsync(count);
                return response;
            }
            catch (Exception oex)
            {
                return Array.Empty<Story>();
            }
        }
    }
}
