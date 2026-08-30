using ReenbitBooking.Application.Features.Rooms.Queries.GetRooms;

namespace ReenbitBooking.Application.Common.Interfaces;

public interface IRoomQueries
{
    Task<IEnumerable<RoomDto>> GetRoomsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<RoomDto> GetRoomByIdAsync(Guid id, CancellationToken cancellationToken);
}