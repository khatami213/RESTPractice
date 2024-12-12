using Authorization.Domain.Models.ReadModels.Users;

namespace Authorization.Application.Services;

public interface IUserService
{
    Task<bool> IsAnExistingUser(string userName);

    Task<UserReadModel> IsValidUserCredentials(string userName, string password);

    Task<List<string>> GetUserRole(string userName);

    Task<UserReadModel?> GetUserById(long id);
}
