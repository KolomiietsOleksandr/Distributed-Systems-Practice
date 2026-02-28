namespace EvolutionaryArchitecture.Fitnet.Common.Events.EventBus;

using System.Reflection;
using InMemory;

public static class EventBusModule
{
    /// <summary>
    /// Registers a very simple in-memory integration event bus (based on MediatR).
    /// 
    /// If no assemblies are provided, it will register handlers from all currently loaded assemblies.
    /// </summary>
    public static IServiceCollection AddEventBus(this IServiceCollection services, params Assembly[] assemblies)
    {
        var targetAssemblies = assemblies.Length > 0
            ? assemblies
            : AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(assembly => !assembly.IsDynamic)
                .ToArray();

        return services.AddInMemoryEventBus(targetAssemblies);
    }
}
