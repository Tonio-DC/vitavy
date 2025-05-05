using Vitavy.Portal.Application.Models;

namespace Vitavy.Portal.Application.Infrastructure;

public interface IUserRoleRepository
{
    Task<List<User>> GetUsers(string? searchString, int? maxLimit, CancellationToken cancellationToken);
    Task<List<Role>> GetRoles(CancellationToken cancellationToken);
    Task<List<Role>> GetUserRoles(User user, CancellationToken cancellationToken);
    Task RemoveUserRole(User user, Role role, CancellationToken cancellationToken);
    Task AddUserRole(User user, Role role, CancellationToken cancellationToken);
    Task CreateRole(string roleName, CancellationToken cancellationToken);
    Task DeleteRole(Role role, CancellationToken cancellationToken);
}