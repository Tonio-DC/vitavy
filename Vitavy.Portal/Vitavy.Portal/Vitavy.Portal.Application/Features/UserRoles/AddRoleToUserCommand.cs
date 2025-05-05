using MediatR;
using Vitavy.Portal.Application.Models;

namespace Vitavy.Portal.Application.Features.UserRoles;

public record AddRoleToUserCommand(User User, Role Role) : IRequest<Unit>;