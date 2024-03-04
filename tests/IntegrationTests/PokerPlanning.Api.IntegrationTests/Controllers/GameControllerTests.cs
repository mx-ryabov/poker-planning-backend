using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PokerPlanning.Application.src.GameFeature.Results;
using PokerPlanning.Application.src.Results;
using PokerPlanning.Contracts.src.Game;
using PokerPlanning.Domain.src.Models.GameAggregate.Enums;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace PokerPlanning.Api.IntegrationTests.Controllers;

public class GameControllerTests : BaseIntegrationTest
{
    public GameControllerTests(PokerPlanningWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task CreateGame_WhenRequestValid_CreatesAndReturnGameAndToken()
    {
        var requestData = await CreateGameRequest();

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
        var votingSystem = await GetVotingSystem();
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
        var game = await CreateGame();
        var randomParticipantName = GameHelper.GetParticipantName();
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
        var game = await CreateGame();
        SetToken(game!.MasterToken);

        var response = await _client.GetAsync($"/api/games/{game!.Id}");

        response.EnsureSuccessStatusCode();
        var matchResponse = await response.Content.ReadFromJsonAsync<GetGameResult>();
        matchResponse.Should().NotBeNull();
        matchResponse!.Id.Should().Be(game!.Id);
        _client.DefaultRequestHeaders.Authorization = null;
        SetToken(null);
    }

    [Fact]
    public async Task GetGameById_WhenUserIsUnauthorizedForGame_ThrowsUnauthorizedException()
    {
        var game = await CreateGame();
        var response = await _client.GetAsync($"/api/games/{game!.Id}");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetCurrentParticipantByGameId_WhenUserIsAuthorizedForGame_ReturnsParticipantInfo()
    {
        var game = await CreateGame();
        SetToken(game!.MasterToken);

        var response = await _client.GetAsync($"/api/games/{game!.Id}/current-participant");
        response.EnsureSuccessStatusCode();
        var matchResponse = await response.Content.ReadFromJsonAsync<GameParticipantResult>();
        matchResponse.Should().NotBeNull();
        var createdGame = await _dbContext.Games
                .Include(g => g.Participants
                    .Where(p => p.Id == matchResponse!.Id && p.DisplayName == matchResponse!.DisplayName))
                .SingleAsync(g => g.Id == game!.Id);
        createdGame.Should().NotBeNull();
        SetToken(null);
    }

    [Fact]
    public async Task StartVoting_ValidRequest_ReturnsSuccessCodeAndStartVoting()
    {
        var game = await CreateGame();
        SetToken(game!.MasterToken);
        // Add creating and adding a ticket when it's ready
        Guid? ticketId = null;
        var request = new StartVotingRequest(ticketId);

        var response = await _client.PutAsJsonAsync($"/api/games/{game!.Id}/start-voting", request);
        
        response.EnsureSuccessStatusCode();
        var createdGame = await _dbContext.Games
                .SingleAsync(g => g.Id == game!.Id);
        createdGame.Should().NotBeNull();
        createdGame.VotingProcess.IsActive.Should().BeTrue();
        createdGame.VotingProcess.TicketId.Should().Be(ticketId);
        SetToken(null);
    }

    [Fact]
    public async Task FinishVoting_ValidRequest_ReturnsSuccessCodeAndFinishVoting()
    {
        var game = await CreateGame();
        SetToken(game!.MasterToken);
        Guid? ticketId = null;
        await StartVoting(game!.Id, ticketId);
        
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
        SetToken(null);
    }

    [Fact]
    public async Task Vote_ValidRequest_ReturnsSuccessCodeAndChangeVoteForCurrentParticipant()
    {
        // Arrange
        var game = await CreateGame();
        SetToken(game!.MasterToken);

        Guid? ticketId = null;
        await StartVoting(game!.Id, ticketId);

        SetToken(null);
        var joinAsGuestResult = await JoinAsParticipant(game!.Id);

        SetToken(joinAsGuestResult!.Token);
        var participantResult = await GetCurrentParticipant(game!.Id);
        var votingSystem = await GetVotingSystem();
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
        SetToken(null);
    }

    private void SetToken(string? token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    private async Task StartVoting(Guid gameId, Guid? ticketId)
    {
        var startVotingReq = new StartVotingRequest(ticketId);
        await _client.PutAsJsonAsync($"/api/games/{gameId}/start-voting", startVotingReq);
    }

    private async Task<GameParticipantResult?> GetCurrentParticipant(Guid gameId)
    {
        var currentParticipantResponse = await _client.GetAsync($"/api/games/{gameId}/current-participant");
        return await currentParticipantResponse.Content.ReadFromJsonAsync<GameParticipantResult>();
    }

    private async Task<JoinAsGuestGameResult?> JoinAsParticipant(Guid gameId)
    {
        var randomParticipantName = GameHelper.GetParticipantName();
        JoinAsGuestRequest joinAsGuestReq = new(randomParticipantName);
        var joinParticipantResponse = await _client.PostAsJsonAsync($"/api/games/{gameId}/join-as-guest", joinAsGuestReq);
        return await joinParticipantResponse.Content.ReadFromJsonAsync<JoinAsGuestGameResult>();
    }

    private async Task<CreateGameResponse?> CreateGame()
    {
        var requestData = await CreateGameRequest();
        var response = await _client.PostAsJsonAsync("/api/games", requestData);
        return await response.Content.ReadFromJsonAsync<CreateGameResponse>();
    }

    private async Task<CreateGameRequest> CreateGameRequest()
    {
        var votingSystem = await GetVotingSystem();
        votingSystem.Should().NotBeNull();
        return new CreateGameRequest(
                Name: GameHelper.GetName(),
                VotingSystemId: votingSystem!.Id,
                CreatorName: GameHelper.GetCreatorName(),
                IsAutoRevealCards: false
            );
    }

    private async Task<VotingSystemResult?> GetVotingSystem()
    {
        var response = await _client.GetAsync("/api/voting-systems");
        var matchResponse = await response.Content.ReadFromJsonAsync<List<VotingSystemResult>>();
        
        return matchResponse?.ToList()[0];
    }
}

public static class GameHelper
{
    public static string GetName() => $"Game's Name {RandomString(5)}";
    public static string GetCreatorName() => $"Creator Name {RandomString(5)}";
    public static string GetParticipantName() => $"Participant Name {RandomString(5)}";

    private static Random random = new Random();
    private static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
