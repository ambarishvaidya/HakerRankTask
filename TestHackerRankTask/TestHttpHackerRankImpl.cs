using HackerRankClient;
using HackerRankClient.HttpImplementation;
using Microsoft.Extensions.Logging;

namespace TestHackerRankTask
{
    public class TestHttpHackerRankImpl
    {
        HttpHackerRankImpl _hkImpl;
        ILogger<HttpHackerRankImpl> httplogger;
        ILogger<HackerRankWebClientImplementation> webClientlogger;

        [SetUp]
        public void Setup()
        {
            var log = LoggerFactory.Create(lb => lb.SetMinimumLevel(LogLevel.Trace));
            httplogger = log.CreateLogger<HttpHackerRankImpl>();
            webClientlogger = log.CreateLogger<HackerRankWebClientImplementation>();

        }

        [TestCase(@"http://abc/v0/test1.json", @"http://abc/", "v0/", "test1.json")]
        [TestCase(@"http://abc/v0/item/test1.json", @"http://abc/", "v0/", "item","test1.json")]
        [TestCase(@"http://abc/v0/item/test1.json", @" http://abc  ", "/v0/", "item", "test1.json")]
        [TestCase(@"http://abc/v0/item/{0}.json", @" http://abc  ", "/v0/", "item", "{0}.json")]
        public void BuildUrl_ValidInputs_ExecutesSuccessfully(string expectedResp, params string[] tokens)
        {
            _hkImpl = new HttpHackerRankImpl("", "", httplogger);
            Assert.That(_hkImpl.BuildUrl(tokens), Is.EqualTo(expectedResp));
        }
    }
}
