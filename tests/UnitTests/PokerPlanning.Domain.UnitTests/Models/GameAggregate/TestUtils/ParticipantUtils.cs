using PokerPlanning.Domain.src.Models.GameAggregate.Entities;
using PokerPlanning.Domain.src.Models.GameAggregate.Enums;
using PokerPlanning.Domain.src.Models.UserAggregate.GuestUserAggregate;
using PokerPlanning.Domain.UnitTests.Models.GameAggregate.Constants;

namespace PokerPlanning.Domain.UnitTests.Models.GameAggregate.TestUtils;

public static class ParticipantUtils
{
    public static Participant CreateParticipant(ParticipantRole role)
    {
        return Participant.Create(
            displayName: ParticipantConstants.DisplayName,
            role: role,
            user: GuestUser.Create(ParticipantConstants.DisplayName)
        );
    }
}
