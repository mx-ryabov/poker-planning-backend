using PokerPlanning.Domain.src.Models.GameAggregate.Entities;
using PokerPlanning.Domain.src.Models.GameAggregate.Enums;
using PokerPlanning.Domain.src.Models.UserAggregate.GuestUserAggregate;
using PokerPlanning.TestUtils.ModelUtils.Constants;

namespace PokerPlanning.TestUtils.ModelUtils;

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
