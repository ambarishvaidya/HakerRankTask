using HackerRankClient;
using HackerRankClient.HttpImplementation;

namespace TestHackerRankTask
{
    public class Tests
    {
        private IHackerRankWebClient _hkWebClient_Good, _hkWebClient_Bad;

        [SetUp]
        public void Setup()
        {
            _hkWebClient_Good = new HackerRankWebClientImplementation(new HttpHackerRankImpl("https://hacker-news.firebaseio.com/", "v0"));            
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
            _hkWebClient_Bad = new HackerRankWebClientImplementation(new HttpHackerRankImpl(url, version));
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