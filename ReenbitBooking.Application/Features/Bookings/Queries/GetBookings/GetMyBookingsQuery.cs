using MediatR;

namespace ReenbitBooking.Application.Features.Bookings.Queries.GetBookings;

public record GetMyBookingsQuery(Guid UserId) : IRequest<IEnumerable<BookingDto>>;