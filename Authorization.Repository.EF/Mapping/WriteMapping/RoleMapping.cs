using Authorization.Domain.Models.WriteModels.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorization.Repository.EF.Mapping.WriteMapping;

public class RoleMapping : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Role", "sec");
        builder.HasKey(x => x.Id);
        builder
        .HasMany(e => e.Permissions)
        .WithMany(e => e.Roles)
        .UsingEntity(
            "RolePermission",
            l => l.HasOne(typeof(Permission)).WithMany().HasForeignKey("PermissionId").HasPrincipalKey(nameof(Permission.Id)),
            r => r.HasOne(typeof(Role)).WithMany().HasForeignKey("RoleId").HasPrincipalKey(nameof(Role.Id)),
            j => j.ToTable("RolePermission", "sec").HasKey("RoleId", "PermissionId"));
    }
}
