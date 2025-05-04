using FluentResults;
using MediatR;
using Vitavy.Portal.Application.Infrastructure;

namespace Vitavy.Portal.Application.Features.CreateRole;

public class CreateRoleCommandHandler(IUserRoleRepository userRoleRepository) : IRequestHandler<CreateRoleCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        await userRoleRepository.CreateRole(request.RoleName, cancellationToken);
        return Result.Ok(Unit.Value);
    }
}