using Microsoft.EntityFrameworkCore;
using ReenbitBooking.Domain.Entities;

namespace ReenbitBooking.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Booking> Bookings { get; }
    DbSet<Room> Rooms { get; }
    DbSet<Slot> Slots { get; }
    DbSet<User> Users { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}