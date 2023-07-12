using HackerRankClient.Model;

namespace HackerRankClient
{
    public interface IHackerRankWebClient
    {
        Story GetStory(int storyId);
        Task<Story> GetStoryAsync(int storyId);
        IEnumerable<int> GetAllStoryIds();
        IEnumerable<Story> GetTopStories(int count);
    }
}
