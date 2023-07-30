using HackerNewsClient.Model;
using log4net.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Text.Json;

namespace HackerNewsClient.Util
{
    public class CommonOperations : ICommonOperations
    {
        private IConfiguration _configuration;
        private ILogger<CommonOperations> _log;
        private IHttpClientFactory _httpClientFactory;
        private string _url;
        private string _version;

        private string _bestStoriesUrl;
        private string _specificStoryUrl;

        private const string _beststoriestail = "beststories.json";
        private const string _itemtoken = "item";
        private const char _slash = '/';
        private char[] _slashChar = new char[] { _slash };

        public CommonOperations(string url, string version, ILogger<CommonOperations> logger, IHttpClientFactory httpClientFactory)
        {
            _log = logger;
            _httpClientFactory = httpClientFactory;
            _url = url;
            _version = version;
            Setup();
        }
        public CommonOperations(IConfiguration config, ILogger<CommonOperations> logger, IHttpClientFactory httpClientFactory)
        {
            _log = logger;
            _httpClientFactory = httpClientFactory;
            _configuration = config;
            _url = config["HackerNewsUrl"];
            _version = config["HackerNewsApiVersion"];
            Setup();
        }

        private void Setup()
        {
            Func<string> BuildBestStoriesUrl = () => BuildUrl(_url, _version, _beststoriestail);
            Func<string> BuildSpecificStoryUrl = () => BuildUrl(_url, _version, _itemtoken, "{0}.json");

            _bestStoriesUrl = BuildBestStoriesUrl();
            _specificStoryUrl = BuildSpecificStoryUrl();

            _log.LogInformation($"Best Stories URL {_bestStoriesUrl}");
            _log.LogInformation($"Specific Story URL {_specificStoryUrl}" + " - {0} is replaced by the requested id.");
        }

        public string BuildUrl(params string[] values)
        {
            return string.Join('/', values.Select(v => v.Trim().TrimStart(_slashChar).TrimEnd(_slashChar)).ToArray());
        }

        public string Url => _url;

        public string Version => _version;

        public string TopStoriesUrl => _bestStoriesUrl;

        public string SpecificStoryUrl(int storyId) => string.Format(_specificStoryUrl, storyId);

        public async Task<IEnumerable<int>> TopStoryIdsAsync()
        {
            using (HttpClient client = _httpClientFactory.CreateClient())
            {
                var response = await client.GetAsync(_bestStoriesUrl);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<int[]>(responseBody);
            }
        }

        public async Task<Story> GetStoryAsync(int id)
        {
            using (HttpClient client = _httpClientFactory.CreateClient())
            {
                var url = string.Format(_specificStoryUrl, id);
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Story>(responseBody);
            }
        }

        public async Task<ConcurrentDictionary<int, List<Story>>> TopStoriesAsync()
        {
            var allids = await TopStoryIdsAsync();
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
            return storyDictionary;
        }
    }
}
