using HackerRankClient.Model;

namespace HackerRankClient
{
    public interface IHackerRankWebClient
    {
        Task<StoryReadDto> GetStoryAsync(int storyId);
        Task<IEnumerable<int>> GetAllStoryIdsAsync();
        Task<IEnumerable<StoryReadDto>> GetTopStoriesAsync(int count);
    }
}
