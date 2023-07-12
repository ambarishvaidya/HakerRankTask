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
            return Enumerable.Range(1, 200).ToArray();
        }

        [HttpGet("GetTopStories/{count}")]
        public IEnumerable<int> GetTopStories(int count)
        {
            return Enumerable.Range(1, count);
        }

        [HttpGet("GetStory/{id}")]
        public IEnumerable<int> GetStory(int id)
        {
            return new[] { id };
        }
    }
}
