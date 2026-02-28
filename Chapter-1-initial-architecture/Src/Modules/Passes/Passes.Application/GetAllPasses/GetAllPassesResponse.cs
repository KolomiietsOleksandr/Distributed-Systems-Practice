namespace EvolutionaryArchitecture.Fitnet.Passes.GetAllPasses;

using EvolutionaryArchitecture.Fitnet.Passes.Data;

public record GetAllPassesResponse(IReadOnlyCollection<PassDto> Passes)
{
    public static GetAllPassesResponse Create(IReadOnlyCollection<PassDto> passes) => new(passes);
}

public record PassDto(Guid Id, Guid CustomerId)
{
    public static PassDto From(Pass pass) => new(pass.Id, pass.CustomerId);
}
