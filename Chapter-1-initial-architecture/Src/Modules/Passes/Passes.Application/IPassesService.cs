namespace EvolutionaryArchitecture.Fitnet.Passes.Application;

public interface IPassesService
{
    Task<GetAllPasses.GetAllPassesResponse> GetAllAsync(CancellationToken ct);

    /// <returns>true if the pass existed and was updated</returns>
    Task<bool> MarkAsExpiredAsync(Guid passId, DateTimeOffset occurredAt, CancellationToken ct);

    /// <returns>Id of created pass</returns>
    Task<Guid> RegisterPassAsync(Guid customerId, DateTimeOffset from, DateTimeOffset to, DateTimeOffset occurredAt, CancellationToken ct);
}
