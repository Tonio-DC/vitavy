using MediatR;
using Vitavy.Pilot.Application.Features.BackgroundServiceAction;

namespace Vitavy.Pilot.Worker.BackgroundServices;

public class PilotBackgroundService(ILogger<PilotBackgroundService> logger, IMediator mediator) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Vitavy pilot Background Service is starting.");
        await mediator.Send(new BackgroundServiceActionCommand(), stoppingToken);
        logger.LogInformation("Vitavy pilot Background Service has started.");
    }
}