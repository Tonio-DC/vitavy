using System.Globalization;
using FluentResults;
using MediatR;
using Vitavy.Infrastructure.Contracts;
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

        var pilotResponse = await eventBusRpcProducer.PublishRpc(command, Constants.RoutingConstants.RoutingKey.Pilot,
            Constants.RoutingConstants.ProducerId, cancellationToken);
        
        return Result.Ok(pilotResponse.Temperature.ToString(CultureInfo.InvariantCulture));
    }
}