using FluentResults;
using MediatR;
using Vitavy.Pilot.Application.Models;

namespace Vitavy.Pilot.Application.Features.PilotAction;

public record PilotActionCommand(string Name, string City, DateTime Date, TimeSpan Time) : IRequest<Result<LauchPilotActionResponse>>;