using HackerRankClient.HttpImplementation;

namespace TestHackerRankTask
{
    public class TestHttpHackerRankImpl
    {
        HttpHackerRankImpl _hkImpl;
        [SetUp]
        public void Setup()
        {
            
        }

        [TestCase(@"http://abc/v0/test1.json", @"http://abc/", "v0/", "test1.json")]
        [TestCase(@"http://abc/v0/item/test1.json", @"http://abc/", "v0/", "item","test1.json")]
        [TestCase(@"http://abc/v0/item/test1.json", @" http://abc  ", "/v0/", "item", "test1.json")]
        [TestCase(@"http://abc/v0/item/{0}.json", @" http://abc  ", "/v0/", "item", "{0}.json")]
        public void BuildUrl_ValidInputs_ExecutesSuccessfully(string expectedResp, params string[] tokens)
        {
            _hkImpl = new HttpHackerRankImpl("", "");
            Assert.That(_hkImpl.BuildUrl(tokens), Is.EqualTo(expectedResp));
        }
    }
}
