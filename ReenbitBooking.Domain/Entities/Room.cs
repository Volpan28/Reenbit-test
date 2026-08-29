namespace ReenbitBooking.Domain.Entities;

public class Room
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public int Capacity { get; set; }

    /// <summary>
    /// Soft-disable flag used by Admins instead of hard-deleting a Room that already has Slots/Bookings.
    /// </summary>
    public bool IsActive { get; set; } = true;

    public ICollection<Slot> Slots { get; set; } = new List<Slot>();
}
