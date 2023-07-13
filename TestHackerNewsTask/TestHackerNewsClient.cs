using HackerNewsClient;
using HackerNewsClient.Cache;
using HackerNewsClient.HttpImplementation;
using HackerNewsClient.Util;
using log4net;
using Microsoft.Extensions.Logging;
using System.Security.Policy;

namespace TestHackerNewsTask
{
    public class Tests
    {
        private IHackerNewsWebClient _hkWebClient_Good, _hkWebClient_Bad;
        private ICommonOperations _commonOperations;
        private IHackerNewsCache _cache;
        private IHttpHackerNews _httpHackerNews;

        ILogger<HttpHackerNewsImpl> httplogger;
        ILogger<HackerNewsWebClientImpl> webClientlogger;
        ILogger<CommonOperations> operLogger;
        ILogger<HackerNewsCacheImpl> cacheLogger;

        [SetUp]
        public void Setup()
        {
            var log = LoggerFactory.Create(lb => lb.SetMinimumLevel(LogLevel.Trace));
            httplogger = log.CreateLogger<HttpHackerNewsImpl>();
            webClientlogger = log.CreateLogger<HackerNewsWebClientImpl>();
            operLogger = log.CreateLogger<CommonOperations>();
            cacheLogger = log.CreateLogger<HackerNewsCacheImpl>();

            _commonOperations = new CommonOperations("https://hacker-news.firebaseio.com/", "v0", operLogger);
            _cache = new HackerNewsCacheImpl(_commonOperations, cacheLogger);
            _httpHackerNews = new HttpHackerNewsImpl(_commonOperations, _cache, httplogger);
            _hkWebClient_Good = new HackerNewsWebClientImpl(_httpHackerNews, webClientlogger);            
        }

        [Test]
        public void GetAllStoryIdsAsync_ValidInput_ExecutesSuccessfully()
        {
            Assert.That(_hkWebClient_Good.GetAllStoryIdsAsync().Result.Any());
        }

        [TestCase("https://hacker-news.firebasedio.com/", "v0")]
        [TestCase("https://hacker-news.firebaseio.com/", "v")]        
        public void GetAllStoryIdsAsync_IncorrectUrlOrVersion_ThrowsException(string url, string version)
        {
            var operations = new CommonOperations(url, version, operLogger);
            var cache = new HackerNewsCacheImpl(operations, cacheLogger);
            var httpHacker = new HttpHackerNewsImpl(operations, cache, httplogger);
            _hkWebClient_Bad = new HackerNewsWebClientImpl(httpHacker, webClientlogger);
            Assert.ThrowsAsync<Exception>(() => _hkWebClient_Bad.GetAllStoryIdsAsync());            
        }

        [Test]
        public void GetStoryAsync_ValidInput_ExecutesSuccessfully()
        {
            var ids = _hkWebClient_Good.GetAllStoryIdsAsync().Result;
            var firstId = ids.First();
            Assert.That(_hkWebClient_Good.GetStoryAsync(firstId).Result, Is.Not.Null);
        }

        [Test]
        public void GetStoryAsync_NegativeId_ThrowsException()
        {
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _hkWebClient_Good.GetStoryAsync(-1));
        }

        [TestCase(1)]        
        [TestCase(100)]
        public void GetTopStoriesAsync_ValidInput_ExecutesSuccessfully(int count)
        {
            var stories = _hkWebClient_Good.GetTopStoriesAsync(count).Result;
            Assert.That(stories.Count(), Is.EqualTo(count));
        }

        [TestCase(-1)]
        [TestCase(0)]
        public void GetTopStoriesAsync_InvalidInput_ThrowsException(int count)
        {
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _hkWebClient_Good.GetTopStoriesAsync(count));            
        }
    }
}