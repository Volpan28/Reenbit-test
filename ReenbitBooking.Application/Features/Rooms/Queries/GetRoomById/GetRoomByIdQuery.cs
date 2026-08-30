using MediatR;
using ReenbitBooking.Application.Features.Rooms.Queries.GetRooms;

namespace ReenbitBooking.Application.Features.Rooms.Queries.GetRoomById;

public record GetRoomByIdQuery(Guid Id) : IRequest<RoomDto>;