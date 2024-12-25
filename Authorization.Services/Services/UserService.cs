using Authorization.Application.Services;
using Authorization.Domain.Models.ReadModels.Users;
using Authorization.Domain.Models.WriteModels.Users;
using Authorization.Domain.Repositories.Users;
using Core.Utilities;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security;

namespace Authorization.Services.Services;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly IUserReadRepository _userRepository;

    public UserService(ILogger<UserService> logger, IUserReadRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserReadModel?>> GetAllAsync()
    {
        return await _userRepository.GetAll(u => u.Roles);
    }

    public async Task<UserReadModel?> GetUserById(long id)
    {
        return await _userRepository.GetById(id, u => u.Roles);
    }

    public async Task<UserReadModel?> GetByUsername(string username)
    {
        return await _userRepository.GetByUsername(username, u => u.Roles);
    }

    public async Task<List<string?>> GetUserRole(string username)
    {
        if (await IsAnExistingUser(username) != true)
        {
            return null;
        }

        List<string> roles = new List<string>();
        var user = await _userRepository.GetByUsername(username, u => u.Roles);
        if (user != null)
            roles = user.Roles.Select(x => x.Name).ToList();

        return roles;
    }

    public async Task<bool> IsAnExistingUser(string username)
    {
        return await _userRepository.GetByUsername(username) != null;
    }

    public async Task<UserReadModel?> IsValidUserCredentials(string username, string password)
    {
        _logger.LogInformation($"Validating user [{username}]");
        if (string.IsNullOrWhiteSpace(username))
        {
            return null;
        }

        var user = await _userRepository.GetByUsername(username);
        //var salt = PasswordUtility.GenerateSalt();
        var hashPassword = PasswordUtility.ComputeHash(password, user.Salt);

        if (user == null || hashPassword != user.Password)
            return null;

        return user;
    }

    #region Permissions
    public async Task<UserReadModel?> GetRolesPermissions(string username)
    {
        return await _userRepository.GetWithRolesPermissions(username);
    }

    public async Task<IEnumerable<PermissionReadModel?>> GetUserPermissions(string username)
    {
        var user = await _userRepository.GetWithRolesPermissions(username);
        if (user == null)
            return new List<PermissionReadModel>();
        return user.Roles != null ? user.Roles.SelectMany(r => r.Permissions).Distinct() : new List<PermissionReadModel>();
    }

    public async Task<bool> HasPermission(string username, string permission)
    {
        var permissions = await GetUserPermissions(username);

        return permissions.Any(p => p.Title == permission);
    }

    public async Task<bool> HasAnyPermission(string username, IEnumerable<string> permissions)
    {
        var permissionList = await GetUserPermissions(username);

        return permissionList.Any(x => permissions.Contains(x.Title));
    }

    public async Task<bool> HasAllPermissions(string username, IEnumerable<string> permissions)
    {
        var permissionList = await GetUserPermissions(username);

        return permissionList.All(x => permissions.Contains(x.Title));
    }
    #endregion
}
