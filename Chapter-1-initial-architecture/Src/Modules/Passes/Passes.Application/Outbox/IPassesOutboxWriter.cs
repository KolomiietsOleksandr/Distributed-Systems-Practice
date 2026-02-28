namespace EvolutionaryArchitecture.Fitnet.Passes.Application;

public interface IPassesOutboxWriter
{
    Task EnqueueAsync(
        string type,
        string payload,
        Guid correlationId,
        DateTimeOffset createdAt,
        CancellationToken ct);
}
