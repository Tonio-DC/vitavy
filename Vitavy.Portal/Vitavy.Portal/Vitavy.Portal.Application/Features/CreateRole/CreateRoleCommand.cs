using FluentResults;
using MediatR;

namespace Vitavy.Portal.Application.Features.CreateRole;

public record CreateRoleCommand(string RoleName) : IRequest<Result<Unit>>;