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

        public IEnumerable<int> GetAllStoryIds()
        {
            HttpClient client = new HttpClient();
            var response = client.GetAsync("https://hacker-news.firebaseio.com/v0/beststories.json").Result;
            response.EnsureSuccessStatusCode();
            string responseBody = response.Content.ReadAsStringAsync().Result;
            var resp = JsonSerializer.Deserialize<int[]>(responseBody);
            return resp;
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

        public Story GetStory(int storyId)
        {
            HttpClient client = new HttpClient();
            var response = client.GetAsync($"https://hacker-news.firebaseio.com/v0/item/{storyId}.json").Result;
            response.EnsureSuccessStatusCode();
            string responseBody = response.Content.ReadAsStringAsync().Result;
            var resp = JsonSerializer.Deserialize<Story>(responseBody);
            return resp;
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

        public IEnumerable<Story> GetTopStories(int count)
        {
            var allids = GetAllStoryIds();
            ConcurrentDictionary<int, List<Story>> storyDictionary = new ConcurrentDictionary<int, List<Story>>();
            Parallel.ForEach(allids, (id) => 
            {
                Story story = GetStory(id);
                storyDictionary.AddOrUpdate(story.score, 
                    new List<Story>() { story }, 
                    (k, v) => {
                        v.Add(story);
                        return v;
                    });
            });
            var descendingOrder = storyDictionary.Keys.OrderByDescending(k => k);
            Story[] stories = new Story[count];
            int counter = 0;
            foreach (int i in descendingOrder) 
            {
                stories[counter] = storyDictionary[i].First();
                counter++;
                if (counter >= count)
                    break;
            }
            return stories;
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
