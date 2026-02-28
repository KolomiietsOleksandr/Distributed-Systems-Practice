namespace EvolutionaryArchitecture.Fitnet.ArchitectureTests.Common;

using System.Reflection;
using NetArchTest.Rules;

internal static class Solution
{
    private static readonly Assembly[] Assemblies =
    [
        typeof(Program).Assembly,
        typeof(EvolutionaryArchitecture.Fitnet.Passes.PassesModule).Assembly,
        typeof(EvolutionaryArchitecture.Fitnet.Common.Events.IIntegrationEvent).Assembly,
        typeof(EvolutionaryArchitecture.Fitnet.Contracts.SignContract.Events.ContractSignedEvent).Assembly
    ];

    internal static Types Types => Types.InAssemblies(Assemblies);
}
