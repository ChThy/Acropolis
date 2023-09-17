using Acropolis.Application;
using Acropolis.Application.Events;
using Acropolis.Application.Events.Infrastructure;
using Acropolis.Application.Mediator;
using Acropolis.Application.Messenger;
using Acropolis.Domain.Repositories;

namespace Acropolis.Api.HostedServices;

public class MessageConsumer : BackgroundService
{
    private readonly InMemoryMessageBus messageBus;
    private readonly IIncomingRequestRepostory incomingRequestRepostiory;
    private readonly IServiceScopeFactory serviceScopeFactory;
    private readonly ILogger<MessageConsumer> logger;

    public MessageConsumer(
        InMemoryMessageBus messageBus,
        IIncomingRequestRepostory incomingRequestRepostiory,
        IServiceScopeFactory serviceScopeFactory,
        ILogger<MessageConsumer> logger)
    {
        this.messageBus = messageBus;
        this.incomingRequestRepostiory = incomingRequestRepostiory;
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
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var requestTranslators = scope.ServiceProvider.GetServices<IRequestCommandTranslator>().ToArray();
        var applicableTranslators = requestTranslators.Where(e => e.CanHandle(requestReceived)).ToArray();

        if (applicableTranslators.Length == 0)
        {
            await AskClarification(mediator, requestReceived);
            return;
        }

        await ProcessRequest(requestReceived, applicableTranslators, mediator);
    }

    private async ValueTask AskClarification(IMediator mediator, RequestReceived requestReceived)
    {
        var command = new SendMessage("What to do with this?", new Dictionary<string, string>
        {
            ["OriginalMessage"] = requestReceived.Request.Message,
            ["ChatId"] = requestReceived.Request.Params["ChatId"],
            ["MessageId"] = requestReceived.Request.Params["MessageId"]
        });
        await mediator.Send(command);
    }

    private async Task ProcessRequest(RequestReceived requestReceived, IRequestCommandTranslator[] applicableTranslators, IMediator mediator)
    {
        foreach (var requestTranslator in applicableTranslators)
        {
            var command = requestTranslator.CreateCommand(requestReceived);
            await mediator.Send(command);
        }
    }
}
