namespace EvolutionaryArchitecture.Fitnet.Common.Events.EventBus.InMemory;

using System.Reflection;

public static class InMemoryEventBusModule
{
    public static IServiceCollection AddInMemoryEventBus(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddScoped<IEventBus, InMemoryEventBus>();
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(assemblies));

        return services;
    }
}
