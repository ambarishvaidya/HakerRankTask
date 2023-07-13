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
        private ILogger<HakerRankController> _log;
        public HakerRankController(IHackerRankWebClient hkWebClient, ILogger<HakerRankController> logger) 
        { 
            hackerRankWebClient = hkWebClient;
            _log = logger;
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
                _log.LogError("Exception in GetAllStories. Ex - " + oex.Message + Environment.NewLine + "StackTrace - " + oex.StackTrace);
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
                _log.LogError($"Exception in GetTopStories for {count}. Ex - " + oex.Message + Environment.NewLine + "StackTrace - " + oex.StackTrace);
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
                _log.LogError($"Exception in GetStory for {id}. Ex - " + oex.Message + Environment.NewLine + "StackTrace - " + oex.StackTrace);
                return BadRequest(oex.Message);
            }
        }
    }
}
