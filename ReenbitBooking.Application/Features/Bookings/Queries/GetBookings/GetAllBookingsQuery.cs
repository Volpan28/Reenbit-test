using MediatR;

namespace ReenbitBooking.Application.Features.Bookings.Queries.GetBookings;

public record GetAllBookingsQuery() : IRequest<IEnumerable<BookingDto>>;