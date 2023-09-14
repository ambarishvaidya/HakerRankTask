using HackerNewsClient.Model;
using System.Collections.Concurrent;

namespace HackerNewsClient.Util;

public interface ICommonOperations
{
    string Url { get; }
    string Version { get; }
    string TopStoriesUrl { get; }
    string SpecificStoryUrl(int storyId);
    Task<ConcurrentDictionary<int, List<Story>>> TopStoriesAsync();
    Task<IEnumerable<int>> TopStoryIdsAsync();
    Task<Story> GetStoryAsync(int storyId);
}
