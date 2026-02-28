namespace EvolutionaryArchitecture.Fitnet.Passes.Infrastructure.Persistence;

using System.Text.Json;
using EvolutionaryArchitecture.Fitnet.Passes.Application;
using EvolutionaryArchitecture.Fitnet.Passes.MarkPassAsExpired.Events;
using EvolutionaryArchitecture.Fitnet.Passes.RegisterPass.Events;

/// <summary>
/// Instead of publishing events directly, we store them in the Outbox.
/// Later, the Outbox Processor will publish them reliably.
/// </summary>
internal sealed class OutboxPassesEventPublisher(IPassesOutboxWriter outboxWriter) : IPassesEventPublisher
{
    public Task PassRegisteredAsync(Guid passId, DateTimeOffset occurredAt, CancellationToken ct)
    {
        var @event = PassRegisteredEvent.Create(passId, occurredAt);
        var payload = JsonSerializer.Serialize(@event);

        return outboxWriter.EnqueueAsync(
            type: OutboxMessageTypes.PassRegistered,
            payload: payload,
            correlationId: passId,
            createdAt: occurredAt,
            ct: ct);
    }

    public Task PassExpiredAsync(Guid passId, Guid customerId, DateTimeOffset occurredAt, CancellationToken ct)
    {
        var @event = PassExpiredEvent.Create(passId, customerId, occurredAt);
        var payload = JsonSerializer.Serialize(@event);

        return outboxWriter.EnqueueAsync(
            type: OutboxMessageTypes.PassExpired,
            payload: payload,
            correlationId: passId,
            createdAt: occurredAt,
            ct: ct);
    }
}
