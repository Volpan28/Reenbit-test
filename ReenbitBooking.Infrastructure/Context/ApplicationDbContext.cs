using Microsoft.EntityFrameworkCore;
using ReenbitBooking.Application.Common.Interfaces;
using ReenbitBooking.Domain.Entities;

namespace ReenbitBooking.Infrastructure.Context;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Slot> Slots => Set<Slot>();
    public DbSet<User> Users => Set<User>();
    
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => base.SaveChangesAsync();

    protected void onmodelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(builder);
    }
    
}