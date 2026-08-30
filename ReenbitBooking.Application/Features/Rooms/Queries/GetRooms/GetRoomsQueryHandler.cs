using MediatR;
using ReenbitBooking.Application.Common.Interfaces;

namespace ReenbitBooking.Application.Features.Rooms.Queries.GetRooms;

public class GetRoomsQueryHandler(IRoomQueries _roomQueries) : IRequestHandler<GetRoomsQuery, IEnumerable<RoomDto>>
{
    public async Task<IEnumerable<RoomDto>> Handle(GetRoomsQuery request, CancellationToken cancellationToken)
    {
        return await _roomQueries.GetRoomsAsync(request.PageNumber, request.PageSize, cancellationToken);
    }
}