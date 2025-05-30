﻿using Acropolis.Application.Events;
using Acropolis.Application.Events.VideoDownloader;
using Acropolis.Application.Extensions.MassTransitExtensions;
using MassTransit;

namespace Acropolis.Application.Sagas.DownloadVideo;

public class DownloadVideoSaga : MassTransitStateMachine<DownloadVideoState>
{
    public DownloadVideoSaga()
    {
        InstanceState(e => e.CurrentState);
        SetCompletedWhenFinalized();
        
        WhenEnter(Downloaded, x => x.TransitionTo(Final));
        WhenEnter(DownloadSkipped, x => x.TransitionTo(Final));
        
        Initially(
            When(WhenUrlRequestReceived)
                .ThenAsync(async ctx =>
                {
                    var (saga, message) = ctx.Deconstruct();

                    saga.RequestedTimestamp = message.Timestamp;
                    saga.Url = message.Url;

                    await ctx.Publish(new VideoDownloadRequested(saga.Url));
                })
                .TransitionTo(DownloadRequested));

        During(DownloadRequested,
            Ignore(WhenUrlRequestReceived),
            When(WhenVideoDownloaded)
                .Publish(ctx =>
                {
                    var (saga, message) = ctx.Deconstruct();
                    return new UrlRequestReplyRequested(
                        saga.CorrelationId,
                        saga.Url,
                        $"Downloaded {message.VideoMetaData.VideoTitle}. Location: {message.StorageLocation} ");
                })
                .TransitionTo(Downloaded),
            When(WhenVideoDownloadSkipped)
                .TransitionTo(DownloadSkipped),
            When(WhenVideoDownloadFailed)
                .Then(x =>
                {
                    var (saga, message) = x.Deconstruct();
                    saga.ErrorMessage = message.ErrorMessage;
                    saga.ErrorTimestamp = message.Timestamp;
                })
                .Publish(ctx =>
                {
                    var (saga, message) = ctx.Deconstruct();
                    return new UrlRequestReplyRequested(
                        saga.CorrelationId,
                        saga.Url,
                        $"Download failed: {message.ErrorMessage}");
                })
                .TransitionTo(DownloadFailed)
        );
        
        During(DownloadFailed,
            When(WhenUrlRequestReceived)
                .Then(ctx =>
                {
                    var (saga, message) = ctx.Deconstruct();
                    saga.ErrorTimestamp = null;
                    saga.ErrorMessage = null;
                })
                .Publish(ctx =>
                {
                    var (saga, message) = ctx.Deconstruct();
                    return new VideoDownloadRequested(saga.Url);
                })
                .TransitionTo(DownloadRequested),
            When(WhenRetryFailedVideoDownloadRequested)
                .Then(ctx =>
                {
                    var (saga, message) = ctx.Deconstruct();
                    saga.ErrorTimestamp = null;
                    saga.ErrorMessage = null;
                })
                .Publish(ctx =>
                {
                    var (saga, message) = ctx.Deconstruct();
                    return new VideoDownloadRequested(saga.Url);
                })
                .TransitionTo(DownloadRequested),
            Ignore(WhenVideoDownloaded),
            Ignore(WhenVideoDownloadSkipped),
            Ignore(WhenVideoDownloadFailed)
        );
        
        During(Downloaded,
            When(WhenVideoDownloaded)
                .TransitionTo(Final));

        Event(() => WhenUrlRequestReceived,
            e => e.CorrelateById(ctx => ctx.Message.RequestId).CorrelateBy(saga => saga.Url, ctx => ctx.Message.Url));
        Event(() => WhenVideoDownloaded,
            e => e.CorrelateBy(saga => saga.Url, ctx => ctx.Message.Url));
        Event(() => WhenVideoDownloadSkipped,
            e => e.CorrelateBy(saga => saga.Url, ctx => ctx.Message.Url));
        Event(() => WhenVideoDownloadFailed,
            e => e.CorrelateBy(saga => saga.Url, ctx => ctx.Message.Url));
        Event(() => WhenRetryFailedVideoDownloadRequested,
            e => e.CorrelateBy(saga => saga.Url, ctx => ctx.Message.Url));
    }

    public Event<UrlRequestReceived> WhenUrlRequestReceived { get; } = null!;
    public Event<VideoDownloaded> WhenVideoDownloaded { get; } = null!;
    public Event<VideoDownloadSkipped> WhenVideoDownloadSkipped { get; } = null!;
    public Event<VideoDownloadFailed> WhenVideoDownloadFailed { get; } = null!;
    public Event<RetryFailedVideoDownloadRequested> WhenRetryFailedVideoDownloadRequested { get; } = null!;

    public State DownloadRequested { get; } = null!;
    public State Downloaded { get; } = null!;
    public State DownloadFailed { get; } = null!;
    public State DownloadSkipped { get; } = null!;
}