using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PokerPlanning.Api.IntegrationTests.Utilities;
using PokerPlanning.Application.src.GameFeature.Results;
using PokerPlanning.Contracts.src.Game;
using PokerPlanning.Domain.src.Models.GameAggregate.Enums;
using PokerPlanning.Domain.src.Models.TicketAggregate.Enums;
using System.Net;
using System.Net.Http.Json;
using System.Net.Sockets;

namespace PokerPlanning.Api.IntegrationTests.Controllers;

public class GameControllerTests : BaseIntegrationTest
{
    private readonly GameHelper _helper;
    public GameControllerTests(PokerPlanningWebApplicationFactory factory) : base(factory)
    {
        _helper = new GameHelper(_client);
    }

    [Fact]
    public async Task CreateGame_WhenRequestValid_CreatesAndReturnGameAndToken()
    {
        var requestData = await _helper.CreateGameRequest();

        var response = await _client.PostAsJsonAsync("/api/games", requestData);

        response.EnsureSuccessStatusCode();
        var matchResponse = await response.Content.ReadFromJsonAsync<CreateGameResponse>();
        matchResponse.Should().NotBeNull();
        matchResponse!.Name.Should().Be(requestData.Name);

        var createdGame = await _dbContext.Games
                .Include(g => g.Participants)
                .SingleAsync(g => g.Id == matchResponse.Id);
        createdGame.Should().NotBeNull();
        createdGame.Participants.Should().HaveCount(1);
        var master = createdGame.Participants[0];
        master.Role.Should().Be(ParticipantRole.Master);
        master.UserId.Should().NotBeNull();
    }

