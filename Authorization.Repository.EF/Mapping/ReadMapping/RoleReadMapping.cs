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

        builder.Metadata.SetNavigationAccessMode(PropertyAccessMode.Property);
    }
}
