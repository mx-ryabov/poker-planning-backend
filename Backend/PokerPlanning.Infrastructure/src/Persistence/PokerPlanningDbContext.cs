using Microsoft.EntityFrameworkCore;
using PokerPlanning.Domain.src.Models.GameAggregate;
using PokerPlanning.Domain.src.Models.GameAggregate.Entities;
using PokerPlanning.Domain.src.Models.UserAggregate;
using PokerPlanning.Domain.src.Models.VotingSystemAggregate;
using PokerPlanning.Domain.src.Models.VotingSystemAggregate.Entities;

namespace PokerPlanning.Infrastructure.src.Persistence;

public class PokerPlanningDbContext : DbContext
{
    public PokerPlanningDbContext()
    { }

    public PokerPlanningDbContext(DbContextOptions<PokerPlanningDbContext> options)
        : base(options)
    { }

    public DbSet<User> Users { get; set; }
    public DbSet<VotingSystem> VotingSystems { get; set; }
    public DbSet<VotingSystemVote> VotingSystemVotes { get; set; }
    /*public DbSet<GameAggregate> Games { get; set; }
    public DbSet<Participant> Participants { get; set; }
    public DbSet<VotingResult> VotingResults { get; set; }
    public DbSet<VotingResultVote> VotingResultVotes { get; set; }*/

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PokerPlanningDbContext).Assembly);
        SetSeedData(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.EnableSensitiveDataLogging(true);
    }

    private static void SetSeedData(ModelBuilder modelBuilder)
    {
        var seedVotingSystems = new List<VotingSystem>();
        var seedVotingSystemVotes = new List<VotingSystemVote>();

        seedVotingSystems.Add(VotingSystem.Create("Fibonacci"));
        seedVotingSystemVotes.Add(VotingSystemVote.Create("0", 0, "🏖️", seedVotingSystems[0].Id));
        seedVotingSystemVotes.Add(VotingSystemVote.Create("1", 1, "⚡", seedVotingSystems[0].Id));
        seedVotingSystemVotes.Add(VotingSystemVote.Create("2", 2, "🚀", seedVotingSystems[0].Id));
        seedVotingSystemVotes.Add(VotingSystemVote.Create("3", 3, "🤔", seedVotingSystems[0].Id));
        seedVotingSystemVotes.Add(VotingSystemVote.Create("5", 4, "😬", seedVotingSystems[0].Id));
        seedVotingSystemVotes.Add(VotingSystemVote.Create("8", 5, "😵", seedVotingSystems[0].Id));
        seedVotingSystemVotes.Add(VotingSystemVote.Create("13", 6, "☠️", seedVotingSystems[0].Id));
        seedVotingSystemVotes.Add(VotingSystemVote.Create("?", 7, "🤡", seedVotingSystems[0].Id));

        seedVotingSystems.Add(VotingSystem.Create("T-shirts"));
        seedVotingSystemVotes.Add(VotingSystemVote.Create("xss", 0, "🏖️", seedVotingSystems[1].Id));
        seedVotingSystemVotes.Add(VotingSystemVote.Create("xs", 1, "⚡", seedVotingSystems[1].Id));
        seedVotingSystemVotes.Add(VotingSystemVote.Create("s", 2, "🚀", seedVotingSystems[1].Id));
        seedVotingSystemVotes.Add(VotingSystemVote.Create("m", 3, "🤔", seedVotingSystems[1].Id));
        seedVotingSystemVotes.Add(VotingSystemVote.Create("l", 4, "😬", seedVotingSystems[1].Id));
        seedVotingSystemVotes.Add(VotingSystemVote.Create("xl", 5, "😵", seedVotingSystems[1].Id));
        seedVotingSystemVotes.Add(VotingSystemVote.Create("xxl", 6, "☠️", seedVotingSystems[1].Id));
        seedVotingSystemVotes.Add(VotingSystemVote.Create("?", 7, "🤡", seedVotingSystems[1].Id));


        modelBuilder.Entity<VotingSystem>().HasData(seedVotingSystems);
        modelBuilder.Entity<VotingSystemVote>().HasData(seedVotingSystemVotes);
    }
}
