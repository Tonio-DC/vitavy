using FluentResults;
using Vitavy.Portal.Application.Features.Pilot;

namespace Vitavy.Portal.Application.Validators;

public interface IPilotCommandValidator
{
    public Task<Result<LaunchPilotActionCommand>> Validate(LaunchPilotActionCommand? command);
}