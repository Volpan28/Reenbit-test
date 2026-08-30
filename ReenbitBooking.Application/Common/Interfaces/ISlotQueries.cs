using ReenbitBooking.Application.Features.Slots.Queries.GetSlotsForRoom;

namespace ReenbitBooking.Application.Common.Interfaces;

public interface ISlotQueries
{
    Task<IEnumerable<SlotDto>> GetSlotsForRoomAsync(Guid roomId, CancellationToken cancellationToken);
}