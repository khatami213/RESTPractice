using Authorization.Application.Services;
using Authorization.Domain.Models.ReadModels.Users;
using Authorization.Domain.Repositories;
using Core.Utilities;
using Microsoft.Extensions.Logging;

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

    public async Task<UserReadModel> IsValidUserCredentials(string userName, string password)
    {
        _logger.LogInformation($"Validating user [{userName}]");
        if (string.IsNullOrWhiteSpace(userName))
        {
            return null;
        }

        var user = await _userRepository.GetByUsername(userName);
        //var salt = PasswordUtility.GenerateSalt();
        var hashPassword = PasswordUtility.ComputeHash(password, user.Salt);

        if (user == null || hashPassword != user.Password)
            return null;

        return user;
    }

    public async Task<bool> IsAnExistingUser(string userName)
    {
        return await _userRepository.GetByUsername(userName) != null;
    }

    public async Task<List<string>> GetUserRole(string userName)
    {
        if (await IsAnExistingUser(userName) != true)
        {
            return null;
        }

        List<string> roles = new List<string>();
        var user = await _userRepository.GetByUsername(userName, u => u.Roles);
        if (user != null)
            roles = user.Roles.Select(x => x.Name).ToList();

        return roles;
    }

    public async Task<UserReadModel?> GetUserById(long id)
    {
        return await _userRepository.GetById(id);
    }
}
