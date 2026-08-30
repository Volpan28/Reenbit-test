using MediatR;

namespace ReenbitBooking.Application.Features.Rooms.Queries.GetRooms;

public record GetRoomsQuery(int PageNumber = 1, int PageSize = 10) : IRequest<IEnumerable<RoomDto>>;