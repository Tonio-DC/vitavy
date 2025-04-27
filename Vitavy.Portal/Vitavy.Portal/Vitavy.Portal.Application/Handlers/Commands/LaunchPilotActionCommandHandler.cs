using MediatR;
using Vitavy.Portal.Application.Requests.Commands;

namespace Vitavy.Portal.Application.Handlers.Commands;

public class LaunchPilotActionCommandHandler : IRequestHandler<LaunchPilotActionCommand, string>
{
    public Task<string> Handle(LaunchPilotActionCommand command, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}