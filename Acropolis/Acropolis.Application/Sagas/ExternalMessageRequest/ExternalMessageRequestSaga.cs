using Acropolis.Application.Events;
using Acropolis.Application.Extensions.MassTransitExteions;
using MassTransit;

namespace Acropolis.Application.Sagas.ExternalMessageRequest;

public class ExternalMessageRequestSaga : MassTransitStateMachine<ExternalMessageRequestState>
{
    private static readonly char[] WhiteSpaceCharacters = ['\t', '\n', ' '];
    
    public ExternalMessageRequestSaga()
    {
        InstanceState(e => e.CurrentState);
        
        Initially(
            When(WhenExternalMessageRequestReceived)
                .ThenAsync(async ctx =>
                {
                    var (saga, message) = ctx.Deconstruct();
                    saga.Channel = message.Channel;
                    saga.ReceivedOn = message.Timestamp;
                    saga.MessageBody = message.MessageBody;
                    saga.MessageProps = message.MessageProps;

                    var urls = ExtractUrls(saga.MessageBody);
                    await ctx.PublishBatch(urls.Select(e => new UrlRequestReceived(saga.CorrelationId, e, saga.ReceivedOn)));
                })
                .TransitionTo(RequestReceived)
            );

        During(RequestReceived,
            Ignore(WhenExternalMessageRequestReceived),
            When(WhenReplyRequested)
                .Publish(ctx =>
                {
                    return new ExternalMessageReplyRequested(
                        ctx.Message.RequestId,
                        ctx.Saga.Channel,
                        ctx.Message.Message,
                        ctx.Saga.MessageProps);
                })
        );
        
        Event(() => WhenExternalMessageRequestReceived, 
            e => e.CorrelateById(ctx => ctx.Message.MessageId));
        Event(() => WhenReplyRequested,
            e => e.CorrelateById(ctx => ctx.Message.RequestId));
    }

    public Event<ExternalMessageRequestReceived> WhenExternalMessageRequestReceived { get; set; } = null!;
    public Event<UrlRequestReplyRequested> WhenReplyRequested { get; set; } = null!;
    
    public State RequestReceived { get; set; } = null!;

    private static string[] ExtractUrls(string input)
    {
        var urls = input.Split(WhiteSpaceCharacters, StringSplitOptions.RemoveEmptyEntries)
            .Where(e => e.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                        e.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ||
                        e.StartsWith("www.", StringComparison.OrdinalIgnoreCase))
            .ToArray();
        return urls;
    }
}