namespace EvolutionaryArchitecture.Fitnet.Passes.Infrastructure.Persistence;

using EvolutionaryArchitecture.Fitnet.Passes.Application;
using EvolutionaryArchitecture.Fitnet.Passes.Data.Database;
using Microsoft.EntityFrameworkCore;

internal sealed class EfPassesOutboxWriter(PassesPersistence db) : IPassesOutboxWriter
{
    public async Task EnqueueAsync(
        string type,
        string payload,
        Guid correlationId,
        DateTimeOffset createdAt,
        CancellationToken ct)
    {
        // Idempotency: do not enqueue duplicates for the same (type, correlationId).
        var alreadyEnqueued = await db.OutboxMessages
            .AsNoTracking()
            .AnyAsync(x => x.Type == type && x.CorrelationId == correlationId, ct);
        if (alreadyEnqueued)
        {
            return;
        }

        await db.OutboxMessages.AddAsync(OutboxMessage.Create(type, payload, correlationId, createdAt), ct);
    }
}
