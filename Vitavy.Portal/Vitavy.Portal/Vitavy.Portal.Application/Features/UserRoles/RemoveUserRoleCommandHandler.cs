using MediatR;
using Vitavy.Portal.Application.Infrastructure;

namespace Vitavy.Portal.Application.Features.UserRoles;

public class RemoveUserRoleCommandHandler(IUserRoleRepository userRoleRepository) : IRequestHandler<RemoveUserRoleCommand, Unit>
{
    public async Task<Unit> Handle(RemoveUserRoleCommand command, CancellationToken cancellationToken)
    {
        await userRoleRepository.RemoveUserRole(command.User, command.UserRole, cancellationToken);
        return Unit.Value;
    }
}