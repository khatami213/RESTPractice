using Authorization.Domain.Models.ReadModels.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorization.Repository.EF.Mapping.ReadMapping;

public class RoleReadMapping : IEntityTypeConfiguration<RoleReadModel>
{
    public void Configure(EntityTypeBuilder<RoleReadModel> builder)
    {
        builder.ToTable("Role", "sec");
        builder.HasKey(x => x.Id);

        builder
        .HasMany(e => e.Permissions)
        .WithMany(e => e.Roles)
        .UsingEntity(
            "RolePermission",
            l => l.HasOne(typeof(PermissionReadModel)).WithMany().HasForeignKey("PermissionId").HasPrincipalKey(nameof(PermissionReadModel.Id)),
            r => r.HasOne(typeof(RoleReadModel)).WithMany().HasForeignKey("RoleId").HasPrincipalKey(nameof(RoleReadModel.Id)),
            j => j.ToTable("RolePermission", "sec").HasKey("RoleId", "PermissionId"));

        builder.Metadata.SetNavigationAccessMode(PropertyAccessMode.Property);
    }
}
