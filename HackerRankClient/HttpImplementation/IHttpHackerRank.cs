using HackerRankClient.Model;

namespace HackerRankClient.HttpImplementation
{
    public interface IHttpHackerRank
    {
        Task<IEnumerable<int>> GetAllStoryIdsAsync();
        Task<Story> GetStoryAsync(int id);
        Task<IEnumerable<Story>> GetTopStoriesAsync(int count);
    }
}
