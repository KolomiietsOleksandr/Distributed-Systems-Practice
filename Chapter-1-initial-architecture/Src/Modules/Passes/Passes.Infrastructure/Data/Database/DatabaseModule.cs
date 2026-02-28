namespace EvolutionaryArchitecture.Fitnet.Passes.Data.Database;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

public static class DatabaseModule
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var dbOptionsSection = configuration.GetSection(PassesPersistenceOptions.SectionName);

        services.AddOptions<PassesPersistenceOptions>()
            .Bind(dbOptionsSection)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddDbContext<PassesPersistence>((provider, options) =>
        {
            var dbOptions = provider.GetRequiredService<IOptions<PassesPersistenceOptions>>().Value;
            options.UseNpgsql(dbOptions.Passes);
        });

        return services;
    }

    public static WebApplication UseDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<PassesPersistence>();
        db.Database.Migrate();

        return app;
    }
}
