using Authorization.Domain.Models.WriteModels.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authorization.Repository.EF.Mapping.WriteMapping;

public class UserMapping : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User", "sec");
        builder.HasKey(x => x.Id);
        builder
        .HasMany(e => e.Roles)
        .WithMany(e => e.Users)
        .UsingEntity(
            "UserRole",
            l => l.HasOne(typeof(Role)).WithMany().HasForeignKey("RoleId").HasPrincipalKey(nameof(Role.Id)),
            r => r.HasOne(typeof(User)).WithMany().HasForeignKey("UserId").HasPrincipalKey(nameof(User.Id)),
            j => j.ToTable("UserRole","sec").HasKey("UserId", "RoleId"));
    }
}
