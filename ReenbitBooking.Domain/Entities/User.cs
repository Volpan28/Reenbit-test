using ReenbitBooking.Domain.Enums;

namespace ReenbitBooking.Domain.Entities;

public class User
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public UserRole Role { get; set; } = UserRole.RegularUser;

    public DateTime CreatedAtUtc { get; set; }

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
