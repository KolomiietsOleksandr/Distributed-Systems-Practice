namespace EvolutionaryArchitecture.Fitnet.Passes.Infrastructure.Persistence;

using EvolutionaryArchitecture.Fitnet.Passes.Application;
using EvolutionaryArchitecture.Fitnet.Passes.Data;
using EvolutionaryArchitecture.Fitnet.Passes.Data.Database;
using Microsoft.EntityFrameworkCore;

internal sealed class EfPassesRepository(PassesPersistence db) : IPassesRepository
{
    public async Task AddAsync(Pass pass, CancellationToken ct)
    {
        await db.Passes.AddAsync(pass, ct);
    }

    public Task<Pass?> FindAsync(Guid passId, CancellationToken ct) =>
        db.Passes.FirstOrDefaultAsync(pass => pass.Id == passId, ct);

    public async Task<IReadOnlyList<Pass>> GetAllAsync(CancellationToken ct) =>
        await db.Passes.AsNoTracking().ToListAsync(ct);

    public Task SaveChangesAsync(CancellationToken ct) => db.SaveChangesAsync(ct);
}
