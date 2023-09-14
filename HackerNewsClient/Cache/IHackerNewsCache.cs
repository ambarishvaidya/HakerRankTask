using HackerNewsClient.Model;
using System.Collections.Concurrent;

namespace HackerNewsClient.Cache;

public interface IHackerNewsCache
{
    bool IsReady {  get; }
    ConcurrentDictionary<int, List<Story>> Data { get; }
}
