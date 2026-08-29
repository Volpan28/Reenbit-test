using Microsoft.EntityFrameworkCore;
using ReenbitBooking.Domain.Entities;

namespace ReenbitBooking.Infrastructure.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Room> Room => Set<Room>();
    public DbSet<Slot> Slot => Set<Slot>();
    public DbSet<User> User => Set<User>();
    
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => base.SaveChangesAsync();

    protected void onmodelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(builder);
    }
    
}