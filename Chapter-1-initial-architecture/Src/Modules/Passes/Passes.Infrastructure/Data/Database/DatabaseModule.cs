namespace EvolutionaryArchitecture.Fitnet.Passes.Data.Database;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

public static class DatabaseModule
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var dbOptionsSection = configuration.GetSection("Database:Passes");

        services.AddOptions<PassesPersistenceOptions>()
            .Bind(dbOptionsSection)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddDbContext<PassesPersistence>((provider, options) =>
        {
            var dbOptions = provider.GetRequiredService<IOptions<PassesPersistenceOptions>>().Value;
            options.UseNpgsql(dbOptions.ConnectionString);
        });

        return services;
    }

    public static WebApplication UseDatabase(this WebApplication app)
    {
        app.UseAutomaticMigrations<PassesPersistence>();

        return app;
    }
}
