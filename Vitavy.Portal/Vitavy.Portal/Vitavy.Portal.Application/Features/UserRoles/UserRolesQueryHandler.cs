using MediatR;
using Vitavy.Portal.Application.Infrastructure;
using Vitavy.Portal.Application.Models;

namespace Vitavy.Portal.Application.Features.UserRoles;

public class UserRolesQueryHandler(IUserRoleRepository userRoleRepository) : IRequestHandler<UserRolesQuery, List<Role>>
{
    public async Task<List<Role>> Handle(UserRolesQuery request, CancellationToken cancellationToken)
    {
        var userRoles = await userRoleRepository.GetUserRoles(request.User, cancellationToken);
        return userRoles;
    }
}