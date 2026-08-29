using System.ComponentModel.DataAnnotations;
using ReenbitBooking.Domain.Enums;

namespace ReenbitBooking.Domain.Entities;

public class Booking
{
    public Guid Id { get; set; }

    public Guid SlotId { get; set; }

    public Slot? Slot { get; set; }

    public Guid UserId { get; set; }

    public User? User { get; set; }

    public BookingStatus Status { get; set; } = BookingStatus.Confirmed;

    public DateTimeOffset BookedAtUtc { get; set; }

    public DateTimeOffset? CancelledAtUtc { get; set; }
    
    [Timestamp]
    public byte[] RowVersion { get; set; } = [];
}
