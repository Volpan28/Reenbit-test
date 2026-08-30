using MediatR;

namespace ReenbitBooking.Application.Features.Slots.Queries.GetSlotsForRoom;

public record GetSlotsForRoomQuery(Guid RoomId) : IRequest<IEnumerable<SlotDto>>;