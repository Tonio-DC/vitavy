using System.Globalization;
using FluentResults;
using MediatR;
using Vitavy.Infrastructure.Abtraction.Contracts;
using Vitavy.Portal.Application.Constants;
using Vitavy.Portal.Application.Models;
using Vitavy.Portal.Application.Validators;

namespace Vitavy.Portal.Application.Features.Pilot;

public class LaunchPilotActionCommandHandler(IPilotCommandValidator pilotCommandValidator, IEventBusRpcProducer<LaunchPilotActionCommand, PilotResponse> eventBusRpcProducer) : IRequestHandler<LaunchPilotActionCommand, Result<string>>
{
    public async Task<Result<string>> Handle(LaunchPilotActionCommand command, CancellationToken cancellationToken)
    {
        var commandResult = await pilotCommandValidator.Validate(command);
        if (commandResult.IsFailed)
        {
            return Result.Fail<string>(commandResult.Errors);
        }

        await eventBusRpcProducer.StartRpc(RoutingConstants.ProducerId, cancellationToken);
        var pilotResponse = await eventBusRpcProducer.PublishRpc(command, RoutingConstants.RoutingKey.Pilot,
            RoutingConstants.ProducerId, cancellationToken);
        
        return Result.Ok(pilotResponse.Temperature.ToString(CultureInfo.InvariantCulture));
    }
}