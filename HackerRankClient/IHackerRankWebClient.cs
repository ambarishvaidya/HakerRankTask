using HackerRankClient.Model;

namespace HackerRankClient
{
    public interface IHackerRankWebClient
    {
        Task<Story> GetStoryAsync(int storyId);
        Task<IEnumerable<int>> GetAllStoryIdsAsync();
        Task<IEnumerable<Story>> GetTopStoriesAsync(int count);
    }
}
