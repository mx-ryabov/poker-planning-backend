using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using PokerPlanning.Domain.src.BaseModels;
using PokerPlanning.Domain.src.Models.EmailToNotifyAggregate;
using PokerPlanning.Domain.src.Models.GameAggregate;
using PokerPlanning.Domain.src.Models.GameAggregate.Entities;
using PokerPlanning.Domain.src.Models.TicketAggregate;
using PokerPlanning.Domain.src.Models.UserAggregate;
using PokerPlanning.Domain.src.Models.UserAggregate.Enums;
using PokerPlanning.Domain.src.Models.VotingSystemAggregate;
using PokerPlanning.Domain.src.Models.VotingSystemAggregate.Entities;
using PokerPlanning.Infrastructure.src.Authentication;

namespace PokerPlanning.Infrastructure.src.Persistence;

public class PokerPlanningDbContext : IdentityDbContext<ApplicationUser, ApplicationUserRole, Guid>
{
    public PokerPlanningDbContext()
    { }

    public PokerPlanningDbContext(DbContextOptions<PokerPlanningDbContext> options)
        : base(options)
    { }
    public DbSet<User> DomainUsers { get; set; }
    public DbSet<VotingSystem> VotingSystems { get; set; }
    public DbSet<VotingSystemVote> VotingSystemVotes { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Participant> Participants { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<VotingResult> VotingResults { get; set; }
    public DbSet<VotingResultVote> VotingResultVotes { get; set; }
    public DbSet<EmailToNotify> EmailsToNotify { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Ignore<IReadOnlyList<IDomainEvent>>()
            .ApplyConfigurationsFromAssembly(typeof(PokerPlanningDbContext).Assembly);

        SetSeedData(modelBuilder);
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entity.GetProperties())
            {
                if (property.Name == "Id" && property.ClrType == typeof(Guid))
                {
                    property.ValueGenerated = ValueGenerated.Never;
                }
            }
        }
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.EnableSensitiveDataLogging(true);
    }

    private static void SetSeedData(ModelBuilder modelBuilder)
    {
        SeedVotingSystems(modelBuilder);
        SeedUserRoles(modelBuilder);
    }

    private static void SeedVotingSystems(ModelBuilder modelBuilder)
    {
        var seedVotingSystems = new List<VotingSystem>();
        var seedVotingSystemVotes = new List<VotingSystemVote>();

        seedVotingSystems.Add(VotingSystem.Create("Fibonacci", new Guid("6a113d25-34c9-4b49-985c-2df6dd67650c")));
        seedVotingSystemVotes.Add(VotingSystemVote.Create("0", 0, "🏖️", seedVotingSystems[0].Id, new Guid("bf0d4051-84a7-4162-8f48-580d4e488df2")));
        seedVotingSystemVotes.Add(VotingSystemVote.Create("1", 1, "⚡", seedVotingSystems[0].Id, new Guid("5addfe74-4e6d-4dde-8c1b-01b856a57b2a")));
        seedVotingSystemVotes.Add(VotingSystemVote.Create("2", 2, "🚀", seedVotingSystems[0].Id, new Guid("f1f6b021-7f60-4b21-b297-6be2b93f0cb9")));
        seedVotingSystemVotes.Add(VotingSystemVote.Create("3", 3, "🤔", seedVotingSystems[0].Id, new Guid("a05dcc39-4781-4407-9255-94d8cc847657")));
        seedVotingSystemVotes.Add(VotingSystemVote.Create("5", 4, "😬", seedVotingSystems[0].Id, new Guid("9875f3f1-cbb0-40ee-b649-de48b706b7ba")));
        seedVotingSystemVotes.Add(VotingSystemVote.Create("8", 5, "😵", seedVotingSystems[0].Id, new Guid("8d23dd52-a81e-41b5-a9ac-2df12cd6d667")));
        seedVotingSystemVotes.Add(VotingSystemVote.Create("13", 6, "☠️", seedVotingSystems[0].Id, new Guid("07746553-c20a-4337-9fea-d37d9a473e78")));
        seedVotingSystemVotes.Add(VotingSystemVote.Create("?", 7, "🤡", seedVotingSystems[0].Id, new Guid("2c60634e-02ae-4b06-9894-c4f928d3037b")));

        seedVotingSystems.Add(VotingSystem.Create("T-shirts", new Guid("8eccd01f-53e5-4e6f-8553-64c81448a480")));
        seedVotingSystemVotes.Add(VotingSystemVote.Create("xss", 0, "🏖️", seedVotingSystems[1].Id, new Guid("66de38f8-1e99-4e39-9eef-ceeff15dd938")));
        seedVotingSystemVotes.Add(VotingSystemVote.Create("xs", 1, "⚡", seedVotingSystems[1].Id, new Guid("c5a7d8f5-432a-447a-81f6-9870e07e237a")));
        seedVotingSystemVotes.Add(VotingSystemVote.Create("s", 2, "🚀", seedVotingSystems[1].Id, new Guid("46d6301b-4fc9-4e78-a797-891592197821")));
        seedVotingSystemVotes.Add(VotingSystemVote.Create("m", 3, "🤔", seedVotingSystems[1].Id, new Guid("3c25140f-c8fb-409f-b54d-04835c143319")));
        seedVotingSystemVotes.Add(VotingSystemVote.Create("l", 4, "😬", seedVotingSystems[1].Id, new Guid("ee166192-1c6d-4e8a-81d2-b111a0ec0a6b")));
        seedVotingSystemVotes.Add(VotingSystemVote.Create("xl", 5, "😵", seedVotingSystems[1].Id, new Guid("46c84b9b-a443-4ac3-8550-02d63c57c6d7")));
        seedVotingSystemVotes.Add(VotingSystemVote.Create("xxl", 6, "☠️", seedVotingSystems[1].Id, new Guid("0bc7aa91-6a87-4386-994d-105fb13b6d98")));
        seedVotingSystemVotes.Add(VotingSystemVote.Create("?", 7, "🤡", seedVotingSystems[1].Id, new Guid("28e4cc55-2acf-4138-b781-8ae4da22582a")));


        modelBuilder.Entity<VotingSystem>().HasData(seedVotingSystems);
        modelBuilder.Entity<VotingSystemVote>().HasData(seedVotingSystemVotes);
    }

    private static void SeedUserRoles(ModelBuilder modelBuilder)
    {
        var roles = new List<ApplicationUserRole>
        {
            new(nameof(UserRole.Guest)) { Id = ApplicationUserRole.GetRoleId(UserRole.Guest) },
            new(nameof(UserRole.Member)) { Id = ApplicationUserRole.GetRoleId(UserRole.Member) },
            new(nameof(UserRole.Admin)) { Id = ApplicationUserRole.GetRoleId(UserRole.Admin) }
        };

        modelBuilder.Entity<ApplicationUserRole>().HasData(roles);
    }


}
