namespace ReenbitBooking.Domain.Entities;

public class Room
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public int Capacity { get; set; }
    
    public bool IsActive { get; set; } = true;

    public ICollection<Slot> Slots { get; set; } = new List<Slot>();
}
