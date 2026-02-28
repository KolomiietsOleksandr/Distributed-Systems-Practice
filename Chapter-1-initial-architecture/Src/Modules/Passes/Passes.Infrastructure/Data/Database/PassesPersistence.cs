namespace EvolutionaryArchitecture.Fitnet.Passes.Data.Database;

using Microsoft.EntityFrameworkCore;

internal sealed class PassesPersistence(DbContextOptions<PassesPersistence> options) : DbContext(options)
{
    private const string Schema = "Passes";

    public DbSet<Pass> Passes => Set<Pass>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    public DbSet<PassesSagaState> Sagas => Set<PassesSagaState>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);
        modelBuilder.ApplyConfiguration(new PassEntityConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageEntityConfiguration());
        modelBuilder.ApplyConfiguration(new PassesSagaStateEntityConfiguration());
    }
}
