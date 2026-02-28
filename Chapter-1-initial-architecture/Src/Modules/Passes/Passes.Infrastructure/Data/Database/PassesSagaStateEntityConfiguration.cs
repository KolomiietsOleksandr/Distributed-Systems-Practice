namespace EvolutionaryArchitecture.Fitnet.Passes.Data.Database;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class PassesSagaStateEntityConfiguration : IEntityTypeConfiguration<PassesSagaState>
{
    public void Configure(EntityTypeBuilder<PassesSagaState> builder)
    {
        builder.ToTable("SagaStates");
        builder.HasKey(x => x.SagaId);

        builder.Property(x => x.SagaType)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.CorrelationId).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<int>();

        // Retry safety: only one saga per (Type, CorrelationId)
        builder.HasIndex(x => new { x.SagaType, x.CorrelationId }).IsUnique();
    }
}
