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
        public HackerRankWebClientImplementation()
        {
            _httpClient = new HttpHackerRankImpl("https://hacker-news.firebaseio.com/", "v0");
        }

        internal HackerRankWebClientImplementation(IHttpHackerRank httpClient)
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
            catch (Exception oex)
            {
                return Array.Empty<int>();
            }
        }

        public async Task<StoryReadDto> GetStoryAsync(int storyId)
        {
            try
            {
                var response = await _httpClient.GetStoryAsync(storyId);
                return Mapper.Map<StoryReadDto>(response);

            }
            catch (Exception oex)
            {
                return new StoryReadDto();
            }
        }

        public async Task<IEnumerable<StoryReadDto>> GetTopStoriesAsync(int count)
        {
            try
            {
                var response = await _httpClient.GetTopStoriesAsync(count);
                return Mapper.Map<IEnumerable<StoryReadDto>>(response);
            }
            catch (Exception oex)
            {
                return Array.Empty<StoryReadDto>();
            }
        }
    }
}
