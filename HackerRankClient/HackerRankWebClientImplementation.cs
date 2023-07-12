using HackerRankClient.Model;
using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HackerRankClient
{
    public class HackerRankWebClientImplementation : IHackerRankWebClient
    {
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
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("https://hacker-news.firebaseio.com/v0/beststories.json");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var resp = JsonSerializer.Deserialize<int[]>(responseBody);
            return resp;
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
            HttpClient client = new HttpClient();
            var response = await client.GetAsync($"https://hacker-news.firebaseio.com/v0/item/{storyId}.json");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            var resp = JsonSerializer.Deserialize<Story>(responseBody);
            return resp;
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
            var allids = await GetAllStoryIdsAsync();
            ConcurrentDictionary<int, List<Story>> storyDictionary = new ConcurrentDictionary<int, List<Story>>();
            ParallelOptions parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount };
            await Parallel.ForEachAsync(allids, parallelOptions, async (id, ct) =>
            {
                Story story = await GetStoryAsync(id);
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
    }
}
