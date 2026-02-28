namespace EvolutionaryArchitecture.Fitnet.Passes.Infrastructure;

using EvolutionaryArchitecture.Fitnet.Passes.Application;
using EvolutionaryArchitecture.Fitnet.Passes.Data.Database;
using EvolutionaryArchitecture.Fitnet.Passes.Infrastructure.Persistence;

public static class PassesInfrastructureModule
{
    public static IServiceCollection AddPassesInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);

        services.AddScoped<IPassesRepository, EfPassesRepository>();
        services.AddScoped<IPassesEventPublisher, EventBusPassesEventPublisher>();
        services.AddScoped<IPassesService, PassesService>();

        return services;
    }

    public static WebApplication UsePassesInfrastructure(this WebApplication app)
    {
        app.UseDatabase();

        return app;
    }
}
