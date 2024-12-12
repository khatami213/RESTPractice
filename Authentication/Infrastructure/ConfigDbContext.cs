using Authorization.Repository.EF;
using Core.EF.Infrastracture.Config.DbContext;
using Core.EF.Infrastracture.EF;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Infrastructure;

public static class ConfigDbContext
{
    public static void AddDbContextServices(this IServiceCollection services, IConfiguration configuration)
    {
        DbContextConfig.Configs.Add("dbCommand", typeof(MainDataDbContext));
        DbContextConfig.Configs.Add("dbQuery", typeof(MainDataReadDbContext));

        services.AddScoped<CoreDbContext>(serviceProvider =>
        {
            var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            var endpoint = httpContextAccessor.HttpContext?.GetEndpoint();
            var controllerActionDescriptor = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>();
            var @namespace = controllerActionDescriptor?.ControllerTypeInfo.Namespace;

            var contextType = DbContextConfig.GetContext(@namespace);

            return contextType == null
                ? serviceProvider.GetRequiredService<MainDataDbContext>()
                : (CoreDbContext)serviceProvider.GetRequiredService(contextType);
        });

        var dbCommandConnectionString = configuration.GetConnectionString("dbCommand");
        services.AddDbContext<MainDataDbContext>(options =>
        {
            options.UseSqlServer(dbCommandConnectionString);
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
        }, ServiceLifetime.Scoped);


        var dbQueryConnectionString = configuration.GetConnectionString("dbQuery");
        services.AddDbContext<MainDataReadDbContext>(options =>
        {
            options.UseSqlServer(dbQueryConnectionString);
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }, ServiceLifetime.Scoped);


    }
}
