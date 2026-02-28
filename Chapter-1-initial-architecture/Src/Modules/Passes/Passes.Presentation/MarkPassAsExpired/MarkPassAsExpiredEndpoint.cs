namespace EvolutionaryArchitecture.Fitnet.Passes.MarkPassAsExpired;

using EvolutionaryArchitecture.Fitnet.Passes.Application;

internal static class MarkPassAsExpiredEndpoint
{
    internal static void MapMarkPassAsExpired(this IEndpointRouteBuilder app) => app.MapPatch(
            PassesApiPaths.MarkPassAsExpired,
            async (
                Guid id,
                IPassesService passesService,
                TimeProvider timeProvider,
                CancellationToken cancellationToken) =>
            {
                var updated = await passesService.MarkAsExpiredAsync(id, timeProvider.GetUtcNow(), cancellationToken);
                return updated ? Results.NoContent() : Results.NotFound();
            })
        .WithSummary("Marks pass which expired")
        .WithDescription(
            "This endpoint is used to mark expired pass. Based on that it is possible to offer new contract to customer.")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);
}
