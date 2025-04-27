using FluentResults;
using MediatR;
using Vitavy.Pilot.Application.Models;

namespace Vitavy.Pilot.Application.Features.PilotAction;

public class PilotActionCommandHandler : IRequestHandler<PilotActionCommand, Result<LauchPilotActionResponse>>
{
    public async Task<Result<LauchPilotActionResponse>> Handle(PilotActionCommand request, CancellationToken cancellationToken)
    {
        var random = new Random();
        var temperature = random.Next(400)/10 as decimal? ?? 0;
        return await Task.FromResult(
            Result.Ok(
                new LauchPilotActionResponse(
                    request.Name,
                    request.City,
                    request.Date, 
                    request.Time,
                    temperature)));
    }
}