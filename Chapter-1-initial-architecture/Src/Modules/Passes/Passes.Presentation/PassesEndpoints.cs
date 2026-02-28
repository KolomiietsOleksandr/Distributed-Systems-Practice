namespace EvolutionaryArchitecture.Fitnet.Passes;

using EvolutionaryArchitecture.Fitnet.Passes.GetAllPasses;
using MarkPassAsExpired;

internal static class PassesEndpoints
{
    internal static IEndpointRouteBuilder MapPassesEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGetAllPasses();
        app.MapMarkPassAsExpired();

        return app;
    }
}
