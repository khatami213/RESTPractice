using Authorization.Domain.Models.ReadModels.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorization.Repository.EF.Mapping.ReadMapping;

public class UserReadMapping : IEntityTypeConfiguration<UserReadModel>
{
    public void Configure(EntityTypeBuilder<UserReadModel> builder)
    {
        builder.ToTable("User", "sec");
        builder.HasKey(x => x.Id);
        builder
        .HasMany(e => e.Roles)
        .WithMany(e => e.Users)
        .UsingEntity(
            "UserRole",
            l => l.HasOne(typeof(RoleReadModel)).WithMany().HasForeignKey("RoleId").HasPrincipalKey(nameof(RoleReadModel.Id)),
            r => r.HasOne(typeof(UserReadModel)).WithMany().HasForeignKey("UserId").HasPrincipalKey(nameof(UserReadModel.Id)),
            j => j.ToTable("UserRole","sec").HasKey("UserId", "RoleId"));

        builder
        .HasMany(e => e.Projects)
        .WithMany(e => e.Users)
        .UsingEntity(
            "UserProject",
            l => l.HasOne(typeof(ProjectReadModel)).WithMany().HasForeignKey("ProjectId").HasPrincipalKey(nameof(ProjectReadModel.Id)),
            r => r.HasOne(typeof(UserReadModel)).WithMany().HasForeignKey("UserId").HasPrincipalKey(nameof(UserReadModel.Id)),
            j => j.ToTable("UserProject", "sec").HasKey("UserId", "ProjectId"));

        builder.Metadata.SetNavigationAccessMode(PropertyAccessMode.Property);
    }
}
