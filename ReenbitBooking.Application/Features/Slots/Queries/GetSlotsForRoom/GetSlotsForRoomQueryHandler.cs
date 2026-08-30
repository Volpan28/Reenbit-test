using MediatR;
using ReenbitBooking.Application.Common.Interfaces;

namespace ReenbitBooking.Application.Features.Slots.Queries.GetSlotsForRoom;

public class GetSlotsForRoomQueryHandler(ISlotQueries _slotQueries) : IRequestHandler<GetSlotsForRoomQuery, IEnumerable<SlotDto>>
{
    public async Task<IEnumerable<SlotDto>> Handle(GetSlotsForRoomQuery request, CancellationToken cancellationToken)
    {
        return await _slotQueries.GetSlotsForRoomAsync(request.RoomId, cancellationToken);
    }
}