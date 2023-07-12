using HackerRankClient;

namespace TestHackerRankTask
{
    public class Tests
    {
        private IHackerRankWebClient _hkWebClient;

        [SetUp]
        public void Setup()
        {
            _hkWebClient = new HackerRankWebClientImplementation();
        }

        [Test]
        public void GetAllStoryIds_ValidInput_ExecutesSuccessfully()
        {
            Assert.That(_hkWebClient.GetAllStoryIds().Any());
        }

        [Test]
        public void GetStory_ValidInput_ExecutesSuccessfully()
        {
            var ids = _hkWebClient.GetAllStoryIds();
            var firstId = ids.First();
            Assert.That(_hkWebClient.GetStory(firstId), Is.Not.Null);
        }

        [Test]
        public void GetStoryAsync_ValidInput_ExecutesSuccessfully()
        {
            var ids = _hkWebClient.GetAllStoryIds();
            var firstId = ids.First();
            Assert.That(_hkWebClient.GetStoryAsync(firstId).Result, Is.Not.Null);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(66)]
        [TestCase(100)]
        public void GetTopStories_ValidInput_ExecutesSuccessfully(int count)
        {
            var stories = _hkWebClient.GetTopStories(count);
            Assert.That(stories.Count(), Is.EqualTo(count));
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