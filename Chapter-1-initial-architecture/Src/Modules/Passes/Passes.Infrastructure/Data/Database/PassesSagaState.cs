namespace EvolutionaryArchitecture.Fitnet.Passes.Data.Database;

using EvolutionaryArchitecture.Fitnet.Passes.Application;

internal sealed class PassesSagaState
{
    public Guid SagaId { get; init; }
    public string SagaType { get; init; } = string.Empty;
    public Guid CorrelationId { get; init; }

    public PassesSagaStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public static PassesSagaState Started(string sagaType, Guid correlationId, DateTimeOffset now) =>
        new()
        {
            SagaId = Guid.NewGuid(),
            SagaType = sagaType,
            CorrelationId = correlationId,
            Status = PassesSagaStatus.Started,
            CreatedAt = now,
            UpdatedAt = now
        };

    public void MarkCompleted(DateTimeOffset now)
    {
        if (Status == PassesSagaStatus.Completed)
        {
            return;
        }

        Status = PassesSagaStatus.Completed;
        UpdatedAt = now;
    }
}
