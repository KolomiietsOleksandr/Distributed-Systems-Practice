namespace EvolutionaryArchitecture.Fitnet.IntegrationTests.Passes.RegisterPass;

using Common.TestEngine.Configuration;
using Common.TestEngine.IntegrationEvents.Handlers;
using EvolutionaryArchitecture.Fitnet.Contracts.SignContract.Events;
using EvolutionaryArchitecture.Fitnet.Passes.RegisterPass.Events;
using EvolutionaryArchitecture.Fitnet.Common.Events.EventBus;
using EvolutionaryArchitecture.Fitnet.Passes.Application;
using Microsoft.Extensions.DependencyInjection;

public sealed class RegisterPassTests : IClassFixture<WebApplicationFactory<Program>>, IClassFixture<DatabaseContainer>, IAsyncLifetime
{
    private readonly WebApplicationFactory<Program> _applicationInMemory;
    private readonly IEventBus _fakeEventBus = Substitute.For<IEventBus>();
    private readonly HttpClient _applicationHttpClient;

    public RegisterPassTests(WebApplicationFactory<Program> applicationInMemoryFactory,
        DatabaseContainer database)
    {
        _applicationInMemory = applicationInMemoryFactory
            .WithContainerDatabaseConfigured(database.ConnectionString!)
            .WithFakeEventBus(_fakeEventBus);
        _applicationHttpClient = _applicationInMemory.CreateClient();
    }

    public ValueTask InitializeAsync() => ValueTask.CompletedTask;

    public async ValueTask DisposeAsync()
    {
        _applicationHttpClient.Dispose();
        await _applicationInMemory.DisposeAsync();
    }

    [Fact]
    internal async Task Given_contract_signed_event_Then_should_register_pass()
    {
        // Arrange
        using var integrationEventHandlerScope =
            new IntegrationEventHandlerScope<ContractSignedEvent>(_applicationInMemory);
        var @event = ContractSignedEventFaker.Create();

        // Act
        await integrationEventHandlerScope.Consume(@event, TestContext.Current.CancellationToken);

        await ProcessPassesOutboxAsync();

        // Assert
        EnsureThatPassRegisteredEventWasPublished();
    }

    private void EnsureThatPassRegisteredEventWasPublished() => _fakeEventBus.Received(1)
        .PublishAsync(Arg.Any<PassRegisteredEvent>(), Arg.Any<CancellationToken>());

    private async Task ProcessPassesOutboxAsync()
    {
        using var scope = _applicationInMemory.Services.CreateScope();
        var processor = scope.ServiceProvider.GetRequiredService<IPassesOutboxProcessor>();
        await processor.ProcessAsync(CancellationToken.None);
    }
}
