using AutoMapper;
using MediatR;
using Vitavy.Infrastructure.Abtraction.Contracts;
using Vitavy.Pilot.Application.Features.PilotAction;
using Vitavy.Pilot.Application.Models;
using Vitavy.Pilot.Worker.Constants;

namespace Vitavy.Pilot.Worker.BackgroundServices;

public class PilotBackgroundService(ILogger<PilotBackgroundService> logger, IMediator mediator, IEventBusConsumer eventBusConsumer, IMapper mapper) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Vitavy pilot Background Service is starting.");
        await eventBusConsumer.Subscribe(
            async (LauchPilotActionMessage message, CancellationToken cancellationToken) =>
            {
                var command = mapper.Map<PilotActionCommand>(message);
                var result = await mediator.Send(command, cancellationToken);
                logger.LogInformation($"Vitavy pilot Background Service is working.");
                return result;
            },
            RoutingConstants.ConsumerId,
            stoppingToken);
        logger.LogInformation("Vitavy pilot Background Service has started.");
    }
}