using Authorization.Application.Services;
using Authorization.Domain.Repositories;
using Authorization.Repository.EF.Repositories.Users;
using Authorization.Services.Services;

namespace Authentication.Infrastructure;

public static class ConfigRepositoryService
{
    public static void AddRepositoryServices(this IServiceCollection services)
    {
        services.AddScoped<IUserWriteRepository, UserWriteRepository>();
        services.AddScoped<IUserReadRepository, UserReadRepository>();
        services.AddScoped<IUserService, UserService>();
    }
}
