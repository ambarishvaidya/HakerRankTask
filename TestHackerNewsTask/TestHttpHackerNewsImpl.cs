using HackerNewsClient.Util;
using Microsoft.Extensions.Logging;

namespace TestHackerNewsTask
{
    public class TestHttpHackerNewsImpl
    {
        ILogger<CommonOperations> operLogger;

        [SetUp]
        public void Setup()
        {
            var log = LoggerFactory.Create(lb => lb.SetMinimumLevel(LogLevel.Trace));
            operLogger = log.CreateLogger<CommonOperations>();
        }

        [TestCase(@"http://abc/v0/test1.json", @"http://abc/", "v0/", "test1.json")]
        [TestCase(@"http://abc/v0/item/test1.json", @"http://abc/", "v0/", "item", "test1.json")]
        [TestCase(@"http://abc/v0/item/test1.json", @" http://abc  ", "/v0/", "item", "test1.json")]
        [TestCase(@"http://abc/v0/item/{0}.json", @" http://abc  ", "/v0/", "item", "{0}.json")]
        public void BuildUrl_ValidInputs_ExecutesSuccessfully(string expectedResp, params string[] tokens)
        {
            var co = new CommonOperations("", "", operLogger);
            Assert.That(co.BuildUrl(tokens), Is.EqualTo(expectedResp));
        }
    }
}
