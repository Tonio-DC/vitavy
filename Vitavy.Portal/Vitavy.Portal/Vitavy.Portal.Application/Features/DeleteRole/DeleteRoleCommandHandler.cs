using FluentResults;
using MediatR;
using Vitavy.Portal.Application.Infrastructure;

namespace Vitavy.Portal.Application.Features.DeleteRole;

public class DeleteRoleCommandHandler(IUserRoleRepository userRoleRepository) : IRequestHandler<DeleteRoleCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        await userRoleRepository.DeleteRole(request.Role, cancellationToken);
        return Result.Ok(Unit.Value);
    }
}