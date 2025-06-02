namespace PokerPlanning.Application.src.Common.Interfaces.Services;

public interface ITaskScheduler
{
    void ScheduleTask(string id, Func<IServiceProvider, Task> taskFactory, TimeSpan delay);
    void CancelTask(string id);
}
