using Authorization.Domain.Models.ReadModels.Users;
using Authorization.Domain.Models.WriteModels.Users;
using Authorization.Repository.EF.Mapping.WriteMapping;
using Core.EF.Infrastracture.EF;
using Microsoft.EntityFrameworkCore;

namespace Authorization.Repository.EF;

public class MainDataDbContext : CoreDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<RolePermission> RolePermissions { get; set; }
    public DbSet<Project> Projects { get; set; }

    public MainDataDbContext(DbContextOptions<MainDataDbContext> dbContextOptions) : base(dbContextOptions)
    {

    }
    public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserMapping).Assembly);
        modelBuilder.Ignore<UserReadModel>();
        modelBuilder.Ignore<RoleReadModel>();
        modelBuilder.Ignore<PermissionReadModel>();
        modelBuilder.Ignore<ProjectReadModel>();
        base.OnModelCreating(modelBuilder);
    }
}
