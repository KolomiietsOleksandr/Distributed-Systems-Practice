namespace EvolutionaryArchitecture.Fitnet.Passes;

using EvolutionaryArchitecture.Fitnet.Passes.Infrastructure;

public static class PassesModule
{
    public static IServiceCollection AddPasses(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPassesInfrastructure(configuration);

        return services;
    }

    public static WebApplication UsePasses(this WebApplication app)
    {
        app.UsePassesInfrastructure();

        return app;
    }

    public static IEndpointRouteBuilder MapPasses(this IEndpointRouteBuilder app) =>
        app.MapPassesEndpoints();
}
