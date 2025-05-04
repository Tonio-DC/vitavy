using MediatR;
using Vitavy.Portal.Application.Models;

namespace Vitavy.Portal.Application.Features.UserRoles;

public record RemoveUserRoleCommand(User User, Role UserRole) :  IRequest<Unit>;