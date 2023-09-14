using AutoMapper;
using HackerNewsClient.HttpImplementation;
using HackerNewsClient.Model;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace HackerNewsClient;

public class HackerNewsWebClientImpl : IHackerNewsWebClient
{
    private static IMapper Mapper;
    static HackerNewsWebClientImpl()
    {
        Mapper = StoryProfile.Mapper;
    }

    private IHttpHackerNews _httpClient;
    private ILogger<HackerNewsWebClientImpl> _log;

    public HackerNewsWebClientImpl(IHttpHackerNews httpClient, ILogger<HackerNewsWebClientImpl> logger)
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
