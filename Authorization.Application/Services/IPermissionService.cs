using Authorization.Domain.Models.WriteModels.Users;

namespace Authorization.Application.Services;

public interface IPermissionService
{
    Task<IEnumerable<Permission>?> GetRolePermissions(string roleName);
    Task<IEnumerable<Permission>?> GetRolesPermissions(List<Role> roles);
}
