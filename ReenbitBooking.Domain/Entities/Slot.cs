using System.ComponentModel.DataAnnotations;
using ReenbitBooking.Domain.Enums;

namespace ReenbitBooking.Domain.Entities;

public class Slot
{
    public Guid Id { get; set; }

    public Guid RoomId { get; set; }

    public Room? Room { get; set; }

    public DateTimeOffset StartTimeUtc { get; set; }

    public DateTimeOffset EndTimeUtc { get; set; }

    public SlotStatus Status { get; set; } = SlotStatus.Available;

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    
    [Timestamp]
    public byte[] RowVersion { get; set; } = [];
}
