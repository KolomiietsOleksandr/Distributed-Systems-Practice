namespace EvolutionaryArchitecture.Fitnet.Passes.Application;

using EvolutionaryArchitecture.Fitnet.Passes.Data;
using EvolutionaryArchitecture.Fitnet.Passes.GetAllPasses;

public sealed class PassesService(
    IPassesRepository repository,
    IPassesEventPublisher eventPublisher,
    IPassesSagaStore sagaStore) : IPassesService
{
    public async Task<GetAllPassesResponse> GetAllAsync(CancellationToken ct)
    {
        var passes = await repository.GetAllAsync(ct);

        return new GetAllPassesResponse(
            passes.Select(pass => PassDto.From(pass))
                .ToArray());
    }

    public async Task<bool> MarkAsExpiredAsync(Guid passId, DateTimeOffset occurredAt, CancellationToken ct)
    {
        var pass = await repository.FindAsync(passId, ct);
        if (pass is null)
        {
            return false;
        }

        pass.MarkAsExpired(occurredAt);
        // Start a simple saga and enqueue an outbox message in the same EF transaction.
        await sagaStore.StartAsync(PassesSagaTypes.ExpirePass, pass.Id, occurredAt, ct);
        await eventPublisher.PassExpiredAsync(pass.Id, pass.CustomerId, occurredAt, ct);
        await repository.SaveChangesAsync(ct);

        return true;
    }

    public async Task<Guid> RegisterPassAsync(Guid customerId, DateTimeOffset from, DateTimeOffset to, DateTimeOffset occurredAt, CancellationToken ct)
    {
        var pass = Pass.Register(customerId, from, to);

        await repository.AddAsync(pass, ct);

        // Start a simple saga and enqueue an outbox message in the same EF transaction.
        await sagaStore.StartAsync(PassesSagaTypes.RegisterPass, pass.Id, occurredAt, ct);
        await eventPublisher.PassRegisteredAsync(pass.Id, occurredAt, ct);
        await repository.SaveChangesAsync(ct);

        return pass.Id;
    }
}
