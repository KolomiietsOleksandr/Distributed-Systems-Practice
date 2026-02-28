namespace EvolutionaryArchitecture.Fitnet.Passes.Application;

public interface IPassesSagaStore
{
    Task StartAsync(string sagaType, Guid correlationId, DateTimeOffset occurredAt, CancellationToken ct);
    Task CompleteAsync(string sagaType, Guid correlationId, DateTimeOffset occurredAt, CancellationToken ct);
}
