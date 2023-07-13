using HackerNewsClient.Cache;
using HackerNewsClient.Model;
using HackerNewsClient.Util;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace HackerNewsClient.HttpImplementation
{
    public class HttpHackerNewsImpl : IHttpHackerNews
    {
        private ILogger<HttpHackerNewsImpl> _log;
        private ICommonOperations _operations;
        private IHackerNewsCache _cache;
        internal HttpHackerNewsImpl() { }

        public HttpHackerNewsImpl(ICommonOperations operations, IHackerNewsCache cache, ILogger<HttpHackerNewsImpl> logger)
        {
            _log = logger;
            _operations = operations;
            _cache = cache;
        }

        public async Task<IEnumerable<int>> GetAllStoryIdsAsync()
        {
            return await _operations.TopStoryIdsAsync();
        }

        public async Task<Story> GetStoryAsync(int id)
        {
            return await _operations.GetStoryAsync(id);
        }

        public async Task<IEnumerable<Story>> GetTopStoriesAsync(int count)
        {
            ConcurrentDictionary<int, List<Story>> storyDictionary = _cache.IsReady ? _cache.Data : await _operations.TopStoriesAsync();
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
