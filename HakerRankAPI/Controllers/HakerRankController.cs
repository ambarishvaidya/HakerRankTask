using HackerRankClient;
using HackerRankClient.Model;
using Microsoft.AspNetCore.Mvc;

namespace HakerRankAPI.Controllers
{
    [Route("api/HakerRank")]
    [ApiController]
    public class HakerRankController : Controller
    {
        private IHackerRankWebClient hackerRankWebClient;
        public HakerRankController(IHackerRankWebClient hkWebClient) 
        { 
            hackerRankWebClient = hkWebClient;
        }
        [HttpGet]
        public IEnumerable<int> GetAllStories()
        {
            return hackerRankWebClient.GetAllStoryIdsAsync().Result;            
        }

        [HttpGet("GetTopStories/{count}")]
        public IEnumerable<StoryReadDto> GetTopStories(int count)
        {
            return hackerRankWebClient.GetTopStoriesAsync(count).Result;            
        }

        [HttpGet("GetStory/{id}")]
        public StoryReadDto GetStory(int id)
        {
            return hackerRankWebClient.GetStoryAsync(id).Result;            
        }
    }
}
