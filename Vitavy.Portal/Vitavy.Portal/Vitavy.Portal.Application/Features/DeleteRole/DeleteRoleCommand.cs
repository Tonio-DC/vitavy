using FluentResults;
using MediatR;
using Vitavy.Portal.Application.Models;

namespace Vitavy.Portal.Application.Features.DeleteRole;

public record DeleteRoleCommand(Role Role) : IRequest<Result<Unit>>;