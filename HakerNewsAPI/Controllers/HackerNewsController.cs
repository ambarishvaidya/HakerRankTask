using HackerNewsClient;
using HackerNewsClient.Model;
using Microsoft.AspNetCore.Mvc;

namespace HackerNewsAPI.Controllers
{
    [Route("api/HackerNews")]
    [ApiController]
    public class HackerNewsController : Controller
    {
        private IHackerNewsWebClient hackerNewsWebClient;
        private ILogger<HackerNewsController> _log;
        public HackerNewsController(IHackerNewsWebClient hkWebClient, ILogger<HackerNewsController> logger) 
        { 
            hackerNewsWebClient = hkWebClient;
            _log = logger;
        }
        [HttpGet]
        public ActionResult<IEnumerable<int>> GetAllStories()
        {
            try
            {
                return Ok(hackerNewsWebClient.GetAllStoryIdsAsync().Result);
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
                return Ok(hackerNewsWebClient.GetTopStoriesAsync(count).Result);
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
                return Ok(hackerNewsWebClient.GetStoryAsync(id).Result);
            }            
            catch (Exception oex)
            {
                _log.LogError($"Exception in GetStory for {id}. Ex - " + oex.Message + Environment.NewLine + "StackTrace - " + oex.StackTrace);
                return BadRequest(oex.Message);
            }
        }
    }
}
