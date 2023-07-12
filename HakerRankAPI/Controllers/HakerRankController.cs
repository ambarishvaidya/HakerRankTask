using HackerRankClient;
using HackerRankClient.Model;
using Microsoft.AspNetCore.Mvc;

namespace HakerRankAPI.Controllers
{
    [Route("api/HakerRank")]
    [ApiController]
    public class HakerRankController : Controller
    {
        [HttpGet]
        public IEnumerable<int> GetAllStories()
        {
            return (new HackerRankWebClientImplementation()).GetAllStoryIdsAsync().Result;            
        }

        [HttpGet("GetTopStories/{count}")]
        public IEnumerable<Story> GetTopStories(int count)
        {
            return (new HackerRankWebClientImplementation()).GetTopStoriesAsync(count).Result;            
        }

        [HttpGet("GetStory/{id}")]
        public Story GetStory(int id)
        {
            return (new HackerRankWebClientImplementation()).GetStoryAsync(id).Result;            
        }
    }
}
