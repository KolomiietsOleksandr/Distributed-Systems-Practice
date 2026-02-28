namespace EvolutionaryArchitecture.Fitnet.Passes.GetAllPasses;

using EvolutionaryArchitecture.Fitnet.Passes.Application;

internal static class GetAllPassesEndpoint
{
    internal static void MapGetAllPasses(this IEndpointRouteBuilder app) => app.MapGet(
            PassesApiPaths.GetAll,
            async (IPassesService passesService, CancellationToken cancellationToken) =>
            {
                var response = await passesService.GetAllAsync(cancellationToken);
                return Results.Ok(response);
            })
        .WithSummary("Returns all passes")
        .WithDescription("This endpoint is used to get all passes for a given customer")
        .Produces<GetAllPassesResponse>()
        .Produces(StatusCodes.Status500InternalServerError);
}
