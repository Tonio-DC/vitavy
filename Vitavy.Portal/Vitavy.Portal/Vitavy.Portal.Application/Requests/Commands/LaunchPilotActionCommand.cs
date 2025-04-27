using MediatR;

namespace Vitavy.Portal.Application.Requests.Commands;

public record LaunchPilotActionCommand(string Name, string City, DateTime Date, TimeSpan Time) : IRequest<string>;