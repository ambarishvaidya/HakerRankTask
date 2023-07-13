using HackerNewsClient;
using HackerNewsClient.HttpImplementation;
using Microsoft.Extensions.Logging;

namespace TestHackerNewsTask
{
    public class TestHttpHackerNewsImpl
    {
        HttpHackerNewsImpl _hkImpl;
        ILogger<HttpHackerNewsImpl> httplogger;
        ILogger<HackerNewsWebClientImpl> webClientlogger;

        [SetUp]
        public void Setup()
        {
            var log = LoggerFactory.Create(lb => lb.SetMinimumLevel(LogLevel.Trace));
            httplogger = log.CreateLogger<HttpHackerNewsImpl>();
            webClientlogger = log.CreateLogger<HackerNewsWebClientImpl>();

        }

        [TestCase(@"http://abc/v0/test1.json", @"http://abc/", "v0/", "test1.json")]
        [TestCase(@"http://abc/v0/item/test1.json", @"http://abc/", "v0/", "item","test1.json")]
        [TestCase(@"http://abc/v0/item/test1.json", @" http://abc  ", "/v0/", "item", "test1.json")]
        [TestCase(@"http://abc/v0/item/{0}.json", @" http://abc  ", "/v0/", "item", "{0}.json")]
        public void BuildUrl_ValidInputs_ExecutesSuccessfully(string expectedResp, params string[] tokens)
        {
            _hkImpl = new HttpHackerNewsImpl("", "", httplogger);
            Assert.That(_hkImpl.BuildUrl(tokens), Is.EqualTo(expectedResp));
        }
    }
}
