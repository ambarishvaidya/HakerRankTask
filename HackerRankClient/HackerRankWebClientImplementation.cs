using AutoMapper;
using HackerRankClient.HttpImplementation;
using HackerRankClient.Model;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace HackerRankClient
{
    public class HackerRankWebClientImplementation : IHackerRankWebClient
    {
        private static IMapper Mapper;
        static HackerRankWebClientImplementation()
        {
            Mapper = StoryProfile.Mapper;
        }

        private IHttpHackerRank _httpClient;
        private ILogger<HackerRankWebClientImplementation> _log;

        public HackerRankWebClientImplementation(IHttpHackerRank httpClient, ILogger<HackerRankWebClientImplementation> logger)
        {
            _httpClient = httpClient;
            _log = logger;
        }


        public async Task<IEnumerable<int>> GetAllStoryIdsAsync()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                var response = await _httpClient.GetAllStoryIdsAsync();
                return response;
            }
            catch (HttpRequestException oex)
            {
                _log.LogError("HttpRequestException thrown in GetAllStoryIdsAsync. Ex - " + oex.Message
                    + Environment.NewLine + "StackTrace - " + oex.StackTrace);
                throw new Exception("Incorrect DataURL. Contact Server Support!", oex);
            }
            catch (Exception)
            {                
                throw;
            }
            finally
            {
                stopwatch.Stop();
                _log.LogInformation($"PERF : GetAllStoryIdsAsync : {stopwatch.Elapsed.TotalMilliseconds} ms.");
                stopwatch = null;
            }
        }

        public async Task<StoryReadDto> GetStoryAsync(int storyId)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                if(storyId < 0)
                    throw new ArgumentOutOfRangeException("StoryId has to be a positive number. ");

                var response = await _httpClient.GetStoryAsync(storyId);
                if (response == null)
                    throw new ArgumentOutOfRangeException("No story for " + storyId.ToString() + ".");
                return Mapper.Map<StoryReadDto>(response);
            }
            catch (HttpRequestException oex)
            {
                _log.LogError($"HttpRequestException thrown in GetStoryAsync for {storyId}. Ex - " + oex.Message
                    + Environment.NewLine + "StackTrace - " + oex.StackTrace);
                throw new Exception("Incorrect DataURL. Contact Server Support!", oex);
            }                        
            catch (Exception)
            {
                throw;
            }
            finally
            {
                stopwatch.Stop();
                _log.LogInformation($"PERF : GetStoryAsync({storyId}) : {stopwatch.Elapsed.TotalMilliseconds} ms.");
                stopwatch = null;
            }
        }

        public async Task<IEnumerable<StoryReadDto>> GetTopStoriesAsync(int count)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                if (count <= 0)
                    throw new ArgumentOutOfRangeException("Count has to be a positive number greater than 0.");

                var response = await _httpClient.GetTopStoriesAsync(count);
                return Mapper.Map<IEnumerable<StoryReadDto>>(response);
            }
            catch (HttpRequestException oex)
            {
                _log.LogError($"HttpRequestException thrown in GetTopStoriesAsync for {count}. Ex - " + oex.Message
                    + Environment.NewLine + "StackTrace - " + oex.StackTrace);
                throw new Exception("Incorrect DataURL. Contact Server Support!", oex);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                stopwatch.Stop();
                _log.LogInformation($"PERF : GetTopStoriesAsync({count}) : {stopwatch.Elapsed.TotalMilliseconds} ms.");
                stopwatch = null;
            }
        }
    }
}
