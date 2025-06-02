namespace PokerPlanning.Application.src.Common.Interfaces.Services;

public interface IGameTimer
{
    void Start(Guid gameId, Guid userId, TimeSpan delay, Action OnReveal);
    void Stop(Guid gameId);
}
