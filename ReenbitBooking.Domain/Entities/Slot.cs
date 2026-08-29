using System.ComponentModel.DataAnnotations;
using ReenbitBooking.Domain.Enums;

namespace ReenbitBooking.Domain.Entities;

/// <summary>
/// A fixed, bookable time window on a Room's schedule. Status transitions (e.g. Available -> Booked)
/// must go through a concurrency-checked update: <see cref="RowVersion"/> is used as an EF Core
/// concurrency token so two users racing to book the same Slot cannot both succeed.
/// </summary>
public class Slot
{
    public Guid Id { get; set; }

    public Guid RoomId { get; set; }

    public Room? Room { get; set; }

    public DateTime StartTimeUtc { get; set; }

    public DateTime EndTimeUtc { get; set; }

    public SlotStatus Status { get; set; } = SlotStatus.Available;

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    /// <summary>
    /// Database-generated row version used as the EF Core optimistic concurrency token.
    /// Any update sent with a stale value results in a <c>DbUpdateConcurrencyException</c>,
    /// which callers must translate into an HTTP 409 Conflict.
    /// </summary>
    [Timestamp]
    public byte[] RowVersion { get; set; } = [];
}
