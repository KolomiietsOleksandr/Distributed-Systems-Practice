namespace EvolutionaryArchitecture.Fitnet.Passes.Data.Database;

internal sealed class OutboxMessage
{
    public Guid Id { get; init; }
    public string Type { get; init; } = string.Empty;
    public string Payload { get; init; } = string.Empty;
    public Guid CorrelationId { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? ProcessedAt { get; private set; }

    public static OutboxMessage Create(string type, string payload, Guid correlationId, DateTimeOffset createdAt) =>
        new()
        {
            Id = Guid.NewGuid(),
            Type = type,
            Payload = payload,
            CorrelationId = correlationId,
            CreatedAt = createdAt
        };

    public void MarkProcessed(DateTimeOffset processedAt) => ProcessedAt = processedAt;
}
