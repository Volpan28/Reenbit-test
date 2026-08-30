using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReenbitBooking.Domain.Entities;

namespace ReenbitBooking.Infrastructure.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.SlotId)
            .IsRequired();

        builder.HasIndex(b => b.UserId);
        
        builder.Property(b => b.Status)
            .IsRequired();
        
        builder.Property(b => b.BookedAtUtc)
            .IsRequired();
        
        builder.Property(b => b.RowVersion)
            .IsRowVersion()
            .IsRequired();
        
        builder.HasOne(b => b.User)
            .WithMany(b => b.Bookings)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(b => b.Slot)
            .WithMany(s => s.Bookings)
            .OnDelete(DeleteBehavior.Restrict);
    }
}