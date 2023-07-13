using HackerRankClient;
using HackerRankClient.HttpImplementation;

namespace TestHackerRankTask
{
    public class Tests
    {
        private IHackerRankWebClient _hkWebClient;

        [SetUp]
        public void Setup()
        {
            _hkWebClient = new HackerRankWebClientImplementation(new HttpHackerRankImpl("https://hacker-news.firebaseio.com/", "v0"));
        }

        [Test]
        public void GetAllStoryIdsAsync_ValidInput_ExecutesSuccessfully()
        {
            Assert.That(_hkWebClient.GetAllStoryIdsAsync().Result.Any());
        }

        [Test]
        public void GetStoryAsync_ValidInput_ExecutesSuccessfully()
        {
            var ids = _hkWebClient.GetAllStoryIdsAsync().Result;
            var firstId = ids.First();
            Assert.That(_hkWebClient.GetStoryAsync(firstId).Result, Is.Not.Null);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(66)]
        [TestCase(100)]
        public void GetTopStoriesAsync_ValidInput_ExecutesSuccessfully(int count)
        {
            var stories = _hkWebClient.GetTopStoriesAsync(count).Result;
            Assert.That(stories.Count(), Is.EqualTo(count));
        }
    }
}