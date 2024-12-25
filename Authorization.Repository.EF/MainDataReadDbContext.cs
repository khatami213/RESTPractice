using Authorization.Domain.Models.ReadModels.Users;
using Authorization.Domain.Models.WriteModels.Users;
using Authorization.Repository.EF.Mapping.ReadMapping;
using Core.EF.Infrastracture.EF;
using Microsoft.EntityFrameworkCore;

namespace Authorization.Repository.EF;

public class MainDataReadDbContext : CoreDbContext
{
    public DbSet<UserReadModel> Users { get; set; }
    public DbSet<RoleReadModel> Roles { get; set; }
    public DbSet<UserRoleReadModel> UserRoles { get; set; }
    public DbSet<ProjectReadModel> Projects { get; set; }
    public MainDataReadDbContext(DbContextOptions<MainDataReadDbContext> dbContextOptions) : base(dbContextOptions)
    {
    }

    public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserReadMapping).Assembly);
        modelBuilder.Ignore<User>();
        modelBuilder.Ignore<Role>();
        modelBuilder.Ignore<Permission>();
        modelBuilder.Ignore<Project>();
        base.OnModelCreating(modelBuilder);
    }
}
