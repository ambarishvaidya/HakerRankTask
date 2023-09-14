using HackerNewsClient.Model;
using HackerNewsClient.Util;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace HackerNewsClient.Cache;

public class HackerNewsCacheImpl : IHackerNewsCache, IDisposable
{
    private ILogger<HackerNewsCacheImpl> _log;
    private ICommonOperations _commonOperations;
    private ConcurrentDictionary<int, List<Story>> _data;

    private System.Timers.Timer _timer;
    private bool isReady = false;
    private Stopwatch _stopWatch;
    public HackerNewsCacheImpl(ICommonOperations operations, ILogger<HackerNewsCacheImpl> logger)
    {
        _log = logger;
        _commonOperations = operations;
        _timer = new System.Timers.Timer();
        _timer.Elapsed += BuildCache;
        isReady = false;
        _timer.Start();
        _log.LogInformation($"Timer started to build Cache.");
        _stopWatch = Stopwatch.StartNew();
    }


    private void BuildCache(object? sender, System.Timers.ElapsedEventArgs e)
    {
        _stopWatch.Start();
        _timer.Stop();
        _data = _commonOperations.TopStoriesAsync().Result;
        isReady = true;
        _stopWatch.Stop();
        _log.LogInformation($"PERF : BuildCache : {_stopWatch.Elapsed.TotalMilliseconds} ms");
        _timer.Interval = 1000;
        _stopWatch.Reset();
        _timer.Start();
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    public bool IsReady => isReady;

    public ConcurrentDictionary<int, List<Story>> Data => _data;
}
