using System.ComponentModel.DataAnnotations;
using ReenbitBooking.Domain.Enums;

namespace ReenbitBooking.Domain.Entities;

/// <summary>
/// A User's reservation of a specific Slot. Carries its own <see cref="RowVersion"/> so that
/// concurrent state changes (e.g. two Admins cancelling the same Booking) are resolved via
/// optimistic concurrency rather than a last-write-wins overwrite.
/// </summary>
public class Booking
{
    public Guid Id { get; set; }

    public Guid SlotId { get; set; }

    public Slot? Slot { get; set; }

    public Guid UserId { get; set; }

    public User? User { get; set; }

    public BookingStatus Status { get; set; } = BookingStatus.Confirmed;

    public DateTime BookedAtUtc { get; set; }

    public DateTime? CancelledAtUtc { get; set; }

    /// <summary>
    /// Database-generated row version used as the EF Core optimistic concurrency token.
    /// Any update sent with a stale value results in a <c>DbUpdateConcurrencyException</c>,
    /// which callers must translate into an HTTP 409 Conflict.
    /// </summary>
    [Timestamp]
    public byte[] RowVersion { get; set; } = [];
}
