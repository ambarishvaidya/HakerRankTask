using HackerNewsClient;
using HackerNewsClient.HttpImplementation;
using log4net;
using Microsoft.Extensions.Logging;

namespace TestHackerNewsTask
{
    public class Tests
    {
        private IHackerNewsWebClient _hkWebClient_Good, _hkWebClient_Bad;
        ILogger<HttpHackerNewsImpl> httplogger;
        ILogger<HackerNewsWebClientImpl> webClientlogger;

        [SetUp]
        public void Setup()
        {
            var log = LoggerFactory.Create(lb => lb.SetMinimumLevel(LogLevel.Trace));
            httplogger = log.CreateLogger<HttpHackerNewsImpl>();
            webClientlogger = log.CreateLogger<HackerNewsWebClientImpl>();

            _hkWebClient_Good = new HackerNewsWebClientImpl(new HttpHackerNewsImpl("https://hacker-news.firebaseio.com/", "v0", httplogger), webClientlogger);            
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
            _hkWebClient_Bad = new HackerNewsWebClientImpl(new HttpHackerNewsImpl(url, version, httplogger), webClientlogger);
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