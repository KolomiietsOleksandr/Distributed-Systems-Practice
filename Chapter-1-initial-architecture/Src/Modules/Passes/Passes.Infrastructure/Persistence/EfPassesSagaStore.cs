namespace EvolutionaryArchitecture.Fitnet.Passes.Infrastructure.Persistence;

using EvolutionaryArchitecture.Fitnet.Passes.Application;
using EvolutionaryArchitecture.Fitnet.Passes.Data.Database;
using Microsoft.EntityFrameworkCore;

internal sealed class EfPassesSagaStore(PassesPersistence db) : IPassesSagaStore
{
    public async Task StartAsync(string sagaType, Guid correlationId, DateTimeOffset occurredAt, CancellationToken ct)
    {
        // Retry safe: only one saga per (Type, CorrelationId).
        var exists = await db.Sagas
            .AsNoTracking()
            .AnyAsync(x => x.SagaType == sagaType && x.CorrelationId == correlationId, ct);
        if (exists)
        {
            return;
        }

        await db.Sagas.AddAsync(PassesSagaState.Started(sagaType, correlationId, occurredAt), ct);
    }

    public async Task CompleteAsync(string sagaType, Guid correlationId, DateTimeOffset occurredAt, CancellationToken ct)
    {
        var saga = await db.Sagas
            .FirstOrDefaultAsync(x => x.SagaType == sagaType && x.CorrelationId == correlationId, ct);
        if (saga is null)
        {
            return;
        }

        // Idempotency: if already completed, do nothing.
        saga.MarkCompleted(occurredAt);
    }
}
