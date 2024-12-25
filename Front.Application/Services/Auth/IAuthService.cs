using Front.Domain.Models.Auth;
using Front.Domain.Models.Projects;

namespace Front.Application.Services.Auth;

public interface IAuthService
{
    Task<LoginResponse?> Login(LoginRequest request);
    Task Logout();
    Task<UserInfo> GetUserInfo();
}
