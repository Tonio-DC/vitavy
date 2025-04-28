using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Vitavy.Infrastructure.Abtraction.Contracts;
using Vitavy.Pilot.Application.Constants;
using Vitavy.Pilot.Application.Features.PilotAction;
using Vitavy.Pilot.Application.Models;

namespace Vitavy.Pilot.Application.Features.BackgroundServiceAction;

public class BackgroundServiceActionCommandHandler(ILogger<BackgroundServiceActionCommandHandler> logger, IMediator mediator, IEventBusConsumer eventBusConsumer, IMapper mapper) : IRequestHandler<BackgroundServiceActionCommand, Unit>
{
    public async Task<Unit> Handle(BackgroundServiceActionCommand request, CancellationToken cancellationToken)
    {
        await eventBusConsumer.Subscribe(
            async (LauchPilotActionMessage message, CancellationToken cancellationToken) =>
            {
                var command = mapper.Map<PilotActionCommand>(message);
                var result = await mediator.Send(command, cancellationToken);
                logger.LogInformation($"Vitavy pilot Background Service is working.");
                return result;
            },
            RoutingConstants.ConsumerId,
            cancellationToken);
        return Unit.Value;
    }
}