using PokerPlanning.Domain.src.Common.DTO;
using PokerPlanning.Domain.src.Models.TicketAggregate;
using PokerPlanning.Domain.src.Models.TicketAggregate.Enums;
using PokerPlanning.TestUtils.ModelUtils.Constants;

namespace PokerPlanning.TestUtils.ModelUtils;

public static class TicketUtils
{
    public static Ticket CreateTicket()
    {
        return Ticket.CreateNew(
            title: TicketConstants.Title,
            type: TicketConstants.Type,
            gameId: Guid.Parse(TicketConstants.GameId)
        );
    }

    public static UpdateTicketDTO GetUpdateTicketDTO()
    {
        return new UpdateTicketDTO(
                Title: "Ticket Id",
                Description: "Ticket Description",
                Link: "http://test.org",
                Type: TicketType.Story,
                Identifier: "TI-123",
                Estimation: "5"
            );
    }
}
