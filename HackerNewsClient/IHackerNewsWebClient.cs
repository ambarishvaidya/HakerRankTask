using HackerNewsClient.Model;

namespace HackerNewsClient;

public interface IHackerNewsWebClient
{
    Task<StoryReadDto> GetStoryAsync(int storyId);
    Task<IEnumerable<int>> GetAllStoryIdsAsync();
    Task<IEnumerable<StoryReadDto>> GetTopStoriesAsync(int count);
}
