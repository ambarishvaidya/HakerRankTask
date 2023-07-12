using HackerRankClient.Model;

namespace HackerRankClient
{
    public interface IHackerRankWebClient
    {
        Story GetStory(int storyId);
        Task<Story> GetStoryAsync(int storyId);
        IEnumerable<int> GetAllStoryIds();
        Task<IEnumerable<int>> GetAllStoryIdsAsync();
        IEnumerable<Story> GetTopStories(int count);
        Task<IEnumerable<Story>> GetTopStoriesAsync(int count);
    }
}
