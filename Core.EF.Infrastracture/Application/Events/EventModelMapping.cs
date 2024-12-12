using Core.Contract.Application.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core.EF.Infrastracture.Application.Events;

public class EventModelMapping : IEntityTypeConfiguration<EventModel>
{
    public void Configure(EntityTypeBuilder<EventModel> builder)
    {
        builder.ToTable("Events");
        builder.HasKey(e => e.Id);
    }
}
