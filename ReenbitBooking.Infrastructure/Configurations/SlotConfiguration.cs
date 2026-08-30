using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReenbitBooking.Domain.Entities;

namespace ReenbitBooking.Infrastructure.Configurations;

public class SlotConfiguration : IEntityTypeConfiguration<Slot>
{
    public void Configure(EntityTypeBuilder<Slot> builder)
    {
        builder.ToTable("Slots");
        
        builder.HasKey(s=> s.Id);

        builder.HasIndex(s => new { s.RoomId, s.StartTimeUtc });

        builder.Property(s => s.RoomId)
            .IsRequired();
        
        builder.Property(s => s.StartTimeUtc)
            .IsRequired();
        
        builder.Property(s => s.EndTimeUtc)
            .IsRequired();
        
        builder.Property(s => s.Status)
            .IsRequired();
        
        builder.Property(s => s.RowVersion)
            .IsRowVersion()
            .IsRequired();
    }
}