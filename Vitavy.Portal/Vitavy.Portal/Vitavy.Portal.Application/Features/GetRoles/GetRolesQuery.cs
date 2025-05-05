using MediatR;
using Vitavy.Portal.Application.Models;

namespace Vitavy.Portal.Application.Features.GetRoles;

public record GetRolesQuery() : IRequest<List<Role>>;