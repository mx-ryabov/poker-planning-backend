using Microsoft.Extensions.DependencyInjection;
using PokerPlanning.Application.src.Common.Interfaces.Services;

namespace PokerPlanning.Infrastructure.src.Services;

public class CustomTaskScheduler : ITaskScheduler
{
    private readonly Dictionary<string, CancellationTokenSource> _cancelationTokenSourceMap;
    private readonly IServiceScopeFactory _scopeFactory;

    public CustomTaskScheduler(IServiceScopeFactory scopeFactory)
    {
        _cancelationTokenSourceMap = new Dictionary<string, CancellationTokenSource>();
        _scopeFactory = scopeFactory;
    }

    public void ScheduleTask(string id, Func<IServiceProvider, Task> taskFactory, TimeSpan delay)
    {
        var cancellationTokenSource = new CancellationTokenSource();
        _cancelationTokenSourceMap.Add(id, cancellationTokenSource);

        Task.Run(async () =>
        {
            try
            {
                await Task.Delay(delay, cancellationTokenSource.Token);

                using var scope = _scopeFactory.CreateScope();
                var serviceProvider = scope.ServiceProvider;

                await taskFactory(serviceProvider);
            }
            finally
            {
                _cancelationTokenSourceMap.Remove(id);
            }
        }, cancellationTokenSource.Token);
    }

    public void CancelTask(string id)
    {
        if (_cancelationTokenSourceMap.TryGetValue(id, out CancellationTokenSource? cancellationTokenSource))
        {
            _cancelationTokenSourceMap.Remove(id);
            cancellationTokenSource.Cancel();
        }
    }
}
