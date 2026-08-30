using MediatR;
using ReenbitBooking.Application.Common.Interfaces;
using ReenbitBooking.Application.Features.Rooms.Queries.GetRooms;

namespace ReenbitBooking.Application.Features.Rooms.Queries.GetRoomById;

public class GetRoomByIdQueryHandler(IRoomQueries _roomQueries) : IRequestHandler<GetRoomByIdQuery, RoomDto>
{
    public async Task<RoomDto> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
    {
        return await _roomQueries.GetRoomByIdAsync(request.Id, cancellationToken);
    }
}