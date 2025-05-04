using MediatR;
using Vitavy.Portal.Application.Infrastructure;
using Vitavy.Portal.Application.Models;

namespace Vitavy.Portal.Application.Features.SearchUsers;

public class SearchUsersQueryHandler(IUserRoleRepository userRoleRepository) : IRequestHandler<SearchUsersQuery, List<User>>
{
    public async Task<List<User>> Handle(SearchUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await userRoleRepository.GetUsers(request.SearchRequest, request.MaxLimit, cancellationToken);
        return users;
    }
}