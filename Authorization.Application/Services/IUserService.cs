using Authorization.Domain.Models.ReadModels.Users;
using Authorization.Domain.Models.WriteModels.Users;

namespace Authorization.Application.Services;

public interface IUserService
{
    Task<IEnumerable<UserReadModel?>> GetAllAsync();
    
    Task<UserReadModel?> GetUserById(long id);

    Task<UserReadModel?> GetByUsername(string username);

    Task<List<string?>> GetUserRole(string username);

    Task<bool> IsAnExistingUser(string username);

    Task<UserReadModel?> IsValidUserCredentials(string username, string password);


    #region Permission
    Task<UserReadModel?> GetRolesPermissions(string username);
    Task<IEnumerable<PermissionReadModel?>> GetUserPermissions(string username);
    Task<bool> HasPermission(string username, string permission);
    Task<bool> HasAnyPermission(string username, IEnumerable<string> permissions);
    Task<bool> HasAllPermissions(string username, IEnumerable<string> permissions);
    #endregion
}
