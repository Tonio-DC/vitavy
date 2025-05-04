using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Vitavy.Portal.Application.Features.GetRoles;
using Vitavy.Portal.Application.Features.SearchUsers;
using Vitavy.Portal.Application.Features.UserRoles;
using Vitavy.Portal.Application.Models;

namespace Vitavy.Portal.Blazor.Components.Pages;

public partial class RoleManagement(IMediator mediator, ISnackbar snackbar) : ComponentBase
{
    private List<Role>  _userRoles = new();
    private string _newRoleName;
    private User? selectedUser;
    private Role? selectedUserRole;
    private bool userLoaded;
    public bool RoleSelectionDisabled => !userLoaded;

    private async Task<IEnumerable<User>> UserSearch(string value, CancellationToken token)
    {
        // if text is null or empty, don't return values (drop-down will not open)
        // if (string.IsNullOrEmpty(value))
        //     return [];
        var searchUsersQuery = new SearchUsersQuery(value, 30);
        var users = await mediator.Send(searchUsersQuery, token);
        return users;
    }
    
    //Roles dans Keycloak
    // realm-management:
    // - view-clients
    // - view-users
    // - manage-clients
    // - manage-users

    private async Task LoadUser(User? user)
    {
        if (user == null)
            return;
        var userRolesQuery = new UserRolesQuery(user);
        _userRoles = await mediator.Send(userRolesQuery, CancellationToken.None);
        userLoaded = true;
        snackbar.Add($"Les rôles de l'utilisateur {user.Firstname} {user.Lastname} ont été chargés.", Severity.Info);
    }

    private string UserToStringFunction(User? user)
    {
        if (user is null)
            return string.Empty;
        return $"{user.Firstname} {user.Lastname}";
    }

    private async Task RemoveUserRole(User user, Role userRole)
    {
        await mediator.Send(new RemoveUserRoleCommand(user, userRole), CancellationToken.None);
        await LoadUser(user);
        snackbar.Add($"Le rôle {userRole.Name} de l'utilisateur {user.Firstname} {user.Lastname} a bien été retiré.", Severity.Success);
    }


    private async Task<IEnumerable<Role>>? UserRolesSearch(string? arg1, CancellationToken cancellationToken)
    {
        var rolesQuery = new GetRolesQuery();
        var roles = await mediator.Send(rolesQuery, cancellationToken);
        return roles.Except(_userRoles, new RoleNameComparer());
    }
    
    private class RoleNameComparer : IEqualityComparer<Role>
    {
        public bool Equals(Role? x, Role? y) => x?.Name == y?.Name;
        public int GetHashCode(Role obj) => obj.Name?.GetHashCode() ?? 0;
    }

    private string? UserRolesToStringFunction(Role? role)
    {
        return role is null ? string.Empty : role.Name;
    }

    private async Task AddRoleToUser(User user, Role newUserRole)
    {
        var addRoleToUserCommand = new AddRoleToUserCommand(user, newUserRole);
        await mediator.Send(addRoleToUserCommand, CancellationToken.None);
        await LoadUser(user);
        snackbar.Add($"Le rôle {newUserRole.Name} a bien été ajouté à l'utilisateur {user.Firstname} {user.Lastname}.", Severity.Success);
    }
}