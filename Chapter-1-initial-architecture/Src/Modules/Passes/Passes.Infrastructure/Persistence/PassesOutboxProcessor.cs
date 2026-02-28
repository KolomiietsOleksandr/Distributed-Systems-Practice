namespace EvolutionaryArchitecture.Fitnet.Passes.Infrastructure.Persistence;

using System.Text.Json;
using EvolutionaryArchitecture.Fitnet.Common.Events.EventBus;
using EvolutionaryArchitecture.Fitnet.Passes.Application;
using EvolutionaryArchitecture.Fitnet.Passes.Data.Database;
using EvolutionaryArchitecture.Fitnet.Passes.MarkPassAsExpired.Events;
using EvolutionaryArchitecture.Fitnet.Passes.RegisterPass.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

internal sealed class PassesOutboxProcessor(
    PassesPersistence db,
    IEventBus eventBus,
    IPassesSagaStore sagaStore,
    ILogger<PassesOutboxProcessor> logger) : IPassesOutboxProcessor
{
    private const int BatchSize = 20;

    public async Task ProcessAsync(CancellationToken ct)
    {
        var pendingMessages = await db.OutboxMessages
            .Where(x => x.ProcessedAt == null)
            .OrderBy(x => x.CreatedAt)
            .Take(BatchSize)
            .ToListAsync(ct);

        if (pendingMessages.Count == 0)
        {
            return;
        }

        foreach (var message in pendingMessages)
        {
            try
            {
                await HandleAsync(message, ct);
                message.MarkProcessed(DateTimeOffset.UtcNow);
            }
            catch (Exception ex)
            {
                // In a real system we'd add retry counters / dead-lettering.
                logger.LogError(ex, "Failed to process outbox message {MessageId} ({MessageType})", message.Id, message.Type);
            }
        }

        await db.SaveChangesAsync(ct);
    }

    private async Task HandleAsync(OutboxMessage message, CancellationToken ct)
    {
        switch (message.Type)
        {
            case OutboxMessageTypes.PassRegistered:
            {
                var @event = JsonSerializer.Deserialize<PassRegisteredEvent>(message.Payload)
                            ?? throw new InvalidOperationException("Invalid outbox payload for PassRegistered");

                await eventBus.PublishAsync(@event, ct);
                await sagaStore.CompleteAsync(PassesSagaTypes.RegisterPass, message.CorrelationId, DateTimeOffset.UtcNow, ct);
                break;
            }
            case OutboxMessageTypes.PassExpired:
            {
                var @event = JsonSerializer.Deserialize<PassExpiredEvent>(message.Payload)
                            ?? throw new InvalidOperationException("Invalid outbox payload for PassExpired");

                await eventBus.PublishAsync(@event, ct);
                await sagaStore.CompleteAsync(PassesSagaTypes.ExpirePass, message.CorrelationId, DateTimeOffset.UtcNow, ct);
                break;
            }
            default:
                logger.LogWarning("Unknown outbox message type: {MessageType}", message.Type);
                break;
        }
    }
}
