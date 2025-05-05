using MediatR;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Vitavy.Portal.Application.Features.CreateRole;
using Vitavy.Portal.Application.Features.DeleteRole;
using Vitavy.Portal.Application.Features.GetRoles;
using Vitavy.Portal.Application.Models;

namespace Vitavy.Portal.Blazor.Components.Pages;

public partial class RoleReferential(IMediator mediator, ISnackbar snackbar) : ComponentBase
{
    public string _roleToAdd { get; set; }
    private List<Role> ExistingRoles = new List<Role>();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await RefreshRolesGrid();
    }

    private async Task RefreshRolesGrid()
    {
        var getRolesQuery = new GetRolesQuery();
        ExistingRoles = await mediator.Send(getRolesQuery, CancellationToken.None);
    }

    private async Task CreateRole(string roleName, CancellationToken cancellationToken =  default)
    {
        var createRoleCommand = new CreateRoleCommand(roleName);
        _ = await mediator.Send(createRoleCommand, cancellationToken);
        await RefreshRolesGrid();
        _roleToAdd = string.Empty;
        snackbar.Add($"Le rôle {roleName} a bien été créé dans le référentiel de rôles de Vitavy.", Severity.Success);
    }

    private async Task DeleteRole(Role role, CancellationToken cancellationToken =  default)
    {
        var deleteRoleCommand = new DeleteRoleCommand(role);
        _ = await mediator.Send(deleteRoleCommand, cancellationToken);
        await RefreshRolesGrid();
        snackbar.Add($"Le rôle {role.Name} a bien été supprimé du référentiel de rôles de Vitavy.", Severity.Success);
    }
}