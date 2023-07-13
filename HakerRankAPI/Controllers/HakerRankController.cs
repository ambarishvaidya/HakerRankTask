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
        public ActionResult<IEnumerable<int>> GetAllStories()
        {
            try
            {
                return Ok(hackerRankWebClient.GetAllStoryIdsAsync().Result);
            }
            catch(Exception oex)
            {
                return BadRequest(oex.Message);
            }
        }

        [HttpGet("GetTopStories/{count}")]
        public ActionResult<IEnumerable<StoryReadDto>> GetTopStories(int count)
        {
            try
            {
                return Ok(hackerRankWebClient.GetTopStoriesAsync(count).Result);
            }
            catch (Exception oex)
            {
                return BadRequest(oex.Message);
            }
        }

        [HttpGet("GetStory/{id}")]
        public ActionResult<StoryReadDto> GetStory(int id)
        {
            try
            {
                return Ok(hackerRankWebClient.GetStoryAsync(id).Result);
            }            
            catch (Exception oex)
            {
                return BadRequest(oex.Message);
            }
        }
    }
}
