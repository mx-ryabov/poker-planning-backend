using Microsoft.Extensions.DependencyInjection;
using PokerPlanning.Application.src.Common.Errors;
using PokerPlanning.Application.src.Common.Interfaces.Persistence;
using PokerPlanning.Application.src.Common.Interfaces.Services;
using PokerPlanning.Application.src.GameFeature.Errors;

namespace PokerPlanning.Infrastructure.src.Services;

public class GameTimer : IGameTimer
{
    private readonly ITaskScheduler _scheduler;
    public GameTimer(ITaskScheduler scheduler)
    {
        _scheduler = scheduler;
    }

    public void Start(Guid gameId, Guid userId, TimeSpan delay, Action OnReveal)
    {
        var taskId = gameId.ToString();
        _scheduler.ScheduleTask(
            id: taskId,
            taskFactory: async (serviceProvider) =>
            {
                var cancellationTokenSource = new CancellationTokenSource();
                var cancellationToken = cancellationTokenSource.Token;
                var gameRepository = serviceProvider.GetRequiredService<IGameRepository>();
                var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();

                var participant = await gameRepository.GetParticipant(gameId, userId, cancellationToken) ?? throw new NotFoundException("Participant");
                var game = participant.Game;
                var result = game.RevealCards(participant);

                if (!result.Success)
                {
                    throw new ChangingVotingProcessException(String.Join("; ", result.Errors));
                }
                await unitOfWork.SaveAsync(cancellationToken);
                OnReveal();
            },
            delay
        );
    }

    public void Stop(Guid gameId)
    {

        var taskId = gameId.ToString();
        _scheduler.CancelTask(taskId);
    }
}
