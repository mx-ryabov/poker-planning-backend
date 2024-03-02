using FluentAssertions;
using PokerPlanning.Application.src.Results;
using System.Net.Http.Json;

namespace PokerPlanning.Api.IntegrationTests.Controllers;

public class VotingSystemControllerTests : BaseIntegrationTest
{
    public VotingSystemControllerTests(PokerPlanningWebApplicationFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task GetVotingSystems_WhenRequest_ReturnsVotingSystems()
    {
        var response = await _client.GetAsync("/api/voting-systems");

        response.EnsureSuccessStatusCode();
        var matchResponse = await response.Content.ReadFromJsonAsync<IEnumerable<VotingSystemResult>>();
        matchResponse.Should().NotBeNull();
        matchResponse.Should().BeOfType<List<VotingSystemResult>>();
        matchResponse?.Count().Should().Be(2);
    }
}
