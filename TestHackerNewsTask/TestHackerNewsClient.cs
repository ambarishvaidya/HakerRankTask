using HackerNewsClient;
using HackerNewsClient.Cache;
using HackerNewsClient.HttpImplementation;
using HackerNewsClient.Model;
using HackerNewsClient.Util;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Concurrent;

namespace TestHackerNewsTask
{
    public class Tests
    {
        private IHackerNewsWebClient _hkWebClient_Good, _hkWebClient_Bad;
        private Mock<ICommonOperations> _commonOperationsMock;
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

            _commonOperationsMock = new Mock<ICommonOperations>();
            
            _cache = new HackerNewsCacheImpl(_commonOperationsMock.Object, cacheLogger);
            _httpHackerNews = new HttpHackerNewsImpl(_commonOperationsMock.Object, _cache, httplogger);
            _hkWebClient_Good = new HackerNewsWebClientImpl(_httpHackerNews, webClientlogger);
        }

        [Test]
        public void GetAllStoryIdsAsync_ValidInput_ExecutesSuccessfully()
        {
            _commonOperationsMock.Setup(co => co.TopStoryIdsAsync()).ReturnsAsync(Enumerable.Range(1, 10));
            Assert.That(_hkWebClient_Good.GetAllStoryIdsAsync().Result.Any());
        }

        [Test]
        public void GetStoryAsync_ValidInput_ExecutesSuccessfully()
        {
            _commonOperationsMock.Setup(co => co.GetStoryAsync(It.IsAny<int>())).ReturnsAsync(new HackerNewsClient.Model.Story());
            Assert.That(_hkWebClient_Good.GetStoryAsync(It.IsAny<int>()).Result, Is.Not.Null);
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
            var items = Enumerable.Range(1, count).Select(i => new KeyValuePair<int, List<Story>>(i, new List<Story>() { new Story() { id = i }}));
            var cd = new ConcurrentDictionary<int, List<Story>>(items);
            _commonOperationsMock.Setup(co => co.TopStoriesAsync()).ReturnsAsync(cd);
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

    public class MyHttpClient : IHttpClientFactory
    {
        public HttpClient CreateClient(string name)
        {
            return this.CreateClient();
        }
    }
}