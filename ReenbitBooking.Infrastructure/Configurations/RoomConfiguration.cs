using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReenbitBooking.Domain.Entities;

namespace ReenbitBooking.Infrastructure.Configurations;

public class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.ToTable("Rooms");
        
        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(r => r.Location)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(r => r.Capacity)
            .IsRequired();
        
        builder.Property(r => r.IsActive)
            .IsRequired();
        
        builder.HasMany(r => r.Slots)
            .WithOne(r => r.Room)
            .HasForeignKey(r => r.RoomId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}