namespace EvolutionaryArchitecture.Fitnet.Passes.RegisterPass.Events;

using EvolutionaryArchitecture.Fitnet.Common.Events;

public record PassRegisteredEvent(Guid Id, Guid PassId, DateTimeOffset OccurredDateTime) : IIntegrationEvent
{
    public static PassRegisteredEvent Create(Guid passId, DateTimeOffset occurredAt) =>
        new(Guid.NewGuid(), passId, occurredAt);
}
