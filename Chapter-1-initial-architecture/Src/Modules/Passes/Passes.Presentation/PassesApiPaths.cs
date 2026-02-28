namespace EvolutionaryArchitecture.Fitnet.Passes;

internal static class PassesApiPaths
{
    // Avoid dependency on Fitnet.ApiPaths (host project).
    private const string Root = "api";

    internal const string Passes = $"{Root}/passes";

    internal const string GetAll = Passes;
    internal const string MarkPassAsExpired = $"{Passes}/{{id:guid}}";
}
