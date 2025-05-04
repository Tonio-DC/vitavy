using MediatR;
using Vitavy.Portal.Application.Models;

namespace Vitavy.Portal.Application.Features.SearchUsers;

public record SearchUsersQuery(string SearchRequest, int? MaxLimit) : IRequest<List<User>>;