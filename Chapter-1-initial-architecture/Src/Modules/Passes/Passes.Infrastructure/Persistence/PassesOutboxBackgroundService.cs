namespace EvolutionaryArchitecture.Fitnet.Passes.Infrastructure.Persistence;

using EvolutionaryArchitecture.Fitnet.Passes.Application;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

internal sealed class PassesOutboxBackgroundService(
    IServiceScopeFactory scopeFactory,
    ILogger<PassesOutboxBackgroundService> logger) : BackgroundService
{
    private static readonly TimeSpan Delay = TimeSpan.FromSeconds(2);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var processor = scope.ServiceProvider.GetRequiredService<IPassesOutboxProcessor>();
                await processor.ProcessAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Passes outbox background processing failed");
            }

            await Task.Delay(Delay, stoppingToken);
        }
    }
}
