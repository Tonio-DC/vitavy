using MediatR;
using Vitavy.Portal.Application.Infrastructure;
using Vitavy.Portal.Application.Models;

namespace Vitavy.Portal.Application.Features.GetRoles;

public class GetRolesQueryHandler(IUserRoleRepository userRoleRepository) : IRequestHandler<GetRolesQuery, List<Role>>
{
    public async Task<List<Role>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await userRoleRepository.GetRoles(cancellationToken);
        return roles;
    }
}