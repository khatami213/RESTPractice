using Authorization.Application.Services;
using Authorization.Domain.Repositories.Users;
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

        services.AddScoped<IRoleWriteRepository, RoleWriteRepository>();
        services.AddScoped<IRoleReadRepository, RoleReadRepository>();
        services.AddScoped<IRoleService, RoleService>();

        services.AddScoped<IPermissionWriteRepository, PermissionWriteRepository>();
        services.AddScoped<IPermissionReadRepository, PermissionReadRepository>();
        services.AddScoped<IPermissionService, PermissionService>();
    }
}
