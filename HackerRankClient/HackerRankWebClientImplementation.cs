using AutoMapper;
using HackerRankClient.HttpImplementation;
using HackerRankClient.Model;

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
        
        public HackerRankWebClientImplementation(IHttpHackerRank httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<IEnumerable<int>> GetAllStoryIdsAsync()
        {
            try
            {
                var response = await _httpClient.GetAllStoryIdsAsync();
                return response;
            }
            catch (HttpRequestException oex)
            {
                throw new Exception("Incorrect DataURL. Contact Server Support!", oex);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<StoryReadDto> GetStoryAsync(int storyId)
        {
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
                throw new Exception("Incorrect DataURL. Contact Server Support!", oex);
            }                        
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<StoryReadDto>> GetTopStoriesAsync(int count)
        {
            try
            {
                if (count <= 0)
                    throw new ArgumentOutOfRangeException("Count has to be a positive number greater than 0.");

                var response = await _httpClient.GetTopStoriesAsync(count);
                return Mapper.Map<IEnumerable<StoryReadDto>>(response);
            }
            catch (HttpRequestException oex)
            {
                throw new Exception("Incorrect DataURL. Contact Server Support!", oex);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
