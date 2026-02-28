namespace EvolutionaryArchitecture.Fitnet.Passes.Application;

public interface IPassesOutboxProcessor
{
    Task ProcessAsync(CancellationToken ct);
}
