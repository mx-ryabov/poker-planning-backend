using PokerPlanning.Domain.src.Models.GameAggregate;
using PokerPlanning.Domain.src.Models.GameAggregate.Entities;
using PokerPlanning.Domain.src.Models.GameAggregate.Enums;
using PokerPlanning.Domain.UnitTests.Models.GameAggregate.Constants;

namespace PokerPlanning.Domain.UnitTests.Models.GameAggregate.TestUtils;

public static class GameUtils
{
    public static Game CreateGame(Participant master, int initialParticipantsCount = 0)
    {
        var game = Game.Create(
            name: GameConstants.Name,
            link: GameConstants.Link,
            settings: GameConstants.Settings,
            votingSystemId: Guid.NewGuid(),
            master: master
        );
        for (int i = 0; i < initialParticipantsCount; i++)
        {
            game.AddParticipant(ParticipantUtils.CreateParticipant(ParticipantRole.VotingMember));
        }
        return game;
    }
}
