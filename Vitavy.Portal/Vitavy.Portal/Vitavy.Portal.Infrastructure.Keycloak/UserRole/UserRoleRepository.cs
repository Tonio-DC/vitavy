using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using AutoMapper;
using Microsoft.Extensions.Options;
using Vitavy.Portal.Application.Infrastructure;
using Vitavy.Portal.Application.Models;
using Vitavy.Portal.Infrastructure.Keycloak.Models;
using Vitavy.Portal.Infrastructure.Keycloak.Services;

namespace Vitavy.Portal.Infrastructure.Keycloak.UserRole;

public class UserRoleRepository(HttpClient httpClient, IMapper mapper, IOptions<KeycloakSetting> keycloakSetting, ITokenService tokenService) : IUserRoleRepository
{
    public async Task<List<User>> GetUsers(string? searchString, int? maxLimit, CancellationToken cancellationToken)
    {
        var searchParameter = string.Empty;
        var searchParamterBuilder = new StringBuilder();
        if (!string.IsNullOrEmpty(searchString) || maxLimit != null)
        {
            searchParamterBuilder.Append('?');
            if (!string.IsNullOrEmpty(searchString))
            {
                searchParamterBuilder.Append("search=").Append(searchString).Append('&');
            }

            if (maxLimit.HasValue)
            {
                searchParamterBuilder.Append("max=").Append(maxLimit.Value).Append('&');
            }
            searchParamterBuilder.Length--;
            searchParameter = searchParamterBuilder.ToString();
        }
        var users = await httpClient.GetFromJsonAsync<List<KeycloakUser>>($"users{searchParameter}", cancellationToken: cancellationToken);
        return mapper.Map<List<User>>(users);
    }

    public async Task<List<Role>> GetRoles(CancellationToken cancellationToken)
    {
        var clientGuid = keycloakSetting.Value.ClientGuid;
        var roles = await httpClient.GetFromJsonAsync<List<KeycloakRole>>($"clients/{clientGuid}/roles", cancellationToken: cancellationToken);
        return mapper.Map<List<Role>>(roles);
    }

    public async Task<List<Role>> GetUserRoles(User user, CancellationToken cancellationToken)
    {
        var clientGuid = keycloakSetting.Value.ClientGuid;
        var roles = await httpClient.GetFromJsonAsync<List<KeycloakRole>>($"users/{user.Id}/role-mappings/clients/{clientGuid}", cancellationToken: cancellationToken);
        return mapper.Map<List<Role>>(roles);
    }

    public async Task RemoveUserRole(User user, Role role, CancellationToken cancellationToken)
    {
        var clientGuid = keycloakSetting.Value.ClientGuid;
        var request = new HttpRequestMessage(HttpMethod.Delete, $"users/{user.Id}/role-mappings/clients/{clientGuid}");
        //request.Headers.Add("Content-Type", "application/json");
        var content = new List<KeycloakRole>
        {
            mapper.Map<KeycloakRole>(role)
        };
        request.Content = JsonContent.Create(content);
        var response = await httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task AddUserRole(User user, Role role, CancellationToken cancellationToken)
    {
        var clientGuid = keycloakSetting.Value.ClientGuid;
        var request = new HttpRequestMessage(HttpMethod.Post, $"users/{user.Id}/role-mappings/clients/{clientGuid}");
        //request.Headers.Add("Content-Type", "application/json");
        var content = new List<KeycloakRole>
        {
            mapper.Map<KeycloakRole>(role)
        };
        request.Content = JsonContent.Create(content);
        var response = await httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task CreateRole(string roleName, CancellationToken cancellationToken)
    {
        var clientGuid = keycloakSetting.Value.ClientGuid;
        var request = new HttpRequestMessage(HttpMethod.Post, $"clients/{clientGuid}/roles");
        //request.Headers.Add("Content-Type", "application/json");
        var content = new
        {
            Name = roleName
        };
        request.Content = JsonContent.Create(content);
        var response = await httpClient.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteRole(Role role, CancellationToken cancellationToken)
    {
        var clientGuid = keycloakSetting.Value.ClientGuid;
        var response = await httpClient.DeleteAsync($"clients/{clientGuid}/roles/{role.Name}", cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}