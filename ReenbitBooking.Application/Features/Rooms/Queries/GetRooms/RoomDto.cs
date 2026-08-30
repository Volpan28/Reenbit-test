namespace ReenbitBooking.Application.Features.Rooms.Queries.GetRooms;

public record RoomDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Location { get; init; } = string.Empty;
    public int Capacity { get; init; }
}