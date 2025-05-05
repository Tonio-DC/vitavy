using MediatR;
using Vitavy.Portal.Application.Infrastructure;

namespace Vitavy.Portal.Application.Features.UserRoles;

public class AddRoleToUserCommandHandler(IUserRoleRepository userRoleRepository) : IRequestHandler<AddRoleToUserCommand, Unit>
{
    public async Task<Unit> Handle(AddRoleToUserCommand request, CancellationToken cancellationToken)
    {
        await userRoleRepository.AddUserRole(request.User, request.Role, cancellationToken);
        return Unit.Value;
    }
}