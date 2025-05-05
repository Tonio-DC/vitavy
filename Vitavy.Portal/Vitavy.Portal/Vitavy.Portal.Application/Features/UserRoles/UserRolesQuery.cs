using MediatR;
using Vitavy.Portal.Application.Models;

namespace Vitavy.Portal.Application.Features.UserRoles;

public record UserRolesQuery(User User) : IRequest<List<Role>>;