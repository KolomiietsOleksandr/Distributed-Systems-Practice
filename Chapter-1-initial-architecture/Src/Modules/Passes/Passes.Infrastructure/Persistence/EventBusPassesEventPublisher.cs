namespace EvolutionaryArchitecture.Fitnet.Passes.Infrastructure.Persistence;

using EvolutionaryArchitecture.Fitnet.Common.Events.EventBus;
using EvolutionaryArchitecture.Fitnet.Passes.Application;
using EvolutionaryArchitecture.Fitnet.Passes.MarkPassAsExpired.Events;
using EvolutionaryArchitecture.Fitnet.Passes.RegisterPass.Events;

internal sealed class EventBusPassesEventPublisher(IEventBus eventBus) : IPassesEventPublisher
{
    public Task PassRegisteredAsync(Guid passId, DateTimeOffset occurredAt, CancellationToken ct) =>
        eventBus.PublishAsync(PassRegisteredEvent.Create(passId, occurredAt), ct);

    public Task PassExpiredAsync(Guid passId, Guid customerId, DateTimeOffset occurredAt, CancellationToken ct) =>
        eventBus.PublishAsync(PassExpiredEvent.Create(passId, customerId, occurredAt), ct);
}
