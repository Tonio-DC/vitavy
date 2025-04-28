using MediatR;

namespace Vitavy.Pilot.Application.Features.BackgroundServiceAction;

public record BackgroundServiceActionCommand() : IRequest<Unit>;