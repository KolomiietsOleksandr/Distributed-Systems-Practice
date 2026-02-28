namespace EvolutionaryArchitecture.Fitnet.Passes.RegisterPass;

using EvolutionaryArchitecture.Fitnet.Common.Events;
using EvolutionaryArchitecture.Fitnet.Contracts.SignContract.Events;
using EvolutionaryArchitecture.Fitnet.Passes.Application;

internal sealed class ContractSignedEventHandler(IPassesService passesService)
    : IIntegrationEventHandler<ContractSignedEvent>
{
    public Task Handle(ContractSignedEvent @event, CancellationToken ct) =>
        passesService.RegisterPassAsync(
            @event.ContractCustomerId,
            @event.SignedAt,
            @event.ExpireAt,
            @event.OccurredDateTime,
            ct);
}
