using MediatR;

namespace ReenbitBooking.Application.Features.Slots.Commands.DeleteSlot;

public record DeleteSlotCommand(Guid Id) : IRequest<Unit>;