namespace EvolutionaryArchitecture.Fitnet.Passes.Application;

using EvolutionaryArchitecture.Fitnet.Passes.Data;

public interface IPassesRepository
{
    Task AddAsync(Pass pass, CancellationToken ct);
    Task<Pass?> FindAsync(Guid passId, CancellationToken ct);
    Task<IReadOnlyList<Pass>> GetAllAsync(CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
