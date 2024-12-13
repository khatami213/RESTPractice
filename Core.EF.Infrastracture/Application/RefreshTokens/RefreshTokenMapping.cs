using Core.Contract.Application.Jwt;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.EF.Infrastracture.Application.RefreshTokens;

public class RefreshTokenMapping : IEntityTypeConfiguration<RefreshTokenModel>
{
    public void Configure(EntityTypeBuilder<RefreshTokenModel> builder)
    {
        builder.ToTable("RefreshTokens");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.IsUsed).HasDefaultValue(false);
        builder.Property(x => x.IsRevoked).HasDefaultValue(false);
    }
}
