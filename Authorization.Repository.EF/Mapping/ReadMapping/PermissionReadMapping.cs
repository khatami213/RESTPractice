using Authorization.Domain.Models.ReadModels.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Repository.EF.Mapping.ReadMapping;

public class PermissionReadMapping : IEntityTypeConfiguration<PermissionReadModel>
{
    public void Configure(EntityTypeBuilder<PermissionReadModel> builder)
    {
        builder.ToTable("Permission", "sec");
        builder.HasKey(x => x.Id);

        builder.Metadata.SetNavigationAccessMode(PropertyAccessMode.Property);
    }
}
