using MediatR;

namespace ReenbitBooking.Application.Features.Bookings.Commands.CreateBooking;

public record CreateBookingCommand(Guid SlotId, Guid UserId) : IRequest<Guid>;