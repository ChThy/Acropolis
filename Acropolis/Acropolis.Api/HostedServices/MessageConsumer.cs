using Acropolis.Application;
using Acropolis.Application.Events;
using Acropolis.Application.Events.Infrastructure;
using Acropolis.Application.Mediator;

namespace Acropolis.Api.HostedServices;

public class MessageConsumer : BackgroundService
{
    private readonly InMemoryMessageBus messageBus;
    private readonly IServiceScopeFactory serviceScopeFactory;
    private readonly ILogger<MessageConsumer> logger;

    public MessageConsumer(
        InMemoryMessageBus messageBus,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<MessageConsumer> logger)
    {
        this.messageBus = messageBus;
        this.serviceScopeFactory = serviceScopeFactory;
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        messageBus.Subscribe(nameof(RequestToMessageTranslation), RequestToMessageTranslation);

        logger.LogInformation("Start consuming messages");
        await messageBus.Consume();
    }

    private async ValueTask RequestToMessageTranslation(IMessage message)
    {
        if (message is not RequestReceived requestReceived)
        {
            return;
        }
        using var scope = serviceScopeFactory.CreateScope();
        var requestTranslators = scope.ServiceProvider.GetServices<IRequestCommandTranslator>().ToArray();

        foreach (var requestTranslator in requestTranslators.Where(e => e.CanHandle(requestReceived.Request.Message, requestReceived.Request.Params)))
        {
            var command = requestTranslator.CreateCommand(requestReceived.Request.Message, requestReceived.Request.Params);
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            await mediator.Send(command);
        }
    }
}
