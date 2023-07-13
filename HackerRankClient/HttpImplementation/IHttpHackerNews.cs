using HackerNewsClient.Model;

namespace HackerNewsClient.HttpImplementation
{
    public interface IHttpHackerNews
    {
        Task<IEnumerable<int>> GetAllStoryIdsAsync();
        Task<Story> GetStoryAsync(int id);
        Task<IEnumerable<Story>> GetTopStoriesAsync(int count);
    }
}
