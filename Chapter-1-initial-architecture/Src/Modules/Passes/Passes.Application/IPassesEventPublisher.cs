namespace EvolutionaryArchitecture.Fitnet.Passes.Application;

public interface IPassesEventPublisher
{
    Task PassRegisteredAsync(Guid passId, DateTimeOffset occurredAt, CancellationToken ct);
    Task PassExpiredAsync(Guid passId, Guid customerId, DateTimeOffset occurredAt, CancellationToken ct);
}