    [Fact]
    public async Task CreateGame_WhenRequestInvalid_ThrowsBadRequestException()
    {
        var votingSystem = await _helper.GetVotingSystem();
        votingSystem.Should().NotBeNull();
        var invalidData = new{};

        var response = await _client.PostAsJsonAsync("/api/games", invalidData);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [MemberData(nameof(JoinAsGuestInvalidRequestData))]
    public async Task JoinAsGuest_WhenRequestDataInvalid_ThrowsBadRequestException(object requestData, string gameId)
    {
        var response = await _client.PostAsJsonAsync($"/api/games/{gameId}/join-as-guest", requestData);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    public static IEnumerable<object[]> JoinAsGuestInvalidRequestData =>
        new List<object[]>
        {
            new object[] { new { }, "qwe" },
            new object[] { new { }, Guid.NewGuid().ToString() },
        };

    [Fact]
    public async Task JoinAsGuest_WhenRequestDataValid_CreatesParticipantAndReturnsToken()
    {
        var game = await _helper.CreateGame();
        var randomParticipantName = _helper.GetParticipantName();
        JoinAsGuestRequest requestData = new(randomParticipantName);
        var response = await _client.PostAsJsonAsync($"/api/games/{game!.Id}/join-as-guest", requestData);

        response.EnsureSuccessStatusCode();
        var matchResponse = await response.Content.ReadFromJsonAsync<JoinAsGuestGameResult>();
        matchResponse.Should().NotBeNull();
        matchResponse!.Token.Should().BeOfType<string>();
        var createdGame = await _dbContext.Games
                .Include(g => g.Participants)
                .SingleAsync(g => g.Id == game!.Id);
        createdGame.Should().NotBeNull();
        createdGame.Participants.Count().Should().Be(2);
        createdGame.Participants.Single(p => p.DisplayName == randomParticipantName).Should().NotBeNull();
    }

    [Fact]
    public async Task GetGameById_WhenUserIsAuthorizedForGame_ReturnsGameResult()
    {
        var game = await _helper.CreateGame();
        _helper.SetToken(game!.MasterToken);

        var response = await _client.GetAsync($"/api/games/{game!.Id}");

        response.EnsureSuccessStatusCode();
        var matchResponse = await response.Content.ReadFromJsonAsync<GetGameResult>();
        matchResponse.Should().NotBeNull();
        matchResponse!.Id.Should().Be(game!.Id);
        _client.DefaultRequestHeaders.Authorization = null;
        _helper.SetToken(null);
    }

    [Fact]
    public async Task GetGameById_WhenUserIsUnauthorizedForGame_ThrowsUnauthorizedException()
    {
        var game = await _helper.CreateGame();
        var response = await _client.GetAsync($"/api/games/{game!.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetCurrentParticipantByGameId_WhenUserIsAuthorizedForGame_ReturnsParticipantInfo()
    {
        var game = await _helper.CreateGame();
        _helper.SetToken(game!.MasterToken);

        var response = await _client.GetAsync($"/api/games/{game!.Id}/current-participant");
        response.EnsureSuccessStatusCode();
        var matchResponse = await response.Content.ReadFromJsonAsync<GameParticipantResult>();
        matchResponse.Should().NotBeNull();
        var createdGame = await _dbContext.Games
                .Include(g => g.Participants
                    .Where(p => p.Id == matchResponse!.Id && p.DisplayName == matchResponse!.DisplayName))
                .SingleAsync(g => g.Id == game!.Id);
        createdGame.Should().NotBeNull();
        _helper.SetToken(null);
    }

    [Fact]
    public async Task StartVoting_ValidRequest_ReturnsSuccessCodeAndStartVoting()
    {
        var game = await _helper.CreateGame();
        _helper.SetToken(game!.MasterToken);
        var ticket = await _helper.AddTicketToGame(game.Id);
        var request = new StartVotingRequest(ticket!.Id);

        var response = await _client.PutAsJsonAsync($"/api/games/{game!.Id}/start-voting", request);
        
        response.EnsureSuccessStatusCode();
        var createdGame = await _dbContext.Games
                .SingleAsync(g => g.Id == game!.Id);
        createdGame.Should().NotBeNull();
        createdGame.VotingProcess.IsActive.Should().BeTrue();
        createdGame.VotingProcess.TicketId.Should().Be(ticket!.Id);
        _helper.SetToken(null);
    }

    [Fact]
    public async Task FinishVoting_ValidRequest_ReturnsSuccessCodeAndFinishVoting()
    {
        var game = await _helper.CreateGame();
        _helper.SetToken(game!.MasterToken);
        var ticket = await _helper.AddTicketToGame(game.Id);
        await _helper.StartVoting(game!.Id, ticket!.Id);
        
        var response = await _client.PutAsJsonAsync($"/api/games/{game!.Id}/finish-voting", new object());

        response.EnsureSuccessStatusCode();
        var createdGame = await _dbContext.Games
                .Include(g => g.VotingResults)
                .ThenInclude(vr => vr.Votes)
                .Include(g => g.Participants)
                .SingleAsync(g => g.Id == game!.Id);
        createdGame.Should().NotBeNull();
        createdGame.VotingProcess.IsActive.Should().BeFalse();
        createdGame.VotingProcess.TicketId.Should().Be(null);
        createdGame.VotingResults.Count.Should().Be(1);
        createdGame.VotingResults.First().Votes.Count.Should().Be(1);
        createdGame.Participants.Find(p => p.VoteId != null).Should().BeNull();
        _helper.SetToken(null);
    }

    [Fact]
    public async Task Vote_ValidRequest_ReturnsSuccessCodeAndChangeVoteForCurrentParticipant()
    {
        // Arrange
        var game = await _helper.CreateGame();
        _helper.SetToken(game!.MasterToken);

        var ticket = await _helper.AddTicketToGame(game.Id);
        await _helper.StartVoting(game!.Id, ticket!.Id);

        _helper.SetToken(null);
        var joinAsGuestResult = await _helper.JoinAsParticipant(game!.Id);

        _helper.SetToken(joinAsGuestResult!.Token);
        var participantResult = await _helper.GetCurrentParticipant(game!.Id);
        var votingSystem = await _helper.GetVotingSystem();
        var vote = votingSystem?.Votes.First();
        DoVoteRequest request = new(vote!.Id) { };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/games/{game!.Id}/vote", request);

        // Assert
        response.EnsureSuccessStatusCode();
        var createdParticipant = await _dbContext.Participants
            .Where(p => p.Id == participantResult!.Id && p.GameId == game!.Id)
            .SingleOrDefaultAsync();
        createdParticipant.Should().NotBeNull();
        createdParticipant!.VoteId.Should().Be(vote!.Id);
        _helper.SetToken(null);
    }

    [Fact]
    public async Task AddTicket_ValidRequest_ReturnsTicketResult()
    {
        // Arrange
        var game = await _helper.CreateGame();
        _helper.SetToken(game!.MasterToken);
        var ticketName = _helper.GetTicketName();
        var ticketType = TicketType.Task;
        AddTicketRequest request = new(ticketName, ticketType) { };

        // Act
        var response = await _client.PostAsJsonAsync($"/api/games/{game!.Id}/ticket", request);

        // Assert
        response.EnsureSuccessStatusCode();
        var matchResponse = await response.Content.ReadFromJsonAsync<TicketResult>();
        matchResponse.Should().NotBeNull();
        matchResponse!.Title.Should().Be(ticketName);
        matchResponse!.Type.Should().Be(ticketType);
        var createdGame = await _dbContext.Games
            .Include(g => g.Tickets)
            .SingleOrDefaultAsync(g => g.Id == game.Id);
        createdGame!.Tickets.Count.Should().Be(1);
        createdGame!.Tickets[0].Id.Should().Be(matchResponse.Id);
        createdGame!.Tickets[0].Title.Should().Be(ticketName);
        createdGame!.Tickets[0].Type.Should().Be(ticketType);
        _helper.SetToken(null);
    }

    [Fact]
    public async Task UpdateTicket_ValidRequest_ReturnsTicketResult()
    {
        // Arrange
        var game = await _helper.CreateGame();
        _helper.SetToken(game!.MasterToken);
        var addedTicket = await _helper.AddTicketToGame(game.Id);
        UpdateTicketRequest request = _helper.CreateUpdateRequest();

        // Act
        var response = await _client.PutAsJsonAsync($"/api/games/{game!.Id}/ticket/{addedTicket!.Id}", request);

        // Assert
        response.EnsureSuccessStatusCode();
        var matchResponse = await response.Content.ReadFromJsonAsync<TicketResult>();
        matchResponse.Should().NotBeNull();
        matchResponse!.Title.Should().Be(request.Data.Title);
        matchResponse!.Description.Should().Be(request.Data.Description);
        matchResponse!.Estimation.Should().Be(request.Data.Estimation);
        matchResponse!.Type.Should().Be(request.Data.Type);
        var createdGame = await _dbContext.Games
            .Include(g => g.Tickets)
            .SingleOrDefaultAsync(g => g.Id == game.Id);
        createdGame!.Tickets[0].Id.Should().Be(matchResponse.Id);
        createdGame!.Tickets[0].Title.Should().Be(request.Data.Title);
        createdGame!.Tickets[0].Type.Should().Be(request.Data.Type);
        createdGame!.Tickets[0].Description.Should().Be(request.Data.Description);
        createdGame!.Tickets[0].Estimation.Should().Be(request.Data.Estimation);
        _helper.SetToken(null);
    }

    [Fact]
    public async Task DeleteTicket_ValidRequest_ReturnsTrue()
    {
        // Arrange
        var game = await _helper.CreateGame();
        _helper.SetToken(game!.MasterToken);
        var addedTicket = await _helper.AddTicketToGame(game.Id);

        // Act
        var response = await _client.DeleteAsync($"/api/games/{game!.Id}/ticket/{addedTicket!.Id}");

        // Assert
        response.EnsureSuccessStatusCode();
        var matchResponse = await response.Content.ReadFromJsonAsync<bool>();
        matchResponse.Should().BeTrue();
        var createdGame = await _dbContext.Games
            .Include(g => g.Tickets)
            .SingleOrDefaultAsync(g => g.Id == game.Id);
        createdGame!.Tickets.Count.Should().Be(0);
        _helper.SetToken(null);
    }
}
