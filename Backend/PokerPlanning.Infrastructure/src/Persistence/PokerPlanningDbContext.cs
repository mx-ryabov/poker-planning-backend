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
        base.OnModelCreating(modelBuilder);
    }
}
