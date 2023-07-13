using HackerNewsClient.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Security.Policy;

namespace HackerNewsClient.Cache
{
    public class HackerNewsCacheImpl : IHackerNewsCache
    {
        private ILogger<HackerNewsCacheImpl> _log;
        private IConfiguration _configuration;
        private System.Timers.Timer _timer;
        private bool isReady = false;

        private string _url;
        private string _version;

        private string _bestStoriesUrl;
        private string _specificStoryUrl;

        private const string _beststoriestail = "beststories.json";
        private const string _itemtoken = "item";
        private const char _slash = '/';
        private char[] _slashChar = new char[] { _slash };

        public HackerNewsCacheImpl(IConfiguration config, ILogger<HackerNewsCacheImpl> logger)
        {
            _log = logger;
            _configuration = config;
            _url = config["HackerNewsUrl"];
            _version = config["HackerNewsApiVersion"];
            _timer = new System.Timers.Timer();
            _timer.Elapsed += BuildCache;
            isReady = false;
            _timer.Interval = 2000;
            _timer.Start();
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

        private void BuildCache(object? sender, System.Timers.ElapsedEventArgs e)
        {
            using(HttpClient client = new HttpClient())
            {
                var ids = client.GetAsync(_bestStoriesUrl).Result;
            }
            isReady = true;
        }

        public bool IsReady => isReady;

        public ConcurrentDictionary<int, List<Story>> Data => throw new NotImplementedException();
    }
}
