using Core.Contract.Application.Events;
using Core.Contract.Application.Jwt;
using Core.EF.Infrastracture.Application.Events;
using Core.EF.Infrastracture.Application.RefreshTokens;
using Microsoft.EntityFrameworkCore;

namespace Core.EF.Infrastracture.EF;

public class CoreDbContext : DbContext
{
    public DbSet<EventModel> EventModels { get; set; }
    public DbSet<RefreshTokenModel> RefreshTokenModels { get; set; }

    public CoreDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) 
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventModelMapping).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RefreshTokenMapping).Assembly);
    }
}
