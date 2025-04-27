using FluentResults;
using MediatR;

namespace Vitavy.Portal.Application.Features.Pilot;

public record LaunchPilotActionCommand(string Name, string City, DateTime Date, TimeSpan Time) : IRequest<Result<string>>;