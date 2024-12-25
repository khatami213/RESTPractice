using Authorization.Application.Services;
using Authorization.Domain.Models.WriteModels.Users;
using Authorization.Domain.Repositories.Users;

namespace Authorization.Services.Services;

public class PermissionService : IPermissionService
{
    private readonly IPermissionWriteRepository _permissionWriteRepository;
    private readonly IPermissionReadRepository _permissionReadRepository;

    public PermissionService(IPermissionWriteRepository permissionWriteRepository, 
        IPermissionReadRepository permissionReadRepository)
    {
        _permissionWriteRepository = permissionWriteRepository;
        _permissionReadRepository = permissionReadRepository;
    }

    public Task<IEnumerable<Permission>?> GetRolePermissions(string roleName)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Permission>?> GetRolesPermissions(List<Role> roles)
    {
        throw new NotImplementedException();
    }
}
