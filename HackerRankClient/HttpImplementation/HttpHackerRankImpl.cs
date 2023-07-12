using HackerRankClient.Model;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;

namespace HackerRankClient.HttpImplementation
{
    public class HttpHackerRankImpl : IHttpHackerRank
    {
        private string _url;
        private string _version;

        private string _bestStoriesUrl;
        private string _specificStoryUrl;

        private const string _beststoriestail = "beststories.json";
        private const string _itemtoken = "item";
        private const char _slash = '/';
        private char[] _slashChar = new char[] { _slash };

        internal HttpHackerRankImpl(){ }

        public HttpHackerRankImpl(string url, string version)
        {
            _url = url;
            _version = version;
            Setup();
        }

        private void Setup()
        {
            Func<string> BuildBestStoriesUrl = () => BuildUrl(_url,_version, _beststoriestail); 
            Func<string> BuildSpecificStoryUrl = () => BuildUrl(_url, _version, _itemtoken, "{0}.json");

            _bestStoriesUrl = BuildBestStoriesUrl();
            _specificStoryUrl = BuildSpecificStoryUrl();
        }

        public string BuildUrl(params string[] values)
        {
            return string.Join('/', values.Select(v => v.Trim().TrimStart(_slashChar).TrimEnd(_slashChar)).ToArray());            
        }

        public async Task<IEnumerable<int>> GetAllStoryIdsAsync()
        {
            using(HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(_bestStoriesUrl);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<int[]>(responseBody);                
            }
        }

        public async Task<Story> GetStoryAsync(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                var url = string.Format(_specificStoryUrl, id);
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Story>(responseBody);                
            }
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
