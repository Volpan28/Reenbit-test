using MediatR;

namespace ReenbitBooking.Application.Features.Slots.Commands.CreateSlot;

public record CreateSlotCommand(Guid RoomId, DateTimeOffset StartTimeUtc, DateTimeOffset EndTimeUtc) : IRequest<Guid>;